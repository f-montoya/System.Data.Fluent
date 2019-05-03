using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public interface IDbParameterFactory
    {
        DbParameter CreateInputParameter(string name, object value);

        DbParameter CreateInputOutputParameter(string name, object value);

        DbParameter CreateOutputParameter(string name, Type type);

        DbParameter CreateCursorParameter(string name);
    }
}
