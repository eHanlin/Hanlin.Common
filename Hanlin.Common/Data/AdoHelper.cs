using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Hanlin.Common.Utils;
using log4net;

namespace Hanlin.Common.Data
{
    public static class AdoHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static AdoHelper()
        {
            CommandTimeout = 60;
            BulkCopyTimeout = 60;
        }

        public static int CommandTimeout { get; set; }
        public static int BulkCopyTimeout { get; set; }

        public static bool DatabaseExists(string connName, string databseName, string schemaName = null)
        {
            var fullname = databseName;
            if (schemaName != null)
            {
                fullname = schemaName + "." + databseName;
            }

            const string sql = "SELECT * FROM SYS.DATABASES WHERE NAME='{0}'";

            var result = GetDataTable(connName, sql, fullname);
            return result.Rows.Count > 0;
        }

        public static void CreateIfNotExist(string connName, string createScript, string tableName, string schemaName)
        {
            using (IDbConnection conn = CreateConnectionFromName(connName))
            {
                const string nonquerySqlTemplate = @"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}')
                    {2}
                ";
                var parameterValues = new[] { schemaName, tableName, createScript };
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format(nonquerySqlTemplate, parameterValues);
                conn.Open();
                using (new DisposableTimer("(ExecuteNoneQuery) @" + connName,
                    Combine(nonquerySqlTemplate, parameterValues)))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Truncate(string connName, string tableName)
        {
            using (IDbConnection conn = CreateConnectionFromName(connName))
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("truncate table {0}", tableName);
                conn.Open();
                cmd.ExecuteNonQuery(); 
            }
        }

/*        public static void TruncateWithDelete(string connName, string tableName)
        {
            using (IDbConnection conn = CreateConnectionFromName(connName))
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("delete from {0}", tableName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }*/

        public static void ExecuteScriptsInDirectory(string connName, string relativeDir)
        {
            using (IDbConnection conn = CreateConnectionFromName(connName))
            {
                conn.Open();
                ExecuteScriptsInDirectory(conn, relativeDir);
            }
        }

        public static void ExecuteScriptsInDirectory(IDbConnection conn, string relativeDir)
        {
            var scriptDir = new DirectoryInfo(relativeDir);
            var scriptFiles = scriptDir.GetFiles("*.sql");

            foreach (var file in scriptFiles)
            {
                string script = null;
                using (var reader = file.OpenText())
                {
                    script = reader.ReadToEnd();
                }
                Log.Info("Executing script:");
                Log.Info(script);

                ExecuteNonQuery(conn, script);
            }
        }

        public static void ExecuteScript(string connName, string relativeFilePath)
        {
            using (IDbConnection conn = CreateConnectionFromName(connName))
            {
                conn.Open();
                ExecuteScript(conn, relativeFilePath);
            }
        }

        public static int ExecuteScript(IDbConnection conn, string relativeFilePath)
        {
            var scriptFile = new FileInfo(relativeFilePath);

            string script = null;
            using (var reader = scriptFile.OpenText())
            {
                script = reader.ReadToEnd();
            }

            Log.Info("Executing script:");
            Log.Info(script);

            return ExecuteNonQuery(conn, script);
        }

        public static int ExecuteNonQuery(string connName, string nonquerySqlTemplate, params string[] parameterValues)
        {
            int rowCount;
            using (IDbConnection conn = CreateConnectionFromName(connName))
            {
                conn.Open();
                rowCount = ExecuteNonQuery(conn, nonquerySqlTemplate, parameterValues);
            }
            return rowCount;
        }

        public static int ExecuteNonQuery(IDbConnection conn, string nonquerySqlTemplate, params string[] parameterValues)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandText = string.Format(nonquerySqlTemplate, parameterValues);
            int rowCount;
            using (new DisposableTimer("(ExecuteNoneQuery) ", Combine(nonquerySqlTemplate, parameterValues)))
            {
                rowCount = cmd.ExecuteNonQuery();
            }
            return rowCount;
        }

        public static int ExecuteScalar(IDbConnection conn, string scalarSqlTemplate, params string[] parameterValues)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandText = string.Format(scalarSqlTemplate, parameterValues);

            Log.Debug(cmd.CommandText);

