using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Model;
using BookingApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Booking")]
    public class BookingController : Controller
    {
        // GET: api/Booking
        [HttpGet]
        public List<Booking> Get()
        {
            return BookingDataRepository.GetAll();
        }

        // GET: api/Booking/
        [HttpGet("{pnr}", Name = "Get")]
        public Booking Get(string pnr)
        {
            return BookingDataRepository.GetBookingByPnr(pnr);
        }
    }
}
