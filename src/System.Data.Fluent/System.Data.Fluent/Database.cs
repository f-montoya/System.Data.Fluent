using System.Data.Fluent.Abstraction;
using System.Data.Fluent.Impl;

namespace System.Data.Fluent
{
    public static class Database
    {
        public static IConnectionBuilder Use(IDbEngineProvider engine, string connectionString)
        {
            Check.IsNull(engine, "engine");
            Check.IsNull(connectionString, "connectionString");

            return new ConnectionBuilder(new Context
            {
                DbEngineProvider = engine,
                ConnectionString = connectionString
            });
        }
    }
}
