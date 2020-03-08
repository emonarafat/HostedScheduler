namespace WorkerService1
{
    using Dapper;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;

    using WorkerService1.Helpers;

    /// <summary>
    /// Defines the <see cref="Worker" />
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// Defines the _appConfig
        /// </summary>
        private readonly AppConfig _appConfig;

        /// <summary>
        /// Defines the _dbConnection
        /// </summary>
        private readonly IDbConnection _dbConnection;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<Worker> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{Worker}"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        /// <param name="appConfig">The appConfig<see cref="AppConfig"/></param>
        /// <param name="dbConnection">The dbConnection<see cref="IDbConnection"/></param>
        public Worker(ILogger<Worker> logger, IOptions<AppConfig> options, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
            _appConfig = options.Value;
        }

        /// <summary>
        /// The ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken">The stoppingToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var databases = (await _dbConnection.QueryAsync<string>(string.Format(Constants.DataBaseListQueryByPrefix, _appConfig.DbPrefix))).AsList();
                using (var transaction = new TransactionScope())
                {
                    foreach (var db in databases)
                    {
                        var tables = (await _dbConnection.QueryAsync<string>(string.Format(Constants.ALL_TABLES, db))).AsList();

                        foreach (var table in tables)
                        {

                            try
                            {
                                var i = await _dbConnection.ExecuteAsync(string.Format(Constants.REBUILD_SCRIPT, table), commandType: CommandType.Text);

                            }
                            catch
                            {

                            }

                        }

                    }
                    transaction.Complete();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
