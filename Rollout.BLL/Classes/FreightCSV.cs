using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Rollout.Common;
using log4net;


namespace Rollout.BLL
{
    public class FreightCSV : GenericCSV, IStrictCSV
    {
        #region PrivateMembers
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region PublicMembers
        public List<Header> HeaderRow { get; set; }
        #endregion

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
        private Header PopulateHeader(string name, Regex reg, Regex firstRowReg)
        {
            Header column = new Header();
            column.ColumnName = name;
            column.ColumnRegex = reg;
            column.FirstColumnRegex = firstRowReg;
            column.CSVHasColumn = false;
            return column;
        } // PopulateHeader

        /// <summary>
        /// Populate the Required Header Rows
        /// </summary>
        private void PopulateHeaderRow()
        {
            Regex intreg = new Regex(RegexHelper.MatchInteger);
            Regex nonBlankReg = new Regex(RegexHelper.MatchNonBlank);
            Regex decimalNum = new Regex(RegexHelper.MatchDecimalNumber);
            this.HeaderRow.Add(PopulateHeader("ORDER #", intreg, intreg));
            // Removed "WEIGHT" from the spreadsheet 
            this.HeaderRow.Add(PopulateHeader("TRACKING #", nonBlankReg, nonBlankReg));
            this.HeaderRow.Add(PopulateHeader("FREIGHT", decimalNum, decimalNum));
        }
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
            // Now make sure the first row has the requisite data in it; this row is an exception to the others
            // And requires more data fields -- However, don't worry about validating this data until the rest of the file is valid
            if (true == rowsValid)
            {
                DataRow firstRow = DT.Rows[0];
                foreach (Header h in HeaderRow)
                {
                    firstRow["RowValid"] = h.FirstColumnRegex.IsMatch(firstRow[h.ColumnName].ToString());
                    if (false == (bool)firstRow["RowValid"])
                    {
                        log.Error($"The first data row requires {h.ColumnName} to be non-blank and valid. The column contained {firstRow[h.ColumnName].ToString()} -- row data: {string.Join(",", firstRow.ItemArray)}");
                        rowsValid = false;
                    }
                }
            }
            return rowsValid;
        } // ValidateRows

        /// <summary>
        /// Read in the FreightCSV
        /// </summary>
        public void ReadFreight()
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
                throw;
            }
        } // ReadFreight

        /// <summary>
        /// Write the FreightCSV to a file
        /// We probably won't need this at all
        /// </summary>
        public void WriteFreight()
        {
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
        } // WriteConcept

        public int RowCount => DT.Rows.Count;
        #endregion

        #region Constructors
        public FreightCSV() : this(@"C:\Freight.CSV", "\t") { }
        public FreightCSV(string filename) : this(filename, "\t") { }

        /// <summary>
        /// Define what a ShipToCSV consists of
        /// </summary>
        public FreightCSV(string filename, string delimiter)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Instantiating FreightCSV file: {filename} using delimeter {delimiter}");
            HeaderRow = new List<Header>();
            PopulateHeaderRow();
            this.FileName = filename;
            this.DeLimiter = delimiter;
        } // Constructor
        #endregion
    }
}
