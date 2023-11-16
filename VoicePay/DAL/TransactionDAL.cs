using System.Data.SqlClient;
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

        public Transaction GetTransactionsByDay(string UEN, int dtYear, int dtMonth, int dtDay)
        {
            Transaction transaction = new Transaction();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT b.PayeeName,b.[Mobile No], t.SenderAccountNumber, t.Amount, TransactionDateTime
                                FROM TransactionDetails t
                                INNER JOIN Stall s
                                ON s.UEN = t.ReceiverUEN
                                INNER JOIN BankDetails b
                                ON b.AccountNumber = t.SenderAccountNumber
                                WHERE t.ReceiverUEN = @UEN
                                AND YEAR(TransactionDateTime) = @dtYear
                                AND MONTH(TransactionDateTime) = @dtMonth
                                AND DAY(TransactionDateTime) = @dtDay";

            cmd.Parameters.AddWithValue("@UEN", UEN);
            cmd.Parameters.AddWithValue("@dtYear", dtYear);
            cmd.Parameters.AddWithValue("@dtMonth", dtMonth);
            cmd.Parameters.AddWithValue("@dtDay", dtDay);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    transaction.Payee = reader.GetString(0); //0: 1st column
                    transaction.SenderAccountNumber = reader.GetString(1);  //1: 2nd column
                    transaction.Amount = reader.GetDecimal(2); //1: 2nd column
                    transaction.PayeeTelNo = reader.GetString(3);
                    transaction.TransactionDateTime = reader.GetDateTime(4);            
                };
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return transaction;
        }

        public Transaction GetTransactionsByMonth(string UEN, int dtYear, int dtMonth)
        {
            Transaction transaction = new Transaction();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT b.PayeeName,b.[Mobile No], t.SenderAccountNumber, t.Amount, TransactionDateTime
                                FROM TransactionDetails t
                                INNER JOIN Stall s
                                ON s.UEN = t.ReceiverUEN
                                INNER JOIN BankDetails b
                                ON b.AccountNumber = t.SenderAccountNumber
                                WHERE t.ReceiverUEN = @UEN
                                AND YEAR(TransactionDateTime) = @dtYear
                                AND MONTH(TransactionDateTime) = @dtMonth";

            cmd.Parameters.AddWithValue("@UEN", UEN);
            cmd.Parameters.AddWithValue("@dtYear", dtYear);
            cmd.Parameters.AddWithValue("@dtMonth", dtMonth);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    transaction.Payee = reader.GetString(0); //0: 1st column
                    transaction.SenderAccountNumber = reader.GetString(1);  //1: 2nd column
                    transaction.Amount = reader.GetDecimal(2); //1: 2nd column
                    transaction.PayeeTelNo = reader.GetString(3);
                    transaction.TransactionDateTime = reader.GetDateTime(4);
                };
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return transaction;
        }
    }
}