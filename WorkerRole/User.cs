using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkerRole
{
    public class User:TableEntity
    {
        public string UserName { get; set; }
        public int Balance { get; set; }


        public User(string userName)
        {           
            UserName = userName;
            Balance = 1000;
            PartitionKey = "users";
            RowKey = UserName;
        }

        public User()
        {
        }
    }
}
