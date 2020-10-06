using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Rollout.EF;
using Rollout.Common;

namespace Rollout.BLL
{
    public static class JDE
    {
        #region PrivateMembers
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Populate a single F4217 record
        /// </summary>
        /// <param name="freightLine"></param>
        /// <returns></returns>
        private static F4217 PopulateF4217Detail(FreightLine freightLine)
        {
            F4217 val = new F4217();
            /* Set the audit data */
            val.XIUPMJ = CommonFunctions.DateStringToJulian(DateTime.Today.ToString());
            val.XITDAY = CommonFunctions.TimeToJDETime(DateTime.Now);
            val.XIPID = "Rollout";
            val.XIJOBN = "Rollout";
            val.XIUSER = "Rollout";
            /* Set the actual data */
            val.XISHPN = freightLine.shipment;
            val.XIRSSN = 1;
            val.XISQNR = 1; /* We can only process a single box */
            val.XIREFQ = "CN";
            val.XIREFN = freightLine.trackingNumber;
            return val;
        } // PopulateF4217Detail

        /// <summary>
        /// Populate a single F4943 record
        /// </summary>
        /// <param name="freightLine"></param>
        /// <returns></returns>
        private static F4943 PopulateF4943Detail(FreightLine freightLine)
        {
            F4943 val = new F4943();
            /* Set the audit data */
            val.SPUPMJ = CommonFunctions.DateStringToJulian(DateTime.Today.ToString());
            val.SPTDAY = CommonFunctions.TimeToJDETime(DateTime.Now);
            val.SPPID = "Rollout";
            val.SPJOBN = "Rollout";
            val.SPUSER = "Rollout";
            /* Set the actual data */
            val.SPSHPN = freightLine.shipment;
            val.SPRSSN = 0;
            val.SPPLT = " ";
            val.SPOSEQ = 1; /* We can only process a single box */
            val.SPEQTY = "BOX1";
            val.SPLGTS = 0;
            val.SPWTHS = 0;
            val.SPHGTS = 0;
            val.SPGTHS = 0;
            val.SPLUOM = "IN";
            val.SPCVUM = "FC";
            val.SPGWEI = Math.Round(freightLine.weight * 100);
            val.SPWTUM = "LB";
            val.SPVCUD = 0;
            val.SPREFN = freightLine.trackingNumber;
            val.SPREFQ = "CN";
            return val;
        } // PopulateF4217Detail

