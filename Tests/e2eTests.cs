using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    internal class e2eTests
    {
        private HttpClient _apiClient;

        private TestServer _testApi;

        [SetUp]
        public void SetUp()
        {
            _testApi = new TestServer(new WebHostBuilder().ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>());

            _apiClient = _testApi.CreateClient();
        }

        [Test]
        public async Task IfIBookAGoodBookingICanGetASuccessfulResponse()
        {
            var response = await _apiClient.PostAsync("api/book", new StringContent(JsonConvert.SerializeObject(new Booking
            {
                Floor = 1, 
                NumberOfBeds = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1), 
            })));

            Assert.That(response.StatusCode == HttpStatusCode.OK);

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BookingResponse>(resultJson); 
            Assert.That(result.IsError, Is.False);
        }

        [Test]
        public async Task IfIBookABadBookingICanGetAFailureResponse()
        {
            var response = await _apiClient.PostAsync("api/book", new StringContent(JsonConvert.SerializeObject(new Booking
            {
                Floor = 2, 
                NumberOfBeds = 3,
                Pets = 99,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1), 
            })));

            Assert.That(response.StatusCode == HttpStatusCode.OK);

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BookingResponse>(resultJson); 
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FriendlyErrorMessage, Is.EqualTo("Can not book more than 2 pets per room."));
        }

        [Test]
        public async Task IfIBookAInvalidBookingIGetBackBadRequest()
        {
            var response = await _apiClient.PostAsync("api/book", new StringContent(JsonConvert.SerializeObject(new Booking
            {
                Floor = -1
            })));

            Assert.That(response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}