using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class CustomerStorage
    {
        public int customerStorageId { get; set; }
        public int customerId { get; set; }
        public int storageId { get; set; }
        public CustomerStorage()
        {
            customerStorageId = 0;
            customerId = 0;
            storageId = 0;
        }
    }
}
