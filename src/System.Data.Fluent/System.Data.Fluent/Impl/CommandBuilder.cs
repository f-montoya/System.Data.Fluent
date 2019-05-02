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
        readonly DbContext context;
        readonly string commandText;
        readonly CommandType commandType;
        readonly IList<Action<IParameterBuilder>> parametersActionList = new List<Action<IParameterBuilder>>();

        public CommandBuilder(DbContext context, string commandText, CommandType commandType)
        {
            this.context = context;
            this.commandText = commandText;
            this.commandType = commandType;
        }

        #region ICommandBuilder

        public ICommandBuilder Parameters(Action<IParameterBuilder> parametersAction)
        {
            parametersActionList.Add(parametersAction);
            return this;
        }

        public int Execute()
        {
            return ExecuteInternal(command => command.ExecuteNonQuery());
        }

        public int Execute(Action<IDataParameterCollection> inspectParameters)
        {
            Check.IsNull(inspectParameters, nameof(inspectParameters));

            return ExecuteInternal(command =>
            {
                var r = command.ExecuteNonQuery();
                inspectParameters(command.Parameters);
                return r;
            });
        }

        public T GetScalar<T>()
        {
            var value = ExecuteInternal(command => command.ExecuteScalar());

            return context.DbValueConverter.ConvertDbValue<T>(value);
        }

        public IList<T> GetScalarList<T>()
        {
            var list = new List<T>();

            GetScalarList<T>(value =>
            {
                list.Add(value);
                return true;
            });

            return list;
        }

        public void GetScalarList<T>(Func<T, bool> callback)
        {
            ExecuteQueryInternal(reader =>
            {
                var dataMappingList = CreateDataMappingList<T>(reader);
                var moreRecords = true;

                while (moreRecords && reader.Read())
                {
                    var value = context.DbValueConverter.ConvertDbValue<T>(reader.GetValue(0));
                    moreRecords = callback.Invoke(value);
                }
            });
        }

        public T GetFirst<T>() where T : class
        {
            var value = default(T);

            ExecuteQueryInternal(reader =>
            {
                if (reader.Read())
                {
                    var dataMappingList = CreateDataMappingList<T>(reader);
                    value = BuildEntity<T>(reader, dataMappingList);
                }
            });

            return value;
        }

        public void GetFirst(Action<IDataRecord> callback)
        {
            ExecuteQueryInternal(reader =>
            {
                if (reader.Read())
                {
                    callback.Invoke(reader);
                }
            });
        }

        public IList<T> GetList<T>() where T : class
        {
            var list = new List<T>();

            GetList<T>(t => 
            {
                list.Add(t);
                return true;
            });

            return list;
        }

        public void GetList<T>(Func<T, bool> callback) where T : class
        {
            GetListInternal(callback);
        }

        public void GetList(Func<IDataRecord, bool> callback)
        {
            GetListInternal(callback);
        }

        #endregion

        #region IFunctionBuilder

        IFunctionBuilder IFunctionBuilder.Parameters(Action<IParameterBuilder> parametersAction)
        {
            parametersActionList.Add(parametersAction);
            return this;
        }

        public T Execute<T>()
        {
            var returnValue = default(T);

            ExecuteInternal(command =>
            {
                var returnParameter = context.DbParameterFactory.CreateOutputParameter("retval", typeof(T));
                returnParameter.Direction = ParameterDirection.ReturnValue;

                command.ExecuteNonQuery();

                var value = command.Parameters["retval"].Value;
                returnValue = context.DbValueConverter.ConvertDbValue<T>(value);
            });

            return returnValue;
        }

        #endregion

        #region Private members

        void ExecuteInternal(Action<DbCommand> callback)
        {
            using (var command = CreateCommand())
            {
                callback.Invoke(command);
            }
        }

        T ExecuteInternal<T>(Func<DbCommand, T> callback)
        {
            using (var command = CreateCommand())
            {
                return callback.Invoke(command);
            }
        }

        DbCommand CreateCommand()
        {
            var command = context.DbProviderFactory.CreateCommand();
            command.Connection = context.Connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Parameters.AddRange(GetParameters());

            if (command.Connection.State != ConnectionState.Open) command.Connection.Open();

            return command;
        }

        DbParameter[] GetParameters()
        {
            var list = new List<DbParameter>();

            foreach (var parameterAction in parametersActionList)
            {
                var parameterBuilder = new ParameterBuilder(context.DbParameterFactory);

                parameterAction.Invoke(parameterBuilder);

                list.AddRange(parameterBuilder.Parameters);
            }

            return list.ToArray();
        }

        void ExecuteQueryInternal(Action<IDataReader> callback)
        {
            ExecuteInternal(command =>
            {
                using (var reader = command.ExecuteReader())
                {
                    callback.Invoke(reader);
                }
            });
        }

        void GetListInternal<T>(Func<T, bool> callback)
        {
            ExecuteQueryInternal(reader =>
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

        #endregion
    }
}
