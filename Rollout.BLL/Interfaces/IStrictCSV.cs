using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rollout.BLL
{ 
    interface IStrictCSV
    {
        List<Header> HeaderRow { get; set; }
        bool ValidateHeader();
        bool ValidateRows();
    }
}
