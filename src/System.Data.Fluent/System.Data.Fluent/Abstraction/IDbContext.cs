using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Abstraction
{
    internal interface IDbContext : IDisposable
    {
        DbConnection Connection { get; }

        DbProviderFactory DbProviderFactory { get; }

        DbParameterFactory DbParameterFactory { get; }

        DbValueConverter DbValueConverter { get; }
    }
}
