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

namespace driveSync.Controllers
{
    public class DriverDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves a list of drivers from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of DriverDTO objects representing the list of drivers.
        /// </returns>
        /// <example>
        /// GET: api/DriverData/ListDrivers
        /// </example>
        [HttpGet]
        [Route("api/DriverData/ListDrivers")]
        public IEnumerable<DriverDTO> Drivers()
        {
            List<Driver> Drivers = db.Drivers.ToList();
            List<DriverDTO> DriverDTOs = new List<DriverDTO>();

            Drivers.ForEach(d => DriverDTOs.Add(new DriverDTO()
            {
                DriverId = d.DriverId,
                firstName = d.firstName,
                lastName = d.lastName,
                username = d.username,
                email = d.email
            }));

            return DriverDTOs;

        }
        /// <summary>
        /// Validates a passenger's credentials by checking if the user exists in the database and if the provided password matches.
        /// </summary>
        /// <param name="passenger">The Passenger object containing username and password for validation.</param>
        /// <returns>
        /// IHttpActionResult representing the result of the validation process:
        ///   - If the user exists and the password matches, returns Ok with the validated Passenger object.
        ///   - If the user exists but the password does not match, returns BadRequest with a message indicating incorrect password.
        ///   - If the user does not exist, returns BadRequest with a message indicating that the user was not found.
        /// </returns>
        /// <example></example>

        [HttpPost]
        [Route("api/DriverData/Validate")]
        public IHttpActionResult Validate(Driver driver)
        {
            Debug.WriteLine(driver.username);
            Debug.WriteLine(driver.password);

            // returns true or false if user exist or not

            bool isUserExist = (db.Drivers.Where(p => p.username == driver.username)
                                   .FirstOrDefault() == null) ? false : true;

            // Debug.WriteLine(isUserExist + "isUserExists");

            if (isUserExist)
            {
                // validate user
                Driver validatedDriver = db.Drivers.Where(d => d.username == d.username)
                                               .Where(d => d.password == d.password).FirstOrDefault();
                if (validatedDriver != null)
                {
                    return Ok(validatedDriver);

                }

                return BadRequest("Wrong password");

            }
            else
            {
                // return with a message
                return BadRequest("User not found");
            }
        }

        // <summary>
        /// Adds a new driver to the database.
        /// </summary>
        /// <param name="driver">The driver object containing information about the new driver.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the addition operation.
        /// </returns>
        /// <example>
        /// POST: api/DriverData/AddDriver
        /// </example>
        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult AddDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = driver.DriverId }, driver);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PassengerExists(int id)
        {
            return db.Passengers.Count(e => e.PassengerId == id) > 0;
        }
    }
}