        /// <summary>
        /// Populate the non-entry specific fields in the F0101Z2 record
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="batch"></param>
        private static void PopulateUnchangingF0101Z2(ref F0101Z2 entry, string batch)
        {
            
            entry.SZEDUS = "808718";     // Ray Lillback (for now)
            entry.SZEDBT = batch;        // 32 bit integer
            entry.SZEDSP = "";           // Let the successfully processed record be empty
            entry.SZTNAC = "A";          // Add the record
            entry.SZMCU = "";            // Leaving this empty defaults the Business unit to 1
            entry.SZTAX = "";
            entry.SZAT1 = "S3";          // Suwanee ship to
            entry.SZDRIN = "0";          // Inbound processing
            entry.SZTYTN = "";           // Use default Transaction type in P0100041, if defined
            entry.SZEDDL = null;         // Don't provide information on detail lines
            entry.SZPNID = "Rollout";    // Process ID
            entry.SZAN8 = null;          // Use the next number to define the value
            entry.SZSIC = "";            // We don't use the SIC codes
            entry.SZLNGP = "";           // English
            entry.SZCM = "";             // Credit message isn't needed for ship to addresses
            entry.SZTAXC = "C";          // If someone enters a tax-id, print it out in EIN format
            entry.SZAT2 = "N";           // No AR/AP netting
            entry.SZAT3 = "";            // Reserved
            entry.SZAT4 = "";            // Reserved
            entry.SZATR = "Y";           // This is a customer record
            entry.SZAT5 = "";            // Reserved
            entry.SZATP = "N";           // Not a supplier
            entry.SZATPR = "";           // We are not using this data dictionary reference.
            entry.SZAB3 = "";            // Reserved
            entry.SZATE = "N";           // Not an employee
            entry.SZSBLI = "";           // We default this to blank at Kinetico
            entry.SZEFTB = CommonFunctions.DateStringToJulian(System.DateTime.Today.ToString());       // Just set the active date to Today
            entry.SZAN81 = null;         // This needs to be the ship-to, AN82/83/85 and PA8 are all the parent address number
            entry.SZAN84 = null;
            entry.SZAN86 = null;
            entry.SZAC01 = "SUW";        // Used to designate a Suwanee customer
            entry.SZAC02 = "SUW";        // Used in datawarehouse reports showing revenue
            entry.SZAC03 = "";           // Not used
            entry.SZAC04 = "";           // Not used
            entry.SZAC05 = "A";          // Active customer
            entry.SZAC06 = "";           // Not used
            entry.SZAC07 = "";           // Not used
            // AC08 is for the concept we are using
            entry.SZAC09 = "";           // Not used
            entry.SZAC10 = "";           // Not used
            entry.SZAC11 = "";           // Not used
            entry.SZAC12 = "";           // Not used
            entry.SZAC13 = "";           // Not used
            entry.SZAC14 = "";           // Not used
            entry.SZAC15 = "";           // Not used
            entry.SZAC16 = "";           // Not used
            entry.SZAC17 = "";           // Not used
            entry.SZAC18 = "";           // Not used
            entry.SZAC19 = "";           // Not used
            entry.SZAC20 = "";           // Not used
            entry.SZAC21 = "";           // Not used
            entry.SZAC22 = "";           // Not used
            entry.SZAC23 = "";           // Not used
            entry.SZAC24 = "";           // Not used
            entry.SZAC25 = "";           // Not used
            entry.SZAC26 = "";           // Not used
            entry.SZAC27 = "";           // Not used
            entry.SZAC28 = "";           // Not used
            entry.SZAC29 = "";           // Not used
            entry.SZAC30 = "";           // Not used
            entry.SZGLBA = "";           // Sold-to don't need bank accounts
            entry.SZPTI = null;          // Don't need time in
            entry.SZPDI = null;          // Don't need date in
            entry.SZMSGA = "N";          // Only display current messages
            entry.SZRMK = null;          // This isn't used anywhere
            entry.SZTXCT = "";           // Ship-to doesn't need tax ex cert
            entry.SZTX2 = "";            // Ship-to doesn't need tax extra
            entry.SZALP1 = "";           // Name for double byte encoding
            entry.SZURCD = "";           // User defined code Not used
            entry.SZURDT = 0;            // User defined code Not used
            entry.SZURAT = 0;            // User defined code Not used
            entry.SZURAB = 0;            // User defined code Not used
            entry.SZURRF = "";           // User defined code Not used
            entry.SZMLNM = "";           // If the mailing name is blank, it uses SZALPH
            entry.SZMLN1 = "";           // 2ndary mailing name
            entry.SZADD3 = "";           // Not used for ship to
            entry.SZADD4 = "";           // Not used for ship to
            entry.SZCTR = "US";          // Can we default the country to US?
            entry.SZAR1 = "";            // We aren't importing the phone numbers
            entry.SZPH1 = "";            // We aren't importing the phone numbers
            entry.SZPHT1 = "";           // We aren't importing the phone numbers
            entry.SZAR2 = "";            // We aren't importing the phone numbers
            entry.SZPH2 = "";            // We aren't importing the phone numbers
            entry.SZPHT2 = "";           // We aren't importing the phone numbers
            entry.SZTICKER = "";         // Stock ticker
            entry.SZEXCHG = "";          // Stock exchange traded on
            entry.SZDUNS = "";           // DUNS number
            entry.SZCLASS01 = "";        // Not used
            entry.SZCLASS02 = "";        // Not used
            entry.SZCLASS03 = "";        // Not used
            entry.SZCLASS04 = "";        // Not used
            entry.SZCLASS05 = "";        // Not used
            entry.SZNOE = 0;             // Not using # of employees
            entry.SZGROWTHR = 0;         // Company growth rate
            entry.SZYEARSTAR = "";       // Don't care about starting year 
            entry.SZAEMPGP = "";         // We are using CSS so leave blank
            entry.SZACTIN = "";          // Reserved
            entry.SZREVRNG = "";         // Not using revenue range
            /* The following fields are ignored, but we need them for EF to work */
            entry.SZEDCT = "";
            entry.SZEDFT = "";
            entry.SZEDDT = 0;
            entry.SZDC = "";
            entry.SZTORG = "";
            entry.SZUSER = "";
            entry.SZPID = "";
            entry.SZJOBN = "";
            entry.SZUPMJ = 0;
            entry.SZTDAY = 0;
            entry.SZUPMT = 0;
            /* The following are in the file, but not documented by JDE.  Set a default */
            entry.SZSCCLTP = "";          // Reserved for future
            entry.SZPRGF = "N";           // Purge Flag
            return;
        }

