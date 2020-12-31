using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public class FreightLine
    {
        private string _trackingNumber;
        public double order { get; set; }
        public double shipment { get; set; }
        public double weight { get; set; }
        public double cost { get; set; }
        public double pkgnumber { get; set; }
        public string trackingNumber
        {
            get { return _trackingNumber; }
            set
            {
                if (30 < value.Length)
                {
                    _trackingNumber = value.Substring(0, 30);
                }
                else
                {
                    _trackingNumber = value;
                }
            }
        }
    }
}
