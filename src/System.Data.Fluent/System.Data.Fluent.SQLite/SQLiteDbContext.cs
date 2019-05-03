using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.SQLite
{
    public class SQLiteDbContext : DbContext
    {
        protected SQLiteDbContext(string connectionString) : base(connectionString)
        {
        }

        public override bool SupportsProcedures => false;

        public override bool SupportsFunctions => false;

        public override DbProviderFactory DbProviderFactory { get; } = new SQLiteFactory();

        public override DbParameterFactory DbParameterFactory { get; } = new SQLiteDbParameterFactory();

        public override DbValueConverter DbValueConverter { get; } = new SQLiteDbValueConverter();
    }
}
