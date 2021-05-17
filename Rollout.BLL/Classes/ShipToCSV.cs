using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rollout.Common;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;
using System.Reflection;

namespace Rollout.BLL
{
    public class ShipToCSV : GenericCSV, IStrictCSV
    {
        #region PrivateMembers
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region public members
        public List<Header> HeaderRow { get; set; }
        #endregion

        #region constructors
        public ShipToCSV() : this(@"C:\ShipTo.CSV", "\t") { }
        public ShipToCSV(string filename) : this(filename, "\t") { }

        /// <summary>
        /// Define what a ShipToCSV consists of
        /// </summary>
        public ShipToCSV(string filename, string delimiter)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Instantiating ShipToCSV file: {filename} using delimeter {delimiter}");
            HeaderRow = new List<Header>();
            PopulateHeaderRow();
            this.FileName = filename;
            this.DeLimiter = delimiter;
        } // Constructor
        #endregion

        #region Private Methods
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
            Regex intOrBlankReg = new Regex(RegexHelper.MatchIntegerOrBlank);
            Regex nonBlankReg = new Regex(RegexHelper.MatchNonBlank);
            Regex ZipCodeUSA = new Regex(RegexHelper.MatchZipCodeUSA);
            Regex ZipCodes = new Regex(RegexHelper.MatchZipCodes);
            this.HeaderRow.Add(PopulateHeader("SHIP TO NAME", nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("STORE NUMBER", nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("CONCEPT CODE", nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("ADDRESS 1", nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("ADDRESS 2", anyreg));
            this.HeaderRow.Add(PopulateHeader("CITY", nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("STATE", nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("ZIP", ZipCodes));
            this.HeaderRow.Add(PopulateHeader("TAX AREA CODE", nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("TAX EXPLANATION CODE", nonBlankReg));
        }

        /// <summary>
        /// Create the data table columns
        /// </summary>
        private void PopulateColumns()
        {
            foreach(Header h in HeaderRow)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = h.ColumnName;
                dc.DataType = typeof(string); // all CSV columns are strings
                DT.Columns.Add(dc);
            }
        }

        /// <summary>
        /// Add a single datarow to the DataTable by copying the right row in the conceptCSV into the ShipToCSV
        /// </summary>
        /// <param name="storenumber"></param>
        /// <param name="concept"></param>
        /// <param name="conceptID"></param>
        private void PopulateDataRow(string storenumber, ConceptCSV concept, string conceptID)
        {
            DataRow r = DT.NewRow();
            string StoreNumberMinusConcept = storenumber.Substring(0, storenumber.LastIndexOf('-'));
            string selection = "[STORE NUMBER] = '" + StoreNumberMinusConcept + "'";
            DataRow[] ConceptRow = concept.DT.Select(selection);
            foreach (Header h in HeaderRow)
            {
                if (0 == String.Compare("TAX AREA CODE", h.ColumnName.ToUpper()))
                {
                    r["TAX AREA CODE"] = String.Empty; // Filled out later for F03012 load
                }
                else if (0 == String.Compare("CONCEPT CODE", h.ColumnName.ToUpper()))
                {
                    r["CONCEPT CODE"] = conceptID;
                }
                else if (0 == String.Compare("JDE ADDRESS", h.ColumnName.ToUpper()))
                {
                    r["JDE ADDRESS"] = 0; // The Z-File upload populates the JDE address in JDE
                }
                else if (0 == String.Compare("TAX EXPLANATION CODE", h.ColumnName.ToUpper()))
                {
                    r["TAX EXPLANATION CODE"] = String.Empty; // Filled out later for F03012 load
                }
                else
                {
                    r[h.ColumnName.ToUpper()] = ConceptRow[0].Field<string>(h.ColumnName.ToUpper());
                }
            }
            log.Debug($"Adding row to ShipToCSV: {String.Join(",", r.ItemArray)}");
            DT.Rows.Add(r);
        } // PopulateDataRow
        #endregion

        #region public methods
        /// <summary>
        /// Loop through all the header columns and see if any are missing
        /// </summary>
        /// <returns></returns>
        public bool ValidateHeader()
        {
            bool isValid = true;
            foreach (Header h in HeaderRow)
            {
                if (!h.CSVHasColumn)
                {
                    h.CSVHasColumn = CheckForColumn(h.ColumnName);
                    if (!h.CSVHasColumn)
                    {
                        log.Error($"CSV File missing column {h.ColumnName}");
                        isValid = false;
                    }
                }
            }
            return isValid;
        } // ValidateHeader

        /// <summary>
        /// Using a regex, validate all the rows in the spreadsheet.
        /// </summary>
        /// <returns></returns>
        public bool ValidateRows()
        {
            bool rowsValid = true;
            foreach (DataRow r in DT.Rows)
            {
                foreach (Header h in HeaderRow)
                {
                    r["RowValid"] = h.ColumnRegex.IsMatch(r[h.ColumnName].ToString());
                    if (false == (bool)r["RowValid"])
                    {
                        log.Error($"{h.ColumnName} has {r[h.ColumnName].ToString()} invalid in row {DT.Rows.IndexOf(r)} -- row data: {string.Join(",", r.ItemArray)}");
                        rowsValid = false;
                    }
                }
            }
            return rowsValid;
        }

        /// <summary>
        /// Using a regex, validate all the rows in the spreadsheet.
        /// Change the regex for the tax area to disallow blank entries, if bool is false
        /// </summary>
        /// <param name="AllowEmptyTaxRow"></param>
        /// <returns></returns>
        public bool ValidateRows(bool AllowEmptyTaxRow)
        {
            bool rowsValid = true;
            if (false == AllowEmptyTaxRow)
            {
                Header h = this.HeaderRow.Find(n => "TAX AREA CODE" == n.ColumnName);
                if (null != h)
                {
                    Regex reg = new Regex(RegexHelper.MatchNonBlank);
                    h.ColumnRegex = reg;
                }
                h = this.HeaderRow.Find(n => "TAX EXPLANATION CODE" == n.ColumnName);
                if (null != h)
                {
                    Regex reg = new Regex(RegexHelper.MatchNonBlank);
                    h.ColumnRegex = reg;
                }
            }
            else
            {
                Header h = this.HeaderRow.Find(n => "TAX AREA CODE" == n.ColumnName);
                if (null != h)
                {
                    Regex reg = new Regex(RegexHelper.MatchAnything);
                    h.ColumnRegex = reg;
                }
                h = this.HeaderRow.Find(n => "TAX EXPLANATION CODE" == n.ColumnName);
                if (null != h)
                {
                    Regex reg = new Regex(RegexHelper.MatchAnything);
                    h.ColumnRegex = reg;
                }
            }
            foreach (DataRow r in DT.Rows)
            {
                foreach (Header h in HeaderRow)
                {
                    r["RowValid"] = h.ColumnRegex.IsMatch(r[h.ColumnName].ToString());
                    if (false == (bool)r["RowValid"])
                    {
                        log.Error($"{h.ColumnName} data = {r[h.ColumnName].ToString()} is invalid in row {DT.Rows.IndexOf(r)} -- row data: {string.Join(",", r.ItemArray)}");
                        rowsValid = false;
                    }
                }
            }
            return rowsValid;
        }

        /// <summary>
        /// Read the spreadsheet into the data table
        /// </summary>
        public void ReadShipTo()
        {
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
                log.Error("Error reading shipto" + eBLL.Message);
                throw;
            }
        }

        /// <summary>
        /// Write the ship to CSV to the filesystem
        /// </summary>
        public void WriteShipTo()
        {
            foreach (DataRow r in DT.Rows)
            {
                r["TAX EXPLANATION CODE"] = "S";
            }
            try
            {
                DT.Columns.Remove("RowValid");
                WriteCSV();
            }
            catch (Exception eBLL)
            {
                log.Error("Error writing concept CSV" + eBLL.Message);
                throw;
            }
        }

        /// <summary>
        /// Populate the empty ShipToCSV object
        /// </summary>
        /// <param name="missing"></param>
        /// <param name="concept"></param>
        public void PopulateSpreadsheet(List<string> missing, ConceptCSV concept)
        {
            if (null == DT)
            {
                DT = new DataTable();
                PopulateColumns();
                string conceptID = JDE.GetConceptID(Double.Parse(concept.DT.Rows[0].Field<string>("CUSTOMER NUMBER")));
                foreach (string shipto in missing)
                {
                    PopulateDataRow(shipto, concept, conceptID);
                }
            }
            else
            {
                log.Error("The DataTable for ShipToCSV already exists.");
                throw new System.AggregateException("Error, the DataTable for ShipToCSV already exists & must be deleted first.");
            }
        }
        #endregion
    } // class
} // Namespace
