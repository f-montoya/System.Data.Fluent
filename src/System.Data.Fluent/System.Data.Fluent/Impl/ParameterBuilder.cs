using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Fluent.Abstraction;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal class ParameterBuilder : IParameterBuilder
    {
        readonly IDbEngineProvider engine;

        public ParameterBuilder(Context context)
        {
            engine = context.DbEngineProvider;
            Parameters = engine.CreateParameterCollection();
        }

        public IDataParameterCollection Parameters { get; }

        public IParameterBuilder Add(string name, object value)
        {
            Parameters.Add(engine.CreateInputParameter(name, value));
            return this;
        }

        public IParameterBuilder AddCursor(string name)
        {
            Parameters.Add(engine.CreateCursorParameter(name));
            return this;
        }

        public IParameterBuilder AddInOut(string name, object value)
        {
            Parameters.Add(engine.CreateInputOutputParameter(name, value));
            return this;
        }

        public IParameterBuilder AddOut<T>(string name)
        {
            return AddOut(name, typeof(T));
        }

        public IParameterBuilder AddOut(string name, Type type)
        {
            Parameters.Add(engine.CreateOutputParameter(name, type));
            return this;
        }
    }
}
