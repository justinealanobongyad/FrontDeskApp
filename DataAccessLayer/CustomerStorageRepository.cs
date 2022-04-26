using DomainLayer;
using System.Data;
using System.Data.OleDb;

namespace DataAccessLayer
{
    public class CustomerStorageRepository
    {
        private string _dataPath = String.Format(@"{0}\{1}", Environment.CurrentDirectory, "FrontDeskAppData.xlsx");
        private string _sheetName = "Customer_Storages";
        private string _connectionString = String.Empty;

        public CustomerStorageRepository()
        {
            this._connectionString = $@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = { this._dataPath }; Extended Properties = 'Excel 12.0 Xml;HDR=Yes;'";
        }

        public int CreateCustomerStorage(CustomerStorage customerStorage)
        {
            int currentCustomerStorageId = GetCustomerStorages().Count + 1;
            try
            {
                using (OleDbConnection oleDbConnection = new OleDbConnection(_connectionString))
                {
                    DataTable dataTable = new DataTable();
                    string insertCommand = $"INSERT INTO [{_sheetName}$] (customerStorageId, customerId, storageId) VALUES (" + currentCustomerStorageId + "," + customerStorage.customerId + "," + customerStorage.storageId + ")";
                    OleDbCommand command = new OleDbCommand(insertCommand, oleDbConnection);
                    oleDbConnection.Open();
                    command.ExecuteNonQuery();
                    oleDbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("An error occured while adding the customer storage details.");
                currentCustomerStorageId = 0;
            }
            return currentCustomerStorageId;
        }

        public List<CustomerStorage> GetCustomerStoragesPerCustomer(int customerId)
        {
            List<CustomerStorage> customerStorages = new List<CustomerStorage>();
            try
            {
                using (OleDbConnection oleDbConnection = new OleDbConnection(_connectionString))
                {
                    DataTable dataTable = new DataTable();
                    string selectCommand = $"SELECT * FROM [{_sheetName}$] WHERE customerId='" + customerId +"'";
                    OleDbCommand command = new OleDbCommand(selectCommand, oleDbConnection);
                    oleDbConnection.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                    oleDbConnection.Close();
                    customerStorages = SetData(dataTable);
                }
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("An error occured while fetching the storage details.");
                customerStorages = new List<CustomerStorage>();
            }
            return customerStorages;
        }


        public List<CustomerStorage> GetCustomerStorages()
        {
            List<CustomerStorage> customerStorages = new List<CustomerStorage>();
            try
            {
                using (OleDbConnection oleDbConnection = new OleDbConnection(_connectionString))
                {
                    DataTable dataTable = new DataTable();
                    string selectCommand = $"SELECT * FROM [{_sheetName}$]";
                    OleDbCommand command = new OleDbCommand(selectCommand, oleDbConnection);
                    oleDbConnection.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                    oleDbConnection.Close();
                    customerStorages = SetData(dataTable);
                }
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("An error occured while fetching the storage details.");
                customerStorages = new List<CustomerStorage>();
            }
            return customerStorages;
        }

        private List<CustomerStorage> SetData(DataTable dataTable)
        {
            List<CustomerStorage> customerStorages = new List<CustomerStorage>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                CustomerStorage customerStorage = new CustomerStorage();
                customerStorage.customerStorageId = Convert.ToInt32(dataRow["customerStorageId"].ToString());
                customerStorage.customerId = Convert.ToInt32(dataRow["customerId"].ToString());
                customerStorage.storageId = Convert.ToInt32(dataRow["storageId"].ToString());
                customerStorages.Add(customerStorage);
            }
            return customerStorages;
        }

    }
}