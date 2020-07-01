using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rollout.BLL;

namespace Rollout.UnitTests
{
    [TestClass]
    public class JDETest
    {
        [TestMethod]
        public void GetConceptID_byABAN8()
        {
            float address = 4590;
            string expected = "DN";
            string result = JDE.GetConceptID(address);
            Assert.IsTrue(0 == String.Compare(expected, result));
        }

        [TestMethod]
        public void GetConceptID_byABALKY()
        {
            string name = "2020PH1";
            string expected = "DN";
            string result = JDE.GetConceptID(name);
            Assert.IsTrue(0 == String.Compare(expected, result));
        }

        [TestMethod]
        public void ReturnSoldTos_NotFound()
        {
            List<string> list = new List<string>();
            list.Add("2020PH1");
            list.Add("ACUARIO");
            list.Add("ALL TEMP");
            list.Add("ECOLAB");
            list.Add("DONT FIND ME");
            list.Add("DONT FIND ME 1");
            list.Add("DONT FIND ME 2");

            List<string> expected = new List<string>();
            expected.Add("DONT FIND ME");
            expected.Add("DONT FIND ME 1");
            expected.Add("DONT FIND ME 2");

            List<string> NotFound = JDE.FindMissingShipTos(list);


            Assert.IsTrue(3 == NotFound.Count &&
                          NotFound.Contains("DONT FIND ME") &&
                          NotFound.Contains("DONT FIND ME 1") &&
                          NotFound.Contains("DONT FIND ME 2") );
        }

        [TestMethod]
        public void ItemsInBranch_Good()
        {
            string branch = "1A";
            List<string> itemList = new List<string>();
            itemList.Add("1010");
            itemList.Add("11007");
            itemList.Add("11000");
            itemList.Add("10175");

            List<string> expected = new List<string>();
            expected.Add("10175");

            List<string> returned = JDE.GetMissingItems(itemList, branch);

            Assert.IsTrue(returned.Contains("10175") && 1 == returned.Count);
        }

        [TestMethod]
        public void ItemsInBranch_All()
        {
            string branch = "1A";
            List<string> itemList = new List<string>();
            itemList.Add("1010");
            itemList.Add("11007");
            itemList.Add("11000");

            List<string> returned = JDE.GetMissingItems(itemList, branch);

            Assert.IsTrue(0 == returned.Count);
        }

        [TestMethod]
        public void TestNextNumber()
        {
            double NextBatch = JDE.GetNextBatchNumber();
            double NextDocument = JDE.GetDocumentNumbers(10);
            NextBatch = JDE.GetNextBatchNumber();
            NextDocument = JDE.GetDocumentNumbers(1);
        }


    }
}
