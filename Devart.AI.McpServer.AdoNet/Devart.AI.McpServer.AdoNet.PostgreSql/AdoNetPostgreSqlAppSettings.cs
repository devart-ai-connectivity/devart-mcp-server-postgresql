// --------------------------------------------------------------------------
// <copyright file="AdoNetPostgreSqlAppSettings.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

namespace Devart.AI.McpServer.AdoNet.PostgreSql
{
  internal sealed class AdoNetPostgreSqlAppSettings : McpAppSettings
  {
    public override string ServerName => $"Devart {Properties.ProductInfo.ProductFullName}";

    public override string SourceName => "PostgreSQL";

    public override bool OnPremise => true;
  }
}
