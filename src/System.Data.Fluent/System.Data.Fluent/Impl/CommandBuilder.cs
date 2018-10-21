using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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


        #region ICommandBuilder, IFunctionBuilder

        void ICommandBuilder.Execute()
        {
            ExecuteInternal(context, command => command.ExecuteNonQuery());
        }

        void ICommandBuilder.Execute(Action<IDataParameterCollection> inspectParameters)
        {
            ExecuteInternal(context, command =>
            {
                command.ExecuteNonQuery();
                inspectParameters.Invoke(command.Parameters);
            });
        }

        T IFunctionBuilder.Execute<T>()
        {
            T retval = default(T);

            ExecuteInternal(context, command =>
            {
                command.Parameters.Add(context.DbEngineProvider.CreateReturnParameter("retval", typeof(T)));
                command.ExecuteNonQuery();
                retval = context.DbValueProvider.ConvertDbValue<T>(command.Parameters["retvat"]);
            });

            return retval;
        }

        T ICommandBuilder.GetFirst<T>()
        {
            var value = default(T);

            GetListInternal<T>(t => { value = t; return false; });

            return value;
        }

        IList<T> ICommandBuilder.GetList<T>()
        {
            return ((ICommandBuilder)this).GetList<T>(CancellationToken.None);
        }

        IList<T> ICommandBuilder.GetList<T>(CancellationToken cancellationToken)
        {
            var list = new List<T>();

            ((ICommandBuilder)this).GetList<T>(t =>
            {
                list.Add(t);
                return !cancellationToken.IsCancellationRequested;
            });

            if(cancellationToken.IsCancellationRequested)
            {
                list.Clear();
            }

            return list;
        }

        void ICommandBuilder.GetList<T>(Func<T, bool> action)
        {
            GetListInternal(action);
        }

        void ICommandBuilder.GetDataRecordList(Func<IDataRecord, bool> action)
        {
            GetListInternal(action);
        }

        void ICommandBuilder.GetDataRecordFirst(Action<IDataRecord> action)
        {
            GetListInternal<IDataRecord>(t => { action.Invoke(t); return false; });
        }

        T ICommandBuilder.GetScalar<T>()
        {
            T value = default(T);

            GetListInternal<IDataRecord>(rec =>
            {
                value = context.DbValueProvider.ConvertDbValue<T>(rec.GetValue(0));
                return false;
            });

            return value;
        }

        IList<T> ICommandBuilder.GetScalarList<T>()
        {
            return ((ICommandBuilder)this).GetScalarList<T>(CancellationToken.None);
        }

        IList<T> ICommandBuilder.GetScalarList<T>(CancellationToken cancellationToken)
        {
            var list = new List<T>();

            ((ICommandBuilder)this).GetScalarList<T>(value =>
            {
                list.Add(value);
                return !cancellationToken.IsCancellationRequested;
            });

            if(cancellationToken.IsCancellationRequested)
            {
                list.Clear();
            }

            return list;
        }

        void ICommandBuilder.GetScalarList<T>(Func<T, bool> action)
        {
            GetListInternal<IDataRecord>(rec =>
            {
                var value = rec.GetValue(0);
                var valueConverted = context.DbValueProvider.ConvertDbValue<T>(value);
                return action.Invoke(valueConverted);
            });
        }

        ICommandBuilder ICommandBuilder.Parameters(Action<IParameterBuilder> parametersAction)
        {
            parametersActionList.Add(parametersAction);
            return this;
        }

        IFunctionBuilder IFunctionBuilder.Parameters(Action<IParameterBuilder> parametersAction)
        {
            parametersActionList.Add(parametersAction);
            return this;
        }

        #endregion

        #region Private members


        void ExecuteInternal(Context context, Action<IDbCommand> action)
        {
            var engine = context.DbEngineProvider;

            using (var connection = engine.CreateConnection(context.ConnectionStringSettings.ConnectionString))
            using (var command = engine.CreateCommand(connection))
            {
                connection.Open();

                command.CommandText = context.Command;
                command.CommandType = context.CommandType;
                command.Parameters.AddRange(GetParameters());

                action.Invoke(command);
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

        void ExecuteQueryInternal(Context context, Action<IDataReader> action)
        {
            ExecuteInternal(context, command =>
            {
                using (var reader = command.ExecuteReader())
                {
                    action.Invoke(reader);
                }
            });
        }

        void GetListInternal<T>(Func<T, bool> action)
        {
            ExecuteQueryInternal(context, reader =>
            {
                var dataMappingList = CreateDataMappingList<T>(reader);
                var moreRecords = true;

                while (moreRecords && reader.Read())
                {
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
                var propertyValue = context.DbValueProvider.ConvertDbValue(recordValue, mapping.Property.PropertyType);

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
