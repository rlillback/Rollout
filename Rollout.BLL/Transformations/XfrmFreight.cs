using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        private static FreightLine PopulateFreightLine(DataRow r)
        {
            FreightLine line = new FreightLine();
            line.cost = Double.Parse(r.Field<String>("FREIGHT"));
            line.order = Double.Parse(r.Field<String>("ORDER #"));
            line.shipment = 0;  // This gets populated later
            line.trackingNumber = r.Field<String>("TRACKING #");
            line.weight = Double.Parse(r.Field<String>("WEIGHT"));
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
            Freight freightUpdate = new Freight();
            freightUpdate.freight_lines = new List<FreightLine>();
            foreach (DataRow r in csv.DT.Rows)
            {
                FreightLine line = PopulateFreightLine(r);
                freightUpdate.freight_lines.Add(line);
            }
            // Now populate all the shipment numbers based on the order numbers
            JDE.GetShipmentNumbers(ref freightUpdate);
            return freightUpdate;
        } // CSVToFreight
        #endregion
    }
}
