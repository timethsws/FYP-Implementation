﻿using System;
namespace SinSenseInfastructure
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Db context helper.
    /// </summary>
    public static class DbContextHelper
    {
        /// <summary>
        /// Configures the db context.
        /// </summary>
        /// <param name="appConfiguration">App configuration.</param>
        /// <param name="builder">Db Context Options Builder.</param>
        /// <param name="configName">Configuration key name.</param>
        public static void ConfigureDbContext(this IConfiguration appConfiguration, DbContextOptionsBuilder builder, string configName)
        {
            // Get the connection string configuration
            var dbConfig = appConfiguration.GetSection("AppSettings").GetValue<string>(configName);
            if (string.IsNullOrWhiteSpace(dbConfig))
            {
                // Invalid DB configuration found!
                throw new InvalidOperationException(string.Format(
                    "Either invalid or missing {0} configuration. Valid configuration should starts with SqlServer or SQLite",
                    configName));
            }

            var connectionString = appConfiguration.GetConnectionString(dbConfig);

            if (dbConfig.StartsWith("SqlServer", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotImplementedException("SQL Server is not supported at the moment");
            }

            if (dbConfig.StartsWith("SQLite", StringComparison.InvariantCultureIgnoreCase))
            {
                builder.UseSqlite(connectionString, options =>
                             options.MigrationsAssembly("SinSense.SQLite"));
                return;
            }

            if (dbConfig.StartsWith("MySQL", StringComparison.InvariantCultureIgnoreCase))
            {
                //builder.UseMySql(connectionString, options =>
                //             options
                //                 .ServerVersion(new Version(5, 7, 12), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)
                //                 .MigrationsAssembly("SinSense.MySQL"));
                //return;
                throw new NotImplementedException("MySQL is not supported at the moment");
            }

            // Invalid DB configuration found!
            throw new InvalidOperationException(string.Format(
                "Either invalid or missing {0} - {1}configuration. Valid configuration should starts with MySQL or SQLite",
                configName,dbConfig));
        }
    }
}
