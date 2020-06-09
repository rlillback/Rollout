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
            ShipToCSV TestShip = new ShipToCSV(@"D:\gitHub\Rollout\TestFiles\TestShipTo.csv", ",");
            TestShip.PopulateSpreadsheet(TestMissing, TestConcept);
            TestShip.WriteCSV();
        }

        [TestMethod]
        public void ReadShipToCSVandPopulateF0101Z2()
        {
            ShipToCSV PopulatedCSV = new ShipToCSV(@"D:\gitHub\Rollout\TestFiles\TestShipTo.csv", ",");
            PopulatedCSV.ReadShipTo();
            PopulatedCSV.ValidateHeader();
            PopulatedCSV.ValidateRows();
            ShipTo PopulatedShipTo = XfrmShipTo.CSVToShipTo(PopulatedCSV);
            JDE.PopulateF0101Z2(PopulatedShipTo);
        }
    }
}
