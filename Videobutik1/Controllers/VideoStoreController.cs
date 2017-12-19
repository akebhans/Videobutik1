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
        CustomerContext dbCustomers = new CustomerContext();

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
            var customers = dbCustomers.Customers.ToList();

            return View(customers);
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

        public ActionResult CreateCustomer()
        {

            return View();

        }

        [HttpPost]
        public ActionResult CreateCustomer(CustomerModel customer)
        {
            try
            {
                dbCustomers.Customers.Add(customer);
                dbCustomers.SaveChanges();
                return RedirectToAction("ListCustomers");
            }
            catch
            {
                return View();
            }

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

        public ActionResult EditMovie(MovieModel movie)
        {
            return View(movie);
        }

        [HttpPost]
        public ActionResult EditMovie(string dummy, MovieModel movie)
        {
            try
            {
                MovieModel editMovie;
                using (var ctx = new MovieContext())
                {
                    editMovie = ctx.Movies.Where(m => m.MovieId == movie.MovieId).FirstOrDefault<MovieModel>();
                }

                if (editMovie != null)
                {
                    editMovie = movie;
                }

                //save modified entity using new Context
                using (var ctx = new MovieContext())
                {
                    // Mark entity as modified
                    ctx.Entry(editMovie).State = System.Data.Entity.EntityState.Modified;

                    // call SaveChanges
                    ctx.SaveChanges();
                }

                return RedirectToAction("ListMovies");
            }
            catch
            {
                return View();
            }

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


        public ActionResult EditCustomer(CustomerModel customer)
        {
            return View(customer);
        }

        [HttpPost]
        public ActionResult EditCustomer(string dummy,CustomerModel customer)
        {
            try
            {
                CustomerModel editCustomer;
                using (CustomerContext ctx = new CustomerContext())
                {
                    editCustomer = ctx.Customers.Where(i => i.CustomerId == customer.CustomerId).FirstOrDefault<CustomerModel>();
                }
                if (editCustomer != null)
                {
                    editCustomer = customer;
                }
                using (CustomerContext newctx = new CustomerContext())
                {
                    // Mark entity as modified
                    newctx.Entry(editCustomer).State = System.Data.Entity.EntityState.Modified;

                    // call SaveChanges
                    newctx.SaveChanges();
                    return RedirectToAction("ListCustomers");
                }
            }
            catch
            {
                return View(customer);
            }
        }

        public ActionResult DeleteCustomer(string dummy,CustomerModel customer)
        {
            return View(customer);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(CustomerModel customer)
        {
            try
            {
                // Need to instantiate a new context to make remove work
                using (var context = new CustomerContext())
                {
                    // Note: Attatch to the entity:
                    context.Customers.Attach(customer);
                    context.Customers.Remove(customer);
                    var nrOfObjectsChanged = context.SaveChanges();
                    return RedirectToAction("ListCustomers");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}