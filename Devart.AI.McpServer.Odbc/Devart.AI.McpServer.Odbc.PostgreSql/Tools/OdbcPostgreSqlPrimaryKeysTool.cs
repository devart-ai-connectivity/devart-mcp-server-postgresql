// --------------------------------------------------------------------------
// <copyright file="OdbcPostgreSqlPrimaryKeysTool.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Devart.AI.McpServer.Extensions;
using Devart.AI.McpServer.Interfaces;
using Devart.AI.McpServer.Tools;

namespace Devart.AI.McpServer.Odbc.PostgreSql.Tools
{
  internal sealed class OdbcPostgreSqlPrimaryKeysTool(McpConfiguration serverConfiguration) : PrimaryKeysTool(serverConfiguration)
  {
    protected override async Task<DataTable> GetMetadataTable(
      DbConnection connection,
      string schema,
      string tableName,
      IServiceProvider services,
      CancellationToken cancellationToken)
    {
      const string sql =
"""
SELECT
  tc.constraint_name AS "PK_NAME",
  kcu.column_name    AS "COLUMN_NAME"
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
  ON  kcu.constraint_catalog = tc.constraint_catalog
  AND kcu.constraint_schema  = tc.constraint_schema
  AND kcu.constraint_name    = tc.constraint_name
WHERE tc.constraint_type = 'PRIMARY KEY'
  AND tc.table_catalog = ?
  AND tc.table_schema  = ?
  AND tc.table_name    = ?
ORDER BY tc.constraint_name, kcu.ordinal_position
""";

      var database = services.GetRequiredService<IDatabase>();
      var commandHelper = services.GetRequiredService<ICommandHelper>();

      await using var reader = await database.ExecuteReaderAsync(
        connection,
        sql,
        cmd =>
        {
          commandHelper.AddParameter(cmd, connection.Database);
          commandHelper.AddParameter(cmd, schema);
          commandHelper.AddParameter(cmd, tableName);
        },
        cancellationToken
      ).ConfigureAwait(false);

      return await reader.ToDataTableAsync(OdbcConstants.PrimaryKeysCollectionName, cancellationToken).ConfigureAwait(false);
    }
  }
}
