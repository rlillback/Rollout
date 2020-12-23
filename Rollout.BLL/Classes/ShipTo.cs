using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public class ShipTo
    {
        private string _conceptID;

        public string batch { get; set; }

        public string ConceptID
        {
            get { return _conceptID; }
            set { _conceptID = value.ToUpper(); }
        }

        public double ParentAddress { get; set; }

        public List<ShipToLine>  NewShipTos { get; set; }

        public bool ValidateTaxCodes()
        {
            return JDE.ValidateTaxCodes(this.NewShipTos.Select(n => n.TaxAreaCode.Trim()).Distinct().ToList());
        }

        public bool ValidateTaxExplanations()
        {
            return JDE.ValidateTaxExplanations(this.NewShipTos.Select(n => n.TaxExplanationCode.Trim()).Distinct().ToList());
        }
    }
}
