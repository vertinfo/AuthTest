using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthTest.Models
{
    public class Login
    {
        public string user { get; set; } = "";
        public string password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string vxres { get; set; } = "";
        public string code { get; set; }
        public string myprofile { get; set; }
    }
}
