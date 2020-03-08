namespace ThreeS.Jobs
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Defines the <see cref="IDbConnectionFactory" />
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// The CreateDbConnection
        /// </summary>
        /// <param name="connectionName">The connectionName<see cref="string"/></param>
        /// <returns>The <see cref="IDbConnection"/></returns>
        IDbConnection CreateDbConnection(string connectionName);
    }

    /// <summary>
    /// Defines the <see cref="DapperDbConnectionFactory" />
    /// </summary>
    public class DapperDbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// The CreateDbConnection
        /// </summary>
        /// <param name="connectionName">The connectionName<see cref="string"/></param>
        /// <returns>The <see cref="IDbConnection"/></returns>
        public IDbConnection CreateDbConnection(string connectionName)
        {

            return new SqlConnection(connectionName);
        }
    }
}
