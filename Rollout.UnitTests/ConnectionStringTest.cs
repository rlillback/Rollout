using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rollout.EF;

namespace Rollout.UnitTests
{
    [TestClass]
    public class ConnectionStringTest
    {
        protected string metadata = "res://*/JDE.csdl|res://*/JDE.ssdl|res://*/JDE.msl";
        protected string dataSource = "sr02\\n0e9sql01";
        protected string initialCatalogue = "JDE_DEVELOPMENT";
        protected string expectedResult = "metadata=res://*/JDE.csdl|res://*/JDE.ssdl|res://*/JDE.msl;provider=System.Data.SqlClient;provider connection string=\"data source=sr02\\n0e9sql01;initial catalog=JDE_DEVELOPMENT;integrated security=True;MultipleActiveResultSets=True;Application Name=EntityFramework\"";

        [TestMethod]
        public void ConnectionHelper_CreateConnectionString()
        {
            string connectionString = ConnectionHelper.CreateConnectionString(
                this.metadata,
                this.dataSource,
                this.initialCatalogue);
            Assert.AreEqual(expectedResult.ToLower(),
                            connectionString.ToLower());
        }

        [TestMethod]
        public void ConnectionHelper_CreateContext()
        {
            using (JDEEntities jde = ConnectionHelper.CreateConnection(this.metadata, this.dataSource, this.initialCatalogue))
            {
                jde.F0101.AsNoTracking().Select(n => 4590 == n.ABAN8);
            }
        }

        [TestMethod]
        public void ConnectionHelper_CreateContextJDE()
        {
            using (JDEEntities jde = ConnectionHelper.CreateConnection())
            {
                jde.F0101.AsNoTracking().Select(n => 4590 == n.ABAN8);
            }
        }
    }
}
