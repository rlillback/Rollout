using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public class ShipTo
    {
        public string batch { get; set; }

        public string ConceptID { get; set; }

        public double ParentAddress { get; set; }

        public string StoreName { get; set; }

        public List<ShipToLine>  NewShipTos { get; set; }
    }
}
