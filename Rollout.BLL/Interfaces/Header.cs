using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public class Header
    {
        public string ColumnName { get; set; }
        public Regex ColumnRegex { get; set; }
        public Regex FirstColumnRegex { get; set; }
        public bool CSVHasColumn { get; set; }
    };
}
