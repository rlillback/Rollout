using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace Rollout.EF
{
    public class ConnectionHelper
    {
        protected const string thisMetaData = "res://*/JDE.csdl|res://*/JDE.ssdl|res://*/JDE.msl";
#if DEBUG
        protected const string thisDataSource = "SR02";
#else
        protected const string thisDataSource = "N0E9SQL01";
#endif
        protected const string thisInitialCatalogue = "JDE_PRODUCTION";
        protected const string thisUserId = "SuwaneeRollouts";
        protected const string thisP = "SuwaneeRollouts";
        protected const string appName = "EntityFramework";
        protected const string providerName = "System.Data.SqlClient";

        public static string CreateConnectionString()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = thisDataSource;
            sqlBuilder.InitialCatalog = thisInitialCatalogue;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.UserID = thisUserId;
            sqlBuilder.Password = thisP;
            sqlBuilder.ApplicationName = appName;

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
            efBuilder.Metadata = thisMetaData;
            efBuilder.Provider = providerName;
            efBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;

            return efBuilder.ConnectionString;
        }

        public static string CreateConnectionString(string metaData, string dataSource, string initialCatalog)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = dataSource;
            sqlBuilder.InitialCatalog = initialCatalog;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.ApplicationName = appName;

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
            efBuilder.Metadata = metaData;
            efBuilder.Provider = providerName;
            efBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;

            return efBuilder.ConnectionString;
        }

        public static JDEEntities CreateConnection(string metaData, string dataSource, string initialCatalog)
        {
            return new JDEEntities(ConnectionHelper.CreateConnectionString(metaData, dataSource, initialCatalog));
        }

        public static JDEEntities CreateConnection()
        {
            return new JDEEntities(ConnectionHelper.CreateConnectionString());
        }
    }
}
