﻿using System;
using SnapObjects.Data;
using SnapObjects.Data.SqlServer;

namespace delconsdb_api
{
    public class DelConsDBDataContext : SqlServerDataContext
	{
        public DelConsDBDataContext(string connectionString)
            : this(new SqlServerDataContextOptions<DelConsDBDataContext>(connectionString))
        {
        }

        public DelConsDBDataContext(IDataContextOptions<DelConsDBDataContext> options)
            : base(options)
        {
        }
        
        public DelConsDBDataContext(IDataContextOptions options)
            : base(options)
        {
        }
    }
}
