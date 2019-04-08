using System.Threading.Tasks;
using Models;

namespace Logic
{
    public interface IHotelBookingLogic {
        Task<BookingResponse> BookAsync(Booking booking);
        Task<decimal> GetPetSurchargeAsync();
    }
}