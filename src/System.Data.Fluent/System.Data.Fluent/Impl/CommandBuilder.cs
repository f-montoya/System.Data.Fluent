using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Fluent.Abstraction;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal sealed class CommandBuilder : ICommandBuilder, IFunctionBuilder
    {
        readonly Context context;
        readonly IList<Action<IParameterBuilder>> parametersActionList = new List<Action<IParameterBuilder>>();

        public CommandBuilder(Context context)
        {
            this.context = context;
        }

        #region ICommandBuilder

        public ICommandBuilder Parameters(Action<IParameterBuilder> parametersAction)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync()
        {
            return ExecuteAsync(CancellationToken.None);
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            context.CancellationToken = cancellationToken;

            await ExecuteInternalAsync(async cmd =>
            {
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            });
        }

        public Task ExecuteAsync(Action<DbParameterCollection> inspectParameters)
        {
            return ExecuteAsync(inspectParameters, CancellationToken.None);
        }

        public async Task ExecuteAsync(Action<DbParameterCollection> inspectParameters, CancellationToken cancellationToken)
        {
            context.CancellationToken = cancellationToken;

            await ExecuteInternalAsync(async cmd =>
            {
                await cmd.ExecuteNonQueryAsync(cancellationToken);
                inspectParameters.Invoke(cmd.Parameters);
            });
        }

        public Task<T> GetScalarAsync<T>()
        {
            return GetScalarAsync<T>(CancellationToken.None);
        }

        public async Task<T> GetScalarAsync<T>(CancellationToken cancellationToken)
        {
            context.CancellationToken = cancellationToken;
            object value = null;

            await ExecuteInternalAsync(async cmd => value = await cmd.ExecuteScalarAsync(cancellationToken));

            return context.DbEngineProvider.ValueProvider.ConvertDbValue<T>(value);
        }

        public Task<IList<T>> GetScalarListAsync<T>()
        {
            return GetScalarListAsync<T>(CancellationToken.None);
        }

        public async Task<IList<T>> GetScalarListAsync<T>(CancellationToken cancellationToken)
        {
            context.CancellationToken = cancellationToken;
            var list = new List<T>();

            await GetScalarListAsync<T>(v => { list.Add(v); return true; }, cancellationToken);

            return list;
        }

        public Task GetScalarListAsync<T>(Func<T, bool> action)
        {
            return GetScalarListAsync<T>(action, CancellationToken.None);
        }

        public async Task GetScalarListAsync<T>(Func<T, bool> action, CancellationToken cancellationToken)
        {
            context.CancellationToken = cancellationToken;

            await ExecuteQueryInternalAsync(async reader =>
            {
                var moreRecords = true;

                while (moreRecords && await reader.ReadAsync())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var value = reader.GetValue(0);
                    var valueOfT = context.DbEngineProvider.ValueProvider.ConvertDbValue<T>(value);

                    moreRecords = action(valueOfT);
                }
            });

        }

        public Task<T> GetFirstAsync<T>() where T : class
        {
            return GetFirstAsync<T>(CancellationToken.None);
        }

        public async Task<T> GetFirstAsync<T>(CancellationToken cancellationToken) where T : class
        {
            context.CancellationToken = cancellationToken;
            T value = null;

            await GetListInternalAsync<T>( v => { value = v; return false; });

            return value;
        }

        public async Task GetFirstAsync(Action<IDataRecord> action)
        {
            await ExecuteQueryInternalAsync(context)
        }

        public Task GetFirstAsync(Action<IDataRecord> action, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetListAsync<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetListAsync<T>(CancellationToken cancellationToken) where T : class
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync<T>(Func<T, bool> action) where T : class
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync<T>(Func<T, bool> action, CancellationToken cancellationToken) where T : class
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync(Func<IDataRecord, bool> action)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync(Func<IDataRecord, bool> action, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }



        #endregion


        #region Private members

        async Task ExecuteInternalAsync(Func<DbCommand, Task> actionAsync)
        {
            var engine = context.DbEngineProvider.ProviderFactory;

            using (var connection = engine.CreateConnection())
            using (var command = engine.CreateCommand())
            {
                connection.ConnectionString = context.ConnectionString;
                command.CommandText = context.Command;
                command.CommandType = context.CommandType;
                command.Parameters.AddRange(GetParameters());

                context.CancellationToken.ThrowIfCancellationRequested();

                await connection.OpenAsync(context.CancellationToken);
                await actionAsync.Invoke(command);
            }
        }

        IEnumerable GetParameters()
        {
            foreach (var parameterAction in parametersActionList)
            {
                var parameterBuilder = new ParameterBuilder(context);

                parameterAction.Invoke(parameterBuilder);

                foreach (var parameter in parameterBuilder.Parameters)
                {
                    yield return parameter;
                }
            }
        }

        async Task ExecuteQueryInternalAsync(Func<DbDataReader, Task> actionAsync)
        {
            await ExecuteInternalAsync(async command =>
            {
                using (var reader = await command.ExecuteReaderAsync(context.CancellationToken))
                {
                    await actionAsync.Invoke(reader);
                }
            });
        }

        async Task GetListInternalAsync<T>(Func<T, bool> action)
        {
            await ExecuteQueryInternalAsync(async reader =>
            {
                var dataMappingList = CreateDataMappingList<T>(reader);
                var moreRecords = true;

                while (moreRecords && await reader.ReadAsync())
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    moreRecords = action.Invoke(BuildEntity<T>(reader, dataMappingList));
                }
            });
        }

        T BuildEntity<T>(IDataRecord record, IEnumerable<DataMapping> mappingList)
        {
            if (typeof(T) == typeof(IDataRecord))
            {
                return (T)record;
            }

            var instance = Activator.CreateInstance<T>();

            foreach (var mapping in mappingList)
            {
                var recordValue = record.GetValue(mapping.Index);
                var propertyValue = context.DbEngineProvider.ValueProvider.ConvertDbValue(recordValue, mapping.Property.PropertyType);

                mapping.Property.SetValue(instance, propertyValue);
            }

            return instance;
        }

        IEnumerable<DataMapping> CreateDataMappingList<T>(IDataReader reader)
        {
            var typeofT = typeof(T);

            if (typeofT == typeof(IDataReader))
            {
                yield break;
            }

            for (var index = 0; index < reader.FieldCount; index++)
            {
                var fieldName = reader.GetName(index);
                var property = typeofT.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property != null)
                {
                    yield return new DataMapping { Index = index, Property = property };
                }
            }
        }


        class DataMapping
        {
            public int Index { get; set; }

            public PropertyInfo Property { get; set; }
        }

        #endregion
    }
}
