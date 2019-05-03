using System.Data.Common;
using System.Data.Fluent.Impl;

namespace System.Data.Fluent
{
    public abstract class DbContext : IDisposable
    {
        readonly string connectionString;

        DbConnection connection;

        protected DbContext(string connectionString)
        {
            Check.IsNull(connectionString, nameof(connectionString));

            this.connectionString = connectionString;
        }

        public DbConnection Connection => connection ?? (connection = CreateConnection());

        public abstract bool SupportsProcedures { get; }

        public abstract bool SupportsFunctions { get; }

        public abstract DbProviderFactory DbProviderFactory { get; }

        public abstract DbParameterFactory DbParameterFactory { get; }

        public abstract DbValueConverter DbValueConverter { get; }

        #region IDisposable 

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private DbConnection CreateConnection()
        {
            var provider = DbProviderFactory ?? throw new ArgumentNullException(nameof(DbProviderFactory));
            var cnn = provider.CreateConnection();
            cnn.ConnectionString = connectionString;
            return cnn;
        }
    }
}
