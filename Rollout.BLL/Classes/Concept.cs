using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public class Concept
    {
        #region PublicMembers
        public string batch { get; set; }

        public double BillToAddress { get; set; }

        private string _ConceptID;
        public string ConceptID
        {
            get { return _ConceptID; }
            set
            {
                if (3 < value.Length)
                {
                    _ConceptID = value.Substring(0, 3);
                }
                else
                {
                    _ConceptID = value;
                }
            }
        }

        public decimal JulianOrderDate { get; set; }

        private string _PONumber;
        public string PONumber
        {
            get { return _PONumber; }
            set
            {
                if (25 < value.Length)
                {
                    _PONumber = value.Substring(0, 25);
                }
                else
                {
                    _PONumber = value;
                }
            }
        }

        private string _OrderedBy;
        public string OrderedBy
        {
            get { return _OrderedBy; }
            set
            {
                if (10 < value.Length)
                {
                    _OrderedBy = value.Substring(0, 10);
                }
                else
                {
                    _OrderedBy = value;
                }
            }
        }

        public double ShippingVendor { get; set; }

        private string _ShippingMode;
        public string ShippingMode
        {
            get { return _ShippingMode; }
            set
            {
                if (3 < value.Length)
                {
                    _ShippingMode = value.Substring(0, 3);
                }
                else
                {
                    _ShippingMode = value;
                }
            }
        }

        public List<ConceptLine> OrderDetails { get; set; }

#if __DO_NOT_COMPILE__
        public bool OrderValid { get; set; }
#endif
#endregion
    }
}
