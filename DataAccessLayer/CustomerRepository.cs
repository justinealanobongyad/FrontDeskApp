using DomainLayer;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace DataAccessLayer
{
    public class CustomerRepository
    {
        private string _dataPath = String.Format(@"{0}\{1}",Environment.CurrentDirectory,"FrontDeskAppData.xlsx"); 
        private string _sheetName = "Customers";
        private string _connectionString = String.Empty;
        public CustomerRepository()
        {
            this._connectionString = $@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = { this._dataPath }; Extended Properties = 'Excel 12.0 Xml;HDR=Yes;'";
        }

        public int CreateCustomer(Customer customer)
        {
            int currentCustomerId = GetCustomers().Count + 1;
            try
            {
                using (OleDbConnection oleDbConnection = new OleDbConnection(_connectionString))
                {
                    DataTable dataTable = new DataTable();
                    string insertCommand = $"INSERT INTO [{_sheetName}$] (customerId, firstName, lastName, phoneNumber) VALUES ("+ currentCustomerId + ",'"+customer.firstName+"','"+customer.lastName+"','"+customer.phoneNumber+"')";
                    OleDbCommand command = new OleDbCommand(insertCommand, oleDbConnection);
                    oleDbConnection.Open();
                    command.ExecuteNonQuery();
                    oleDbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("An error occured while adding the customer details.");
                currentCustomerId = 0;
            }
            return currentCustomerId;
        }

        public Customer GetCustomer(string firstName, string lastName)
        {
            Customer customer = new Customer();
            try
            {
                using (OleDbConnection oleDbConnection = new OleDbConnection(_connectionString))
                {
                    DataTable dataTable = new DataTable();
                    string selectCommand = $"SELECT * FROM [{_sheetName}$] WHERE firstName='" + firstName + "' AND lastName='" + lastName + "'";
                    OleDbCommand command = new OleDbCommand(selectCommand, oleDbConnection);
                    oleDbConnection.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                    oleDbConnection.Close();
                    customer = SetCustomerData(dataTable);
                }
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("An error occured while fetching the customer details.");
                customer = new Customer();
            }
            return customer;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
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
                    customers = SetData(dataTable);
                }
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("An error occured while fetching the customer details.");
                    customers = new List<Customer>();
            }
            return customers;
        }

        private Customer SetCustomerData(DataTable dataTable)
        { 
            Customer customer = new Customer();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                customer.customerId = Convert.ToInt32(dataRow["customerId"].ToString());
                customer.firstName = dataRow["firstName"].ToString() ?? String.Empty;
                customer.lastName = dataRow["lastName"].ToString() ?? String.Empty;
                customer.phoneNumber = dataRow["phoneNumber"].ToString() ?? String.Empty;
            }
            return customer;
        }

        private List<Customer> SetData(DataTable dataTable)
        {
            List<Customer> customers = new List<Customer>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Customer customer = new Customer();
                customer.customerId = Convert.ToInt32(dataRow["customerId"].ToString());
                customer.firstName = dataRow["firstName"].ToString() ?? String.Empty;
                customer.lastName = dataRow["lastName"].ToString() ?? String.Empty;
                customer.phoneNumber = dataRow["phoneNumber"].ToString() ?? String.Empty;
                customers.Add(customer);
            }
            return customers;
        }

    }
}