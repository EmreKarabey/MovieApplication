using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCrossingCuttingConcerns.SeriLog.ConfigurationModels.MSSql
{
    public class MSSqlLogConfiguration
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public bool AutoCreatedSqlTable { get; set; }

        public MSSqlLogConfiguration()
        {
            ConnectionString = string.Empty;
            TableName = string.Empty;
        }


        public MSSqlLogConfiguration(string connectionString, string tableName, bool autoCreatedSqlTable)
        {
            ConnectionString = connectionString;
            TableName = tableName;
            AutoCreatedSqlTable = autoCreatedSqlTable;
        }
    }
}
