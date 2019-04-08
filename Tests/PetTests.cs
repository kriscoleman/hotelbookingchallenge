using System;
using System.Linq;
using System.Threading.Tasks;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class PetTests : BookingLogicTestsBase
    {
        [Test]
        public async Task ICannotBookUpstairsWithPets()
        {
            var bookingResponse = await _bookingLogic.BookAsync(new Booking
            {
                Floor = 2,
                Pets = 1
            });

            Assert.AreEqual(bookingResponse, new BookingResponse
            {
                IsError = true,
                FriendlyErrorMessage = "Cannot book upstairs with pets."
            });
        }

        [Test]
        public void ICanBookDownstairsWithPets() =>
            Assert.That(async ()=> (await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 1,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            })).IsError, Is.False);

        [Test]
        public async Task IfIBookPetsItAddsSurcharge()
        {
            var petSurcharge = await _bookingLogic.GetPetSurchargeAsync();

            var booking = new Booking
            {
                Floor = 1,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            var bookingResponse = await _bookingLogic.BookAsync(booking);
            var costBeforeIncludingPets = bookingResponse.SubTotal;

            booking.Pets = 1;
            bookingResponse = await _bookingLogic.BookAsync(booking);
            Assert.That(bookingResponse.SubTotal, Is.EqualTo(costBeforeIncludingPets + petSurcharge));
            Assert.AreEqual(bookingResponse.LineItems.Single(x => x.Name == "Pet Surcharge"), new LineItem
            {
                Name = "Pet Surcharge", 
                Cost = petSurcharge,
                Quantity = 1
            });

            // 2 pets should be double the cost
            booking.Pets = 2;
            bookingResponse = await _bookingLogic.BookAsync(booking);

            Assert.That(bookingResponse.SubTotal, Is.EqualTo(costBeforeIncludingPets + (petSurcharge * 2)));
            Assert.AreEqual(bookingResponse.LineItems.Single(x => x.Name == "Pet Surcharge"), new LineItem
            {
                Name = "Pet Surcharge", 
                Cost = petSurcharge, 
                Quantity = 2
            });
        }

        [Test]
        public void ICanBook1Pet() =>
            Assert.That(async ()=> (await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 1,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            })).IsError, Is.False);

        [Test]
        public void ICanBook2Pets() =>
            Assert.That(async ()=> (await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 2,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            })).IsError, Is.False);

        [Test]
        public async Task ICannotBookTooManyPets()
        {
            var response = await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 3,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            });
            // can't do 3
            Assert.That(response.IsError, Is.True);
            Assert.That(response.FriendlyErrorMessage, Is.EqualTo("Cannot book more than 2 pets per room."));

            // can't do a bunch
            response = await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = int.MaxValue,
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            });
            Assert.That(response.IsError, Is.True);
            Assert.That(response.FriendlyErrorMessage, Is.EqualTo("Cannot book more than 2 pets per room."));
        }
    }
}