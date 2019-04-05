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
        readonly Context context;

        internal ConnectionBuilder(Context context)
        {
            this.context = context;
        }

        #region IConnectionBuilder

        IFunctionBuilder IConnectionBuilder.WithFunction(string function)
        {
            if (!context.DbEngineProvider.SupportsFunctions)
            {
                throw new NotSupportedException();
            }

            context.Command = function;
            context.CommandType = CommandType.StoredProcedure;

            return new CommandBuilder(context);
        }

        ICommandBuilder IConnectionBuilder.WithProcedure(string procedure)
        {
            if(!context.DbEngineProvider.SupportsProcedures)
            {
                throw new NotSupportedException();
            }

            context.Command = procedure;
            context.CommandType = CommandType.StoredProcedure;

            return new CommandBuilder(context);
        }

        ICommandBuilder IConnectionBuilder.WithSql(string sql)
        {
            context.Command = sql;
            context.CommandType = CommandType.Text;

            return new CommandBuilder(context);
        }

        #endregion
    }
}
