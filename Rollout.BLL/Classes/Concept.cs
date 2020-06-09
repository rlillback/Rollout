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
        public List<ConceptLine> OrderDetails { get; set; }
        public bool OrderValid { get; set; }
        #endregion
    }
}
