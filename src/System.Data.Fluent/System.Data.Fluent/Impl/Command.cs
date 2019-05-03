using System.Collections.Generic;
using System.Data.Common;
using System.Data.Fluent.Abstraction;
using System.Reflection;

namespace System.Data.Fluent.Impl
{
    internal sealed class Command
    {
        readonly DbContext context;
        readonly string commandText;
        readonly CommandType commandType;

        public Command(DbContext context, string commandText, CommandType commandType)
        {
            this.context = context;
            this.commandText = commandText;
            this.commandType = commandType;
        }

        public DbContext Context => context;

        public int CommandTimeout { get; set; }

        public IList<Action<IParameterBuilder>> ParametersActionList { get; } = new List<Action<IParameterBuilder>>();

        public T Execute<T>(Func<DbCommand, T> callback)
        {
            using (var command = CreateCommand())
            {
                return callback.Invoke(command);
            }
        }

        public void Execute(Action<DbCommand> callback)
        {
            using (var command = CreateCommand())
            {
                callback.Invoke(command);
            }
        }

        public void ExecuteQuery(Action<IDataReader> callback)
        {
            Execute(command =>
            {
                using (var reader = command.ExecuteReader())
                {
                    callback.Invoke(reader);
                }
            });
        }

        public T ExecuteFunction<T>()
        {
            var returnValue = default(T);

            Execute(command =>
            {
                var returnParameter = context.DbParameterFactory.CreateOutputParameter("retval", typeof(T));
                returnParameter.Direction = ParameterDirection.ReturnValue;

                command.ExecuteNonQuery();

                var value = command.Parameters["retval"].Value;
                returnValue = context.DbValueConverter.ConvertDbValue<T>(value);
            });

            return returnValue;
        }

        public T GetFirst<T>() where T : class
        {
            var value = default(T);

            ExecuteQuery(reader =>
            {
                if (reader.Read())
                {
                    var dataMappingList = CreateDataMappingList<T>(reader);
                    value = BuildEntity<T>(reader, dataMappingList);
                }
            });

            return value;
        }

        public void GetList<T>(Func<T, bool> callback)
        {
            ExecuteQuery(reader =>
            {
                var dataMappingList = CreateDataMappingList<T>(reader);
                var moreRecords = true;

                while (moreRecords && reader.Read())
                {
                    var entity = BuildEntity<T>(reader, dataMappingList);
                    moreRecords = callback.Invoke(entity);
                }
            });
        }

        public void GetScalarList<T>(Func<T, bool> callback)
        {
            ExecuteQuery(reader =>
            {
                var moreRecords = true;

                while (moreRecords && reader.Read())
                {
                    var value = context.DbValueConverter.ConvertDbValue<T>(reader.GetValue(0));
                    moreRecords = callback.Invoke(value);
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
                var propertyValue = context.DbValueConverter.ConvertDbValue(recordValue, mapping.Property.PropertyType);

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

        DbCommand CreateCommand()
        {
            var command = context.DbProviderFactory.CreateCommand();
            command.Connection = context.Connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Parameters.AddRange(GetParameters());

            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }

            return command;
        }

        DbParameter[] GetParameters()
        {
            var list = new List<DbParameter>();

            foreach (var parameterAction in ParametersActionList)
            {
                var parameterBuilder = new ParameterBuilder(context.DbParameterFactory);

                parameterAction.Invoke(parameterBuilder);

                list.AddRange(parameterBuilder.Parameters);
            }

            return list.ToArray();
        }

    }
}
