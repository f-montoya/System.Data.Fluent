using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.SQLite
{
    public class DbEngineProvider : IDbEngineProvider
    {
        public string Name => "System.Data.SQLite";

        public IDbValueProvider ValueProvider => null;

        public bool SupportsProcedures => false;

        public bool SupportsFunctions => false;

        public IDbConnection CreateConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        public IDbCommand CreateCommand(IDbConnection connection)
        {
            return connection.CreateCommand();
        }

        public IDataParameterCollection CreateParameterCollection()
        {
            return new SQLiteCommand().Parameters;
        }

        public IDataParameter CreateInputParameter(string name, object value)
        {
            return new SQLiteParameter(name, value);
        }

        public IDataParameter CreateInputOutputParameter(string name, object value)
        {
            throw new NotSupportedException();
        }

        public IDataParameter CreateOutputParameter(string name, Type type)
        {
            throw new NotSupportedException();
        }

        public IDataParameter CreateCursorParameter(string name)
        {
            throw new NotSupportedException();
        }

        public IDataParameter CreateReturnParameter(string name, Type type)
        {
            throw new NotSupportedException();
        }
    }
}
