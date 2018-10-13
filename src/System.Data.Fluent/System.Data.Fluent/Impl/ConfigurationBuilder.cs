using System;
using System.Collections.Generic;
using System.Data.Fluent.Abstraction;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal sealed class ConfigurationBuilder : IConfigurationBuilder
    {
        readonly Providers providers;

        public ConfigurationBuilder(Providers providers)
        {
            this.providers = providers;
        }

        public IConfigurationBuilder AddProvider(IDbEngineProvider dataAccessProvider)
        {
            providers.DbEngineProviders.Add(dataAccessProvider.Name, dataAccessProvider);
            return this;
        }

        public IConfigurationBuilder AddProvider(IDbValueProvider valueProvider)
        {
            providers.DbValueProvider = valueProvider;
            return this;
        }
    }
}
