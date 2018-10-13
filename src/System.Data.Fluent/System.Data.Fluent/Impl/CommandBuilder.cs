using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Fluent.Abstraction;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Fluent.Impl
{
    internal class CommandBuilder : ICommandBuilder, IFunctionBuilder
    {
        ConnectionStringSettings connectionStringSettings;
        string command;
        CommandType commandType;

        public CommandBuilder(ConnectionStringSettings connectionStringSettings, string command, CommandType commandType)
        {
            this.connectionStringSettings = connectionStringSettings;
            this.command = command;
            this.commandType = commandType;
        }

        #region ICommandBuilder

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Execute(Action<IDataParameterCollection> inspectParameters)
        {
            throw new NotImplementedException();
        }

        public T Execute<T>()
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(Action<IDataParameterCollection> inspectParameters)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken, Action<IDataParameterCollection> inspectParameters)
        {
            throw new NotImplementedException();
        }


        public T GetFirst<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetFirstAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetFirstAsync<T>(CancellationTokenSource cancellationTokenSource)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetList<T>()
        {
            throw new NotImplementedException();
        }

        public void GetList<T>(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public void GetList<T>(IObservable<T> observable)
        {
            throw new NotImplementedException();
        }

        public void GetList(Action<IDataRecord> action)
        {
            throw new NotImplementedException();
        }

        public void GetList(IObserver<IDataRecord> observable)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetListAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync<T>(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync<T>(IObservable<T> observable)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync(Action<IDataRecord> action)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync(IObserver<IDataRecord> observable)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetListAsync<T>(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync<T>(CancellationToken cancellationToken, Action<T> action)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync<T>(CancellationToken cancellationToken, IObserver<T> observable)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync(CancellationToken cancellationToken, Action<IDataRecord> action)
        {
            throw new NotImplementedException();
        }

        public Task GetListAsync(CancellationToken cancellationToken, IObserver<IDataRecord> observable)
        {
            throw new NotImplementedException();
        }

        public IParameterBuilder Parameters(Action<IParameterBuilder> parametersAction)
        {
            throw new NotImplementedException();
        }

        IFunctionBuilder IFunctionBuilder.Parameters(Action<IParameterBuilder> parametersAction)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFunctionBuilder

        public Task<T> ExecuteAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteAsync<T>(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal


        #endregion
    }
}
