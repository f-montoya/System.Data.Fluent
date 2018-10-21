using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public interface IDbValueProvider
    {
        T ConvertDbValue<T>(object value);

        object ConvertDbValue(object value, Type type);
    }
}
