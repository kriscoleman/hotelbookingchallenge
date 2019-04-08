using System;
using System.Threading.Tasks;
using Models;
using NUnit.Framework;

namespace Tests
{
   [TestFixture]
    public class HandicapAccessibilityTests : BookingLogicTestsBase
    {
        [Test]
        public async Task IfIBookUpstairsAndRequestHandicapAccessibleItFails() =>
            Assert.That(async () => await _bookingLogic.BookAsync(new Booking
            {
                Floor = 2,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow, 
                EndDate = DateTime.UtcNow.AddDays(1), 
                HandicapAccessible = true
            }), Is.EqualTo(new BookingResponse
            {
                IsError = true,
                FriendlyErrorMessage = "Cannot book upstairs when requesting Handicap Accessibility."
            }));

        [Test]
        public async Task IfIBookDownstairsAndRequestHandicapAccessibleItSucceeds() =>
            Assert.That(async () => (await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow, 
                EndDate = DateTime.UtcNow.AddDays(1), 
                HandicapAccessible = true
            })).IsError, Is.False);
    }
}