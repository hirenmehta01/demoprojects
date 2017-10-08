using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBilling.Entities
{
    /// <summary>
    /// which is usefull for bind data from database.
    /// </summary>
    public class User
    {
        public string userid { get; set; }
        public string password { get; set; }
        public int usertypeid { get; set; }
    }
}
