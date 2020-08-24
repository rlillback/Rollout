namespace Rollout.EF
{
    public partial class JDEEntities
    {
        // Overload the JDEEntities class so that we can build the DbContext on the fly.
        // This file is NOT auto generated & must be added to overload the JDEEntities constructor.
        // So, if we pass a connection string to JDEEntities, it will use this function instead of
        // the connection string in the app.config file.
        public JDEEntities(string connectionString)
            : base(connectionString)
        {

        }
    }
}
