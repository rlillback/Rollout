using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public class ConceptLine
    {
        #region PublicMembers
        private string _BranchPlant;
        public string BranchPlant
        {
            get { return _BranchPlant; }
            set
            {
                if (12 < value.Length)
                {
                    _BranchPlant = value.Substring((value.Length - 12), 12).ToUpper(); // Rightmost 12
                }
                else
                {
                    _BranchPlant = value.ToUpper();
                }
            }
        }

        public double DocumentNumber { get; set; }

        private string _jdePartNumber;
        public string JDEPartNumber
        {
            get
            {
                return _jdePartNumber;
            }
            set
            {
                if (25 < value.Length)
                {
                    _jdePartNumber = value.Substring(0, 25).ToUpper();
                }
                else
                {
                    _jdePartNumber = value.ToUpper();
                }
            }
        }

        public decimal JulianRequestedDate { get; set; }

        public decimal LineNumber { get; set; }

        public double Quantity { get; set; }

        public double ShipToAddress { get; set; }
        #endregion
    }
}
