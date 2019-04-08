using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers

{
    /// <summary>
    /// The Booking Api Controller
    /// todo: Auth
    /// </summary>
    [Route("api/book")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMotelBookingLogic _bookingLogic;

        public BookingController(IMotelBookingLogic bookingLogic)
        {
            _bookingLogic = bookingLogic;
        }

        [HttpPost]
        public async Task<IActionResult> BookAsync([FromBody] Booking booking) =>
            (await _bookingLogic.BookingIsValidAsync(booking))
                ? Ok(await _bookingLogic.BookAsync(booking))
                : (IActionResult) BadRequest();
    }
}