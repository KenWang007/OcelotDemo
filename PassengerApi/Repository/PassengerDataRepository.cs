using PassengerApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PassengerApi.Repository
{
    public class PassengerDataRepository
    {
        public static List<Passenger> GetAll()
        {
            return new List<Passenger>()
            {
                new Passenger() { Id = 1001, Name = "test", Address="test address", PhoneNumber = 10086 },
                new Passenger() { Id = 1002, Name = "test2", Address="test address2", PhoneNumber = 10087 },
                new Passenger() { Id = 1003, Name = "test3", Address="test address3", PhoneNumber = 10088 }
            };
        }

        public static Passenger GetPassengerById(int passengerId)
        {
            return GetAll().FirstOrDefault(x => x.Id == passengerId);
        }
    }
}
