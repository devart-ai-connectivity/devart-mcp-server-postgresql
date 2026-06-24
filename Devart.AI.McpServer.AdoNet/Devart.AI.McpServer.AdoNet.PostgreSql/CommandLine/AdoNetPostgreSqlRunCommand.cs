// --------------------------------------------------------------------------
// <copyright file="AdoNetPostgreSqlRunCommand.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Devart.AI.McpServer.AdoNet.CommandLine;
using Devart.AI.McpServer.AdoNet.PostgreSql.Properties;
using Devart.AI.McpServer.Interfaces;

namespace Devart.AI.McpServer.AdoNet.PostgreSql.CommandLine
{
  internal sealed class AdoNetPostgreSqlRunCommand : AdoNetRunCommand
  {
    protected override void SetupConnectionBuilder(IHostApplicationBuilder builder)
    {
      builder.Services.AddSingleton<IConnectionBuilder, AdoNetPostgreSqlConnectionBuilder>();
    }

    protected override void RegisterTools(IMcpServerBuilder serverBuilder, McpConfiguration configuration)
      => serverBuilder.WithTools(AdoNetTools.CreateTools(configuration));

    protected override string ProductFullName => ProductInfo.ProductFullName;

    protected override McpAppSettings CreateAppSettings() => new AdoNetPostgreSqlAppSettings();
  }
}