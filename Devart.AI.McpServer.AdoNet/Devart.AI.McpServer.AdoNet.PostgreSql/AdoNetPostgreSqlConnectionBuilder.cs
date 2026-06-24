// --------------------------------------------------------------------------
// <copyright file="AdoNetPostgreSqlConnectionBuilder.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Devart.AI.McpServer.Interfaces;
using Devart.Data.PostgreSql;

namespace Devart.AI.McpServer.AdoNet.PostgreSql
{
  internal sealed class AdoNetPostgreSqlConnectionBuilder : IConnectionBuilder
  {
    public async Task<DbConnection> CreateConnectionAsync(McpConfiguration configuration, CancellationToken cancellationToken)
    {
      var connection = new PgSqlConnection(configuration.CompleteConnectionString);
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
      return connection;
    }
  }
}
