using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using driveSync.Models;



namespace ridesnShare.Controllers
{
    public class RideDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves a list of trips from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of TripDTO objects representing the list of trips.
        /// </returns>
        /// <example>
        /// GET: api/TripData/ListTrips
        /// </example>
        [HttpGet]
        [Route("api/RideData/ListRides/{id}")]
        public IEnumerable<Ride> Rides(int id)
        {
            List<Ride> Rides = db.Rides.Where(r=>r.DriverId==id).ToList();
            //List<Ride> RideDTOs = new List<Ride>();

            //Rides.ForEach(r => Rides.Add(new Ride()
            //{
            //    //RideId = r.RideId,
            //    DriverId = r.DriverId,
            //    StartLocation = r.startLocation,
            //    EndLocation = r.endLocation,
            //    Price = r.price,
            //    Time = r.Time,
            //    dayOftheweek = r.dayOftheweek
            //}));

            return Rides;

        }
        /// <summary>
        /// Adds a new ride to the database.
        /// </summary>
        /// <param name="ride">The ride object containing information about the new ride.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the addition operation.
        /// </returns>
        /// <example>
        /// POST: api/RideData/AddRide/5
        /// </example>
        [HttpGet]
        [Route("api/RideData/GetDriver/{id}")]

        public Driver GetDriver(int id)
        {
            return db.Drivers.FirstOrDefault(d=> d.DriverId == id);
        }    
        [ResponseType(typeof(Ride))]
        [HttpPost]
        public Ride AddRide(Ride ride)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
            }

            // Find the driver associated with the ride (assuming RideId is actually DriverId)
            Driver driver = db.Drivers.Find(ride.DriverId);

            if (driver == null)
            {
                Debug.WriteLine("Driver doesn't exist");
                //return BadRequest("Driver not found");
                return null;
            }

            // Create an instance of Inventory
            //Inventory inventory = new Inventory
            //{
            //    ItemName = ride.ItemName,
            //    Weight = ride.Weight,
            //    Size = ride.Size,
            //    Quantity = ride.Quantity
            //};

            //// Add the inventory to the database
            //db.Inventories.Add(inventory);
            //db.SaveChanges();

            // Create an instance of Ride
            //Ride newRide = new Ride
            //{
            //    StartLocation = ride.StartLocation,
            //    EndLocation = ride.EndLocation,
            //    Price = ride.Price,
            //    Time = ride.Time,
            //    DayOfTheWeek = ride.DayOfTheWeek,
            //};

            // Add the ride to the database
            db.Rides.Add(ride);
            db.SaveChanges();

            return ride;
        }

    }
}
