using System.Data.Fluent.Abstraction;

namespace System.Data.Fluent.Impl
{
    internal sealed class FunctionBuilder : IFunctionBuilder
    {
        readonly Command command;

        public FunctionBuilder(Command command)
        {
            Check.IsNull(command, nameof(command));

            this.command = command;
        }

        public IFunctionBuilder CommandTimeout(int timeout)
        {
            command.CommandTimeout = timeout;
            return this;
        }
        
        public IFunctionBuilder Parameters(Action<IParameterBuilder> parametersAction)
        {
            command.ParametersActionList.Add(parametersAction);
            return this;
        }

        public T Execute<T>()
        {
            return command.ExecuteFunction<T>();
        }

    }
}
