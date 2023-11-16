    //using System.Data.SqlClient;
    //using VoicePay.Models;

    //namespace VoicePay.DAL
    //{
    //    public class TransactionDAL
    //    {
    //        private IConfiguration Configuration { get; }
    //        private SqlConnection conn;
    //        //Constructor
    //        public TransactionDAL()
    //        {
    //            //Read ConnectionString from appsettings.json file
    //            var builder = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json");
    //            Configuration = builder.Build();
    //            string strConn = Configuration.GetConnectionString(
    //            "PFDConnectionString");
    //            //Instantiate a SqlConnection object with the
    //            //Connection String read.
    //            conn = new SqlConnection(strConn);
    //        }

    //        public List<Transaction> GetTransactions(string UEN, int dtYear, int dtMonth, int dtDay)
    //        {
    //            List<Transaction> transactionList = new List<Transaction>();

    //            //Create a SqlCommand object from connection object
    //            SqlCommand cmd = conn.CreateCommand();

    //            // Specify the SELECT SQL statement
    //            cmd.CommandText = @"SELECT b.PayeeName, b.MobileNo, t.SenderAccountNumber, t.Amount, t.TransactionDateTime
    //                       FROM TransactionDetails t
    //                       INNER JOIN Stall s
    //                       ON s.UEN = t.ReceiverUEN
    //                       INNER JOIN BankDetails b
    //                       ON b.AccountNumber = t.SenderAccountNumber
    //                       WHERE t.ReceiverUEN = '@UEN'
    //                       AND YEAR(TransactionDateTime) = @dtYear";

    //            // Add parameters for month and day if provided
    //            if (dtMonth != -1)
    //            {
    //                cmd.CommandText += " AND MONTH(TransactionDateTime) = @dtMonth";
    //                cmd.Parameters.AddWithValue("@dtMonth", dtMonth);
    //            }

    //            if (dtDay != -1)
    //            {
    //                cmd.CommandText += " AND DAY(TransactionDateTime) = @dtDay";
    //                cmd.Parameters.AddWithValue("@dtDay", dtDay);
    //            }

    //            cmd.Parameters.AddWithValue("@UEN", UEN);
    //            cmd.Parameters.AddWithValue("@dtYear", dtYear);

    //            // Open a database connection
    //            conn.Open();

    //            // Execute the SELECT SQL through a DataReader
    //            SqlDataReader reader = cmd.ExecuteReader();

    //            if (reader.HasRows)
    //            {
    //                while (reader.Read())
    //                {
    //                    transactionList.Add(new Transaction
    //                    {
    //                    Payee = reader.GetString(0), // 0: 1st column
    //                    PayeeTelNo = reader.GetString(1),
    //                    SenderAccountNumber = reader.GetString(2), // 1: 2nd column
    //                    Amount = reader.GetDecimal(3), // 1: 2nd column
    //                    TransactionDateTime = reader.GetDateTime(4)
    //                    });
    //                }
    //            }

    //            // Close DataReader
    //            reader.Close();

    //            // Close the database connection
    //            conn.Close();

    //            return transactionList;
    //        }
    //    }
    //}