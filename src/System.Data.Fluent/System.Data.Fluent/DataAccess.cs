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
    public static class DataAccess
    {
        public static void Configure(Action<IConfigurationBuilder> configureAction)
        {
            throw new NotImplementedException();
        }

        public static IConnectionBuilder Use(string connectionStringName)
        {
            return Use(ConfigurationManager.ConnectionStrings[connectionStringName]);
        }

        public static IConnectionBuilder Use(ConnectionStringSettings connectionStringSettings)
        {
            return new CommandBuilder(connectionStringSettings);
        }
    }
}
