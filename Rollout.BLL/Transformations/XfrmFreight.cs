using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rollout.Common;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public static class XfrmFreight
    {
        #region private_members
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private_methods
        /// <summary>
        /// Populate a single Freight class data row
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private static FreightLine PopulateFreightLine(DataRow r, double pkgnumber)
        {
            FreightLine line = new FreightLine();
            line.cost = Double.Parse(r.Field<String>("FREIGHT"));
            line.order = Double.Parse(r.Field<String>("ORDER #"));
            line.shipment = 0;  // This gets populated later
            line.trackingNumber = r.Field<String>("TRACKING #");
            line.weight = 0;
            line.pkgnumber = pkgnumber;
            return line;
        } // PopulateFreightLine
        #endregion

        #region public_methods
        /// <summary>
        /// Populate the Freight data structure
        /// </summary>
        /// <param name="csv"></param>
        /// <returns>Populated Freight data structure</returns>
        public static Freight CSVToFreight(FreightCSV csv)
        {
            double pkgnumber = 0;
            Freight freightUpdate = new Freight();
            freightUpdate.freight_lines = new List<FreightLine>();
            /* Get a list of the order numbers */
            List<string> orders = csv.DT.AsEnumerable().Select(n => n.Field<String>("ORDER #")).Distinct().ToList();
            /* Loop through each order number */
            foreach (string order in orders)
            {
                pkgnumber = 0;
                /* Now grab all the orders with the same order number */
                List<DataRow> orderList = csv.DT.AsEnumerable().Where(n => order == n.Field<String>("ORDER #")).ToList() ;
                foreach (DataRow r in orderList)
                {
                    pkgnumber++;
                    FreightLine line = PopulateFreightLine(r, pkgnumber);
                    freightUpdate.freight_lines.Add(line);
                }
            }
            // Now populate all the shipment numbers based on the order numbers
            JDE.GetShipmentNumbers(ref freightUpdate, orders);
            return freightUpdate;
        } // CSVToFreight
        #endregion
    }
}
