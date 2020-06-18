using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            DataRow r = csv.DT.Rows[0];
            con.BillToAddress = Double.Parse(r["CUSTOMER NUMBER"].ToString());
            con.ConceptID = JDE.GetConceptID(con.BillToAddress);
            con.JulianOrderDate = CommonFunctions.DateStringToJulian(System.DateTime.Today.ToString());
            con.OrderValid = true;
            con.OrderDetails = new List<ConceptLine>();
            return;
        }

        private static ConceptLine PopulateConceptLine(string StoreNumber, decimal LineNumber, string PartNumber, double Quantity, decimal RequestedJulian, Concept concept)
        {
            ConceptLine line = new ConceptLine();
            line.BranchPlant = BranchPlant;
            string alky = StoreNumber + "-" + concept.ConceptID;
            line.ShipToAddress = (double)JDE.GetAddressFromALKY(alky);
            line.LineNumber = LineNumber;
            line.JDEPartNumber = PartNumber;
            line.Quantity = Quantity;
            return line;
        }
        #endregion

        #region PublicMethods
        public static Concept CSVtoConcept( ConceptCSV csv )
        {
            Concept con = new Concept();
            ConceptLine entry;
            PopulateConceptHeader(ref con, csv);
            foreach (DataRow r in csv.DT.Rows)
            {
                entry = PopulateConceptLine(r["STORE NUMBER"].ToString(), 
                                            1, // Line #1 is the part; per Selecto, a rollout only has a single part per customer
                                            r["PART NUMBER"].ToString(),  // The part number
                                            Double.Parse(r["QUANTITY"].ToString()), 
                                            CommonFunctions.DateStringToJulian(r["REQ'D SHIP DATE"].ToString()), 
                                            con);
                con.OrderDetails.Add(entry);
                entry = PopulateConceptLine(r["STORE NUMBER"].ToString(),
                                            2, // Line #2 is the freight line
                                            "9227", // The freight part number
                                            1, // Qty of 1
                                            CommonFunctions.DateStringToJulian(r["REQ'D SHIP DATE"].ToString()),
                                            con);
                con.OrderDetails.Add(entry);
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
