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

        protected override bool SupportsProcedures => false;

        protected override bool SupportsFunctions => false;

        protected override DbProviderFactory DbProviderFactory { get; } = new SQLiteFactory();

        protected override DbParameterFactory DbParameterFactory { get; } = new SQLiteDbParameterFactory();

        protected override DbValueConverter DbValueConverter { get; } = new SQLiteDbValueConverter();
    }
}
