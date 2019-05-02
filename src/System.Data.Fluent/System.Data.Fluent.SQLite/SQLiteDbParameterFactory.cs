using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.SQLite
{
    internal class SQLiteDbParameterFactory : IDbParameterFactory
    {
        public DbParameter CreateInputParameter(string name, object value)
        {
            return new SQLiteParameter(name, value);
        }

        public DbParameter CreateCursorParameter(string name)
        {
            throw new NotSupportedException();
        }

        public DbParameter CreateInputOutputParameter(string name, object value)
        {
            throw new NotSupportedException();
        }

        public DbParameter CreateOutputParameter(string name, Type type)
        {
            throw new NotSupportedException();
        }
    }
}
