using System;
using System.Collections.Generic;
using System.Data.Fluent;
using System.Data.Fluent.Abstraction;
using System.Data.Fluent.Impl;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data
{
    public static class DbContextExtensions
    {
        public static ICommandBuilder WithSql(this DbContext dbContext, string sql)
        {
            return new CommandBuilder(dbContext, sql, CommandType.Text);
        }

        public static ICommandBuilder WithProcedure(this DbContext dbContext, string procedure)
        {
            if (!dbContext.SupportsProcedures)
            {
                throw new NotSupportedException();
            }

            return new CommandBuilder(dbContext, procedure, CommandType.StoredProcedure);
        }

        public static IFunctionBuilder WithFunction(this DbContext dbContext, string function)
        {
            if (!dbContext.SupportsFunctions)
            {
                throw new NotSupportedException();
            }

            return new CommandBuilder(dbContext, function, CommandType.StoredProcedure);
        }
    }
}
