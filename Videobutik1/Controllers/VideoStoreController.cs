using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videobutik1.Context;
using Videobutik1.Models;

namespace Videobutik1.Controllers
{
    public class VideoStoreController : Controller
    {
        MovieContext dbMovies = new MovieContext();

        // GET: VideoStore
        public ActionResult Index()
        {
            return RedirectToAction("ListMovies");
        }

        public ActionResult ListMovies()
        {
            var movies = dbMovies.Movies.ToList();
            return View(movies);

        }

        public ActionResult ListCustomers()
        {
            List<CustomerModel> customerList = new List<CustomerModel>();
            customerList.Add(new CustomerModel { CustomerId = 1, Name = "Stina Jönsdotter", Address = "Storgatan 1, 12345 Småstad", PhoneNo = "070-1234567" });

            customerList.Add(new CustomerModel { CustomerId = 2, Name = "Jöns Stinasson", Address = "Storgatan 1, 12345 Småstad", PhoneNo = "070-1234567" });

            return View(customerList);
        }

        public ActionResult ListRentals()
        {
            List<RentalModel> rentalList = new List<RentalModel>();
            rentalList.Add(new RentalModel
            {
                CustomerId = 1,
                CustomerName = "Stina Jönsdotter",
                LastReturnDate = new DateTime(2017, 12, 31),
                MovieId = 2,
                MovieTitle = "Ghostducks",
                RentalDate = new DateTime(2017, 12, 1),
                RentalId = 1
            });
            return View(rentalList);
        }

        public ActionResult CreateCustomer(int customerId)
        {
            return View(new CustomerModel { CustomerId = customerId });
        }

        public ActionResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMovie(MovieModel movie)
        {
            try
            {
                dbMovies.Movies.Add(movie);
                dbMovies.SaveChanges();

                return RedirectToAction("ListMovies");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditMovie(int movieId, string name, string director, string genre, int lengthMinutes, int year)
        {
            return View(new MovieModel { MovieId = movieId, Name = name, Director = director, Genre = genre, LengthMinutes = lengthMinutes, Year = year });
        }

        // Adds a dummy parameter to invoke overloading
        public ActionResult DeleteMovie(string dummy, MovieModel movie)
        {
            return View(movie);
        }
        [HttpPost]
        public ActionResult DeleteMovie(MovieModel movie)
        {
            try
            {
// Need to instantiate a new context to make remove work
                using (var context = new MovieContext())
                {
                    // Note: Attatch to the entity:
                    context.Movies.Attach(movie);
                    context.Movies.Remove(movie);
                    var nrOfObjectsChanged = context.SaveChanges();
                    return RedirectToAction("ListMovies");
                }
            }
            catch
            {
                return View();
            }
        }


        public ActionResult EditCustomer(int customerId, string name, string address, string phoneNo)
        {
            return View(new CustomerModel { CustomerId = customerId, Name = name, Address = address, PhoneNo = phoneNo });
        }
    }
}