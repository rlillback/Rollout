using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rollout.BLL;

namespace Rollout.UnitTests
{
    [TestClass]
    public class ConceptCSVTest
    {
        [TestMethod]
        public ConceptCSV TestGoodRead()
        {
            ConceptCSV TestConcept = new ConceptCSV(@"E:\gitHub\Rollout\TestFiles\SageRolloutExample1.csv");
            TestConcept.ReadConcept();
            Assert.IsNotNull(TestConcept);
            return TestConcept;
        }


        [TestMethod]
        public void TestGoodExampleSpreadsheet()
        {
            bool passed = true;
            ConceptCSV TestConcept = TestGoodRead();
            if ( null != TestConcept )
            {
                passed = TestConcept.ValidateHeader();
                if ( passed )
                {
                    passed = TestConcept.ValidateRows();
                    int numRows = TestConcept.RowCount;
                    passed = passed && (56 == numRows);
                    Assert.IsTrue(passed);
                }
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        
    }
}
