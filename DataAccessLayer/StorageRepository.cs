using DomainLayer;
using System.Data;
using System.Data.OleDb;

namespace DataAccessLayer
{
    public class StorageRespository
    {
        private string _dataPath = String.Format(@"{0}\{1}", Environment.CurrentDirectory, "FrontDeskAppData.xlsx");
        private string _sheetName = "Storages";
        private string _connectionString = String.Empty;

        public StorageRespository()
        {
            this._connectionString = $@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = { this._dataPath }; Extended Properties = 'Excel 12.0 Xml;HDR=Yes;'";
        }

        public List<Storage> GetStorages()
        {
            List<Storage> storages = new List<Storage>();
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
                    storages = SetData(dataTable);
                }
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("An error occured while fetching the storage details.");
                storages = new List<Storage>();
            }
            return storages;
        }

        private List<Storage> SetData(DataTable dataTable)
        {
            List<Storage> storages = new List<Storage>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Storage storage = new Storage();
                storage.storageId = Convert.ToInt32(dataRow["storageId"].ToString());
                storage.storageName = dataRow["storageName"].ToString() ?? String.Empty;
                storage.availability = Convert.ToInt32(dataRow["availability"].ToString());
                storages.Add(storage);
            }
            return storages;
        }

    }
}