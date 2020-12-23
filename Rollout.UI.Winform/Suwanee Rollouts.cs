using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Rollout.Common;
using Rollout.BLL;
using log4net;
using System.Reflection;
using System.Threading;
using System.Security.Cryptography;
using System.Data;
using System.Linq;

namespace Rollout.UI.Winform
{
    public partial class main : Form
    {
        #region private_members
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string FileFilter = "txt files (*.txt)|*.txt|all files (*.*)|*.*";
        private ConceptCSV conceptCSV;
        private ShipToCSV MissingShipToCSV;
        private FreightCSV freightCSV;
        #endregion

        #region application and forms
        /// <summary>
        /// Main application entry point
        /// </summary>
        public main()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            log.Debug("Loading application & Initializing components.");
            InitializeComponent();
            log.Debug("Component initialization successful.");
            return;
        }

        /// <summary>
        /// Suwanee Rollout Form On Load procedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            log.Debug("Loading main screen.");
            btn_SaveCSV.Enabled = false;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lb_VersionInfo.Text = "Version : " + version.Major + "." + version.Minor + " (build " + version.Build + ")";
            return;
        }
        #endregion

        #region private functions
        /// <summary>
        /// Validate that the Tax Explanations are valid entries in the UDC
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private bool ValidateTaxExplanations(ShipTo ship)
        {
            bool result = ship.ValidateTaxExplanations();
            if (!result)
            {
                // TODO: Fix this to display the missing information
                log.Error("Failed to validate tax explanations.");
                using (new CenterDialog(this))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show($"Error in csv.  There are invalid tax explanation codes.  See Log file and documentation for more information.", "Data Error", buttons);
                }
            }
            else
            {
                log.Debug("All tax explanations are valid in UDC 00/EX.");
            }
            return result;
        } // ValidateTaxExplanations

        /// <summary>
        /// Verify a valid concept code exists in JDE for this customer.
        /// </summary>
        /// <param name="conceptCSV"></param>
        /// <returns></returns>
        private bool ConceptCodeExists(ConceptCSV conceptCSV)
        {
            bool result;
            string concept = JDE.GetConceptID(Double.Parse(conceptCSV.DT.Rows[0].Field<string>("CUSTOMER NUMBER"))).Trim();
            if (String.Empty == concept)
            {
                log.Error($"Customer Number {conceptCSV.DT.Rows[0].Field<string>("CUSTOMER NUMBER")} does not have a concept code populated in F0101.ABAC08");
                using (new CenterDialog(this))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show($"Customer Number {conceptCSV.DT.Rows[0].Field<string>("CUSTOMER NUMBER")} does not have a concept code populated in the Address Book.\r\nPlease fix this in the spreadsheet or JDE before continuing.", "Data Error", buttons);
                }
                result = false;
            }
            else
            {
                log.Debug($"Customer Number {conceptCSV.DT.Rows[0].Field<string>("CUSTOMER NUMBER")} has a concept.");
                result = true;
            }
            return result;
        } // ConceptCodeExists

        /// <summary>
        /// Make sure all tax codes exist in F4008 and we are in an active date range for that code.
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private bool ValidateTaxCodes(ShipTo ship)
        {
            bool result = ship.ValidateTaxCodes();
            if (!result)
            {
                // TODO: Fix this to display the missing information
                log.Error("Failed to validate tax codes.");
                using (new CenterDialog(this))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show($"Error in csv.  There are missing or non-active tax codes in the CSV.  See Log file and documentation for more information.", "Data Error", buttons);
                }
            }
            else
            {
                log.Debug("All tax codes exist and are active.");
            }
            return result;
        } // ValidateTaxCodes

        /// <summary>
        /// See if there are any JDE Addresses that are 0, which is invalid
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private bool AnyZeroAddresses(ShipTo ship)
        {
            bool result;
            if (0 < ship.NewShipTos.Where(n => 0 == n.JDEAddress).Count())
            {
                List<ShipToLine> MissingShipTos = ship.NewShipTos.Where(n => 0 == n.JDEAddress).ToList(); 
                foreach (ShipToLine line in MissingShipTos)
                {
                    log.Error($"This row failed to retrieve a JDE Address = {String.Join(",", line)}");
                }
                using (new CenterDialog(this))
                {
                    MessageBox.Show($"Error in csv. Failed to retrieve JDE Addresses in one or more rows.  See log file and documentation for more information.", "Error in CSV Data", MessageBoxButtons.OK);
                }
                result = true;
            }
            else
            {
                log.Debug($"All lines retrieved a JDE Address Book number.");
                result = false;
            }
            return result;
        } // AnyZeroAddresses

        /// <summary>
        /// Validate concept header minimal columns are there
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private bool ValidateConceptHeader(ref ConceptCSV csv)
        {
            bool result = csv.ValidateHeader();
            if (!result)
            {
                // TODO: Fix this to display the missing information
                log.Error("Failed to validate required headers");
                using (new CenterDialog(this))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show($"Error in csv.  There are missing required header rows.  See Log file and documentation for more information.", "Error in Rollout CSV Headers", buttons);
                }
            }
            else
            {
                log.Debug("All required columns exist");
            }
            return result;
        } // ValidateConceptHeader

        /// <summary>
        /// Validate the row data
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private bool ValidateConceptRows(ref ConceptCSV csv)
        {
            bool result = csv.ValidateRows();
            if (!result)
            {
                // TODO:  Fix this to display the bad data
                log.Error("Failed to validate row data for one or more rows");
                using (new CenterDialog(this))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show($"Error in csv. Failed to validate data in one or more rows.  See log file and documentation for more information.", "Error in Rollout CSV Data", buttons);
                }
            }
            else
            {
                log.Debug("All row data is valid for all required columns");
            }
            return result;
        } // ValidateConceptRows

        /// <summary>
        /// Check to see if the shipping vendor in the spreadsheet is a valid vendor in JDE
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private bool CheckShippingVendor(ConceptCSV csv)
        {
            bool result = csv.CheckShippingVendor();
            if (!result)
            {
                log.Error($"Vendor number {csv.DT.Rows[0].Field<string>("SHIPPING VENDOR")} is not of type V or V3");
                using (new CenterDialog(this))
                {
                    MessageBox.Show($"Error in csv. The shipping vendor number {csv.DT.Rows[0].Field<string>("SHIPPING VENDOR")} is not a vendor in JDE.");
                }
            }
            else
            {
                log.Debug($"The shipping vendor number {csv.DT.Rows[0].Field<string>("SHIPPING VENDOR")} is a valid vendor.");
            }
            return result;
        } // CheckShippingVendor

        /// <summary>
        /// Validate the required columns exist
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private bool ValidateShipToHeader(ref ShipToCSV csv)
        {
            bool result = csv.ValidateHeader();
            if (!result)
            {
                // TODO: Change this to display the missing information
                log.Error("Failed to validate required headers");
                using (new CenterDialog(this))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show($"Error in csv.  There are missing required header rows.  See Log file and documentation for more information.", "Error in Rollout CSV Headers", buttons);
                }
            }
            else
            {
                log.Debug("All required columns exist");
            }
            return result;
        } // ValidateShipToHeader

        /// <summary>
        /// Validate the data in the shipto CSV
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="AllowBlankTaxAreaCode"></param>
        /// <returns></returns>
        private bool ValidateShipToRows(ref ShipToCSV csv, bool AllowBlankTaxAreaCode)
        {
            bool result = csv.ValidateRows(AllowBlankTaxAreaCode);
            if (!result)
            {
                // TODO:  Fix this to display the bad data
                log.Error("Failed to validate row data for one or more rows");
                using (new CenterDialog(this))
                {
                    MessageBox.Show($"Error in csv. Failed to validate data in one or more rows.  See log file and documentation for more information.", "Error in Rollout CSV Data", MessageBoxButtons.OK);
                }
            }
            else
            {
                log.Debug("All row data is valid for all required columns");
            }
            return result;
        } // ValidateShipToRows

        /// <summary>
        /// Validate the required columns exist in the freight update spreadsheet
        /// </summary>
        /// <param name="freight"></param>
        /// <returns></returns>
        private bool ValidateFreightHeaders(ref FreightCSV csv)
        {
            bool result = csv.ValidateHeader();
            if (!result)
            {
                // TODO: Fix this to display the missing information
                log.Error("Failed to validate required headers");
                using (new CenterDialog(this))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show($"Error in csv.  There are missing required header rows.  See Log file and documentation for more information.", "Error in Freight CSV Headers", buttons);
                }
            }
            else
            {
                log.Debug("All required columns exist");
            }
            return result;
        } // ValidateFreightHeaders

        /// <summary>
        /// Validate the data in all freight update spreadsheet rows
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        private bool ValidateFreightRows(ref FreightCSV csv)
        {
            bool result = csv.ValidateRows();
            if (!result)
            {
                // TODO:  Fix this to display the bad data
                log.Error("Failed to validate row data for one or more rows");
                using (new CenterDialog(this))
                {
                    MessageBox.Show($"Error in csv. Failed to validate data in one or more rows.  See log file and documentation for more information.", "Error in Freight Update CSV Data", MessageBoxButtons.OK);
                }
            }
            else
            {
                log.Debug("All row data is valid for all required columns");
            }
            return result;
        } // ValidateFreightRows

        /// <summary>
        /// The process flow to follow when you encounter missing ship to locations in the spreadsheet.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="conceptCSV"></param>
        private void FollowMissingShipToPath(List<string> names, ConceptCSV conceptCSV, LoadingForm frm)
        {
            // 1.) Tell the user about the missing ship to addresses
            using (new CenterDialog(this))
            {
                MessageBox.Show($"There are {names.Count.ToString()} ship to locations that don't exist in JDE.\r\nPress OK to review the list.\r\nThen Save to CSV to create a CSV.\r\nThis CSV needs to be updated and uploaded to JDE.",
                                "Missing Ship To Locations",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            // 2.) Populate the datatable
            frm.AddText("Populating the datatable for review.");
            MissingShipToCSV = new ShipToCSV();
            MissingShipToCSV.PopulateSpreadsheet(names, conceptCSV);
            this.dgv_DataDisplay.DataSource = MissingShipToCSV.DT;
            // 3.) Make the save to CSV available.
            frm.AddText("Datatable population complete.");
            btn_SaveCSV.Enabled = true;
            return;
        } // FollowMissingShipToPath

        /// <summary>
        /// The control flow for loading a rollout spreadsheet.
        /// </summary>
        private void LoadRolloutFlow()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            this.dgv_DataDisplay.DataSource = null;
            MissingShipToCSV = null; // make sure to clear this out each time we load a spreadsheet
            // 1.) Get the file to load
            string FileToLoad = FileIO.GetFileName(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), FileFilter);
            if (String.Empty != FileToLoad)
            {
                try
                {
                    // 2.) Load the file
                    log.Debug($"Attempting to load file {FileToLoad} using \t as a delimiter");
                    conceptCSV = new ConceptCSV(FileToLoad, "\t");
                    using (LoadingForm frm = new LoadingForm())
                    {
                        frm.AddText("Loading Concept Spreadsheet.");
                        frm.Visible = true;
                        conceptCSV.ReadConcept();
                        // 3.) Validate the required columns exist in the spreadsheet
                        log.Debug($"Attempting to validate all required columns exist in the file.");
                        frm.AddText("Validating Concept Required Columns Exist.");
                        if (!ValidateConceptHeader(ref conceptCSV)) { return; }
                        // 4a.) Validate the data in the rows match the column requirements
                        log.Debug("Validating all row data in all required columns.");
                        frm.AddText("Validating data in concept spreadsheet.");
                        if (!ValidateConceptRows(ref conceptCSV)) { return; }
                        // 4b.) Validate the Shipping Vendor
                        log.Debug("Validating the shipping vendor is V or V3.");
                        frm.AddText("Validating the shipping vendor is a vendor in JDE.");
                        if (!CheckShippingVendor(conceptCSV)) { return; }
                        // 4c.) Validate the concept ID exists for the Customer C3 Record
                        log.Debug("Validating the Customer Number has a concept in ABAC08.");
                        frm.AddText("Validating the Customer Number has a concept code.");
                        if (!ConceptCodeExists(conceptCSV)) { return; }
                        // 5.) Verify the ship to addresses exist
                        log.Debug("Verify all Ship To addresses exist");
                        frm.AddText("Checking for missing ship to addresses in JDE.");
                        List<string> MissingShipTo = conceptCSV.CheckForMissingShipToAddresses();
                        if (0 < MissingShipTo.Count)
                        {
                            // 6a.) We found new ship to addresses, so start down that path
                            log.Debug("There are Ship To addresses that are missing.  Should we add them?");
                            frm.AddText("Found missing ship to addresses.");
                            FollowMissingShipToPath(MissingShipTo, conceptCSV, frm);
                            frm.Visible = false;
                        }
                        else
                        {
                            // 6b.) All ship to addresses exist.  So, verify the item numbers are valid
                            log.Debug("Verify all items exist in JDE.");
                            frm.AddText("No missing ship to addresses.");
                            frm.AddText("Checking for missing item numbers.");
                            List<string> MissingItems = conceptCSV.CheckForMissingItemNumbers();
                            if (0 < MissingItems.Count)
                            {
                                log.Debug("There are item numbers that don't exist in JDE.");
                                using (new CenterDialog(this))
                                {
                                    MessageBox.Show($"The following part numbers don't exist in JDE.  Please change them or add them to the 3SUW branch plant: {String.Join(",", MissingItems)}",
                                                    "Data Error!!",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                                }
                                frm.Close();
                                return;
                            }
                            else
                            {
                                log.Debug("All items exist in JDE");
                                frm.AddText("All items exist in JDE.");
                                using (new CenterDialog(this))
                                {
                                    DialogResult result = MessageBox.Show($"All {conceptCSV.DT.Rows.Count} rows of data are valid in JDE.\r\nLoad the EDI Data into JDE?\r\nSelect No to preview the detail data before load.",
                                                                          "Load EDI Data?",
                                                                          MessageBoxButtons.YesNoCancel,
                                                                          MessageBoxIcon.Information);
                                    if (DialogResult.Cancel == result) { return; }

                                    // 7.) Save the data into a concept & save that to JDE
                                    log.Debug($"Tranform the conceptCSV into a concept");
                                    frm.AddText("Transforming the spreadsheet into a concept object.");
                                    Concept concept = XfrmConcept.CSVtoConcept(conceptCSV);
                                    if (DialogResult.Yes == result)
                                    {
                                        log.Debug($"Populating header file F47011 with data");
                                        frm.AddText("Populating the EDI header file with concept data.");
                                        JDE.PopulateF47011(concept);
                                        log.Debug($"Populating detail file F47012 with data");
                                        frm.AddText("Populating the EDI detail file with concept data & freight lines.");
                                        JDE.PopulateF47012(concept);
                                        frm.AddText("Success!");
                                        // 8.) Prompt the user to go to JDE
                                        log.Debug($"Successfully processed the EDI information into F47011 and F47012");
                                        MessageBox.Show("The concept was successfully loaded into JDE.\r\nPlease go to JDE, review the data, and run the Rollout Order Import report.",
                                                        "Success!",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information);
                                        frm.Close();
                                    }
                                    else if (DialogResult.No == result)
                                    {
                                        frm.AddText("Success!");
                                        frm.Close();
                                        this.dgv_DataDisplay.DataSource = concept.OrderDetails;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception er)
                {
                    log.Error($"{er.Message} + {er.InnerException} + {er.StackTrace}");
                    using (new CenterDialog(this))
                    {
                        MessageBox.Show($"{er.Message} + {er.InnerException} + {er.StackTrace}",
                                         "Error in Rollout",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error);
                    }
                }
            }
        } // LoadRolloutFlow

        /// <summary>
        /// The control flow for saving a Ship To CSV to a file
        /// </summary>
        private void SaveCSVFlow()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            log.Debug($"Attempting to save Ship To CSV to a file.");
            using (LoadingForm frm = new LoadingForm())
            {
                // 1.) Verify we have a ship to CSV
                if (null != MissingShipToCSV)
                {
                    frm.AddText("Get the Ship To filename to save as.");
                    frm.Visible = true;
                    // 2.) Get the filename to save
                    string fileName = FileIO.SaveFileName(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), FileFilter);
                    if (String.Empty != fileName)
                    {
                        // 3.) Save the file
                        MissingShipToCSV.FileName = fileName;
                        MissingShipToCSV.DeLimiter = "\t";
                        try
                        {
                            frm.AddText($"Saving {fileName} as TXT with a <tab> delimiter.");
                            MissingShipToCSV.WriteCSV();
                            frm.AddText($"Successfully saved {fileName}.");
                            this.dgv_DataDisplay.DataSource = null;
                            btn_SaveCSV.Enabled = false;
                            using (new CenterDialog(this))
                            {
                                MessageBox.Show($"SUCCESS!!\r\nFile {fileName} saved successfully.",
                                                 "File Saved Successfully",
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Information);
                                MissingShipToCSV = null;
                            }
                        }
                        catch (Exception er)
                        {
                            log.Error($"{er.Message} + {er.InnerException} + {er.StackTrace}");
                            using (new CenterDialog(this))
                            {
                                MessageBox.Show($"{er.Message} + {er.InnerException} + {er.StackTrace}",
                                                 "Error in Rollout",
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    log.Error($"We don't have a MissingShipToCSV.  Somehow we got here");
                    using (new CenterDialog(this))
                    {
                        MessageBox.Show($"ERROR: The program didn't populate the CSV before enabling this button.\r\nPlease contact IT support.",
                                         "Error in .NET program",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error);
                    }
                    btn_SaveCSV.Enabled = false;
                }
            }
        } // SaveCSVFlow

        /// <summary>
        /// The control flow for uploading and saving a Ship To CSV address book file.
        /// This data flow doesn't need the tax information.
        /// </summary>
        private void AddressBookFlow()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            log.Debug($"Beginning address book F0101 Z-File Load flow.");
            using (LoadingForm frm = new LoadingForm())
            {
                frm.Visible = true;
                MissingShipToCSV = null; // make sure to clear this out each time we load a spreadsheet
                // 1.) Get the file to load
                frm.AddText("Get file name to load.");
                string FileToLoad = FileIO.GetFileName(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), FileFilter);
                if (String.Empty != FileToLoad)
                {
                    try
                    {
                        // 2.) Load the file
                        log.Debug($"Attempting to load file {FileToLoad} using <tab> as a delimiter");
                        frm.AddText($"Attempting to load file {FileToLoad} using <tab> as a delimiter.");
                        MissingShipToCSV = new ShipToCSV(FileToLoad, "\t");
                        MissingShipToCSV.ReadShipTo();
                        frm.AddText($"Successfully read file {FileToLoad}.");
                        // 3.) Validate the required columns exist in the spreadsheet
                        log.Debug($"Attempting to validate all required columns exist in the file.");
                        frm.AddText("Validating required columns exist in the file.");
                        if (!ValidateShipToHeader(ref MissingShipToCSV)) { return; }
                        // 4.) Validate the data in the rows match the column requirements
                        frm.AddText("Validate data is in the correct format for each row and column.");
                        log.Debug("Validating all row data in all required columns");
                        if (!ValidateShipToRows(ref MissingShipToCSV, true)) { return; } // Allow blank TankAreaCode
                        // 5.) Either populate the data table for viewing or load the data into JDE
                        using (new CenterDialog(this))
                        {
                            DialogResult result = MessageBox.Show($"All {MissingShipToCSV.DT.Rows.Count} rows of data are valid.\r\nLoad the Data into JDE?\r\nSelect No to preview the detail data before load.",
                                                                  "Load Data?",
                                                                  MessageBoxButtons.YesNoCancel,
                                                                  MessageBoxIcon.Information);
                            if (DialogResult.Cancel == result) { return; }
                            
                            // 5a.) Save the data into a ShipTo & save that to JDE
                            log.Debug($"Tranform the ShipToCSV into a ShipTo object");
                            frm.AddText($"Converting CSV file to a JDE loadable object.");
                            ShipTo ship = XfrmShipTo.CSVToShipTo(MissingShipToCSV, false); // Don't look up the JDE address, because you don't have any yet

                            if (DialogResult.Yes == result)
                            {
                                log.Debug($"Populating F0101Z2 with data");
                                frm.AddText("Loading JDE F0101Z2 with data.");
                                JDE.PopulateF0101Z2(ship);
                                // 5b.) Prompt the user to go to JDE
                                log.Debug($"Successfully processed the Ship To information into F0101Z2 with batch number {ship.batch}.");
                                frm.AddText($"Successfully processed bactch number: {ship.batch}.");
                                using (new CenterDialog(this))
                                {
                                    MessageBox.Show($"Your BATCH NUMBER is: {ship.batch}\r\n\r\nThe ship to information was successfully loaded into JDE.\r\nPlease go to JDE, and run the address book load UBE.\r\nThen come back and load the customer master information.\r\n\r\n",
                                                    "Success!",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information);
                                }
                            }
                            else if (DialogResult.No == result)
                            {
                                frm.AddText("Successfully created JDE loadable object.");
                                this.dgv_DataDisplay.DataSource = ship.NewShipTos;
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        log.Error($"{er.Message} + {er.InnerException} + {er.StackTrace}");
                        using (new CenterDialog(this))
                        {
                            MessageBox.Show($"{er.Message} + {er.InnerException} + {er.StackTrace}",
                                             "Error in Rollout",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
                        }
                    }
                }
            }
            return;
        } // AddressBookFlow

        /// <summary>
        /// Control flow for the customer master data load
        /// </summary>
        private void CustomerMasterFlow()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            log.Debug($"Beginning customer master F03012 Z-File Load flow.");
            using(LoadingForm frm = new LoadingForm())
            {
                frm.Visible = true;
                MissingShipToCSV = null; // make sure to clear this out each time we load a spreadsheet
                // 1.) Get the file to load
                frm.AddText("Get file name to load.");
                string FileToLoad = FileIO.GetFileName(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), FileFilter);
                if (String.Empty != FileToLoad)
                {
                    try
                    {
                        // 2.) Load the file
                        log.Debug($"Attempting to load file {FileToLoad} using <tab> as a delimiter");
                        frm.AddText($"Attempting to load file {FileToLoad} using <tab> as a delimiter.");
                        MissingShipToCSV = new ShipToCSV(FileToLoad, "\t");
                        MissingShipToCSV.ReadShipTo();
                        frm.AddText($"Successfully read file {FileToLoad}.");
                        // 3.) Validate the required columns exist in the spreadsheet
                        log.Debug($"Attempting to validate all required columns exist in the file.");
                        frm.AddText("Validating required columns exist in the file.");
                        if (!ValidateShipToHeader(ref MissingShipToCSV)) { return; }
                        // 4.) Validate the data in the rows match the column requirements
                        frm.AddText("Validate data is in the correct format for each row and column.");
                        log.Debug("Validating all row data in all required columns");
                        if (!ValidateShipToRows(ref MissingShipToCSV, false)) { return; } // No Blank tax area codes allowed
                        // 5.) Either populate the data table for viewing or load the data into JDE
                        using (new CenterDialog(this))
                        {
                            DialogResult result = MessageBox.Show($"All {MissingShipToCSV.DT.Rows.Count} rows of data appear to be valid -- Further checks required.\r\nTry to Load the Data into JDE?\r\nSelect No to preview the detail data before load.",
                                                                  "Load Data?",
                                                                  MessageBoxButtons.YesNoCancel,
                                                                  MessageBoxIcon.Information);
                            
                            if (DialogResult.Cancel == result) { return; }

                            ShipTo ship = null;
                            // 5a.) Save the data into a ShipTo & save that to JDE
                            log.Debug($"Tranform the ShipToCSV into a ShipTo object");
                            frm.AddText($"Converting CSV file to a JDE loadable object.");
                            ship = XfrmShipTo.CSVToShipTo(MissingShipToCSV, true); // Get the JDE address, since we need it
                                                                                   // 5a.1) Verify there aren't any unfound addresses
                            log.Debug($"Checking that all JDE addresses are > 0");
                            frm.AddText($"Validating JDE Addresses were found for all rows.");
                            if (AnyZeroAddresses(ship)) { return; }
                            // 5a.2) Verify all tax codes are valid
                            log.Debug($"Verifying all tax codes are valid entries in F4008");
                            frm.AddText($"Validating Tax codes are active in JDE.");
                            if (!ValidateTaxCodes(ship)) { return; }
                            // 5a.2.a) Verify all tax code explanations are valid
                            log.Debug($"Verifying all tax code explanations are valid entries in UDC 00/EX");
                            frm.AddText($"Validating Tax Explanations are valid in JDE.");
                            if (!ValidateTaxExplanations(ship)) { return; }

                            if (DialogResult.Yes == result)
                            {
                                // 5a.3) Populate the Z file
                                log.Debug($"Populating F03012Z1 with data");
                                frm.AddText("Loading JDE F03012Z1 with data.");
                                JDE.PopulateF03012Z1(ship);
                                // 5b.) Prompt the user to go to JDE
                                log.Debug($"Successfully processed the Ship To information into F03012Z1 with batch number {ship.batch}.");
                                frm.AddText($"Successfully processed bactch number: {ship.batch}.");
                                using (new CenterDialog(this))
                                {
                                    MessageBox.Show($"Your BATCH NUMBER is: {ship.batch}\r\n\r\nThe CUSTOMER MASTER information was successfully loaded into JDE.\r\nPlease go to JDE, and run the CUSTOMER MASTER load UBE.\r\n",
                                                    "Success!",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information);
                                }
                            }
                            else if (DialogResult.No == result)
                            {
                                // 5c.3) Populate the datatable with concept information
                                frm.AddText("Successfully created JDE loadable object.");
                                this.dgv_DataDisplay.DataSource = ship.NewShipTos;
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        log.Error($"{er.Message} + {er.InnerException} + {er.StackTrace}");
                        using (new CenterDialog(this))
                        {
                            MessageBox.Show($"{er.Message} + {er.InnerException} + {er.StackTrace}",
                                             "Error in Rollout",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
                        }
                    }
                }
            }
            return;
        } // CustomerMasterFlow

        /// <summary>
        /// Control flow for Freight Data update
        /// </summary>
        public void FreightUpdateFlow()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            this.dgv_DataDisplay.DataSource = null;
            MissingShipToCSV = null; // make sure to clear this out each time we load a spreadsheet
            using (LoadingForm frm = new LoadingForm())
            {
                // 1.) Get the file to load
                string FileToLoad = FileIO.GetFileName(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), FileFilter);
                if (String.Empty != FileToLoad)
                {
                    try
                    {
                        // 2.) Load the file
                        log.Debug($"Attempting to load file {FileToLoad} using <tab> as a delimiter");
                        frm.AddText($"Attempting to load file {FileToLoad} using <tab> as a delimiter.");
                        freightCSV = new FreightCSV(FileToLoad, "\t");
                        freightCSV.ReadFreight();
                        frm.AddText($"Successfully read file {FileToLoad}.");
                        // 3.) Validate the required columns exist in the spreadsheet
                        log.Debug($"Attempting to validate all required columns exist in the file.");
                        frm.AddText("Validating required columns exist in the file.");
                        if (!ValidateFreightHeaders(ref freightCSV)) { return; }
                        // 4.) Validate the data in the rows match the column requirements
                        frm.AddText("Validate data is in the correct format for each row and column.");
                        log.Debug("Validating all row data in all required columns");
                        if (!ValidateFreightRows(ref freightCSV)) { return; }
                        // 5.) Either populate the data table for viewing or load the data into JDE
                        using (new CenterDialog(this))
                        {
                            DialogResult result = MessageBox.Show($"All {freightCSV.DT.Rows.Count} rows of data appear to be valid -- Further checks required.\r\nTry to Load the Data into JDE?\r\nSelect No to preview the detail data before load.",
                                                                  "Load Data?",
                                                                  MessageBoxButtons.YesNoCancel,
                                                                  MessageBoxIcon.Information);
                            if (DialogResult.Cancel == result) { return; }

                            // 6.) Save the data into a Freight structure & save that to JDE
                            log.Debug($"Tranform the FreightCSV into a Freight object");
                            frm.AddText($"Converting CSV file to a JDE loadable object.");
                            Freight freight = XfrmFreight.CSVToFreight(freightCSV);
                            // 7.) Validate all shipments exist
                            log.Debug($"Validating that there aren't any 0 shipment numbers");
                            frm.AddText($"Validating that all shipments exist");
                            if (freight.freight_lines.Any(n => 0 == n.shipment)) { return; }

                            if (DialogResult.Yes == result)
                            {
                                log.Debug($"Update Shipment Reference Numbers in F4217");
                                frm.AddText($"Updating Shipment Tracking Numbers in JDE");
                                JDE.PopulateF4217(freight);
                                log.Debug($"Update Package Information in F4943");
                                frm.AddText($"Updating package information in JDE");
                                JDE.PopulateF4943(freight);
                                log.Debug("$Updating freight prices in F4211");
                                frm.AddText($"Updating freight prices on sales orders");
                                JDE.UpdateFreightF4211(freight);
                                using (new CenterDialog(this))
                                {
                                    MessageBox.Show($"The Freight information was successfully loaded into JDE.\r\n",
                                                    "Success!",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information);
                                }
                            }
                            else if (DialogResult.No == result)
                            {
                                this.dgv_DataDisplay.DataSource = freight.freight_lines;
                            }
                        } // using
                    } // try
                    catch (Exception er)
                    {
                        log.Error($"{er.Message} + {er.InnerException} + {er.StackTrace}");
                        using (new CenterDialog(this))
                        {
                            MessageBox.Show($"{er.Message} + {er.InnerException} + {er.StackTrace}",
                                             "Error in Freight Update",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
                        }
                    } // catch
                }               
            } 
            return;
        } // FreightUpdateFlow
        #endregion

        #region buttons
        /// <summary>
        /// Perform this flow when the Load Rollout button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadRollout_DoIt(object sender, EventArgs e)
        {
            LoadRolloutFlow();
            return;
        }

        /// <summary>
        /// Perform this flow when the Save CSV button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveCSV_DoIt(object sender, EventArgs e)
        {
            SaveCSVFlow();
            return;
        }

        /// <summary>
        /// Perform this flow when the Load Address Book button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadAddressBook_DoIt(object sender, EventArgs e)
        {
            this.dgv_DataDisplay.DataSource = null;
            AddressBookFlow();
            return;
        }

        /// <summary>
        /// Perform this flow when the Load Customer Address button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadCustMast_DoIt(object sender, EventArgs e)
        {
            this.dgv_DataDisplay.DataSource = null;
            CustomerMasterFlow();
            return;
        }

        /// <summary>
        /// Perform the Freight Update Flow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FreightUpdate_DoIt(object sender, EventArgs e)
        {
            this.dgv_DataDisplay.DataSource = null;
            FreightUpdateFlow();
            return;
        } // btn_FreightUpdate_DoIt

        /// <summary>
        /// Perform this flow when the close application button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DoClose(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(FileIO)).Location) + @"\" + "log4net.config"));
            log.Debug("Closing Application");
            this.Close();
            return;
        }
        #endregion
    }
}
