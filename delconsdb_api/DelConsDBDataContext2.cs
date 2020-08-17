using System;
using SnapObjects.Data;
using SnapObjects.Data.SqlServer;

namespace delconsdb_api
{
    public class DelConsDBDataContext2 : SqlServerDataContext
	{
        public DelConsDBDataContext2(string connectionString)
            : this(new SqlServerDataContextOptions<DelConsDBDataContext2>(connectionString))
        {
        }

        public DelConsDBDataContext2(IDataContextOptions<DelConsDBDataContext2> options)
            : base(options)
        {
        }
        
        public DelConsDBDataContext2(IDataContextOptions options)
            : base(options)
        {
        }
    }
}
