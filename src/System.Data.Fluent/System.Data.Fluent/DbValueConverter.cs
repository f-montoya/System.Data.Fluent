using System;
using System.Collections.Generic;
using System.Data.Fluent.Impl;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public abstract class DbValueConverter : IDbValueConverter
    {
        IDbValueConverter defaultDbValueProvider { get; } = new DefaultDbValueProvider();

        public T ConvertDbValue<T>(object value)
        {
            return (T)ConvertDbValue(value, typeof(T));
        }

        public virtual object ConvertDbValue(object value, Type type)
        {
            return defaultDbValueProvider.ConvertDbValue(value, type);
        }
    }
}
