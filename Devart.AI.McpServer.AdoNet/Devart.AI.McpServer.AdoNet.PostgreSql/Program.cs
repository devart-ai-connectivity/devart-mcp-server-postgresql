// --------------------------------------------------------------------------
// <copyright file="Program.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

using System.Threading.Tasks;
using Devart.AI.McpServer.AdoNet.PostgreSql.CommandLine;

namespace Devart.AI.McpServer.AdoNet.PostgreSql
{
  internal class Program
  {
    public static Task<int> Main(string[] args) => AdoNetLauncher.Create(new AdoNetPostgreSqlRunCommand()).RunAsync(args);
  }
}
