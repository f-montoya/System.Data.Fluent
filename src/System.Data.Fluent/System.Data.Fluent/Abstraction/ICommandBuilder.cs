using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Fluent.Abstraction
{
    public interface ICommandBuilder
    {
        ICommandBuilder CommandTimeout(int timeout);
        ICommandBuilder Parameters(Action<IParameterBuilder> parametersAction);

        int Execute();
        int Execute(Action<IDataParameterCollection> inspectParameters);

        T GetScalar<T>();

        IList<T> GetScalarList<T>();
        void GetScalarList<T>(Func<T, bool> callback);

        T GetFirst<T>() where T : class;
        void GetFirst(Action<IDataRecord> callback);

        IList<T> GetList<T>() where T : class;
        void GetList<T>(Func<T, bool> callback) where T : class;
        void GetList(Func<IDataRecord, bool> callback);
    }
}
