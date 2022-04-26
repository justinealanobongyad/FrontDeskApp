// See https://aka.ms/new-console-template for more information\

using BusinessLayer;
using DomainLayer;

StorageService _storageService = new StorageService();
List<Storage> storages = _storageService.GetStorages();
Console.WriteLine("FrontDeskApp");
Console.WriteLine("---------------------------------------");
Console.WriteLine("[1] New Customer");
Console.WriteLine("[2] Existing Customer");
Console.Write("Please enter the type of customer [1 or 2]: ");
string customerType = Console.ReadLine() ?? string.Empty;
while (String.IsNullOrEmpty(customerType))
{
    Console.Write("You need to enter the type of customer [1 or 2]: ");
    customerType = Console.ReadLine() ?? string.Empty;
}

Boolean isValidInput = false;
while (!String.IsNullOrEmpty(customerType) && !isValidInput)
{
    if (customerType == "1" || customerType == "2")
    {
        isValidInput = true;
    }
    else
    {
        isValidInput = false;
        Console.WriteLine("Please make sure you entered the correct value [1 or 2] and no space or other characters are included");
        Console.Write("You need to enter the type of customer [1 or 2]: ");
        customerType = Console.ReadLine() ?? string.Empty;
    }
}

Console.WriteLine(" ");
Console.WriteLine("---------------------------------------");
//Create a new customer storage
Customer _customer = new Customer();
if (customerType == "1" && isValidInput)
{
    Console.WriteLine("Create a new storage for the customer");
    Console.Write("Enter the customer first name: ");
    _customer.firstName = Console.ReadLine() ?? string.Empty;
    Console.Write("Enter the customer last name: ");
    _customer.lastName = Console.ReadLine() ?? string.Empty;
    Console.Write("Enter the customer phone number: ");
    _customer.phoneNumber = Console.ReadLine() ?? string.Empty;
    _customer.customerId = _storageService.CreateCustomer(_customer);
}
//Retrieve existing customer storage
else if (customerType == "2" && isValidInput)
{
    Console.WriteLine("Retrieve existing customer package details");
    string firstName = String.Empty;
    string lastName = String.Empty;
    while (_customer.customerId == 0)
    {
        Console.Write("Enter the customer first name: ");
        firstName = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter the customer last name: ");
        lastName = Console.ReadLine() ?? string.Empty;
        _customer = _storageService.GetCustomer(firstName, lastName);

        if(_customer.customerId == 0)
        Console.WriteLine("There is no customer with that first name or that last name. Kindly enter the correct name.");
    }

    List<CustomerStorage> customerStoragePerCustomer = new List<CustomerStorage>();
    customerStoragePerCustomer = _storageService.GetCustomerStoragesPerCustomer(_customer.customerId);
    if (customerStoragePerCustomer.Count > 0)
    {
        Console.WriteLine("Reserved storage area: ");
        foreach (Storage storage in storages)
        {
            CustomerStorage customerStorage = customerStoragePerCustomer.Find(x => x.storageId == storage.storageId) != null ? customerStoragePerCustomer.First(x => x.storageId == storage.storageId) : new CustomerStorage();
            if (storage.storageId == customerStorage.storageId)
            {
                Console.WriteLine("{0} : {1} reserved", storage.storageName, customerStoragePerCustomer.Count(x => x.storageId == storage.storageId));
            }
        }
    }
    else
    {
        Console.WriteLine("No reserved storage area");
    }
}

//Show the tally per (Small, Medium, Large) area
Console.WriteLine(" ");
Console.WriteLine("---------------------------------------");
Console.WriteLine("Check storage area for availability");
List<CustomerStorage> customerStorages = _storageService.GetCustomerStorages();
List<Storage> availableStorages = new List<Storage>();
foreach (Storage storage in storages)
{
    int numberOfOccupiedArea = customerStorages.Count(x => x.storageId == storage.storageId);
    int numberOfAvailableArea = storage.availability - numberOfOccupiedArea;
    if (numberOfAvailableArea != 0)
    {
        availableStorages.Add(storage);
    }
    Console.WriteLine(storage.storageName + " Area : " + numberOfAvailableArea + " out of " + storage.availability + " are available");
}

//Accept boxes based on available storage area
if (availableStorages.Count > 0)
{
    Console.WriteLine(" ");
    Console.WriteLine("---------------------------------------");
    Console.WriteLine("Accepting boxes for the following: ");
    foreach (Storage availableStorage in availableStorages)
    {
        Console.WriteLine("[{0}] {1}", availableStorage.storageId, availableStorage.storageName);
    }
    Console.Write("Please enter storage size to accept: ");
    string storageId = Console.ReadLine() ?? string.Empty;
    while (String.IsNullOrEmpty(storageId))
    {
       Console.Write("Please enter a storage size value shown above: ");
       storageId = Console.ReadLine() ?? string.Empty;
    }

    bool isAccepted = false;
    while (!String.IsNullOrEmpty(storageId) && !isAccepted)
    {
        isAccepted = availableStorages.Any(x => x.storageId == Convert.ToInt32(storageId));
        if (!isAccepted)
        {
            Console.Write("The storage size value you selected is invalid or no longer available. Kindly select a value from the available list: ");
            storageId = Console.ReadLine() ?? string.Empty;
        }   
    }



    //Allocate the storage to the customer
    if (isAccepted)
    {
        CustomerStorage customerStorage = new CustomerStorage();
        customerStorage.customerStorageId = _storageService.CreateCustomerStorage(_customer.customerId, Convert.ToInt32(storageId));
        Console.WriteLine(" ");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Storage area has been successfully reserved for " + _customer.firstName + " " + _customer.lastName);
    }
}
else
{
    Console.WriteLine(" ");
    Console.WriteLine("---------------------------------------");
    Console.WriteLine("Sorry, there is no more storage area available");
}




