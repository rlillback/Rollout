using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rollout.Common
{
    public class GenericCSV
    {
        public DataTable DT { get; set; }
        public String FileName { get; set; }
        public String DeLimiter { get; set; }

        public void ReadCSV() => DT = FileIO.ReadCSV(FileName, DeLimiter);

        public void WriteCSV() => FileIO.WriteCSV(FileName, DT, DeLimiter);
        
    } // class
} // namespace
