namespace ThreeS.Jobs
{
    /// <summary>
    /// Defines the <see cref="Constants" />
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Defines the ALL_TABLES
        /// </summary>
        public const string ALL_TABLES = @" SELECT '[' + table_catalog + '].[' + table_schema + '].[' +  table_name + ']' as tableName FROM [{0}].INFORMATION_SCHEMA.TABLES WITH (READUNCOMMITTED,NOLOCK) WHERE table_type = 'BASE TABLE' AND table_name <>'__MigrationHistory'";

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
        public const string SQLWORKER_DATA_BASE_CONNECTION = "WorkerConnetion";
        /// <summary>
        /// Defines the PERFECT_REBUILD
        /// </summary>
        public const string PERFECT_REBUILD = @" SET NOCOUNT ON;  
            DECLARE @objectid int;  
            DECLARE @indexid int;  
            DECLARE @partitioncount bigint;  
            DECLARE @schemaname nvarchar(130);   
            DECLARE @objectname nvarchar(130);   
            DECLARE @indexname nvarchar(130);   
            DECLARE @partitionnum bigint;  
            DECLARE @partitions bigint;  
            DECLARE @frag float;  
            DECLARE @command nvarchar(4000);   
            -- Conditionally select tables and indexes from the sys.dm_db_index_physical_stats function   
            -- and convert object and index IDs to names.  
            SELECT 
                object_id AS objectid,  
                index_id AS indexid,  
                partition_number AS partitionnum,  
                avg_fragmentation_in_percent AS frag  
            INTO #work_to_do  
            FROM sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL , NULL, 'LIMITED')  
            WHERE avg_fragmentation_in_percent > 10.0 AND index_id > 0;  
 
            -- Declare the cursor for the list of partitions to be processed.  
            DECLARE partitions CURSOR FOR SELECT * FROM #work_to_do;  
 
            -- Open the cursor.  
            OPEN partitions;  
 
            -- Loop through the partitions.  
            WHILE (1=1)  
                BEGIN;  
                    FETCH NEXT 
                        FROM partitions  
                        INTO @objectid, @indexid, @partitionnum, @frag;  
                    IF @@FETCH_STATUS < 0 BREAK;  
                    SELECT @objectname = QUOTENAME(o.name), @schemaname = QUOTENAME(s.name)  
                    FROM sys.objects AS o  
                    JOIN sys.schemas as s ON s.schema_id = o.schema_id  
                    WHERE o.object_id = @objectid;  
                    SELECT @indexname = QUOTENAME(name)  
                    FROM sys.indexes  
                    WHERE  object_id = @objectid AND index_id = @indexid;  
                    SELECT @partitioncount = count (*)  
                    FROM sys.partitions  
                    WHERE object_id = @objectid AND index_id = @indexid;  
 
            -- 30 is an arbitrary decision point at which to switch between reorganizing and rebuilding.  
                    IF @frag < 30.0  
                        SET @command = N'ALTER INDEX ' + @indexname + N' ON ' + @schemaname + N'.' + @objectname + N' REORGANIZE';  
                    IF @frag >= 30.0  
                        SET @command = N'ALTER INDEX ' + @indexname + N' ON ' + @schemaname + N'.' + @objectname + N' REBUILD';  
                    IF @partitioncount > 1  
                        SET @command = @command + N' PARTITION=' + CAST(@partitionnum AS nvarchar(10));
                    PRINT N'Executing: ' + @command;  
            EXEC (@command);  
                    PRINT N'Executed: ' + @command;  
                END;  
 
            -- Close and deallocate the cursor.  
            CLOSE partitions;  
            DEALLOCATE partitions;  
 
            -- Drop the temporary table.  
            DROP TABLE #work_to_do;";

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

        public const string SQL_CHECK_FILE = @"SELECT name ,size/128.0 - CAST(FILEPROPERTY(name, 'SpaceUsed') AS int)/128.0 AS AvailableSpaceInMB
                FROM sys.database_files ;";
        public const string SHRINK_DB = @"ALTER DATABASE {0} SET RECOVERY SIMPLE;
                GO
                -- Shrink the truncated log file to 1 MB. 
                DBCC SHRINKFILE ({1}, 1);
                GO";
        /// <summary>
        /// Defines the REBUILD_SCRIPT
        /// </summary>
        public const string REBUILD_SCRIPT = "ALTER INDEX ALL ON {0} REBUILD WITH (MAXDOP=30);";
    }
}
