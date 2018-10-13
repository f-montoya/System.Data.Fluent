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
        public ConnectionStringSettings ConnectionStringSettings { get; set; }

        public IDbEngineProvider DbEngineProvider { get; set; }

        public IDbValueProvider DbValueProvider { get; set; }

        public string Command { get; set; }

        public CommandType CommandType { get; set; }
    }
}
