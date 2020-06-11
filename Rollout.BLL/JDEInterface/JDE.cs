using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            entry.SZLNGP = "EN";         // English
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
            entry.SZCOUN = "";           // We don't use the county or parish
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
            
            entry.SZAN82 = ParentAddress;
            entry.SZAN83 = ParentAddress;
            entry.SZAN85 = ParentAddress;
            entry.SZPA8 = ParentAddress;

            entry.SZAC08 = line.Concept; // This is where we are placing the concept
            return;
        }
        #endregion

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
                using (JDEEntities jde = new JDEEntities())
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
                throw eJDE;                
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
                using (JDEEntities jde = new JDEEntities())
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
                        throw new System.ArgumentException($"Error {Nickname} isn't in JDE.");
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw eJDE;
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
                using (JDEEntities jde = new JDEEntities())
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
                        throw new System.ArgumentException($"Error {ConceptId} doesn't have a C3 record in JDE.");
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw eJDE;
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
                using (JDEEntities jde = new JDEEntities())
                {
                    found = jde.F0101.AsNoTracking()
                        .Where(n => Names.Contains(n.ABALKY.Trim()))
                        .Select(n => n.ABALKY.Trim())
                        .ToList();                  
                }
                log.Debug($"Found these addresses = {String.Join(",",found)}");
                missing = Names.Except(found).ToList();
                log.Debug($"These names are missing = {String.Join(",", missing)}");
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw eJDE;
            }
            return missing;
        } // FindMissingShipTos

        /// <summary>
        /// Check if a single Address number exists in JDEs Address Book
        /// </summary>
        /// <param name="addressNumber"></param>
        /// <returns></returns>
        public static bool DoesAddressExist(double addressNumber)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Checking for existence of address book number = {addressNumber}");

            bool exists;
            try
            {
                using (JDEEntities jde = new JDEEntities())
                {
                    if (jde.F0101.AsNoTracking().Any(n => n.ABAN8 == addressNumber))
                    {
                        exists = true;
                        log.Debug($"Found {addressNumber}");
                    }
                    else
                    {
                        exists = false;
                        log.Debug($"Did not find {addressNumber}");
                    }
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw eJDE;
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
                using (JDEEntities jde = new JDEEntities())
                {
                    found = jde.F4102.AsNoTracking()
                            .Where(n => branchplant.Trim() == n.IBMCU.Trim() &&
                                        items.Contains(n.IBLITM))
                            .Select(n => n.IBLITM.Trim())
                            .ToList();
                    log.Debug($"Found the following items in the {branchplant} branch: {String.Join(",",found)}");
                }
                missing = items.Except(found).ToList();
                log.Debug($"The following items were not found: {String.Join(",", missing)}");
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw eJDE;
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
                using (JDEEntities jde = new JDEEntities())
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
                throw eJDE;
            }
            return exists;
        }

        public static void PopulateF0101Z2(ShipTo DT)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConceptCSV)).Location) + @"\" + "log4net.config"));
            log.Debug($"Populating F0101Z2");

            Random random = new Random();
            string batch = random.Next().ToString(); // 32-bit integer
            List<F0101Z2> entries = new List<F0101Z2>();
            F0101Z2 entry = null;
            decimal transaction = 1;
            foreach (ShipToLine line in DT.NewShipTos)
            {
                entry = new F0101Z2();
                PopulateUnchangingF0101Z2(ref entry, batch);
                PopulateLineF0101Z2(ref entry, line, DT.ParentAddress, DT.StoreName, DT.ConceptID, transaction);
                entries.Add(entry);
                transaction++;
            }
            try
            {
                using (JDEEntities jde = new JDEEntities())
                {
                    jde.F0101Z2.AddRange(entries);
                    jde.SaveChanges();
                }
            }
            catch (Exception eJDE)
            {
                log.Error($"{eJDE.Message.ToString()} -- INNER: {eJDE.InnerException.ToString()}");
                throw eJDE;
            }
            return;
        }
    } // Class
} // namespace
