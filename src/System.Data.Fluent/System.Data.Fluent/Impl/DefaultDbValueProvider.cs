using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal sealed class DefaultDbValueProvider : IDbValueProvider
    {
        public object ConvertDbValue(object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
