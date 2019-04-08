using System;
using System.Threading.Tasks;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class PetTests : BookingLogicTestsBase
    {
        [Test]
        public void ICannotBookUpstairsWithPets() =>
            Assert.That(async ()=> await _bookingLogic.BookAsync(new Booking
            {
                Floor = 2,
                Pets = 1
            }), Is.EqualTo(new BookingResponse
            {
                IsError = true,
                FriendlyErrorMessage = "Can not book upstairs with pets."
            }));

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
            Assert.That(bookingResponse.LineItems, Contains.Value(new LineItem
            {
                Name = "Pet Surcharge", 
                Cost = petSurcharge,
                Quantity = 1
            }));

            // 2 pets should be double the cost
            booking.Pets = 2;
            bookingResponse = await _bookingLogic.BookAsync(booking);

            Assert.That(bookingResponse.SubTotal, Is.EqualTo(costBeforeIncludingPets + (petSurcharge * 2)));
            Assert.That(bookingResponse.LineItems, Contains.Value(new LineItem
            {
                Name = "Pet Surcharge", 
                Cost = petSurcharge, 
                Quantity = 2
            }));
        }

        [Test]
        public void ICanBookDownstairsWithPets() =>
            Assert.That(async ()=> await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 1
            }), Is.EqualTo(new BookingResponse
            {
                IsError = false
            }));

        [Test]
        public void ICanBook1Pet() =>
            Assert.That(async ()=> await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 1
            }), Is.EqualTo(new BookingResponse
            {
                IsError = false
            }));

        [Test]
        public void ICanBook2Pets() =>
            Assert.That(async ()=> await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 2
            }), Is.EqualTo(new BookingResponse
            {
                IsError = false
            }));

        [Test]
        public void ICannotBookTooManyPets()
        {
            // can't do 3
            Assert.That(async ()=> await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = 3
            }), Is.EqualTo(new BookingResponse
            {
                IsError = true, 
                FriendlyErrorMessage = "Can not book more than 2 pets per room."
            }));

            // can't do a bunch
            Assert.That(async ()=> await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = int.MaxValue
            }), Is.EqualTo(new BookingResponse
            {
                IsError = true,
                FriendlyErrorMessage = "Can not book more than 2 pets per room."
            }));

            // just to be sure, can't do negative either.
            Assert.That(async ()=> await _bookingLogic.BookAsync(new Booking
            {
                Floor = 1,
                Pets = -1
            }), Is.EqualTo(new BookingResponse
            {
                IsError = true,
            }));
        }
    }
}