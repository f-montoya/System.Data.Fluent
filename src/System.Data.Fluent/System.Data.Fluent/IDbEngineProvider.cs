using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public interface IDbEngineProvider
    {
        string Name { get; }

        bool SupportsProcedures { get; }
        bool SupportsFunctions { get; }

        DbProviderFactory ProviderFactory {get; }

        IDbValueConverter ValueProvider { get; }
    }
}
