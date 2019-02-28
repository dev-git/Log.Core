using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Log.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateDb();

            CreateWebHostBuilder(args).Build().Run();
        }

        private static void CreateDb()
        {
            string connectionStr = @"Data Source=logcore.db;";
            SqliteConnection connection = new SqliteConnection(connectionStr);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();

            // Check if table exists
            var tableStr = "select * from sqlite_master where type = 'table' and name = 'LogDetail';";

            cmd.CommandText = tableStr;
            var lok = cmd.ExecuteScalar();

            if (lok is null)
            {
                string createTableStr = "CREATE TABLE LogDetail (logdetail_pk integer primary key autoincrement, logdetail_macaddress text, logdetail_latitude real, " +
                    " logdetail_longtitude real, logdetail_batterylevel integer, when_created datetime default (strftime('%Y-%m-%d %H:%M:%f', 'now')), " +
                    " is_deleted bool default (0), user_modified int default (0))";

                cmd.CommandText = createTableStr;
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
