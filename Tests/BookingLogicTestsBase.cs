using Api;
using Autofac;
using Logic;
using NUnit.Framework;

namespace Tests
{ 
    public class BookingLogicTestsBase
    {
        protected IMotelBookingLogic _bookingLogic;

        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApiModule());
            var container = builder.Build();

            _bookingLogic = container.Resolve<IMotelBookingLogic>();
        }
    }
}