        /// <summary>
        /// Populate the customer specific fields in the F0101Z2 record
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="line"></param>
        /// <param name="ParentAddress"></param>
        /// <param name="StoreName"></param>
        /// <param name="ConceptID"></param>
        /// <param name="transaction"></param>
        private static void PopulateLineF0101Z2(ref F0101Z2 entry, ShipToLine line, double ParentAddress, string StoreName, string ConceptID, decimal transaction)
        {
            entry.SZEDTN = transaction.ToString();
            entry.SZEDLN = 1;

            string tempString = line.StoreNumber + "-" + line.Concept.ToUpper();
            log.Debug($"Assigning {tempString} to SZALKY");
            if ( 20 < tempString.Length )
            {
                entry.SZALKY = tempString.Substring((tempString.Length - 20), 20); // Take rightmost 20 
                log.Info($"Warning: Truncated {tempString} to {entry.SZALKY} for SZALKY");
            }
            else
            {
                entry.SZALKY = tempString;
            }

            tempString = StoreName.ToUpper() +" STORE #" + line.StoreNumber.ToString();
            log.Debug($"Assinging {tempString} to SZALPH");
            if ( 40 < tempString.Length )
            {
                entry.SZALPH = tempString.Substring(tempString.Length - 40, 40); // Take rightmost 40
                log.Info($"Warning: Truncated {tempString} to {entry.SZALPH} for SZALPH");
            }
            else
            {
                entry.SZALPH = tempString;
            }

            tempString = line.Address1.ToUpper();
            log.Debug($"Assigning {tempString} to SZADD1");
            if ( 40 < tempString.Length )
            {
                entry.SZADD1 = tempString.Substring(0, 40);
                log.Info($"Warning: Truncated {tempString} to {entry.SZADD1} for SZADD1");
            }
            else
            {
                entry.SZADD1 = tempString;
            }

            tempString = line.Address2.ToUpper();
            log.Debug($"Assigning {tempString} to SZADD2");
            if (40 < tempString.Length)
            {
                entry.SZADD2 = tempString.Substring(0, 40);
                log.Info($"Warning: Truncated {tempString} to {entry.SZADD2} for SZADD2");
            }
            else
            {
                entry.SZADD2 = tempString;
            }

            entry.SZADDZ = line.Zip;
            entry.SZADDS = line.State;
            entry.SZCTY1 = line.City;
            entry.SZCOUN = line.County;
            
            entry.SZAN82 = ParentAddress;
            entry.SZAN83 = ParentAddress;
            entry.SZAN85 = ParentAddress;
            entry.SZPA8 = ParentAddress;

            entry.SZAC08 = line.Concept; // This is where we are placing the concept
            return;
        }

        /// <summary>
        /// Populate a single F03012Z1 entry with data that doesn't change per line
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="batch"></param>
        private static void PopulateUnchangingF03012Z1(ref F03012Z1 entry, string batch)
        {
            entry.VOEDUS = "808718";        //TODO: Change this from Ray Lillback
            entry.VOEDBT = batch;           // Unique batch number
            entry.VOEDLN = 1;               // We are going to enter each record uniquely in a single item.
            entry.VOEDSP = "";              // Leave blank - is set to Y for a successfully processed record.
            entry.VOTNAC = "A";             // We are adding these to JDE, so action code = A
            entry.VOCO = "03000";           // Hard code the company??
            entry.VOCRCA = "USD";           // Hard code the currency code??
            entry.VOTYTN = "";              // Use the Customer Master MBF processing option (P0100042)
            entry.VODRIN = "0";             // Inbound direction
            entry.VOEDDL = 1;               // Informational only, so set this to a default value
            entry.VOPNID = "";              // Trading Partner ID
            entry.VOARC = "";               // GL Offset.  
            entry.VOMCUR = "";              // Business Unit - A/R Default
            entry.VOOBAR = "";              // Object - A/R Default.  
            entry.VOAIDR = "";              // Subsidiary - A/R Default. What do we populate here ????
            entry.VOKCOR = "";              // Document Company - A/R Model.  
            entry.VODCAR = null;            // Document Number - A/R Model.  
            entry.VODTAR = "";              // Document Type - A/R Model.  
            entry.VOCRCD = "USD";           // Currency Code
            entry.VOACL = null;             // Amount - Credit Limit.  
            entry.VOHDAR = "";              // Hold Invoices.  
            entry.VOTRAR = "";              // Payment Terms. 
            entry.VOSTTO = "P";             // Send Statement To.  
            entry.VORYIN = "";              // Payment Instrument.  Must exist in 00/PY & since this is a ship to, "" should work
            entry.VOSTMT = "N";             // Print Statement.  
            entry.VOATCS = "N";             // Auto Receipt.
            entry.VOSITO = "P";             // Send invoice to Parent.
            entry.VOCYCN = "";              // Statement cycle.  Kinetico uses the default value here.
            entry.VOTSTA = "";              // Temporary Credit Message.  Must exist in UDC 01/CM
            entry.VODLC = null;             // Date of Last Credit Review.
            entry.VODNLT = "";              // Delinquency Notice
            entry.VOPLCR = "";              // Person completing last credit review
            entry.VORVDJ = null;            // Date - Recall for Review
            entry.VOCMGR = "";              // Credit Manager
            entry.VOCLMG = "";              // Collection Manager
            entry.VOCOLL = "";              // Collection Report
            entry.VOAFC = "";               // Apply Finance Charges
            entry.VODT1J = null;            // Last Statement Date
            entry.VODFIJ = null;            // First Invoice Date
            entry.VODLIJ = null;            // Last Invoice Date
            entry.VODLP = null;             // Date Last Paid
            entry.VODB = "";                // Dun & Bradstreet Rating
            entry.VODNBJ = null;            // Dun & Bradstreet Date
            entry.VOTRW = "";               // TRW Rating
            entry.VOTWDJ = null;            // TRW Date
            entry.VOAD = 0;                 // Reserved for future use
            entry.VOAFCP = 0;               // Amount - Prior Year Finance Charge
            entry.VOAFCY = 0;               // Amount - YTD Finance Charges
            entry.VOASTY = 0;               // Amount Invoiced this Year
            entry.VOSPYE = 0;               // Amount Invoiced
            entry.VOALP = 0;                // Amount Last Applied
            entry.VOPOPN = "";              // Person Opening Account
            entry.VODAOJ = CommonFunctions.DateStringToJulian(System.DateTime.Today.ToShortDateString()); // Date - Account Opened
            entry.VOPLY = null;             // Policy Number (Internal)
            entry.VOMAN8 = null;            // Deduction Manager
            entry.VOARL = "";               // Auto Receipts Execution List
            entry.VOAC01 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC02 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC03 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC04 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC05 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC06 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC07 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC08 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC09 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC10 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC11 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC12 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC13 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC14 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC15 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC16 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC17 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC18 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC19 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC20 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC21 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC22 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC23 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC24 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC25 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC26 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC27 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC28 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC29 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOAC30 = "";              // This doesn't update ABAC01 - 30, Sleeper Updates Them
            entry.VOCUSTS = "0";            // Customer Status 0 = Active 1 = Inactive (For CRM)
            entry.VOEDPM = "P";             // Allow batch processing for this customer
            return;
        }

