using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DummyController : ControllerBase
    {

        private readonly ILogger<DummyController> _logger;
        private readonly IConfiguration _configuration;

        public DummyController(ILogger<DummyController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public String Get()
        {
            String r = "";
            try
            {
                string myDb1ConnectionString = _configuration["ConnectionStrings:DefaultConnection"];

                using (SqlConnection connection = new SqlConnection(myDb1ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    r += "SQL Server version:\n";

                    String sql = "SELECT @@Version";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                r += String.Format("{0}\n", reader.GetString(0));
                            }
                        }
                    }

                    r += "\n\nDatabases:\n";

                    sql = "SELECT name, collation_name FROM sys.databases";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                r += String.Format("{0} {1}\n", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                r = e.ToString();
            }

            return r;
        }
    }
}