            int scalarResult = Convert.ToInt32(cmd.ExecuteScalar());
            return scalarResult;
        }

        public static DataTable GetDataTable(string connName, string sqlTemplate, 
            params string[] parameterValues)
        {
            using (IDbConnection conn = CreateConnectionFromName(connName))
            {
                conn.Open();
                return GetDataTable(conn, CreateDataAdapterFromName(connName), sqlTemplate, parameterValues);
            }
        }

        public static DataTable GetDataTable(IDbConnection conn, IDbDataAdapter dataAdapter, string sqlTemplate, params string[] parameterValues)
        {
            var data = new DataSet();
            IDbCommand cmd = conn.CreateCommand();

            cmd.CommandText = parameterValues == null ? sqlTemplate : string.Format(sqlTemplate, parameterValues);

            dataAdapter.SelectCommand = cmd;

            dataAdapter.Fill(data);

            return data.Tables[0];
        }

        public static IDataReader GetDataReader(string connectionStringName, string sqlTemplate, params string[] parameterValues)
        {
            IDbConnection conn = CreateConnectionFromName(connectionStringName);
            conn.Open();
            return GetDataReader(conn, sqlTemplate, parameterValues);
        }

        public static IDataReader GetDataReader(IDbConnection conn, string sqlTemplate, params string[] parameterValues)
        {
            var cmd = conn.CreateCommand();

            // To allow placeholders to be present even if parameterValues has no values. 
            // This enables situation where a placeholder might be in a commented statement in sqlTemplate.
            cmd.CommandText = parameterValues.Length > 0 ? string.Format(sqlTemplate, parameterValues) : sqlTemplate;

            Log.Debug(cmd.CommandText);

            return cmd.ExecuteReader();
        }

        private static string[] Combine(string sqlTemplate, string[] parameterValues)
        {
            var combined = new string[parameterValues.Length + 1];
            combined[0] = sqlTemplate;
            parameterValues.CopyTo(combined, 1);
            return combined;
        }

        public static void SimpleSqlBulkCopy(string sourceConnName, string targetConnName,
            string tableName, string[] columnNames)
        {
            SimpleSqlBulkCopy(sourceConnName, targetConnName, tableName, null, columnNames);
        }

        /// <summary>
        /// Copy data from source to temp schema at the target connection using the given table name and columns.
        /// </summary>
        /// <param name="sourceConnName"></param>
        /// <param name="targetConnName"></param>
        /// <param name="tableName">The name of the table to copy data from the source as well as to store data in the target</param>.
        /// <param name="targetSchemaName">The name of the schema in which the specified table belongs. Use null if not applicable.</param>
        /// <param name="columnNames">Columns to copy (used for sql select).</param>
        public static void SimpleSqlBulkCopy(string sourceConnName, string targetConnName,
            string tableName, string targetSchemaName, string[] columnNames)
        {
            var identityMapping = new Dictionary<string, string>();

            foreach (var col in columnNames)
            {
                identityMapping.Add(col, col);
            }

            SimpleSqlBulkCopy(sourceConnName, targetConnName, tableName, targetSchemaName, identityMapping);
        }

        public static void SimpleSqlBulkCopy(string sourceConnName, string targetConnName,
            string tableName, IDictionary<string, string> columnMappings)
        {
            SimpleSqlBulkCopy(sourceConnName, targetConnName, tableName, null, columnMappings);
        }

        public static void SimpleSqlBulkCopy(string sourceConnName, string targetConnName,
            string tableName, string targetSchemaName, IDictionary<string, string>columnMappings)
        {
            SimpleSqlBulkCopy(sourceConnName, targetConnName, tableName, tableName, targetSchemaName, columnMappings);
        }

        public static void SimpleSqlBulkCopy(string sourceConnName, string targetConnName,
            string fromTableName, string toTableName, string targetSchemaName, IDictionary<string, string> columnMappings)
        {
            string sql = string.Format(
                "select {0} from {1}",
                string.Join(", ", columnMappings.Select(e => e.Key + " as " + e.Value)),
                fromTableName);
            IDataReader reader = AdoHelper.GetDataReader(sourceConnName, sql);
            string qualifiedTableName = targetSchemaName == null
                ? toTableName : targetSchemaName + "." + toTableName;
            AdoHelper.SqlBulkCopy(targetConnName, qualifiedTableName, reader);
        }

        //public delegate string SourceTableNameToDestinationTableName(string sourceTableName);

        //public static void SimpleSqlBulkCopy(string sourceConnName, string targetConnName,

        //    SourceTableNameToDestinationTableName tableMapping, string[] columnNames)

        //{

        //    string selectTemplate = "select {0} from {1}";

        //    string sql = string.Format(selectTemplate, string.Join(", ", columnNames), tableName);

        //    IDataReader reader = AdoHelper.GetDataReader(sourceConnName, sql);

        //    string qualifiedTableName = targetSchemaName == null

        //        ? tableName : targetSchemaName + "." + tableName;

        //    AdoHelper.SqlBulkCopy(targetConnName, qualifiedTableName, reader);

        //}

        /// <summary>
        /// Copy data using the supplied sql from the source to the specified table at the target.
        /// </summary>
        /// <param name="sourceConnName"></param>
        /// <param name="targetConnName"></param>
        /// <param name="targetTableName"></param>
        /// <param name="sourceSqlTemplate"></param>
        /// <param name="sqlParameterValues"></param>
        public static void SqlBulkCopy(string sourceConnName, string targetConnName,
            string targetTableName, string sourceSqlTemplate, params string[] sqlParameterValues)
        {
            IDataReader reader = AdoHelper.GetDataReader(sourceConnName,
                sourceSqlTemplate, sqlParameterValues);

            AdoHelper.SqlBulkCopy(targetConnName,
                targetTableName, reader);
        }

        public static void SqlBulkCopy(string targetConnectionName, string targetTableName,
            DataTable data, params SqlBulkCopyColumnMapping[] mapping)
        {
            SqlBulkCopy(targetConnectionName, targetTableName, data.CreateDataReader(), mapping);
        }

        public static void SqlBulkCopy(string targetConnectionName, string targetTableName, 
            IDataReader reader, params SqlBulkCopyColumnMapping[] mapping)
        {
            using (reader)
            {
                using (SqlConnection sqlConn = 
                    (SqlConnection)CreateConnectionFromName(targetConnectionName))
                {
                    SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConn);
                    sqlBulkCopy.BulkCopyTimeout = BulkCopyTimeout;
                    sqlBulkCopy.DestinationTableName = targetTableName;

                    foreach (var m in mapping)
                    {
                        sqlBulkCopy.ColumnMappings.Add(m);
                    }

                    using (new DisposableTimer("(SqlBulkCopy) Conn: " + targetConnectionName + " Table: " + targetTableName))
                    {
                        sqlConn.Open();
                        sqlBulkCopy.WriteToServer(reader);
                    }
                } 
            }
        }

        private static IDbDataAdapter CreateDataAdapterFromName(string connName)
        {
            Log.InfoFormat("Creating data adapter from connection {0}", connName);

            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connName];
            DbProviderFactory factory = DbProviderFactories.GetFactory(setting.ProviderName);
            return factory.CreateDataAdapter();
        }

        public static IDbConnection CreateConnectionFromName(string connName)
        {
            Log.InfoFormat("Creating connection {0}", connName);

            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connName];
            
            if (setting == null)
            {
                InvalidConnectionName(connName);
            }

            Log.DebugFormat("Attempting to get factory using provider name '{0}'", setting.ProviderName);

            DbProviderFactory factory = DbProviderFactories.GetFactory(setting.ProviderName);
            IDbConnection conn = factory.CreateConnection();
            conn.ConnectionString = setting.ConnectionString;
            return conn;
        }
        
        private static void InvalidConnectionName(string connName)
        {
            throw new ArgumentException(string.Format("Cannot obtain a connection from name '{0}", connName));
        }

        public static string InClause(params string[] values)
        {
            return InClause(values.AsEnumerable());
        }

        public static string InClause(IEnumerable<string> values)
        {
            return string.Join(",", values.Select(v => "'" + v + "'"));
        }

        public static void PrintReader(IDataReader reader)
        {
            StringBuilder builder = new StringBuilder();
            using (reader)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    builder.Append(reader.GetSchemaTable().Columns[i].ColumnName);
                    builder.Append(" | ");
                }
                builder.Append("\n");
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i));
                        if (i < reader.FieldCount - 1)
                        {
                            builder.Append(" | ");
                        }
                    }
                    builder.Append("\n");
                } 
            }
            Log.Info(builder.ToString());
        }

        public static void PrintTable(DataTable data)
        {
            var builder = new StringBuilder();
            foreach (var r in data.AsEnumerable())
            {
                for (int colIndex = 0; colIndex < data.Columns.Count; colIndex++)
                {
                    builder.Append(r.ItemArray[colIndex] + " ");
                }
            }
            Log.Info(builder.ToString());
        }
    }
}
