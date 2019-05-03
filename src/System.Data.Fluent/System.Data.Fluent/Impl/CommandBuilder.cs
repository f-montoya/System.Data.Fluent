using System.Collections.Generic;
using System.Data.Fluent.Abstraction;

namespace System.Data.Fluent.Impl
{
    internal sealed class CommandBuilder : ICommandBuilder
    {
        readonly Command command;

        public CommandBuilder(Command command)
        {
            Check.IsNull(command, nameof(command));

            this.command = command;
        }

        public ICommandBuilder CommandTimeout(int timeout)
        {
            command.CommandTimeout = timeout;
            return this;
        }

        public ICommandBuilder Parameters(Action<IParameterBuilder> parametersAction)
        {
            command.ParametersActionList.Add(parametersAction);
            return this;
        }

        public int Execute()
        {
            return command.Execute(cmd => cmd.ExecuteNonQuery());
        }

        public int Execute(Action<IDataParameterCollection> inspectParameters)
        {
            Check.IsNull(inspectParameters, nameof(inspectParameters));

            return command.Execute(cmd =>
            {
                var r = cmd.ExecuteNonQuery();
                inspectParameters(cmd.Parameters);
                return r;
            });
        }

        public T GetScalar<T>()
        {
            var value = command.Execute(cmd => cmd.ExecuteScalar());

            return command.Context.DbValueConverter.ConvertDbValue<T>(value);
        }

        public IList<T> GetScalarList<T>()
        {
            var list = new List<T>();

            command.GetScalarList<T>(value =>
            {
                list.Add(value);
                return true;
            });

            return list;
        }

        public void GetScalarList<T>(Func<T, bool> callback)
        {
            Check.IsNull(callback, nameof(callback));

            command.GetScalarList(callback);
        }

        public T GetFirst<T>() where T : class
        {
            return command.GetFirst<T>();
        }

        public void GetFirst(Action<IDataRecord> callback)
        {
            Check.IsNull(callback, nameof(callback));

            command.ExecuteQuery(reader =>
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

            command.GetList<T>(data => 
            {
                list.Add(data);
                return true;
            });

            return list;
        }

        public void GetList<T>(Func<T, bool> callback) where T : class
        {
            Check.IsNull(callback, nameof(callback));

            command.GetList(callback);
        }

        public void GetList(Func<IDataRecord, bool> callback)
        {
            Check.IsNull(callback, nameof(callback));

            command.GetList(callback);
        }
    }
}
