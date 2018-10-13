using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Fluent.Abstraction;
using System.Data.Fluent.Impl;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public static class Db
    {
        public static void Configure(Action<IConfigurationBuilder> configureAction)
        {
            Check.IsNull(configureAction, "ConfigureAction");

            Providers = Providers ?? new Providers();

            configureAction(new ConfigurationBuilder(Providers));
        }

        public static IConnectionBuilder Use(string connectionStringName)
        {
            Check.IsNull(connectionStringName, "ConnectionStringName");

            Providers = Providers ?? throw new Exception("Call Db.Confgure() before use it.");

            return Use(ConfigurationManager.ConnectionStrings[connectionStringName]);
        }

        public static IConnectionBuilder Use(ConnectionStringSettings connectionStringSettings)
        {
            Check.IsNull(connectionStringSettings, "ConnectionStringSettings");

            Providers = Providers ?? throw new Exception("Call Db.Confgure() before use it.");

            return new ConnectionBuilder(connectionStringSettings);
        }

        internal static Providers Providers { get; private set; }
    }
}
