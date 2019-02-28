using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Log.Core.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        // GET: api/Log
        [HttpGet]
        public IEnumerable<Models.LogDetail> Get()
        {

            string connectionStr = @"Data Source=logcore.db;";
            SqliteConnection connection = new SqliteConnection(connectionStr);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();

            string selectStr = "select logdetail_pk, logdetail_macaddress, logdetail_latitude, " +
                "logdetail_longtitude, logdetail_batterylevel, datetime(when_created, 'localtime') as when_created " +
                "from LogDetail order by logdetail_pk desc;";

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = selectStr;

            SqliteDataReader dr = cmd.ExecuteReader();
            //System.Data.DataTable dt = new System.Data.DataTable();
            //dt.Load(dr);

            List<Models.LogDetail> logDetailList = new List<Models.LogDetail>();
            while (dr.Read())
            { 
                Console.WriteLine(dr[0].ToString());
                Models.LogDetail logDetail = new Models.LogDetail();
                logDetail.MACAddress = dr[1].ToString();
                logDetail.Latitude = Double.Parse(dr[2].ToString());
                logDetail.Longitude = Double.Parse(dr[3].ToString());
                logDetail.BatteryLevel = Int32.Parse(dr[4].ToString());
                logDetail.WhenCreated = DateTime.Parse(dr[5].ToString());

                logDetailList.Add(logDetail);
            }
            connection.Close();

            return logDetailList;
            //return new string[] { "value1", "value2" };
        }

        // GET: api/Log/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "values";
        }

        // POST: api/Log
        [HttpPost]
        public void Post([FromBody] string value)
        {
            /* fetch('https://localhost:44337/api/Log', {
                  method: 'POST', // or 'PUT'
                  body: JSON.stringify('{ "macAddress" : "C0-11-73-6C-70-27", 
                    "latitude" : "-36.92577745", "longitude" : "174.63647775" , "batteryLevel" : "21" }'), // data can be `string` or {object}!
                  headers:{
                    'Content-Type': 'application/json'
                  }
                }).then(res => res.json())
                .then(response => console.log('Success:', JSON.stringify(response)))
                .catch(error => console.error('Error:', error));
                */

            var log = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.LogDetail>(value);

            string connectionStr = @"Data Source=logcore.db;";
            SqliteConnection connection = new SqliteConnection(connectionStr);
            connection.Open();

            SqliteCommand cmd = connection.CreateCommand();

            string tableStr = String.Format("insert into LogDetail (logdetail_macaddress, logdetail_latitude, logdetail_longtitude, logdetail_batterylevel) " +
                    "values ('{0}', {1}, {2}, {3})", log.MACAddress, log.Latitude, log.Longitude, log.BatteryLevel);

            cmd.CommandText = tableStr;
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        // PUT: api/Log/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
