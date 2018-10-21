using System;
using System.Collections.Generic;
using System.Data.Fluent.Impl;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public abstract class DbValueProvider : IDbValueProvider
    {
        IDbValueProvider defaultDbValueProvider;

        public T ConvertDbValue<T>(object value)
        {
            return (T)ConvertDbValue(value, typeof(T));
        }

        public virtual object ConvertDbValue(object value, Type type)
        {
            defaultDbValueProvider = defaultDbValueProvider ?? new DefaultDbValueProvider();

            return defaultDbValueProvider.ConvertDbValue(value, type);
        }
    }
}
