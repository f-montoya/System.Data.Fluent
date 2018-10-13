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
        Context context;

        internal ConnectionBuilder(Context context)
        {
            this.context = context;
        }

        #region IConnectionBuilder

        public IFunctionBuilder WithFunction(string function)
        {
            context.Command = function;
            context.CommandType = CommandType.StoredProcedure;

            return new CommandBuilder(context);
        }

        public ICommandBuilder WithProcedure(string procedure)
        {
            context.Command = procedure;
            context.CommandType = CommandType.StoredProcedure;

            return new CommandBuilder(context);
        }

        public ICommandBuilder WithSql(string sql)
        {
            context.Command = sql;
            context.CommandType = CommandType.Text;

            return new CommandBuilder(context);
        }

        #endregion
    }
}
