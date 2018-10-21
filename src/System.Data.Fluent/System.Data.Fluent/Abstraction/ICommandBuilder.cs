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
        ICommandBuilder Parameters(Action<IParameterBuilder> parametersAction);

        void Execute();
        void Execute(Action<IDataParameterCollection> inspectParameters);

        T GetScalar<T>();
        IList<T> GetScalarList<T>();
        IList<T> GetScalarList<T>(CancellationToken cancellationToken);
        void GetScalarList<T>(Func<T, bool> action);

        T GetFirst<T>() where T : class;
        IList<T> GetList<T>() where T : class;
        IList<T> GetList<T>(CancellationToken cancellationToken) where T : class;
        void GetList<T>(Func<T, bool> action) where T : class;

        void GetDataRecordList(Func<IDataRecord, bool> action);
        void GetDataRecordFirst(Action<IDataRecord> action);
    }
}
