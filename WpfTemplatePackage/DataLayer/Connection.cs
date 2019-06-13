using System;
using System.Data.Common;
using System.Transactions;
using WpfTemplate.DataLayer.Context;
using Microsoft.Extensions.Logging;

namespace WpfTemplate.DataLayer
{
	public sealed class Connection : IDisposable, IEnlistmentNotification
	{
		private readonly ConnectionFactory _connectionFactory;
		private readonly AmbientContext<Connection> _ambientContext;

		private ILogger _logger;
		private DbConnection _connection;
		private DbTransaction _transaction;
		private Transaction _scopeTransaction;

		private Connection _parent;
		private bool _isInTransactionScope;

		public DatabaseProvider Provider { get; private set; }

		public Connection(ConnectionFactory connectionFactory, DatabaseProvider provider, ILogger logger)
		{
			_connectionFactory = connectionFactory;
			_ambientContext = new AmbientContext<Connection>();

			RegisterToTransactionScope();
			Initialize(provider, logger);
		}

		private void RegisterToTransactionScope()
		{
			if (_connectionFactory.ShouldRegisterToTransactionScope())
			{
				Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
			}

			_isInTransactionScope = Transaction.Current != null;
			_scopeTransaction = Transaction.Current;
		}

		private void Initialize(DatabaseProvider provider, ILogger logger)
		{
			Connection parent = _ambientContext.Get();
			
			if (parent != null)
			{
				_parent = parent;
				_logger = parent._logger;
				
				Provider = parent.Provider;

				_connection = parent._connection;
				_transaction = parent._transaction;
			}
			else
			{
				_logger = logger;
				Provider = provider;

				(_connection, _transaction) = _connectionFactory.CreateDbConnection();

				_ambientContext.Save(this);
			}
		}

		public Command CreateCommand()
		{
			DbCommand command = _connection.CreateCommand();

			if (_transaction != null)
			{
				command.Transaction = _transaction;
			}

			return new Command(command, this, _logger);
		}

		public void Dispose()
		{
			if (_parent != null)
			{
				return;
			}
			
			if (_isInTransactionScope)
			{
				return;
			}

			try
			{
				_transaction.Commit();
			}
			catch (Exception exception)
			{
				try
				{
					_logger.LogError(exception, "Transaction has failed. Trying to rollback.");
					_transaction.Rollback();
				}
				catch (Exception)
				{
					// Do nothing here; transaction is not active.
				}
			}

			_transaction.Dispose();
			_connection.Dispose();
		}

		void IEnlistmentNotification.Commit(Enlistment enlistment)
		{
			_transaction?.Commit();
			
			_transaction?.Dispose();
			_connection.Dispose();
			
			_connectionFactory.UnregisterConnection(_scopeTransaction);

			enlistment.Done();
		}

		void IEnlistmentNotification.Rollback(Enlistment enlistment)
		{
			_transaction?.Rollback();
			_transaction?.Dispose();

			_connection.Close();
			_connection.Dispose();
			
			_connectionFactory.UnregisterConnection(_scopeTransaction);

			enlistment.Done();
		}

		void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
		{
			preparingEnlistment.Prepared();
		}

		void IEnlistmentNotification.InDoubt(Enlistment enlistment)
		{
			enlistment.Done();
		}
	}
}