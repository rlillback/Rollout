using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Rollout.Common;
using System.Data;
using log4net;
using System.IO;
using System.Reflection;

namespace Rollout.BLL
{
    public class ConceptCSV : GenericCSV, IStrictCSV
    {
        #region PrivateMembers
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public List<Header> HeaderRow { get; set; }

        #region PrivateMethods
        /// <summary>
        /// Loop through the datatable & see if it has a header named name
        /// </summary>
        /// <param name="name">The header name to match</param>
        /// <returns>
        /// true = header name was found
        /// false = header name was not found
        /// </returns>
        private bool CheckForColumn(string name)
        {
            bool isThere = false;
            foreach (DataColumn column in DT.Columns)
            {
                if (String.Equals(name.ToUpper(), column.ColumnName.ToUpper()))
                {
                    isThere = true;
                    break;
                }
            }
            return isThere;
        } // CheckForColumn     

        /// <summary>
        /// Populate a single header row and set the has flag to false
        /// </summary>
        /// <param name="name">the column name</param>
        /// <param name="reg">the regular expression for column checking</param>
        /// <returns>a populated Header object</returns>
        private Header PopulateHeader(string name, Regex reg)
        {
            Header column = new Header();
            column.ColumnName = name;
            column.ColumnRegex = reg;
            column.CSVHasColumn = false;
            return column;
        } // PopulateHeader

        /// <summary>
        /// Populate the Required Header Rows
        /// </summary>
        private void PopulateHeaderRow()
        {
            Regex anyreg = new Regex(RegexHelper.MatchAnything);
            Regex intreg = new Regex(RegexHelper.MatchInteger);
            Regex datereg = new Regex(RegexHelper.MatchUSADate);
            this.HeaderRow.Add(PopulateHeader("CUSTOMER NUMBER", anyreg));
            this.HeaderRow.Add(PopulateHeader("PO NUMBER", anyreg));
            this.HeaderRow.Add(PopulateHeader("STORE NUMBER", anyreg));
            this.HeaderRow.Add(PopulateHeader("SHIP TO NAME", anyreg));
            this.HeaderRow.Add(PopulateHeader("ADDRESS 1", anyreg));
            this.HeaderRow.Add(PopulateHeader("ADDRESS 2", anyreg));
            this.HeaderRow.Add(PopulateHeader("CITY", anyreg));
            this.HeaderRow.Add(PopulateHeader("STATE", anyreg));
            this.HeaderRow.Add(PopulateHeader("ZIP", anyreg));
            this.HeaderRow.Add(PopulateHeader("ORDER QTY", intreg));
            this.HeaderRow.Add(PopulateHeader("PART NUMBER", anyreg));
            this.HeaderRow.Add(PopulateHeader("REQ'D SHIP DATE", datereg));
            this.HeaderRow.Add(PopulateHeader("ORDERED BY", anyreg));
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Generic constructor provides default filename and delimiter
        /// </summary>
        public ConceptCSV() : this(@"C:\default.csv", ",") { }

        /// <summary>
        /// If one string is specified, assume it is the filename
        /// and set the delimeter to a comma
        /// </summary>
        /// <param name="filename">path & filename for file</param>
        public ConceptCSV(string filename) : this(filename, ",") { }

        /// <summary>
        /// Define what a ConceptCSV consists of
        /// </summary>
        public ConceptCSV(string filename, string delimiter)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Creating ShipToCSV file: {filename} using delimeter {delimiter}");            
            HeaderRow = new List<Header>();
            PopulateHeaderRow();
            this.FileName = filename;
            this.DeLimiter = delimiter;
        } // Constructor
        #endregion

        #region PublicMethods
        /// <summary>
        /// Loop through all the header columns and see if any are missing
        /// </summary>
        /// <returns></returns>
        public bool ValidateHeader()
        {
            bool isValid = true;
            foreach (Header h in HeaderRow)
            {
                if ( !h.CSVHasColumn )
                {
                    h.CSVHasColumn = CheckForColumn(h.ColumnName);
                    if ( !h.CSVHasColumn )
                    {
                        log.Error($"CSV File missing column {h.ColumnName}");
                        isValid = false;
                    }
                }
            }
            return isValid;
        } // ValidateHeader

        /// <summary>
        /// Loop through all the rows and see if the data is valid
        /// </summary>
        /// <returns>
        /// true = all rows are valid
        /// false = row is invalid
        /// </returns>
        public bool ValidateRows()
        {
            bool rowsValid = true;
            foreach (DataRow r in DT.Rows)
            {
                foreach( Header h in HeaderRow )
                {
                    r["RowValid"] = h.ColumnRegex.IsMatch(r[h.ColumnName].ToString());
                    if ( false == (bool)r["RowValid"] )
                    {
                        log.Error($"{h.ColumnName} has {r[h.ColumnName].ToString()} invalid in row {DT.Rows.IndexOf(r)} -- row data: {string.Join(",", r.ItemArray)}");
                        rowsValid = false;
                    }
                }
            }
            return rowsValid;
        } // ValidateRows

        /// <summary>
        /// Read in the ConceptCSV
        /// </summary>
        public void ReadConcept()
        {
            // TODO: Validate filename and delimeter are set
            try
            {
                ReadCSV();
                DataColumn dc = new DataColumn();
                dc.ColumnName = "RowValid";
                dc.DataType = typeof(bool);
                DT.Columns.Add(dc);
                foreach (DataRow r in DT.Rows)
                {
                    r["RowValid"] = false;
                }
            }
            catch (Exception eBLL)
            {
                log.Error("Error reading concept" + eBLL.Message);
                throw eBLL;
            }
        }

        /// <summary>
        /// Write the ConceptCSV to a file
        /// </summary>
        public void WriteConcept()
        {
            try
            {
                DT.Columns.Remove("RowValid");
                WriteCSV();
            }
            catch (Exception eBLL)
            {
                log.Error("Error writing concept CSV" + eBLL.Message);
                throw eBLL;
            }
        } // WriteConcept

        public List<string> CheckForMissingShipToAddresses()
        {
            List<string> AllShipToNames = new List<string>();
            double SoldTo = double.Parse(DT.Rows[0].Field<string>("CUSTOMER NUMBER"), System.Globalization.CultureInfo.InvariantCulture);
            string ConceptID = JDE.GetConceptID(SoldTo);
            foreach (DataRow r in DT.Rows)
            {
                AllShipToNames.Add(r.Field<string>("STORE NUMBER").Trim()
                                    + "-"
                                    + ConceptID.Trim());
            }
            List<string> missing = JDE.FindMissingShipTos(AllShipToNames);
            return missing;
        }

        public int RowCount => DT.Rows.Count;
        #endregion
    }
}
