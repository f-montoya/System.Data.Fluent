using System.Data.Common;
using System.Data.Fluent.Abstraction;
using System.Data.Fluent.Impl;

namespace System.Data.Fluent
{
    public abstract class DbContext : IDbContextBuilder, IDbContext, IDisposable
    {
        readonly string connectionString;
        DbConnection connection;

        public ICommandBuilder WithSql(string sql)
        {
            return new CommandBuilder(new Command(this, sql, CommandType.Text));
        }

        public ICommandBuilder WithProcedure(string procedure)
        {
            if (!SupportsProcedures)
            {
                throw new NotSupportedException();
            }

            return new CommandBuilder(new Command(this, procedure, CommandType.StoredProcedure));
        }

        public IFunctionBuilder WithFunction(string function)
        {
            if (!SupportsFunctions)
            {
                throw new NotSupportedException();
            }

            return new FunctionBuilder(new Command(this, function, CommandType.StoredProcedure));
        }

        #region Protected members

        protected DbContext(string connectionString)
        {
            Check.IsNull(connectionString, nameof(connectionString));

            this.connectionString = connectionString;
        }

        protected abstract bool SupportsProcedures { get; }

        protected abstract bool SupportsFunctions { get; }

        protected abstract DbProviderFactory DbProviderFactory { get; }

        protected abstract DbParameterFactory DbParameterFactory { get; }

        protected abstract DbValueConverter DbValueConverter { get; }

        #endregion

        #region IDbContext

        DbConnection IDbContext.Connection => connection ?? (connection = CreateConnection());

        DbProviderFactory IDbContext.DbProviderFactory => DbProviderFactory;

        DbParameterFactory IDbContext.DbParameterFactory => DbParameterFactory;

        DbValueConverter IDbContext.DbValueConverter => DbValueConverter;

        #endregion

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

        DbConnection CreateConnection()
        {
            var cnn = DbProviderFactory.CreateConnection();
            cnn.ConnectionString = connectionString;
            return cnn;
        }

    }
}
