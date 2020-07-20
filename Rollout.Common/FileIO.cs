using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
        /// Select a *.csv or *.txt file to load
        /// </summary>
        /// <param name="pathName">the path to the folder to search</param>
        /// <returns>the path & filename of a selected file or String.Empty if canceled</returns>
        public static string GetFileName(string pathName, string Filter)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            string foundFile = String.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = pathName;
                openFileDialog.Filter = Filter;
                openFileDialog.RestoreDirectory = true;

                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    foundFile = openFileDialog.FileName;
                }
            }
            return foundFile;
        } // getFileName

        /// <summary>
        /// Select/create a path and file to save a file into
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static string SaveFileName(string pathName, string Filter)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            string fileName = String.Empty;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = pathName;
                saveFileDialog.Filter = Filter;
                saveFileDialog.Title = "Save File As...";
                saveFileDialog.DefaultExt = ".txt";
                saveFileDialog.RestoreDirectory = true;
                if (DialogResult.OK == saveFileDialog.ShowDialog())
                {
                    fileName = saveFileDialog.FileName;
                }
            }
            return fileName;
        }

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
                throw;
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
                throw;
            }
            return dataTable;
        } // readCSV
    } // class
} // namespace
