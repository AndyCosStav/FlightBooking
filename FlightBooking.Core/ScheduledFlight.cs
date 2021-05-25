using System;
using System.Linq;
using System.Collections.Generic;

namespace FlightBooking.Core
{
    public class ScheduledFlight
    {
        private readonly string _verticalWhiteSpace = Environment.NewLine + Environment.NewLine;
        private readonly string _newLine = Environment.NewLine;
        private const string Indentation = "    ";

        public ScheduledFlight(FlightRoute flightRoute)
        {
            FlightRoute = flightRoute;
            Passengers = new List<Passenger>();
            Rules = new List<BusinessRule>();
        }

        public ScheduledFlight()
        {
        }

        public FlightRoute FlightRoute { get; }
        public Plane Aircraft { get; private set; }
        public List<Passenger> Passengers { get; }
        public List<BusinessRule> Rules { get; }

        public void AddPassenger(Passenger passenger)
        {
            Passengers.Add(passenger);
        }

        public void AddRule(BusinessRule rule)
        {
            Rules.Add(rule);
        }

        public void SetAircraftForRoute(Plane aircraft)
        {
            Aircraft = aircraft;
        }

        public void SetPassenger(List<Passenger> Passengers)
        {
            double costOfFlight = 0;
            double profitFromFlight = 0;
            int totalLoyaltyPointsAccrued = 0;
            int totalLoyaltyPointsRedeemed = 0;
            int totalExpectedBaggage = 0;
            int seatsTaken = 0;

            foreach (var passenger in Passengers)
            {
                switch (passenger.Type)
                {
                    case (PassengerType.General):
                        {
                            profitFromFlight += FlightRoute.BasePrice;
                            totalExpectedBaggage++;
                            break;
                        }
                    case (PassengerType.LoyaltyMember):
                        {
                            if (passenger.IsUsingLoyaltyPoints)
                            {
                                var loyaltyPointsRedeemed = Convert.ToInt32(Math.Ceiling(FlightRoute.BasePrice));
                                passenger.LoyaltyPoints -= loyaltyPointsRedeemed;
                                totalLoyaltyPointsRedeemed += loyaltyPointsRedeemed;
                            }
                            else
                            {
                                totalLoyaltyPointsAccrued += FlightRoute.LoyaltyPointsGained;
                                profitFromFlight += FlightRoute.BasePrice;
                            }
                            totalExpectedBaggage += 2;
                            break;
                        }
                    case (PassengerType.AirlineEmployee):
                        {
                            totalExpectedBaggage += 1;
                            break;
                        }

                    case (PassengerType.Discounted):
                        {
                            profitFromFlight += FlightRoute.BasePrice * 0.5;
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                costOfFlight += FlightRoute.BaseCost;
                seatsTaken++;
            }

            PrintResults(Passengers, costOfFlight, profitFromFlight, totalLoyaltyPointsAccrued, totalLoyaltyPointsRedeemed, totalExpectedBaggage, seatsTaken);

        }

        public void PrintResults(List<Passenger> Passengers, double costOfFlight, double profitFromFlight, int totalLoyaltyPointsAccrued, int totalLoyaltyPointsRedeemed, int totalExpectedBaggage, int seatsTaken)
        {
            var result = "Flight summary for " + FlightRoute.Title;

            result += _verticalWhiteSpace;

            result += "Total passengers: " + seatsTaken;
            result += _newLine;
            result += Indentation + "General sales: " + Passengers.Count(p => p.Type == PassengerType.General);
            result += _newLine;
            result += Indentation + "Loyalty member sales: " + Passengers.Count(p => p.Type == PassengerType.LoyaltyMember);
            result += _newLine;
            result += Indentation + "Airline employee comps: " + Passengers.Count(p => p.Type == PassengerType.AirlineEmployee);
            result += _newLine;
            result += Indentation + "Discounted sales: " + Passengers.Count(p => p.Type == PassengerType.Discounted);


            result += _verticalWhiteSpace;
            result += "Total expected baggage: " + totalExpectedBaggage;

            result += _verticalWhiteSpace;

            result += "Total revenue from flight: " + profitFromFlight;
            result += _newLine;
            result += "Total costs from flight: " + costOfFlight;
            result += _newLine;

            var profitSurplus = profitFromFlight - costOfFlight;

            result += (profitSurplus > 0 ? "Flight generating profit of: " : "Flight losing money of: ") + profitSurplus;

            result += _verticalWhiteSpace;

            result += "Total loyalty points given away: " + totalLoyaltyPointsAccrued + _newLine;
            result += "Total loyalty points redeemed: " + totalLoyaltyPointsRedeemed + _newLine;

            result += _verticalWhiteSpace;

            result += DefineRule(seatsTaken, profitFromFlight, costOfFlight, Rules);

            result += FindAlternatePlane(Passengers,Aircraft);

            System.Console.WriteLine(result);
        }

        public string DefineRule(int seatsTaken, double profitFromFlight, double costOfFlight,List<BusinessRule>Rules)
        {
            var result = " ";

            var profitSurplus = profitFromFlight - costOfFlight;

            foreach (var rule in Rules)
            {
                if (rule.RuleType == RuleType.Relaxed)
                {
                    if (Passengers.Count(p => p.Type == PassengerType.AirlineEmployee) > FlightRoute.MinimumTakeOffPercentage &&
                        seatsTaken < Aircraft.NumberOfSeats)
                    {
                        result += "THIS FLIGHT MAY PROCEED";
                    }
                    else
                    {
                        result += "FLIGHT MAY NOT PROCEED";
                    }

                    return result;

                }

                else if (rule.RuleType == RuleType.Default)
                {
                    if (profitSurplus > 0 &&
                        seatsTaken < Aircraft.NumberOfSeats &&
                        seatsTaken / (double)Aircraft.NumberOfSeats > FlightRoute.MinimumTakeOffPercentage)
                    {
                        result += "THIS FLIGHT MAY PROCEED";
                    }
                    else
                    {
                        result += "FLIGHT MAY NOT PROCEED";
                    }
                    return result;
                }

            }

            return result;

        }

        public string FindAlternatePlane(List<Passenger> passengers, Plane Aircraft)
        {
            string result = "";
            var planeList = new List<Plane>()
            {
                new Plane() {Id = 123, Name = "Bombardier Q400", NumberOfSeats = 7},
                new Plane() {Id = 124, Name = "ATR 640", NumberOfSeats = 20 }
            };
            if (passengers.Count() > Aircraft.NumberOfSeats)
            {
                foreach (var plane in planeList)
                {
                    if (plane.NumberOfSeats >= passengers.Count())
                    {
                        result += _verticalWhiteSpace;
                        result += $"{plane.Name} could handle this flight";
                    }
                }
            }

            return result;

        }

        public void GetSummary()
        {
            SetPassenger(Passengers);
        }


        }
    }
