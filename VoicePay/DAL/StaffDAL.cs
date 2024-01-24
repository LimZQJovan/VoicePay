using System.Data.SqlClient;

namespace VoicePay.DAL
{
    public class StaffDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public StaffDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "PFDConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }
        public bool Login(string loginId, string password, out string UEN, out string location, out string Stallname)
        {
            UEN = "";
            Stallname = "";
            location = "";
            bool authenticated = false;

            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Stall";

            // Open a database connection
            conn.Open();

            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            // Read all records until the end
            while (reader.Read())
            {
                if ((reader.GetString(3).ToLower() == loginId) &&
                  (reader.GetString(4) == password))
                {
                    UEN = reader.GetString(0);
                    Stallname = reader.GetString(1);
                    location = reader.GetString(2); // Corrected line
                    authenticated = true;
                    break; // Exit the while loop
                }
            }

            // Close the DataReader and the database connection
            reader.Close();
            conn.Close();

            return authenticated;
        }

        public string GetUEN(string LoginID)
        {
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT UEN FROM Stall WHERE LoginID = @login";
            cmd.Parameters.AddWithValue("@login", LoginID);

            // Open a database connection
            conn.Open();

            // Execute the SELECT SQL through a DataReader
            string UEN = cmd.ExecuteScalar()?.ToString();

            return UEN;
        }
    }
}
