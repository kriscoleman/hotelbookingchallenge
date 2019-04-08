using System;
using System.Linq;
using System.Threading.Tasks;
using Models;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BasicBookingTests : BookingLogicTestsBase
    {
        [Test]
        public async Task IfIBookA1BedItWillCost50ANight()
        {
            var booking = new Booking
            {
                Floor = 1,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            var bookingResponse = await _bookingLogic.BookAsync(booking);

            Assert.That(bookingResponse.SubTotal, Is.EqualTo(50m));
            Assert.AreEqual(bookingResponse.LineItems.Single(), new LineItem
            {
                Name = "Room with 1 Bed",
                Quantity = 1,
                Cost = 50m
            });

            // 1 more day should add another $50
            booking.EndDate = booking.EndDate.AddDays(1);

            bookingResponse = await _bookingLogic.BookAsync(booking);
            Assert.That(bookingResponse.SubTotal, Is.EqualTo(100m));
            Assert.AreEqual(bookingResponse.LineItems.Single(), new LineItem
            {
                Name = "Room with 1 Bed",
                Quantity = 2,
                Cost = 50m
            });
        }

        [Test]
        public async Task IfIBookA2BedItWillCost75ANight()
        {
            var booking = new Booking
            {
                Floor = 1,
                NumberOfBeds = 2,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            var bookingResponse = await _bookingLogic.BookAsync(booking);

            Assert.That(bookingResponse.SubTotal, Is.EqualTo(75m));
            Assert.AreEqual(bookingResponse.LineItems.Single(), new LineItem
            {
                Name = "Room with 2 Beds",
                Quantity = 1,
                Cost = 75m
            });

            // 1 more day should add another $75
            booking.EndDate = booking.EndDate.AddDays(1);

            bookingResponse = await _bookingLogic.BookAsync(booking);
            Assert.That(bookingResponse.SubTotal, Is.EqualTo(150m));
            Assert.AreEqual(bookingResponse.LineItems.Single(), new LineItem
            {
                Name = "Room with 2 Beds",
                Quantity = 2,
                Cost = 75m
            });
        }

        [Test]
        public async Task IfIBookA3BedItWillCost90ANight()
        {
            var booking = new Booking
            {
                Floor = 1,
                NumberOfBeds = 3,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            var bookingResponse = await _bookingLogic.BookAsync(booking);

            Assert.That(bookingResponse.SubTotal, Is.EqualTo(90m));
            Assert.AreEqual(bookingResponse.LineItems.Single(), new LineItem
            {
                Name = "Room with 3 Beds",
                Quantity = 1,
                Cost = 90m
            });

            // 1 more day should add another $90
            booking.EndDate = booking.EndDate.AddDays(1);

            bookingResponse = await _bookingLogic.BookAsync(booking);
            Assert.That(bookingResponse.SubTotal, Is.EqualTo(180m));
            Assert.AreEqual(bookingResponse.LineItems.Single(), new LineItem
            {
                Name = "Room with 3 Beds",
                Quantity = 2,
                Cost = 90m
            });
        }
    }
}