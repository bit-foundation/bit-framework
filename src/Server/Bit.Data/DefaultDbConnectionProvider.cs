﻿using Bit.Core.Contracts;
using Bit.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Bit.Data
{
    public class DefaultDbConnectionProvider<TDbConnection> : IDbConnectionProvider
        where TDbConnection : DbConnection, new()
    {
        private readonly IDictionary<string, DbConnectionAndTransactionPair> _connections =
            new Dictionary<string, DbConnectionAndTransactionPair>();

        public virtual IScopeStatusManager ScopeStatusManager { get; set; }

        public virtual DbTransaction GetDbTransaction(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            if (!_connections.ContainsKey(connectionString))
            {
                throw new InvalidOperationException($"No connection was created, either call {nameof(GetDbConnection)} or {nameof(GetDbConnectionAsync)}");
            }

            return _connections[connectionString].Transaction;
        }

        public virtual DbConnection GetDbConnection(string connectionString, bool rollbackOnScopeStatusFailure)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            if (!_connections.ContainsKey(connectionString))
            {
                TDbConnection newConnection = new TDbConnection { ConnectionString = connectionString };
                newConnection.Open();
                DbTransaction transaction = newConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                _connections.Add(connectionString, new DbConnectionAndTransactionPair(newConnection, transaction, rollbackOnScopeStatusFailure));
            }

            return _connections[connectionString].Connection;
        }

        public virtual async Task<DbConnection> GetDbConnectionAsync(string connectionString, bool rollbackOnScopeStatusFailure,
            CancellationToken cancellationToken)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            if (!_connections.ContainsKey(connectionString))
            {
                TDbConnection newDbConnection = new TDbConnection { ConnectionString = connectionString };
                await newDbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
                DbTransaction transaction = newDbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                _connections.Add(connectionString, new DbConnectionAndTransactionPair(newDbConnection, transaction, rollbackOnScopeStatusFailure));
            }

            return _connections[connectionString].Connection;
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            bool wasSucceeded = ScopeStatusManager.WasSucceeded();

            foreach (DbConnectionAndTransactionPair connectionAndTransaction in _connections.Values)
            {
                try
                {
                    if (!connectionAndTransaction.RollbackOnScopeStatusFailure)
                        connectionAndTransaction.Transaction.Commit();
                    else if (wasSucceeded)
                        connectionAndTransaction.Transaction.Commit();
                    else
                        connectionAndTransaction.Transaction.Rollback();
                }
                finally
                {
                    connectionAndTransaction.Connection.Dispose();
                }
            }
        }

        private class DbConnectionAndTransactionPair
        {
            public DbConnectionAndTransactionPair(TDbConnection connection, DbTransaction transaction, bool rollbackOnScopeStatusFailure)
            {
                Connection = connection;
                Transaction = transaction;
                RollbackOnScopeStatusFailure = rollbackOnScopeStatusFailure;
            }

            public TDbConnection Connection { get; }

            public DbTransaction Transaction { get; }

            public bool RollbackOnScopeStatusFailure { get; }
        }
    }
}
