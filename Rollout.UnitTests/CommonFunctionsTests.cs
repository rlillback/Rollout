using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rollout.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rollout.UnitTests
{
    [TestClass()]
    public class CommonFunctionsTests
    {
        private string singlemonth = "2/28/2021";
        private string dualmonth = "12/31/2021";
        private string leap = "3/1/2020";
        private uint singlemonthuint = 121059;
        private uint dualmonthuint = 121365;
        private uint leapuint = 120061;

        [TestMethod()]
        public void SingleMonth_ToJulian()
        {
            uint actualresult = CommonFunctions.DateStringToJulian(singlemonth);
            Assert.AreEqual(singlemonthuint, actualresult);
        }

        [TestMethod()]
        public void DualMonth_ToJulian()
        {
            uint actual = CommonFunctions.DateStringToJulian(dualmonth);
            Assert.AreEqual(dualmonthuint, actual);
        }

        [TestMethod()]
        public void LeapYear_ToJulian()
        {
            uint actual = CommonFunctions.DateStringToJulian(leap);
            Assert.AreEqual(leapuint, actual);
        }

        [TestMethod()]
        public void SingleMonth_ToString()
        {
            string actual = CommonFunctions.JulianToDateString(singlemonthuint);
            Assert.AreEqual(0, String.Compare(singlemonth, actual) );
        }

        [TestMethod()]
        public void DualMonth_ToString()
        {
            string actual = CommonFunctions.JulianToDateString(dualmonthuint);
            Assert.AreEqual(0, String.Compare(dualmonth, actual));
        }

        [TestMethod()]
        public void LeapYear_ToString()
        {
            string actual = CommonFunctions.JulianToDateString(leapuint);
            Assert.AreEqual(0, String.Compare(leap, actual));
        }
    }
}