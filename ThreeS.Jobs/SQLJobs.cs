using System.Linq;

namespace ThreeS.Jobs
{
    using Dapper;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using System.Data;

    /// <summary>
    /// Defines the <see cref="SQLJobs" />
    /// </summary>
    public class SQLJobs : ISQLJobs
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
        private readonly ILogger<SQLJobs> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLJobs"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{SQLJobs}"/></param>
        /// <param name="dbConnection">The dbConnection<see cref="IDbConnection"/></param>
        /// <param name="options">The options<see cref="IOptions{AppConfig}"/></param>
        public SQLJobs(ILogger<SQLJobs> logger, IDbConnectionFactory dbConnectionFactory, IOptions<AppConfig> options, IConfiguration configuration)
        {
            _logger = logger;
            _appConfig = options.Value;
            _dbConnection = dbConnectionFactory.CreateDbConnection(configuration.GetConnectionString(Constants.SQLWORKER_DATA_BASE_CONNECTION));
        }

        ///// <summary>
        ///// The ExecuteAsync
        ///// </summary>
        ///// 
        //public void RebuildAllIndex()
        //{

        //    var databases = _dbConnection.Query<string>(string.Format(Constants.DataBaseListQueryByPrefix, _appConfig.DbPrefix)).AsList();
        //    var str = new StringBuilder();
        //    foreach (string table in from db in databases
        //                             let tables = _dbConnection.Query<string>(string.Format(Constants.ALL_TABLES, db)).AsList()
        //                             from string table in tables
        //                             select table)
        //    {
        //        if (_dbConnection.State == ConnectionState.Closed)
        //        {
        //            _dbConnection.Open();
        //        }

        //        str.AppendLine(string.Format(Constants.REBUILD_SCRIPT, table));
        //        str.AppendLine("GO");
        //    }

        //    var cmd = str.ToString();
        //    _dbConnection.Execute(cmd, commandTimeout: 0);

        //}

        public void ShrinkDataBases()
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var databases = _dbConnection.Query<string>(string.Format(Constants.DataBaseListQueryByPrefix, _appConfig.DbPrefix)).AsList();
            databases.ForEach(db =>
            {
                _dbConnection.ChangeDatabase(db);
                var file = _dbConnection.Query<Database_Files>(Constants.SQL_CHECK_FILE).FirstOrDefault(f => f.Name.Contains("_log"))?.Name;
                if (string.IsNullOrEmpty(file))
                    _dbConnection.Execute(string.Format(Constants.SHRINK_DB, db, file));
            });
        }

        public void RebuildOrReorganizeIndexes()
        {

            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var databases = _dbConnection.Query<string>(string.Format(Constants.DataBaseListQueryByPrefix, _appConfig.DbPrefix)).AsList();
            databases.ForEach(db =>
            {
                _dbConnection.ChangeDatabase(db);
                var cmd = string.Format(Constants.PERFECT_REBUILD);
                _logger.LogInformation("Executed Command {CMD}", cmd);
                _dbConnection.Execute(Constants.PERFECT_REBUILD, commandTimeout: 0);

            });
        }
    }
}