        /// <summary>
        /// Populate a single F03012Z1 entry with line-specific data
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ParentAddress"></param>
        /// <param name="line"></param>
        /// <param name="transaction"></param>
        private static void PopulateLineF03012Z1(ref F03012Z1 entry, double ParentAddress, ShipToLine line, decimal transaction)
        {
            entry.VOAN8 = line.JDEAddress;              // AIAN8
            entry.VOTXA1 = line.TaxAreaCode;            // Tax Area Code must exist in F4008
            entry.VOEDTN = transaction.ToString();      // Transaction number
            entry.VOARPY = ParentAddress;               // Alternate Payor
            entry.VOEXR1 = line.TaxExplanationCode;     // Tax Explanation Code.  Must exist in UDC 00/EX
            return;
        }

        /// <summary>
        /// Populate a single F47011 entry with required header data, leave all the rest as default
        /// </summary>
        /// <param name="line"></param>
        /// <param name="concept"></param>
        /// <returns></returns>
        private static F47011 PopulateSingleF47011(ConceptLine line, Concept concept)
        {
            F47011 EdiHeader = new F47011();
            EdiHeader.SYEDOC = line.DocumentNumber;         // Order Number
            EdiHeader.SYEDCT = "RO";                        // Rollout Order
            EdiHeader.SYEKCO = "03000";                     // Don't really need this, but set it to Newbury
            EdiHeader.SYEDST = "850";                       // 850 = New Sales Order
            EdiHeader.SYEDER = "R";                         // Receive/Inbound Transaction
            EdiHeader.SYTPUR = "00";                        // Transaction Set Purpose = Add
            EdiHeader.SYSHAN = line.ShipToAddress;          // Can only populate AN8 or SHAN, not both
            EdiHeader.SYMCU = line.BranchPlant;             // Branch/Plant for revenue
            EdiHeader.SYDRQJ = line.JulianRequestedDate;    // Requested Date
            EdiHeader.SYPDDJ = line.JulianRequestedDate;    // Promised Date
            EdiHeader.SYVR01 = concept.PONumber;            // Purchase order number on the sales order
            EdiHeader.SYVR02 = concept.PONumber;            // Populate the PO number here too
            EdiHeader.SYORBY = concept.OrderedBy;           // Who placed the order with Selecto
            EdiHeader.SYCARS = concept.ShippingVendor;      // JDE Address of the shipping vendor
            EdiHeader.SYMOT = concept.ShippingMode;         // JDE Method Of Transport (Mode of Transport)
            EdiHeader.SYEDSP = "N";                         // Say this line isn't processed - used in data selection
            return EdiHeader;
        }

