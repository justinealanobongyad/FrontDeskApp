using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Storage
    {
        public int storageId { get; set; }
        public string storageName { get; set; }
        public int availability { get; set; }

        public Storage()
        { 
            storageId = 0;
            storageName = String.Empty;
            availability = 0;
        }

    }
}

