using FlightBooking.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace FlightBooking.Tests
{
    public class TestClass
    {

        private static ScheduledFlight _scheduledFlight;

        [Fact]
        public void TestAlternatePlane()
        {
            var passengers = new List<Passenger>
            {
               new Passenger() {Name = "Andy", Age=30, Type= PassengerType.General },
               new Passenger() {Name = "Rita", Age=30, Type= PassengerType.General },
               new Passenger() {Name = "Sue", Age=30, Type= PassengerType.General },
               new Passenger() {Name = "Bob", Age=30, Type= PassengerType.General },
               new Passenger() {Name = "Arnie", Age=30, Type= PassengerType.General },
               new Passenger() {Name = "Billy", Age=30, Type= PassengerType.General },
               new Passenger() {Name = "John", Age=30, Type= PassengerType.General },
               new Passenger() {Name = "Steve", Age=30, Type= PassengerType.General },
            };

            var aircraft = new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 5 };

            var test = new ScheduledFlight();
            var result = test.FindAlternatePlane(passengers,aircraft);

            Assert.Equal("\r\n\r\nATR 640 could handle this flight", result);
        }

    }
}
