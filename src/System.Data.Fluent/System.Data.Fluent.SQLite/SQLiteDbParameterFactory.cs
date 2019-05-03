using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.SQLite
{
    internal class SQLiteDbParameterFactory : DbParameterFactory
    {
        protected override DbParameter CreateParameter() => new SQLiteParameter();

        public override DbParameter CreateInputOutputParameter(string name, object value) => throw new NotSupportedException();

        public override DbParameter CreateOutputParameter(string name, Type type) => throw new NotSupportedException();
    }
}
