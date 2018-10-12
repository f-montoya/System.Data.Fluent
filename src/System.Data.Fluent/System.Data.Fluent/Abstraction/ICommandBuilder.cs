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
        IParameterBuilder Parameters(Action<IParameterBuilder> parametersAction);

        void Execute();
        void Execute(Action<IDataParameterCollection> inspectParameters);

        Task ExecuteAsync();
        Task ExecuteAsync(Action<IDataParameterCollection> inspectParameters);

        Task ExecuteAsync(CancellationToken cancellationToken);
        Task ExecuteAsync(CancellationToken cancellationToken, Action<IDataParameterCollection> inspectParameters);

        IList<T> GetList<T>();
        void GetList<T>(Action<T> action);
        void GetList<T>(IObservable<T> observable);
        void GetList(Action<IDataRecord> action);
        void GetList(IObserver<IDataRecord> observable);

        Task<IList<T>> GetListAsync<T>();
        Task GetListAsync<T>(Action<T> action);
        Task GetListAsync<T>(IObservable<T> observable);
        Task GetListAsync(Action<IDataRecord> action);
        Task GetListAsync(IObserver<IDataRecord> observable);

        Task<IList<T>> GetListAsync<T>(CancellationToken cancellationToken);
        Task GetListAsync<T>(CancellationToken cancellationToken, Action<T> action);
        Task GetListAsync<T>(CancellationToken cancellationToken, IObserver<T> observable);
        Task GetListAsync(CancellationToken cancellationToken, Action<IDataRecord> action);
        Task GetListAsync(CancellationToken cancellationToken, IObserver<IDataRecord> observable);

        T GetFirst<T>();
        Task<T> GetFirstAsync<T>();
        Task<T> GetFirstAsync<T>(CancellationTokenSource cancellationTokenSource);
    }
}
