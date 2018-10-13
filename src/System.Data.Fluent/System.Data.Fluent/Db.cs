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
        static Providers providers;

        public static void Configure(Action<IConfigurationBuilder> configureAction)
        {
            Check.IsNull(configureAction, "ConfigureAction");

            providers = providers ?? new Providers();

            configureAction(new ConfigurationBuilder(providers));
        }

        public static IConnectionBuilder Use(string connectionStringName)
        {
            Check.IsNull(connectionStringName, "ConnectionStringName");

            return Use(ConfigurationManager.ConnectionStrings[connectionStringName]);
        }

        public static IConnectionBuilder Use(ConnectionStringSettings connectionStringSettings)
        {
            Check.IsNull(connectionStringSettings, "ConnectionStringSettings");

            if (providers == null)
            {
                throw new Exception("Call Db.Configure() before use it.");
            }

            if(!providers.DbEngineProviders.ContainsKey(connectionStringSettings.ProviderName))
            {
                throw new Exception($"DbEngineProvider for '{connectionStringSettings.ProviderName}' not found.");
            }

            return new ConnectionBuilder(new Context
            {
                ConnectionStringSettings = connectionStringSettings,
                DbEngineProvider = providers.DbEngineProviders[connectionStringSettings.ProviderName],
                DbValueProvider = providers.DbValueProvider
            });
        }
    }
}
