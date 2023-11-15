using System.Data.SqlClient;
using System.Xml.Linq;
using VoicePay.Models;

namespace VoicePay.DAL
{
    public class TransactionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public TransactionDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "NPCSConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }
    }
}