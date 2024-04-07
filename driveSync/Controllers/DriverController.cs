using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using driveSync.Models;
using System.Web.Script.Serialization;
using System.Net.NetworkInformation;
using System.Data.Entity.Migrations.Model;

namespace driveSync.Controllers
{
    public class DriverController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DriverController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44332/api/DriverData/");
        }

        public ActionResult DriverLoginSubmit(Driver driver)
        {
            Debug.WriteLine(driver.username);
            Debug.WriteLine(driver.password);

            string url = "Validate";
            string jsonpayload = jss.Serialize(driver);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                //if (response.IsSuccessStatusCode)
                {
                    Driver resUser = response.Content.ReadAsAsync<Driver>().Result;

                    //Session.Clear();
                    //Session["userId"] = resUser;
                    //var action = $"PassengerProfile";
                    //return RedirectToAction(action, "Passenger");

                    return RedirectToAction("DriverProfile", "Driver", resUser);

                }
                //else
                //{
                //    Debug.WriteLine("Unsuccessful login attempt.");
                //    return RedirectToAction("Index", "Home"); // Redirect to home page if login fails
                //}
            }
            catch (Exception ex)
            {
                // Log the exception details
                Debug.WriteLine("An error occurred during login: " + ex.Message);
                // Redirect to an error page or handle the error appropriately
                return RedirectToAction("Error", "Driver");
            }
        }

        public ActionResult DriverProfile(Driver driver)
        {


            // Pass user object to the view
            return View(driver);
        }

        //GET: Driver/List
        public ActionResult List()
        {
            try
            {
                // Establish url connection endpoint
                string url = "ListDrivers";

                // Send request to API to retrieve list of drivers
                HttpResponseMessage response = client.GetAsync(url).Result;

                // Check if response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse JSON response into a list of DriverDTO objects
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    IEnumerable<DriverDTO> drivers = jss.Deserialize<IEnumerable<DriverDTO>>(responseData);

                    // Debug info
                    Debug.WriteLine("Number of rides received: " + drivers.Count());

                    // Return the view with the list of drivers
                    return View(drivers);
                }
                else
                {
                    Debug.WriteLine("API request failed with status code: " + response.StatusCode);
                    // Handle unsuccessful response (e.g., return an error view)
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
                // Handle any exceptions (e.g., return an error view)
                return View("Error");
            }
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: User/Create
        // POST: Driver/Create
        [HttpPost]

        public ActionResult AddDriver(Driver driver)
        {
            Debug.WriteLine("the inputted driver name is :");
            Debug.WriteLine(driver.firstName);
            //objective: add a new driver into our system using the API
            //curl -H "Content-Type:application/json" -d @trip.json  https://localhost:44354/api/DriverData/AddDriver

            string url = "AddDriver";

            //convert driver object into a json format to then send to our api
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(driver);

            Debug.WriteLine(jsonpayload);

            //send the json payload to the url through the use of our client
            //setup the postdata as HttpContent variable content
            HttpContent content = new StringContent(jsonpayload);

            //configure a header for our client to specify the content type of app for post 
            content.Headers.ContentType.MediaType = "application/json";

            //check if you can access information from our postasync request, get an httpresponse request and result of the request

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }

        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Passenger/Login
        [HttpGet]
        public ActionResult DriverLogin()
        {

            return View("DriverLogin");

        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
