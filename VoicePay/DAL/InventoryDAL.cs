using System.Data.SqlClient;
using VoicePay.Models;

namespace VoicePay.DAL
{
    public class InventoryDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public InventoryDAL()
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

        public List<Inventory> GetInventoryDetails(string accId)
        {
            List<Inventory> InventoryList = new List<Inventory>();

            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by ParcelID
            cmd.CommandText = "SELECT * FROM Inventory WHERE AccID = @accId";
            cmd.Parameters.AddWithValue("@accId", accId);

            // Open a database connection
            conn.Open();

            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            // Read all records until the end, save data into a parcel list
            while (reader.Read())
            {
                InventoryList.Add(new Inventory
                {
                    // Map database columns to Parcel properties accordingly
                    InventoryID = reader.GetInt32(0),
                    ItemName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    SupplierName = reader.GetString(3),
                    SupplierContactNo = reader.GetString(4)
                }); 
            }

            // Close DataReader and the database connection
            reader.Close();
            conn.Close();

            return InventoryList;
        }

        public Inventory GetInventoryItem(int InventoryId, string accId)
        {
            Inventory inventory = new Inventory();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Inventory
            WHERE InventoryID = @selectedInventoryId
            AND AccID = '@accId'";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedInventoryId", InventoryId);
            cmd.Parameters.AddWithValue("@accId", accId);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    inventory.InventoryID = reader.GetInt32(0);
                    inventory.ItemName = reader.GetString(1);
                    inventory.Quantity = reader.GetInt32(2);
                    inventory.SupplierName = reader.GetString(3);
                    inventory.SupplierContactNo = reader.GetString(4);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return inventory;
        }

        public int Update(Inventory inventory)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Inventory SET ItemName= @ItemName,
                                Quantity = @Quantity, SupplierName = @SupplierName,
                                SupplierContactNo = @SupplierContactNo
                                WHERE InventoryID = @selectedInventoryID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ItemName", inventory.ItemName);
            cmd.Parameters.AddWithValue("@Quantity", inventory.Quantity);
            cmd.Parameters.AddWithValue("@SupplierName", inventory.SupplierName);
            cmd.Parameters.AddWithValue("@SupplierContactNo", inventory.SupplierContactNo);
            cmd.Parameters.AddWithValue("@selectedInventoryID", inventory.InventoryID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public void Add(Inventory inventory)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Inventory (InventoryID, ItemName, Quantity, SupplierName, SupplierContactNo,AccId)
                            VALUES(@InventoryID, @ItemName, @Quantity, @SupplierName, @SupplierContactNo, @AccId)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@InventoryID", inventory.InventoryID);
            cmd.Parameters.AddWithValue("@ItemName", inventory.ItemName);
            cmd.Parameters.AddWithValue("@Quantity", inventory.Quantity);
            cmd.Parameters.AddWithValue("@SupplierName", inventory.SupplierName);
            cmd.Parameters.AddWithValue("@SupplierContactNo", inventory.SupplierContactNo);
            cmd.Parameters.AddWithValue("@AccId", inventory.AccId);
            //A connection to database must be opened before any operations made.
            conn.Open();
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
        }


        public int CountItems(string AccId)
        {
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SQL statement to count the number of parcels with the given delivery status for the given delivery man
            cmd.CommandText = @"SELECT COUNT(*) FROM Parcel
                        WHERE AccId = @AccId";

            // Add the parameters for the deliveryManID and deliveryStatus
            cmd.Parameters.AddWithValue("@AccId", AccId);

            // Open the database connection
            conn.Open();

            // Execute the SQL command and get the count of assigned parcels
            int itemCounts = Convert.ToInt32(cmd.ExecuteScalar());

            // Close the database connection
            conn.Close();

            return itemCounts;
        }

    }
}
