using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBilling.Entities
{
    class Company
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string City { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string VATTinNo { get; set; }
    }
}
