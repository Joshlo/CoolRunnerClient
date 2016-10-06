using System.Collections.Generic;
using System.Reflection;
using CRClient.Enums;

namespace CRClient.Responses
{
    public class DroppointResponse
    {
        public long DroppointId { get; internal set; }
        public Carrier Carrier { get; internal set; }
        public string Name { get; internal set; }
        public long Distance { get; internal set; }
        public Address Address { get; internal set; }
        public Coordinate Coordinate { get; internal set; }
        public OpeningHours OpeningHours { get; internal set; }

        public static DroppointResponse Map(dynamic o)
        {
            return new DroppointResponse
            {
                DroppointId = o.droppoint_id,
                Carrier = o.carrier,
                Name = o.name,
                Distance = o.distance,
                Address = new Address
                {
                    Street = o.address.street,
                    ZipCode = o.address.postal_code,
                    City = o.address.city,
                    CountryCode = o.address.country_code
                },
                Coordinate = new Coordinate
                {
                    Latitude = o.coordinate.latitude,
                    Longitude = o.coordinate.longitude
                },
                OpeningHours = new OpeningHours
                {
                    Days = new List<Day>()
                }
            };
        }
    }

    public class OpeningHours
    {
        public List<Day> Days { get; internal set; }
    }

    public class Day
    {
        public string Weekday { get; internal set; }
        public string From { get; internal set; }
        public string To { get; internal set; }
    }

    public class Coordinate
    {
        public string Latitude { get; internal set; }
        public string Longitude { get; internal set; }
    }

    public class Address
    {
        public string Street { get; internal set; }
        public string ZipCode { get; internal set; }
        public string City { get; internal set; }
        public string CountryCode { get; internal set; }
    }
}