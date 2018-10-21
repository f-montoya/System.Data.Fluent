using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Abstraction
{
    public interface IConfigurationBuilder
    {
        IConfigurationBuilder AddProvider(IDbEngineProvider dataAccessProvider);
    }
}
