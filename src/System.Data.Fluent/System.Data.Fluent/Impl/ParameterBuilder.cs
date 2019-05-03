using System.Collections.Generic;
using System.Data.Common;
using System.Data.Fluent.Abstraction;

namespace System.Data.Fluent.Impl
{
    internal class ParameterBuilder : IParameterBuilder
    {
        readonly IDbParameterFactory factory;

        public ParameterBuilder(IDbParameterFactory factory)
        {
            Check.IsNull(factory, nameof(factory));

            this.factory = factory;
        }

        public IList<DbParameter> Parameters { get; } = new List<DbParameter>();

        public IParameterBuilder Add(string name, object value)
        {
            Check.IsNull(name, nameof(name));

            Parameters.Add(factory.CreateInputParameter(name, value));
            return this;
        }

        public IParameterBuilder AddCursor(string name)
        {
            Check.IsNull(name, nameof(name));

            Parameters.Add(factory.CreateCursorParameter(name));
            return this;
        }

        public IParameterBuilder AddInOut(string name, object value)
        {
            Check.IsNull(name, nameof(name));

            Parameters.Add(factory.CreateInputOutputParameter(name, value));
            return this;
        }

        public IParameterBuilder AddOut<T>(string name)
        {
            Check.IsNull(name, nameof(name));

            return AddOut(name, typeof(T));
        }

        public IParameterBuilder AddOut(string name, Type type)
        {
            Check.IsNull(name, nameof(name));
            Check.IsNull(type, nameof(type));

            Parameters.Add(factory.CreateOutputParameter(name, type));
            return this;
        }
    }
}
