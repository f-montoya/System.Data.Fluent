using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Abstraction
{
    public interface IConnectionBuilder
    {
        ICommandBuilder WithSql(string sql);

        ICommandBuilder WithProcedure(string sql);

        IFunctionBuilder WithFunction(string sql);
    }
}