        /// <summary>
        /// Populate a single F47012 entry with required data
        /// </summary>
        /// <param name="line"></param>
        /// <param name="PONumber"></param>
        /// <returns></returns>
        private static F47012 PopulateOrderDetailF47012(ConceptLine line, string PONumber)
        {
            F47012 OrderLine = new F47012();
            OrderLine.SZEDOC = line.DocumentNumber;         // Must be the same as the order header.  Part of the 1->M relationship.
            OrderLine.SZEDCT = "RO";                        // Must be the same as the order header.  Part of the 1->M relationship.
            OrderLine.SZEKCO = "03000";                     // Must be the same as the order header.  Part of the 1->M relationship.
            OrderLine.SZEDLN = (double)line.LineNumber * 1000; // Line Number in JDE format of 1.000 = 1000
            OrderLine.SZSHAN = line.ShipToAddress;          // Must be same as header
            OrderLine.SZUORG = line.Quantity;               // How many to order
            OrderLine.SZLITM = line.JDEPartNumber;          // The item to order
            OrderLine.SZEDST = "850";                       // 850 = New Sales Order
            OrderLine.SZEDER = "R";                         // Receive (Inbound) Transaction
            OrderLine.SZMCU = line.BranchPlant;             // Branch plant to commit into
            OrderLine.SZDRQJ = line.JulianRequestedDate;    // Requested Date
            OrderLine.SZPDDJ = line.JulianRequestedDate;    // Promised Date
            OrderLine.SZVR01 = PONumber;                    // Purchase Order Reference
            OrderLine.SZVR02 = PONumber;                    // Place the PO here too for batch ship confirm
            OrderLine.SZEDSP = "N";                         // Say this line isn't processed - used in data selection
            return OrderLine;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Verify that a string of tax explanation codes exist in UDC 00/EX
        /// </summary>
        /// <param name="expl"></param>
        /// <returns></returns>
        public static bool ValidateTaxExplanations(List<string> expl)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            List<string> unique = expl.Distinct().ToList();
            log.Debug($"Validating Explanation Codes: {String.Join(",", unique)}");
            bool result = false;
            try
            {
                List<string> missing = new List<string>();
                List<string> found;
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    found = jde.F0005.AsNoTracking()
                            .Where(n => ("00" == n.DRSY.Trim()) && ("EX" == n.DRRT.Trim()) && (unique.Contains(n.DRKY.Trim())))
                            .Select(n => n.DRKY.Trim())
                            .ToList();
                    log.Debug($"Found these explanation codes in UDC 00/EX = {String.Join(",", found)}");
                    missing = unique.Except(found).ToList();
                    if (0 < missing.Count)
                    {
                        log.Error($"These tax explanation codes are missing from UDC 00/EX = {String.Join(",", missing)}");
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return result;
        }
        /// <summary>
        /// Validate a list of tax codes exist in F4008 and are not expired tax codes
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public static bool ValidateTaxCodes(List<string> codes)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            List<string> exceptions = new List<string>(); // list of exceptions that don't need to exist in F4008
            exceptions.Add(String.Empty); // The null tax code is acceptable but doesn't exist in F4008
            List<string> uniqueCodes = codes.Distinct().Except(exceptions).ToList();
            log.Debug($"Validating Tax Codes: {String.Join(",", uniqueCodes)}");
            bool result = false;
            try
            {
                // Make sure all the codes exist in F4008
                List<string> missing = new List<string>();
                List<string> found;
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    found = jde.F4008.AsNoTracking()
                            .Where(n => uniqueCodes.Contains(n.TATXA1.Trim()))
                            .Select(n => n.TATXA1.Trim())
                            .ToList();
                    log.Debug($"Found these tax codes in F4008 = {String.Join(",", found)}");
                    missing = uniqueCodes.Except(found).ToList();
                    if (0 < missing.Count)
                    {
                        log.Error($"These tax codes are missing from F4008 = {String.Join(",", missing)}");
                        result = false;
                    }
                    else
                    {
                        // Now get a list off all the active tax codes in F4008
                        decimal today = CommonFunctions.DateStringToJulian(System.DateTime.Today.ToShortDateString());
                        found = jde.F4008.AsNoTracking()
                                .Where(n => (uniqueCodes.Contains(n.TATXA1.Trim()) && ((today <= n.TAEFDJ) && (today >= n.TAEFTJ))))
                                .Select(n => n.TATXA1.Trim())
                                .ToList();
                        log.Debug($"These tax codes are active in F4008 = {String.Join(",", found)}");
                        missing = uniqueCodes.Except(found).ToList();
                        log.Error($"These tax codes are not active in F4008 = {String.Join(",", missing)}");
                        if (0 < missing.Count)
                        {
                            result = false;
                        }
                        else
                        {
                            log.Debug($"All tax codes are active & the file is ok to process further.");
                            result = true; 
                        }
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return result;
        }
        /// <summary>
        /// Get the county from a zipcode via lookup in F0117
        /// </summary>
        /// <param name="zipcode"></param>
        /// <returns>Returns county or empty if no county found</returns>
        public static string GetCounty(string zipcode)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Getting county from F0117 for zip code = {zipcode}");

            string county;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    county = jde.F0117.AsNoTracking().Where(n => n.A8ADDZ.Trim() == zipcode.ToUpper().Trim()).Select(n => n.A8COUN).FirstOrDefault();
                    log.Debug($"A8ADDZ = {zipcode} returned {county} from F0117");
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return county;
        }

        /// <summary>
        /// Grab the SUW RO Batch next number from JDE
        /// Where NNSY = 47 & we use the custom next number of NNN007
        /// </summary>
        /// <returns>The Next number</returns>
        public static double GetNextBatchNumber()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Getting next batch number");

            double nn;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    F0002 record = jde.F0002.Where(n => "47" == n.NNSY).First();
                    nn = (double)record.NNN007;
                    record.NNN007++;
                    jde.SaveChanges();
                }
                log.Debug($"Successfully retreived batch number = {nn.ToString()}");
            }
            catch (Exception eJDE)
            {
                log.Error($"Error in batch next number retrieval from F0002. {eJDE}");
                throw;
            }
            return nn;
        }

