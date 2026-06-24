// --------------------------------------------------------------------------
// <copyright file="OdbcPostgreSqlForeignKeysTool.cs" company="Devart">
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
using Devart.AI.McpServer.Tools;
using Devart.AI.McpServer.Interfaces;

namespace Devart.AI.McpServer.Odbc.PostgreSql.Tools
{
  internal sealed class OdbcPostgreSqlForeignKeysTool(McpConfiguration serverConfiguration) : ForeignKeysTool(serverConfiguration)
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
  tc.constraint_name    AS "FK_NAME",
  kcu.column_name       AS "FKCOLUMN_NAME",
  kcu2.table_schema     AS "PKTABLE_SCHEM",
  kcu2.table_name       AS "PKTABLE_NAME",
  kcu2.column_name      AS "PKCOLUMN_NAME",
  rc.update_rule        AS "UPDATE_RULE",
  rc.delete_rule        AS "DELETE_RULE"
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
  ON  kcu.constraint_catalog = tc.constraint_catalog
  AND kcu.constraint_schema  = tc.constraint_schema
  AND kcu.constraint_name    = tc.constraint_name
JOIN information_schema.referential_constraints AS rc
  ON  rc.constraint_catalog = tc.constraint_catalog
  AND rc.constraint_schema  = tc.constraint_schema
  AND rc.constraint_name    = tc.constraint_name
JOIN information_schema.key_column_usage AS kcu2
  ON  kcu2.constraint_catalog = rc.unique_constraint_catalog
  AND kcu2.constraint_schema  = rc.unique_constraint_schema
  AND kcu2.constraint_name    = rc.unique_constraint_name
  AND kcu2.ordinal_position   = kcu.position_in_unique_constraint
WHERE
      tc.constraint_type = 'FOREIGN KEY'
  AND tc.table_catalog   = ?
  AND tc.table_schema    = ?
  AND tc.table_name      = ?
ORDER BY
  tc.constraint_name, kcu.ordinal_position
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

      return await reader.ToDataTableAsync(OdbcConstants.ForeignKeysCollectionName, cancellationToken).ConfigureAwait(false);
    }
  }
}
