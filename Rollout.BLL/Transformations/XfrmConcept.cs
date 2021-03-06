﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Rollout.Common;


namespace Rollout.BLL
{
    public static class XfrmConcept
    {
        #region PrivateMembers
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string BranchPlant = "        3SUW";
        #endregion

        #region PrivateMethods
        private static void PopulateConceptHeader(ref Concept con, ConceptCSV csv)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating concept header with {String.Join(",",csv)}");

            try
            {
                con.batch = JDE.GetNextBatchNumber().ToString();
                DataRow r = csv.DT.Rows[0];
                con.BillToAddress = Double.Parse(r["CUSTOMER NUMBER"].ToString());
                con.ConceptID = JDE.GetConceptID(con.BillToAddress);
                con.JulianOrderDate = CommonFunctions.DateStringToJulian(System.DateTime.Today.ToString());
                con.OrderDetails = new List<ConceptLine>();
                con.PONumber = r["PO NUMBER"].ToString();
                con.OrderedBy = r["ORDERED BY"].ToString();
                con.ShippingVendor = Double.Parse(r["SHIPPING VENDOR"].ToString());
                con.ShippingMode = r["SHIPPING METHOD"].ToString();
            }
            catch (Exception eBLL)
            {
                log.Error($"Error populating concept header" + eBLL.Message);
                throw;
            }
            
            return;
        }

        private static ConceptLine PopulateConceptLine(double DocumentNumber, string StoreNumber, decimal LineNumber, string PartNumber, double Quantity, decimal RequestedJulian, Concept concept)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating concept line with {DocumentNumber.ToString()},{StoreNumber},{LineNumber.ToString()},{PartNumber},{Quantity.ToString()},{RequestedJulian.ToString()},{StoreNumber}-{concept.ConceptID}");

            ConceptLine line;
            try
            {
                line = new ConceptLine();
                line.DocumentNumber = DocumentNumber;
                line.BranchPlant = BranchPlant;
                string alky = StoreNumber + "-" + concept.ConceptID;
                line.ShipToAddress = (double)JDE.GetAddressFromALKY(alky);
                line.LineNumber = LineNumber;
                line.JDEPartNumber = PartNumber;
                line.Quantity = Quantity;
                line.JulianRequestedDate = RequestedJulian;
            }
            catch (Exception eBLL)
            {
                log.Error($"Error writing concept line " + eBLL.Message);
                throw;
            }
            return line;
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Convert a csv object into a concept object
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        public static Concept CSVtoConcept( ConceptCSV csv )
        {
            Concept con = new Concept();
            ConceptLine entry;
            int linenumber;
            DataRow r1;
            List<DataRow> rows = new List<DataRow>();
            double document = JDE.GetDocumentNumbers(csv.DT.Rows.Count); // reserve a set of document numbers
            PopulateConceptHeader(ref con, csv);
            // Get a list of unique store numbers
            List<string> stores = csv.DT.AsEnumerable().Select(n => n.Field<string>("STORE NUMBER")).Distinct().ToList();
            // Loop through that customer and add all of that customer's lines to the datatable
            foreach (string store in stores)
            {
                rows = csv.DT.AsEnumerable().Where(n => store == n.Field<string>("STORE NUMBER")).ToList();
                linenumber = 1;  // Start at linenumber 1
                foreach (DataRow r in rows)
                {
                    entry = PopulateConceptLine(document,
                                            r["STORE NUMBER"].ToString(),
                                            linenumber, // Line #
                                            r["PART NUMBER"].ToString(),  // The part number
                                            Double.Parse(r["ORDER QTY"].ToString()),
                                            CommonFunctions.DateStringToJulian(r["REQ'D SHIP DATE"].ToString()),
                                            con);
                    con.OrderDetails.Add(entry);
                    linenumber++;
                }
                // Now add the freight line for this customer
                r1 = rows[0]; // get the first row
                entry = PopulateConceptLine(document,
                                            r1["STORE NUMBER"].ToString(),
                                            linenumber, // Line #2 is the freight line
                                            "9227", // The freight part number
                                            1, // Qty of 1
                                            CommonFunctions.DateStringToJulian(r1["REQ'D SHIP DATE"].ToString()),
                                            con);
                con.OrderDetails.Add(entry);
                document++;
            }
            return con;
        } // CSVtoConcept

        public static ConceptCSV ConceptToCSV( Concept con )
        {
            //TODO: Do I ever need this? 
            ConceptCSV csv = new ConceptCSV();
            return csv;
        } // ConceptToCSV 
        #endregion
    }
}
