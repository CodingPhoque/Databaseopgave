using System;
using System.Collections.Generic;
using System.Text;

namespace HotelDBConnection
{
    public class Facility
    {
        public int Facility_id { get; set; }
        public string  Facility_Name { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return $"ID: {Facility_id}, Name: {Facility_Name}"; 
        }
    }
}
