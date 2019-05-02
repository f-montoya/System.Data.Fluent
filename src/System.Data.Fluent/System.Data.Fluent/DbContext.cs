using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Fluent.Impl;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public abstract class DbContext : IDisposable
    {
        readonly string connectionString;
        readonly IDbValueConverter defaultDbValueConverter = new DefaultDbValueProvider();
        DbConnection connection;

        protected DbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbConnection Connection => connection ?? (connection = CreateConnection());

        public abstract bool SupportsProcedures { get; }

        public abstract bool SupportsFunctions { get; }

        public abstract DbProviderFactory DbProviderFactory { get; }

        public abstract IDbParameterFactory DbParameterFactory { get; }

        public virtual IDbValueConverter DbValueConverter => defaultDbValueConverter;

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
