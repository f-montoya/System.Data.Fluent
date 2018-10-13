using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal sealed class Providers
    {
        public IDictionary<string, IDbEngineProvider> DbEngineProviders { get; } = new Dictionary<string, IDbEngineProvider>();

        public IDbValueProvider DbValueProvider { get; set; } = new DefaultDbValueProvider();
    }
}