        /// <summary>
        /// Reserve an amount of records 
        /// </summary>
        /// <param name="AmountToGet"></param>
        /// <returns>The first next number to use</returns>
        public static double GetDocumentNumbers(double AmountToGet)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Reserving {AmountToGet.ToString()} document numbers");

            double nn;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    F0002 record = jde.F0002.Where(n => "47" == n.NNSY).First();
                    nn = (double)record.NNN006;
                    record.NNN006 = nn + AmountToGet;
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"Error in reserving document numbers from F0002. {eJDE}");
                throw;
            }
            return nn;
        }

        /// <summary>
        /// Get the ABAC08 field for a concept by ABAN8
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public static string GetConceptID ( double Address )
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Retrieving concept ID for F0101.ABAN8 = {Address.ToString()}");

            string conceptID = String.Empty;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    if ( jde.F0101.AsNoTracking().Any(n => n.ABAN8 == Address))
                    {
                        conceptID = jde.F0101.AsNoTracking()
                                   .Where(n => n.ABAN8 == Address)
                                   .Select(n => n.ABAC08).First().ToString().Trim();
                        log.Debug($"Found {conceptID} as concept ID for F0101.ABAN8 = {Address.ToString()}");
                    }
                    else if ( jde.F0101.AsNoTracking().Any(n => n.ABALKY == Address.ToString()))
                    {
                        conceptID = jde.F0101.AsNoTracking()
                                    .Where(n => n.ABALKY == Address.ToString())
                                    .Select(n => n.ABAC08).First().ToString().Trim();
                        log.Debug($"Found {conceptID} as concept ID for F0101.ABLKY = {Address.ToString()}");
                    }
                    else
                    {
                        log.Error($"Cannot find F0101.ABAN8 = {Address.ToString()}");
                        log.Error($"Cannot find F0101.ABALKY = {Address.ToString()}");
                        throw new System.ArgumentException($"Error {Address.ToString()} isn't in JDE.");
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;                
            }          
            return conceptID;
        } // GetConceptID float is passed

        /// <summary>
        /// Get the ABAC08 field for a concept by ABALKY
        /// </summary>
        /// <param name="Nickname"></param>
        /// <returns></returns>
        public static string GetConceptID ( string Nickname )
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Retrieving concept ID for F0101.ABALKY = {Nickname}");

            string conceptID = String.Empty;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    if (jde.F0101.AsNoTracking().Any(n => n.ABALKY == Nickname))
                    {
                        conceptID = jde.F0101.AsNoTracking()
                                    .Where(n => n.ABALKY == Nickname)
                                    .Select(n => n.ABAC08).First().ToString().Trim();
                        log.Debug($"Found {conceptID} as concept ID for F0101.ABLKY = {Nickname}");
                    }
                    else
                    {
                        log.Error($"Cannot find F0101.ABALKY = {Nickname}");
                        log.Error($"Cannot find F0101.ABAN8 = {Nickname}");
                        return String.Empty;
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return conceptID;
        }

        /// <summary>
        /// For a concept id (ABAC08), find the ABAN8 C3 record that contians the concept
        /// </summary>
        /// <param name="ConceptId"></param>
        /// <returns></returns>
        public static double? GetParentFromConcept ( string ConceptId )
        {
            double? parent = null;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Retrieving Parent Address for concept ID = {ConceptId}");

            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    if (jde.F0101.AsNoTracking().Any(n => ((n.ABAC08 == ConceptId) && (n.ABAT1 == "C3"))))
                    {
                        parent = jde.F0101.AsNoTracking()
                                   .Where(n => ((n.ABAC08 == ConceptId) && (n.ABAT1 == "C3")))
                                   .Select(n => n.ABAN8).First();
                    }
                    else
                    {
                        log.Error($"Cannot find F0101.ABAN8 for F0101.ABAT1 = C3 and F0101.ABAC08 = {ConceptId}");
                        return null;
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            log.Debug($"Found {parent} as parent address for concept ID = {ConceptId}");
            return parent;
        }

        /// <summary>
        /// Return a list of SoldTo ABALKYs that aren't in JDE already
        /// </summary>
        /// <param name="Names"></param>
        /// <returns></returns>
        public static List<string> FindMissingShipTos( List<string> Names )
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Checking for existance of these names in JDE = {String.Join(",",Names)}");

            List<string> missing = new List<string>();
            List<string> found;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    found = jde.F0101.AsNoTracking()
                        .Where(n => Names.Contains(n.ABALKY.Trim()))
                        .Select(n => n.ABALKY.Trim())
                        .ToList();                  
                }
                log.Debug($"Found these addresses = {String.Join(",",found)}");
                missing = Names.Except(found).ToList();
                if (0 < missing.Count)
                {
                    log.Error($"These names are missing = {String.Join(",", missing)}");
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return missing;
        } // FindMissingShipTos

        /// <summary>
        /// Check if a single Address number exists in JDEs Address Book
        /// </summary>
        /// <param name="addressNumber"></param>
        /// <returns></returns>
        public static bool DoesAddressExist(double addressNumber, string searchType)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Checking for existence of address book number = {addressNumber}");

            bool exists;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    if (jde.F0101.AsNoTracking().Any(n => (n.ABAN8 == addressNumber) && (n.ABAT1 == searchType)))
                    {
                        exists = true;
                        log.Debug($"Found {addressNumber}");
                    }
                    else
                    {
                        exists = false;
                        log.Error($"Did not find {addressNumber}");
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return exists;
        }

        /// <summary>
        /// Return a string of missing branch plant items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="branchplant"></param>
        /// <returns></returns>
        public static List<string> GetMissingItems(List<string> items, string branchplant)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Checking branch plant {branchplant} for existence of these part numbers = {String.Join(",",items)}");

            List<string> missing = new List<string>();
            List<string> found;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    found = jde.F4102.AsNoTracking()
                            .Where(n => branchplant.Trim() == n.IBMCU.Trim() &&
                                        items.Contains(n.IBLITM))
                            .Select(n => n.IBLITM.Trim())
                            .ToList();
                }
                log.Debug($"Found the following items in the {branchplant} branch: {String.Join(",", found)}");
                missing = items.Except(found).ToList();
                if (0 < missing.Count)
                {
                    log.Error($"The following items were not found: {String.Join(",", missing)}");
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return missing;
        }

        /// <summary>
        /// Check if a single Item number exists in JDEs Item Branch file
        /// </summary>
        /// <param name="partnumber"></param>
        /// <param name="branchplant"></param>
        /// <returns></returns>
        public static bool DoesItemExist(string partnumber, string branchplant)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Checking for existence of part number = {partnumber} in branch = {branchplant}");

            bool exists;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    if (jde.F4102.AsNoTracking().Any(n => partnumber == n.IBLITM && 
                                                          branchplant.Trim() == n.IBMCU.Trim()))
                    {
                        exists = true;
                    }
                    else
                    {
                        exists = false;
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return exists;
        }

        /// <summary>
        /// Get the JDE Address number by field ABALKY
        /// </summary>
        /// <param name="alky"></param>
        /// <returns></returns>
        public static double? GetAddressFromALKY(string alky)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Getting JDE Address where ABALKY = {alky}");

            double? address;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    address = jde.F0101.AsNoTracking().Where(n => n.ABALKY.Trim() == alky.Trim()).Select(n => n.ABAN8).FirstOrDefault();
                    log.Debug($"ABALKY = {alky} returned {address.ToString()} from F0101");
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return address;
        }

        /// <summary>
        /// Populate the JDE F0101Z2 file from the ShipTo construct
        /// </summary>
        /// <param name="DT"></param>
        public static void PopulateF0101Z2(ShipTo DT)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating F0101Z2");

            List<F0101Z2> entries = new List<F0101Z2>();
            F0101Z2 entry;
            decimal transaction = 1;
            foreach (ShipToLine line in DT.NewShipTos)
            {
                entry = new F0101Z2();
                PopulateUnchangingF0101Z2(ref entry, DT.batch);
                PopulateLineF0101Z2(ref entry, line, DT.ParentAddress, DT.StoreName, DT.ConceptID, transaction);
                entries.Add(entry);
                transaction++;
            }
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    jde.F0101Z2.AddRange(entries);
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return;
        }

        /// <summary>
        /// Populate the JDE F03012Z1 file from the ShipToconstruct
        /// </summary>
        /// <param name="DT"></param>
        public static void PopulateF03012Z1(ShipTo DT)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating F03012Z1");

            List<F03012Z1> entries = new List<F03012Z1>();
            F03012Z1 entry;
            decimal transaction = 1;
            foreach (ShipToLine line in DT.NewShipTos)
            {
                entry = new F03012Z1();
                PopulateUnchangingF03012Z1(ref entry, DT.batch);
                PopulateLineF03012Z1(ref entry, DT.ParentAddress, line, transaction);
                entries.Add(entry);
                transaction++;
            }
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    jde.F03012Z1.AddRange(entries);
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return;
        }

        /// <summary>
        /// Populate the JDE F47011 EDI file from the concept
        /// </summary>
        /// <param name="concept"></param>
        public static void PopulateF47011(Concept concept)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating F47011");

            List<F47011> EdiHeaderList = new List<F47011>();
            F47011 EdiHeader;
            IEnumerable<ConceptLine> FirstConceptLines = concept.OrderDetails.Where(n => 1 == n.LineNumber); // grab one record per concept order, grab the 1
            foreach (ConceptLine line in FirstConceptLines)
            {
                EdiHeader = PopulateSingleF47011(line, concept);
                EdiHeaderList.Add(EdiHeader);
            }
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    jde.F47011.AddRange(EdiHeaderList);
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return;
        }

        /// <summary>
        /// Populate the JDE F47012 EDI file from the concept
        /// </summary>
        /// <param name="concept"></param>
        public static void PopulateF47012(Concept concept)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating F47012");

            List<F47012> EdiDetailsList = new List<F47012>();
            F47012 EdiDetail;
            foreach (ConceptLine line in concept.OrderDetails)
            {
                EdiDetail = PopulateOrderDetailF47012(line, concept.PONumber);
                EdiDetailsList.Add(EdiDetail);
            }
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    jde.F47012.AddRange(EdiDetailsList);
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return;
        }

        /// <summary>
        /// For a freight object, populate all of the shipment lines
        /// </summary>
        /// <param name="freight"></param>
        /// <returns></returns>
        static public bool GetShipmentNumbers(ref Freight freight)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Retrieving shipment numbers from orders");
            bool bResult = false;

            double shipment;
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    foreach (FreightLine line in freight.freight_lines)
                    {
                        if (false == Double.TryParse((jde.F4211.AsNoTracking().Where(n => line.order == n.SDDOCO).Select(n => n.SDSHPN).ToString()), out shipment))
                        {
                            line.shipment = 0;
                            log.Error($"Order {line.order} does not have a valid shipment number associated with it");
                        }
                        else
                        {
                            line.shipment = shipment;
                        }
                    }
                    bResult = true;
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return bResult;
        } // GetShipmentNumbers

        /// <summary>
        /// Take a list of freight_lines and add them to F4217
        /// </summary>
        /// <param name="freight"></param>
        static public void PopulateF4217(Freight freight)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating F4217");

            List<F4217> ShipmentDetailsList = new List<F4217>();
            F4217 freightDetail;
            foreach (FreightLine line in freight.freight_lines)
            {
                freightDetail = PopulateF4217Detail(line);
                ShipmentDetailsList.Add(freightDetail);
            }
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    jde.F4217.AddRange(ShipmentDetailsList);
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return;
        } // PopulateF4217

        /// <summary>
        /// Take a list of freight_lines and add them to F4943
        /// </summary>
        /// <param name="freight"></param>
        static public void PopulateF4943(Freight freight)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating F4943");

            List<F4943> ShipmentDetailsList = new List<F4943>();
            F4943 freightDetail;
            foreach (FreightLine line in freight.freight_lines)
            {
                freightDetail = PopulateF4943Detail(line);
                ShipmentDetailsList.Add(freightDetail);
            }
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    jde.F4943.AddRange(ShipmentDetailsList);
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return;
        } // PopulateF4943

        /// <summary>
        /// Add the freight charges to the appropriate sales order line
        /// </summary>
        /// <param name="freight"></param>
        static public void UpdateFreightF4211(Freight freight)
        {
            try
            {
                using (JDEEntities jde = ConnectionHelper.CreateConnection())
                {
                    foreach(FreightLine line in freight.freight_lines)
                    {
                        F4211 f4211_record = jde.F4211.Where(n => line.order == n.SDDOCO && "9227" == n.SDLITM).First();
                        f4211_record.SDUPRC = Math.Round(10000 * line.cost);
                        f4211_record.SDAEXP = Math.Round(100 * line.cost);
                    }
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw;
            }
            return;
        } // UpdateFreightF4211
#endregion
    } // Class
} // namespace
