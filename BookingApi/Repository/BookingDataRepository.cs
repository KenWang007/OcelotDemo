using BookingApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Repository
{
    public class BookingDataRepository
    {
        public static List<Booking> GetAll()
        {
            return new List<Booking>()
            {
                new Booking() { PNR = "ABCABC", Amount = 100, Comment = "TEST1" },
                new Booking() { PNR = "HNSHAJ", Amount = 110, Comment = "TEST2" },
                new Booking() { PNR = "ANASHD", Amount = 120, Comment = "TEST3" }
            };
        }

        public static Booking GetBookingByPnr(string pnr)
        {
            return GetAll().FirstOrDefault(x => x.PNR == pnr);
        }
    }
}
