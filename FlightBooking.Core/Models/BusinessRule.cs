using System;
using System.Collections.Generic;
using System.Text;

namespace FlightBooking.Core
{
    public class BusinessRule
    {
        public RuleType RuleType { get; set; }
    }
    
    public enum RuleType
    {
        Default,
        Relaxed
    }
}
