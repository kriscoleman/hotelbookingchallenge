using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class BookingResponse
    {
        public bool IsError { get; set; }
        public string FriendlyErrorMessage { get; set; }

        public decimal SubTotal { get; set; }

        public List<LineItem> LineItems { get; set; } = new List<LineItem>();

        public override bool Equals(object obj)
        {
            if (!(obj is BookingResponse response)) return false;

            if (LineItems.Count != response.LineItems.Count) return false;

            foreach (var lineItem in LineItems)
            {
                foreach (var responseLineItem in response.LineItems)
                {
                    if (!lineItem.Equals(responseLineItem))
                        return false;
                }
            }

            return IsError == response.IsError && FriendlyErrorMessage == response.FriendlyErrorMessage &&
                   SubTotal == response.SubTotal;
        }
    }
}