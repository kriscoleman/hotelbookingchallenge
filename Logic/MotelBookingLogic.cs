using System;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Logic
{
    /// <summary>
    /// Motel Booking Logic
    /// todo: db/repository
    /// todo: ordering
    /// todo: motel configuration
    /// todo: room availability
    /// todo: concurrent bookings (bookingResponses should reserve room for 5 minutes before placing order to finalize)
    /// </summary>
    public class MotelBookingLogic : IMotelBookingLogic
    {
        public async Task<BookingResponse> BookAsync(Booking booking)
        {
            if (!await BookingIsValidAsync(booking)) throw new InvalidOperationException();

            var response = new BookingResponse();

            if (booking.HandicapAccessible && booking.Floor > 1)
            {
                response.IsError = true;
                response.FriendlyErrorMessage = "Cannot book upstairs when requesting Handicap Accessibility.";
                return response;
            }

            if (booking.Pets > 2)
            {
                response.IsError = true;
                response.FriendlyErrorMessage = "Cannot book more than 2 pets per room.";
                return response;
            }

            if (booking.Pets > 0 && booking.Floor > 1)
            {
                response.IsError = true;
                response.FriendlyErrorMessage = "Cannot book upstairs with pets.";
                return response;
            }

            var days = (booking.EndDate - booking.StartDate).Days;
            if (days <= 0)
            {
                response.IsError = true;
                response.FriendlyErrorMessage = "Bookings must be for at least one day.";
                return response;
            }

            response.LineItems.Add(new LineItem
            {
                Name = GetRoomLineItemName(booking.NumberOfBeds),
                Cost = GetRoomCost(booking.NumberOfBeds),
                Quantity = days
            });

            if (booking.Pets > 0 && booking.Pets <= 2)
                response.LineItems.Add(new LineItem
                {
                    Name = "Pet Surcharge",
                    Cost = await GetPetSurchargeAsync(),
                    Quantity = booking.Pets
                });

            response.SubTotal = response.LineItems.Sum(x => x.Cost * x.Quantity);

            return response;
        }

        public Task<bool> BookingIsValidAsync(Booking booking)
        {
            if (booking.Pets < 0 || booking.NumberOfBeds < 0 || booking.Floor < 0)
                return Task.FromResult(false);

            if (booking.StartDate > booking.EndDate || booking.EndDate < booking.StartDate)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public Task<decimal> GetPetSurchargeAsync()
        {
            // todo: someday, we could pull config from a file or db...
            // or introduce the concept of many motels and introduce motel id's
            return Task.FromResult(20m);
        }

        private decimal GetRoomCost(int bookingNumberOfBeds)
        {
            switch (bookingNumberOfBeds)
            {
                case 1:
                    return 50m;
                case 2:
                    return 75m;
                case 3:
                    return 90m;
                default:
                    throw new Exception("Number of beds must be 1, 2, or 3.");
            }
        }

        private string GetRoomLineItemName(int bookingNumberOfBeds)
        {
            switch (bookingNumberOfBeds)
            {
                case 1:
                    return "Room with 1 Bed";
                case 2:
                    return "Room with 2 Beds";
                case 3:
                    return "Room with 3 Beds";
                default:
                    throw new Exception("Number of beds must be 1, 2, or 3.");
            }
        }
    }
}