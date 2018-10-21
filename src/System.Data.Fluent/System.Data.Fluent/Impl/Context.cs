using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal sealed class Context
    {
        public Context(ConnectionStringSettings connectionStringSettings, IDbEngineProvider dbEngineProvider)
        {
            ConnectionStringSettings = connectionStringSettings;
            dbEngineProvider = DbEngineProvider;
            DbValueProvider = DbEngineProvider.ValueProvider ?? new DefaultDbValueProvider();
        }

        public ConnectionStringSettings ConnectionStringSettings { get; }

        public IDbEngineProvider DbEngineProvider { get; }

        public IDbValueProvider DbValueProvider { get; }

        public string Command { get; set; }

        public CommandType CommandType { get; set; }
    }
}
