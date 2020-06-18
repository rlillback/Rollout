using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace Rollout.BLL
{
    public static class XfrmShipTo
    {
        #region PrivateMembers
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Populate the top common ship to information
        /// </summary>
        /// <param name="shipTo"></param>
        /// <param name="csv"></param>
        /// <returns></returns>
        private static bool PopulateShipToHeader(ref ShipTo shipTo, ShipToCSV csv)
        {
            bool success = true;
            DataRow r = csv.DT.Rows[0];
            shipTo.StoreName = r["SHIP TO NAME"].ToString();
            shipTo.ConceptID = r["CONCEPT CODE"].ToString();
            double? parent = JDE.GetParentFromConcept(shipTo.ConceptID);
            if (null != parent)
            {
                shipTo.ParentAddress = (double)parent;
            }
            else
            {
                log.Debug($"Couldn't find parent for {r["CONCEPT CODE"].ToString()}");
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Populate a single ship-to line from the CSV
        /// </summary>
        /// <param name="r"></param>
        /// <param name="LookupJDEAddress"></param>
        /// <returns></returns>
        private static ShipToLine PopulateShipToLine(DataRow r, bool LookupJDEaddress)
        {
            ShipToLine line = new ShipToLine();
            line.Address1 = (null == r.Field<String>("ADDRESS 1")) ? "" : r.Field<String>("ADDRESS 1").ToUpper();
            line.Address2 = (null == r.Field<String>("ADDRESS 2")) ? "" : r.Field<String>("ADDRESS 2").ToUpper();
            line.City = (null == r.Field<String>("CITY")) ? "" : r.Field<String>("CITY").ToUpper();
            line.State = (null == r.Field<String>("STATE")) ? "" : r.Field<String>("STATE").ToUpper();
            line.Zip = (null == r.Field<String>("ZIP")) ? "" : r.Field<String>("ZIP").ToUpper();
            line.TaxAreaCode = (null == r.Field<String>("TAX AREA CODE")) ? "" : r.Field<String>("TAX AREA CODE").ToUpper();
            line.TaxExplanationCode = (null == r.Field<String>("TAX EXPLANATION CODE")) ? "" : r.Field<String>("TAX EXPLANATION CODE").ToUpper();
            line.Concept = (null == r.Field<String>("CONCEPT CODE")) ? "" : r.Field<String>("CONCEPT CODE").ToUpper();
            line.StoreNumber = (null == r.Field<String>("STORE NUMBER")) ? "" : r.Field<String>("STORE NUMBER").ToUpper();
            if (true == LookupJDEaddress)
            {
                string alky = line.StoreNumber + "-" + line.Concept;
                double? jdeaddress = JDE.GetAddressFromALKY(alky);
                line.JDEAddress = (null == jdeaddress) ? 0 : (double)jdeaddress; 
            }
            else
            {
                line.JDEAddress = 0;
            }
            return line;
        }
        #endregion

        /// <summary>
        /// Populate the ship-to data structure
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="PopulateJDEAddress"></param>
        /// <returns></returns>
        public static ShipTo CSVToShipTo (ShipToCSV csv, bool LookupJDEaddress)
        {
            ShipTo shipTo = new ShipTo();
            shipTo.NewShipTos = new List<ShipToLine>();
            if (!PopulateShipToHeader(ref shipTo, csv))
            {
                //TODO: Error handler
            }
            foreach (DataRow r in csv.DT.Rows)
            {
                ShipToLine line = PopulateShipToLine(r, LookupJDEaddress);
                shipTo.NewShipTos.Add(line);
            }
            return shipTo;
        }
    }
}
