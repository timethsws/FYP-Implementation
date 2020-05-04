using System;
namespace SinSense.Infastructure
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

            if (dbConfig.StartsWith("SQLServer", StringComparison.InvariantCultureIgnoreCase))
            {
                builder.UseSqlServer(connectionString, options =>
                              options.MigrationsAssembly("SinSense.SQLServer"));
                return;
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
                configName, dbConfig));
        }
        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

            return formatted;
        }
    }
}
