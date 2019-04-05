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

        Task ExecuteAsync();
        Task ExecuteAsync(CancellationToken cancellationToken);

        Task ExecuteAsync(Action<DbParameterCollection> inspectParameters);
        Task ExecuteAsync(Action<DbParameterCollection> inspectParameters, CancellationToken cancellationToken);

        Task<T> GetScalarAsync<T>();
        Task<T> GetScalarAsync<T>(CancellationToken cancellationToken);

        Task<IList<T>> GetScalarListAsync<T>();
        Task<IList<T>> GetScalarListAsync<T>(CancellationToken cancellationToken);

        Task GetScalarListAsync<T>(Func<T, bool> action);
        Task GetScalarListAsync<T>(Func<T, bool> action, CancellationToken cancellationToken);

        Task<T> GetFirstAsync<T>() where T : class;
        Task<T> GetFirstAsync<T>(CancellationToken cancellationToken) where T : class;

        Task GetFirstAsync(Action<IDataRecord> action);
        Task GetFirstAsync(Action<IDataRecord> action, CancellationToken cancellationToken);

        Task<IList<T>> GetListAsync<T>() where T : class;
        Task<IList<T>> GetListAsync<T>(CancellationToken cancellationToken) where T : class;

        Task GetListAsync<T>(Func<T, bool> action) where T : class;
        Task GetListAsync<T>(Func<T, bool> action, CancellationToken cancellationToken) where T : class;

        Task GetListAsync(Func<IDataRecord, bool> action);
        Task GetListAsync(Func<IDataRecord, bool> action, CancellationToken cancellationToken);
    }
}
