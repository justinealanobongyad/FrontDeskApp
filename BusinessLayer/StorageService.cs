using DataAccessLayer;
using DomainLayer;

namespace BusinessLayer
{
    public class StorageService
    {
        private readonly CustomerRepository customerRepository = new CustomerRepository();
        private readonly StorageRespository storageRespository = new StorageRespository();
        private readonly CustomerStorageRepository customerStorageRepository = new CustomerStorageRepository();

        #region Customers
        public Customer GetCustomer(string firstName, string lastName)
        {
            Customer existingCustomer = new Customer();
            existingCustomer = customerRepository.GetCustomer(firstName, lastName);
            return existingCustomer;
        }

        public int CreateCustomer(Customer customer)
        {
            customer.customerId = customerRepository.CreateCustomer(customer);
            return customer.customerId;
        }
        #endregion

        #region Storage
        public List<Storage> GetStorages()
        {
            List<Storage> storages = new List<Storage>();
            storages = storageRespository.GetStorages();
            return storages;
        }
        #endregion

        #region Customer Storage
        public int CreateCustomerStorage(int customerId, int storageId)
        {
            CustomerStorage customerStorage = new CustomerStorage();
            customerStorage.customerId = customerId;
            customerStorage.storageId = storageId;
            customerStorage.customerStorageId = customerStorageRepository.CreateCustomerStorage(customerStorage);
            return customerStorage.customerStorageId;
        }

        public List<CustomerStorage> GetCustomerStoragesPerCustomer(int customerId)
        {
            List<CustomerStorage> customerStoragesPerCustomer = new List<CustomerStorage>();
            customerStoragesPerCustomer = customerStorageRepository.GetCustomerStoragesPerCustomer(customerId);
            return customerStoragesPerCustomer;
        }

        public List<CustomerStorage> GetCustomerStorages()
        {
            List<CustomerStorage> customerStorages = new List<CustomerStorage>();
            customerStorages = customerStorageRepository.GetCustomerStorages();
            return customerStorages;
        }
        #endregion

    }
}