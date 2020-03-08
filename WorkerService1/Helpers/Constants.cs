namespace WorkerService1.Helpers
{
    /// <summary>
    /// Defines the <see cref="Constants" />
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Defines the ALL_TABLES
        /// </summary>
        public const string ALL_TABLES = @" SELECT '[' + table_catalog + '].[' + table_schema + '].[' +  table_name + ']' as tableName FROM [{0}].INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' AND table_name <>'__MigrationHistory'";

        public const string REBUILD_SCRIPT = "ALTER INDEX ALL ON {0} REBUILD WITH (MAXDOP=30)";
        /// <summary>
        /// Defines the DataBaseListQueryAllBullMaster_msdb_model_distribution
        /// </summary>
        public const string DataBaseListQueryAllBullMaster_msdb_model_distribution = @"SELECT name FROM master.sys.databases   
            WHERE name NOT IN ('master','msdb','tempdb','model','distribution')  -- databases to exclude
            AND state = 0 -- database is online
            AND is_in_standby = 0 -- database is not read only for log shipping
            ORDER BY 1  ";

        /// <summary>
        /// Defines the DataBaseListQueryByPrefix
        /// </summary>
        public const string DataBaseListQueryByPrefix = @"SELECT name FROM master.sys.databases   
            WHERE name like '{0}%'
            AND state = 0 -- database is online
            AND is_in_standby = 0 -- database is not read only for log shipping
            ORDER BY 1  ";

        /// <summary>
        /// Defines the DEFAULT_DATA_BASE_CONNECTION
        /// </summary>
        public const string DEFAULT_DATA_BASE_CONNECTION = "DefaultConnection";

        /// <summary>
        /// Defines the REBUILD_QUERY
        /// </summary>
        public const string REBUILD_QUERY =
            @"DECLARE @Database NVARCHAR(255)   
            DECLARE @Table NVARCHAR(255)  
            DECLARE @cmd NVARCHAR(1000)  
            ​
            DECLARE DatabaseCursor CURSOR READ_ONLY FOR  
            SELECT name FROM master.sys.databases   
            --WHERE name NOT IN ('master','msdb','tempdb','model','distribution')  -- databases to exclude
            WHERE name like 'hostt_%'
            AND state = 0 -- database is online
            AND is_in_standby = 0 -- database is not read only for log shipping
            ORDER BY 1  
            ​
            OPEN DatabaseCursor  
            ​
            FETCH NEXT FROM DatabaseCursor INTO @Database  
            WHILE @@FETCH_STATUS = 0  
            BEGIN  
            ​
               SET @cmd = 'DECLARE TableCursor CURSOR READ_ONLY FOR SELECT ''['' + table_catalog + ''].['' + table_schema + ''].['' +  
               table_name + '']'' as tableName FROM [' + @Database + '].INFORMATION_SCHEMA.TABLES WHERE table_type = ''BASE TABLE'''   
            ​
               -- create table cursor  
               EXEC (@cmd)  
               OPEN TableCursor   
            ​
               FETCH NEXT FROM TableCursor INTO @Table   
               WHILE @@FETCH_STATUS = 0   
               BEGIN
                  BEGIN TRY   
                     SET @cmd = 'ALTER INDEX ALL ON ' + @Table + ' REBUILD' 
                     PRINT @cmd -- uncomment if you want to see commands
                     EXEC (@cmd) 
                  END TRY
                  BEGIN CATCH
                     PRINT '---'
                     PRINT @cmd
                     PRINT ERROR_MESSAGE() 
                     PRINT '---'
                  END CATCH
            ​
                  FETCH NEXT FROM TableCursor INTO @Table   
               END   
            ​
               CLOSE TableCursor   
               DEALLOCATE TableCursor  
            ​
               FETCH NEXT FROM DatabaseCursor INTO @Database  
            END  
            CLOSE DatabaseCursor   
            DEALLOCATE DatabaseCursor";
    }
}
