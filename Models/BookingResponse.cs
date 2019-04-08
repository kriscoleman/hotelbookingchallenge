using System;
using System.Collections.Generic;

namespace Models
{
    public class BookingResponse
    {
        public bool IsError { get; set; }
        public string FriendlyErrorMessage { get; set; }

        public decimal SubTotal { get; set; }

        public List<LineItem> LineItems { get; set; }
    }
}