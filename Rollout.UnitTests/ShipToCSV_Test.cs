using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Rollout.BLL;

namespace Rollout.UnitTests
{
    [TestClass]
    public class ShipToCSV_Test
    {
        [TestMethod]
        public void ReadConceptAndWriteCSV()
        {
            ConceptCSV TestConcept = new ConceptCSV(@"D:\gitHub\Rollout\TestFiles\SageRolloutExample1.csv");
            TestConcept.ReadConcept();
            List<string> TestMissing = TestConcept.CheckForMissingShipToAddresses();
            ShipToCSV TestShip = new ShipToCSV(@"D:\gitHub\Rollout\TestFiles\SageRolloutMissing.csv", ",");
            TestShip.PopulateSpreadsheet(TestMissing, TestConcept);
            TestShip.WriteCSV();
        }

        [TestMethod]
        public void ReadShipToCSVandPopulateF0101Z2()
        {
            ShipToCSV PopulatedCSV = new ShipToCSV(@"D:\gitHub\Rollout\TestFiles\SageRolloutMissing.csv", ",");
            PopulatedCSV.ReadShipTo();
            PopulatedCSV.ValidateHeader();
            PopulatedCSV.ValidateRows(true);
            ShipTo PopulatedShipTo = XfrmShipTo.CSVToShipTo(PopulatedCSV, false); // Dont lookup JDE address
            JDE.PopulateF0101Z2(PopulatedShipTo);
        }

        [TestMethod]
        public void PopulateF03012Z1()
        {
            ShipToCSV PopulatedCSV = new ShipToCSV(@"D:\gitHub\Rollout\TestFiles\SageRolloutMissing.csv", ",");
            PopulatedCSV.ReadShipTo();
            PopulatedCSV.ValidateHeader();
            PopulatedCSV.ValidateRows(false); // Force non-empty tax area code
            ShipTo PopulatedShipTo = XfrmShipTo.CSVToShipTo(PopulatedCSV, true); // Lookup JDE address
            JDE.PopulateF03012Z1(PopulatedShipTo);
        }

        [TestMethod]
        public void TestSteps()
        {
            bool SkipF0101 = true;
            bool SkipF03012 = false;
            // 1.) Read the concept
            ConceptCSV TestConceptCSV = new ConceptCSV(@"D:\gitHub\Rollout\TestFiles\SageRolloutExample1.csv", ",");
            TestConceptCSV.ReadConcept();
            // 2.) Test for missing entries
            List<string> TestMissing = TestConceptCSV.CheckForMissingShipToAddresses();
            // 3.) If entries are missing, do the following:
            if (0 < TestMissing.Count && !SkipF0101)
            {
                // 4.) Populate the missing CSV using the list of missing items
                ShipToCSV MissingShipToCSV = new ShipToCSV(@"D:\gitHub\Rollout\TestFiles\SageRolloutMissing.csv", ",");
                MissingShipToCSV.PopulateSpreadsheet(TestMissing, TestConceptCSV);
                // 5.) Populate the transformed data structure
                ShipTo MissingShipTo = XfrmShipTo.CSVToShipTo(MissingShipToCSV, false); // Don't look up JDE Addresses
                // 6.) Upload the transformed data structure to the F0101 Z-File
                JDE.PopulateF0101Z2(MissingShipTo);
                // 7.) Now write the CSV to disk
                MissingShipToCSV.WriteCSV();
                // 8.) Now, someone must run R01010Z to load the data into JDE.
                // Popup window
            }
            else if ( !SkipF03012 )
            {
                // TODO: 9.) Check for F03012 records
                // 10.) If F03012 records are missing:
                ShipToCSV MissingShipToCSV = new ShipToCSV(@"D:\gitHub\Rollout\TestFiles\SageRolloutMissing.csv", ",");
                MissingShipToCSV.ReadShipTo();
                if (MissingShipToCSV.ValidateHeader() && // Required row is all there
                    MissingShipToCSV.ValidateRows(false)) // We can't have an empty tax area code
                {
                    // 11.) Convert the CSV into a data structure we can upload
                    ShipTo PopulatedShipTo = XfrmShipTo.CSVToShipTo(MissingShipToCSV, true); // Lookup JDE Address
                    // 12.) Upload the data structure into JDE
                    JDE.PopulateF03012Z1(PopulatedShipTo);
                    // 13.) Now someone must run R03010Z to load the data into JDE.
                }
                else
                {
                    //TODO: Say we don't have a valid spreadsheet
                    int x = 0;
                    x++;
                }
            }
            else
            {
                //TODO: Load the EDI tables
                Concept TestConcept = XfrmConcept.CSVtoConcept(TestConceptCSV);
            }
        }
    }
}
