using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public interface IDbEngineProvider
    {
        string Name { get; }

        IDbConnection CreateConnection(string connectionString);

        IDbCommand CreateCommand(IDbConnection connection);

        IDataParameter CreateInputParameter(string name, object value);
        IDataParameter CreateInputOutputParameter(string name, object value);
        IDataParameter CreateOutputParameter(string name, Type type);
        IDataParameter CreateReturnParameter(string name, Type type);
        IDataParameter CreateCursorParameter(string name);
    }
}
