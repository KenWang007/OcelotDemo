using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Model
{
    public class Booking
    {
        public string PNR { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }
    }
}
