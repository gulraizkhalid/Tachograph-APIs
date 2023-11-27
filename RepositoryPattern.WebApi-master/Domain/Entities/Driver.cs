using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address{ get; set; }
        public int MobileNumber { get; set; }
        public string Nationality { get; set; }
        public int LicenseNumber { get; set; }
    }
}
