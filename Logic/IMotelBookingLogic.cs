using System.Threading.Tasks;
using Models;

namespace Logic
{
    public interface IMotelBookingLogic {
        Task<BookingResponse> BookAsync(Booking booking);
        Task<decimal> GetPetSurchargeAsync();
        Task<bool> BookingIsValidAsync(Booking booking);
    }
}