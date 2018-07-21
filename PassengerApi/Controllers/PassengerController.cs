using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PassengerApi.Model;
using PassengerApi.Repository;

namespace PassengerApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Passenger")]
    public class PassengerController : Controller
    {
        // GET: api/Passenger
        [HttpGet]
        public List<Passenger> Get()
        {
            return PassengerDataRepository.GetAll();
        }

        // GET: api/Passenger/5
        [HttpGet("{id}", Name = "Get")]
        public Passenger Get(int id)
        {
            return PassengerDataRepository.GetPassengerById(id);
        }
    }
}
