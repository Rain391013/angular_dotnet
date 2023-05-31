using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BikeController : ControllerBase
    {
        /*private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };*/
        private readonly IConfiguration _configuration;
        private readonly ILogger<BikeController> _logger;
        

        public BikeController(ILogger<BikeController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            string sqlDataSource = _configuration.GetConnectionString("Default");

            SqlConnection conn = new SqlConnection(sqlDataSource);
            try
            {

                conn.Open();

                string sql = "IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[bikes]') AND type in (N'U')) BEGIN CREATE TABLE[dbo].[bikes]" + "(id Int Identity(1,1) Primary Key, manufacture Varchar(100), model Varchar(100), frame_size Int, price Decimal(10,2),) END";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            
        }

        [HttpGet()]
        public List<Bike> Get()
        {
            List<Bike> list = new List<Bike>();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            using (SqlConnection conn = new SqlConnection (sqlDataSource))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from dbo.bikes", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Bike()
                        {
                            id = (int)reader["id"],
                            Manufacture = reader["manufacture"].ToString(),
                            Model = reader["model"].ToString(),
                            FrameSize = (int)reader["frame_size"],
                            Price = (decimal)reader["price"],

                        });
                    }
                }
            }
            return list;
        }

        [HttpGet("{id}")]
        public Bike Get(int id)
        {

            string sqlDataSource = _configuration.GetConnectionString("Default");
            Bike bike = new Bike();
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from dbo.bikes WHERE id =" + id.ToString(), conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bike = new Bike()
                        {
                            id = (int)reader["id"],
                            Manufacture = reader["manufacture"].ToString(),
                            Model = reader["model"].ToString(),
                            FrameSize = (int)reader["frame_size"],
                            Price = (decimal)reader["price"],
                        };
                    }
                }
            }
            return bike;
        }

        [HttpPost]
        public int Post([FromBody] Bike res)
        {
            string sqlDataSource = _configuration.GetConnectionString("Default");
            var result = 0;
            SqlConnection conn = new SqlConnection(sqlDataSource);
            try
            {

                conn.Open();

                var Manufacture = res.Manufacture;
                var Model = res.Model;
                var FrameSize = res.FrameSize;
                var Price = res.Price;

                string sql = "INSERT INTO dbo.bikes (manufacture, model, frame_size, price) " +
                    "VALUES ('" + Manufacture + "'," +
                    "'" + Model + "'" + ", '" + FrameSize + "'" + ", '" + Price + "')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            return result;

        }

        [HttpPut]
        public int Put([FromBody] Bike res)
        {
            string sqlDataSource = _configuration.GetConnectionString("Default");
            var result = 0;
            SqlConnection conn = new SqlConnection(sqlDataSource);
            try
            {

                conn.Open();

                var BikeId = res.id;
                var BikeManufacture = res.Manufacture;
                var BikeModel = res.Model;
                var BikeFrameSize = res.FrameSize;
                var BikePrice = res.Price;

                string sql = "UPDATE dbo.bikes SET manufacture='" + BikeManufacture + "',model='" + BikeModel + "',frame_size='" + BikeFrameSize + "',price='" + BikePrice + "' WHERE id =" + BikeId;
                SqlCommand cmd = new SqlCommand(sql, conn);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            return result;

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string sqlDataSource = _configuration.GetConnectionString("Default");
            var result = 0;
            SqlConnection conn = new SqlConnection(sqlDataSource);
            try
            {

                conn.Open();

                string sql = "DELETE FROM dbo.bikes WHERE id =" + id;
                SqlCommand cmd = new SqlCommand(sql, conn);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

        }

    }
}