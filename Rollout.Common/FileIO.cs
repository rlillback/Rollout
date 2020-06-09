using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using System.Threading.Tasks;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;

namespace Rollout.Common
{
    public static class FileIO
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the first file you find matching *.csv or *.txt
        /// </summary>
        /// <param name="pathName">the path to the folder to search</param>
        /// <returns>the path & filename of the first *.csv or *.txt in the folder</returns>
        public static string GetFileName(string pathName)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            Regex regEX = new Regex(@"(\w|\-)+(\w|\s|\-)*\.(((c|C)(s|S)(v|V))|((t|T)(x|X)(t|T)))$");
            string[] allFiles = Directory.GetFiles(pathName);
            if (0 == allFiles.Length)
            {
                log.Info("No files found in directory " + pathName);
            }
            else
            {
                log.Info("Found " + allFiles.Length +
                                 " files in directory. " +
                                 "Attempting to match csv or txt.");
            }
            string[] csvFiles = allFiles.Where(n => regEX.IsMatch(n)).ToArray();
            string foundFile = String.Empty;
            if (0 == csvFiles.Length)
            {
                log.Error("CSV or TXT file not found in directory " +
                                 pathName);
            }
            else if (1 < csvFiles.Length)
            {
                /* We have more than one match, so print out the warning */
                log.Warn("Found more than one file in the directory.");
                log.Info("Selecting file named: " + allFiles[0]);
                foundFile = allFiles[0];
            }
            else
            {
                foundFile = allFiles[0];
                log.Info("Found a single file in the directory...");
                log.Info("Selecting file named: " + allFiles[0] + "\r\n");
                foundFile = allFiles[0];
            }
            return foundFile;
        } // getFileName

        /// <summary>
        /// Write a datatable object to a CSV delimited by the passed string
        /// </summary>
        /// <param name="fileName">the path and file name to write</param>
        /// <param name="dt">a DataTable object</param>
        /// <param name="deLimiter">a string delimiter to place between values</param>
        public static void WriteCSV(string fileName, DataTable dt, string deLimiter)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            try
            {
                using (StreamWriter sr = new StreamWriter(fileName))
                {
                    /* Write the header */
                    IEnumerable<string> headerNames =
                         dt.Columns.Cast<DataColumn>().
                         Select(n => n.ColumnName);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Join(deLimiter, headerNames));
                    sr.Write(sb);
                    sr.Flush();
                    log.Debug($"Wrote row to CSV: {string.Join(",", headerNames)}");

                    /* Loop through the data */
                    foreach (DataRow r in dt.Rows)
                    {
                        IEnumerable<string> data = r.ItemArray.
                                                   Select(n => n.ToString());
                        sb.Clear();
                        sb.AppendLine(string.Join(deLimiter, data));
                        sr.Write(sb);
                        sr.Flush();
                        log.Debug($"Wrote row to CSV: {string.Join(",", data)}");
                    }                   
                }
                log.Info($"Successfully wrote {fileName} to disk.");
            }
            catch (Exception ex)
            {
                log.Error("Error reading concept" + ex.Message);
                throw (ex);
            }
            return;
        } // writeCSV

        /// <summary>
        /// Read a CSV into a DataTable structure, it requires a single header
        /// row as the first row
        /// </summary>
        /// <param name="fileName">the path and filename to read</param>
        /// <param name="deLimiter">the field delimiter</param>
        /// <returns>
        /// a DataTable object with all the rows and columns populated from the CSV file.
        /// </returns>
        public static DataTable ReadCSV(string fileName, string deLimiter)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            DataTable dataTable = new DataTable();

            try
            {
                using (var parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(new string[] { deLimiter });
                    log.Debug($"Successfully opened file {fileName}");

                    /*
                     * Add the column headers
                     */
                    string[] columnHeaders = parser.ReadFields();
                    log.Info("Attempting to load column headers...");
                    foreach (string column in columnHeaders)
                    {
                        DataColumn dataColumn = new DataColumn(column);
                        dataColumn.AllowDBNull = true;
                        dataTable.Columns.Add(dataColumn);
                        log.Debug(column + " column added to data table.");
                    }
                    log.Info("Successfully loaded all column headers...");
                    log.Info("There are " +
                                     dataTable.Columns.Count.ToString() +
                                    " columns in this csv file.");

                    /*
                     * Read the column data
                     */
                    log.Info("Attempting to load data rows...");
                    while (!parser.EndOfData)
                    {
                        string[] fieldData = parser.ReadFields();
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (String.Empty == fieldData[i])
                            {
                                fieldData[i] = null;
                            }
                        } // for
                        dataTable.Rows.Add(fieldData);
                        log.Debug($"Added row to datatable: {string.Join(",",fieldData)}");
                    } // while
                    log.Info("Successfully read " +
                                     dataTable.Rows.Count.ToString() +
                                     " rows");
                } // using
            } // try
            catch (Exception ex)
            {
                log.Error("Error reading concept" + ex.Message);
                throw (ex);
            }
            return dataTable;
        } // readCSV
    } // class
} // namespace
