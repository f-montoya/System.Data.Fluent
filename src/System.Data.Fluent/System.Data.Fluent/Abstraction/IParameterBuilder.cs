using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Abstraction
{
    public interface IParameterBuilder
    {
        IParameterBuilder Add(string name, object value);

        IParameterBuilder AddInOut(string name, object value);

        IParameterBuilder AddOut<T>(string name);

        IParameterBuilder AddOut(string name, Type type);

        IParameterBuilder AddCursor(string name);
    }
}
