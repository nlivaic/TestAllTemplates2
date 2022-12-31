using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestAllPipelines2.Data;
using Xunit.Abstractions;

namespace TestAllPipelines2.Api.Tests.Helpers
{
    public class SQLiteDbBuilder
    {
        private readonly DbContextOptions<TestAllPipelines2DbContext> _options;

        /// <summary>
        /// Creates a new DbContext with an open database connection already set up.
        /// Make sure not to call `context.Database.OpenConnection()` from your code.
        /// </summary>
        public SQLiteDbBuilder(
            ITestOutputHelper testOutput,
            SqliteConnection connection,
            List<string> logs = null)   // This parameter is just for demo purposes, to show you can output logs.
        {
            _options = new DbContextOptionsBuilder<TestAllPipelines2DbContext>()
                .UseLoggerFactory(new LoggerFactory(
                    new[] {
                        new TestLoggerProvider(
                            message => testOutput.WriteLine(message),
                            // message => logs?.Add(message),
                            LogLevel.Information
                        )
                    }
                ))
                .UseSqlite(connection)
                .Options;
        }

        internal TestAllPipelines2DbContext BuildContext() =>
            new TestAllPipelines2DbContext(_options);
    }
}
