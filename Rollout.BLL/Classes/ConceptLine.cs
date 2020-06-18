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
        public double ShipToAddress { get; set; }
        public decimal LineNumber { get; set; }

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
                    _jdePartNumber = value.Substring(0, 25);
                }
                else
                {
                    _jdePartNumber = value;
                }
            }
        }
        public double Quantity { get; set; }
        public decimal JulianRequestedDate { get; set; }
        private string _BranchPlant;
        public string BranchPlant
        {
            get { return _BranchPlant; }
            set
            {
                if (12 < value.Length)
                {
                    _BranchPlant = value.Substring((value.Length - 12), 12); // Rightmost 12
                }
                else
                {
                    _BranchPlant = value;
                }
            }
        }
        public bool ValidShipTo { get; set; }
        public bool ValidPartNumber { get; set; }

        private bool _lineValid;
        public bool LineValid
        {
            get
            {
                return (ValidShipTo && ValidPartNumber);
            }
        }
        #endregion

        public void ValidateLine()
        {            
            this.ValidShipTo = JDE.DoesAddressExist(this.ShipToAddress);
            this.ValidPartNumber = JDE.DoesItemExist(this.JDEPartNumber, "3SUW");
        }
    }
}
