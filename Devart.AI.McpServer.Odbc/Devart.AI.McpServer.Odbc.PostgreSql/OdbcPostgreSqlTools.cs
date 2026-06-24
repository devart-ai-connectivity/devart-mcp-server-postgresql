// --------------------------------------------------------------------------
// <copyright file="OdbcPostgreSqlTools.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using ModelContextProtocol.Server;
using Devart.AI.McpServer.Odbc.PostgreSql.Tools;

namespace Devart.AI.McpServer.Odbc.PostgreSql
{
  internal static class OdbcPostgreSqlTools
  {
    public static List<McpServerTool> CreateTools(McpConfiguration configuration)
      => OdbcTools.CreateBuilder(configuration)
        .Add(new OdbcPostgreSqlPrimaryKeysTool(configuration))
        .Add(new OdbcPostgreSqlForeignKeysTool(configuration))
        .Build();
  }
}
