using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Fluent.Abstraction;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal sealed class ConnectionBuilder : IConnectionBuilder
    {
        ConnectionStringSettings connectionStringSettings;

        internal ConnectionBuilder(ConnectionStringSettings connectionStringSettings)
        {
            this.connectionStringSettings = connectionStringSettings;
        }

        #region IConnectionBuilder

        public IFunctionBuilder WithFunction(string function)
        {
            return new CommandBuilder(connectionStringSettings, function, CommandType.StoredProcedure);
        }

        public ICommandBuilder WithProcedure(string procedure)
        {
            return new CommandBuilder(connectionStringSettings, procedure, CommandType.StoredProcedure);
        }

        public ICommandBuilder WithSql(string sql)
        {
            return new CommandBuilder(connectionStringSettings, sql, CommandType.Text);
        }

        #endregion
    }
}
