using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Abstraction
{
    public interface IDbContextBuilder
    {
        ICommandBuilder WithSql(string sql);

        ICommandBuilder WithProcedure(string procedure);

        IFunctionBuilder WithFunction(string function);
    }
}
