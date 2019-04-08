using System;

namespace Models
{
    public class Booking
    {
        public int Floor { get; set; }
        public int Pets { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool HandicapAccessible { get; set; }
        public int NumberOfBeds { get; set; }
    }
}