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
        public double BillToAddress { get; set; }
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
        public List<ConceptLine> OrderDetails { get; set; }
        public bool OrderValid { get; set; }
        #endregion
    }
}
