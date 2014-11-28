using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class User
    {
        public string UserName { get; set; }
        public int Balance { get; set; }


        public User(string userName)
        {
            UserName = userName;
            Balance = 1000;
        }
    }


}
