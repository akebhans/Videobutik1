using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Videobutik1.Context;
using Videobutik1.Models;

namespace Videobutik1.Controllers
{
    public class VideoStoreController : Controller
    {
        DB_Context context = new DB_Context();

        //MovieContext dbMovies = new MovieContext();

        //CustomerContext dbCustomers = new CustomerContext();
        //RentalContext dbRentals = new RentalContext();

        // GET: VideoStore
        public ActionResult Index()
        {
            return RedirectToAction("ListMovies");
        }

        public ActionResult ListMovies()
        {
            var movies = context.Movies.ToList();
            ViewBag.Rented = true;
            return View(movies);

        }

        public ActionResult ListCustomers()
        {
            var customers = context.Customers.ToList();

            return View(customers);
        }

        public ActionResult ListRentals(string sortOrder)
        {
            ViewBag.MovieSortParm = String.IsNullOrEmpty(sortOrder) ? "Movie_desc" : "";
            ViewBag.CustomerSortParm = sortOrder == "Customer" ? "Customer_desc" : "Customer";
            List<RentalModel> rentalList;
            //movieList ) 
            rentalList = context.Rentals.ToList();

            List<RentalModelNames> rentalNamesList = new List<RentalModelNames>();

            foreach (var item in rentalList)
            {
                RentalModelNames listNamesItem = new RentalModelNames();
                listNamesItem.CustomerName = context.Customers.Find(item.CustomerId).Name;
                listNamesItem.MovieName = context.Movies.Find(item.MovieId).Name;
                foreach (PropertyInfo prop in item.GetType().GetProperties())
                {
                    foreach (PropertyInfo propNames in listNamesItem.GetType().GetProperties())
                    {
                        if (propNames.Name == prop.Name)
                        {
                            propNames.SetValue(listNamesItem,prop.GetValue(item));
                        }
                    } 
                }
                rentalNamesList.Add(listNamesItem);
            }

            //foreach (var rentalModel in rentalList.OrderByDescending(s => s.MovieId)) ;

            DB_Context ctx = new DB_Context();

            switch (sortOrder)
            {
                case "Movie_desc":
                    rentalNamesList.Sort((x, y) => x.MovieName.CompareTo(y.MovieName));
                    break;
                case "Customer":
                    rentalNamesList.Sort((x, y) => y.CustomerName.CompareTo(x.CustomerName));
                    break;
                case "Customer_desc":
                    rentalNamesList.Sort((x, y) => x.CustomerName.CompareTo(y.CustomerName));
                    break;
                default:
                    rentalNamesList.Sort((x, y) => y.MovieName.CompareTo(x.MovieName));
                    break;
            }

            return View(rentalNamesList);
        }

        //public ActionResult ListRentals()
        //{
        //    var rentalList = context.Rentals.ToList();

        //    return View(rentalList);
        //}

        public ActionResult ListActiveRentals()
        {
            var rentalList = context.Rentals.ToList();
            List<RentalModel> activeRentalList = new List<RentalModel>();

            foreach (var item in rentalList)
            {
                if (item.ActualReturnDate == null)
                {
                    activeRentalList.Add(item);
                }
            }

            return View(activeRentalList);
        }

        public ActionResult ListCustActiveRentals(int customerId)
        {
            var rentalList = context.Rentals.ToList();
            List<RentalModel> activeRentalList = new List<RentalModel>();

            foreach (var item in rentalList)
            {
                if (item.ActualReturnDate == null && item.CustomerId == customerId)
                {
                    activeRentalList.Add(item);
                }
            }

            return View(activeRentalList);
        }

        public ActionResult ListCustRentals(int customerId)
        {
            var rentalList = context.Rentals.ToList();
            List<RentalModel> myRentalList = new List<RentalModel>();

            foreach (var item in rentalList)
            {
                if (item.CustomerId == customerId)
                {
                    myRentalList.Add(item);
                }
            }

            return View(myRentalList);
        }

        public ActionResult ListMovieRentals(int movieId)
        {
            var rentalList = context.Rentals.ToList();
            List<RentalModel> myRentalList = new List<RentalModel>();

            foreach (var item in rentalList)
            {
                if (item.MovieId == movieId)
                {
                    myRentalList.Add(item);
                }
            }

            myRentalList.Sort((x, y) => y.RentalDate.CompareTo(x.RentalDate));
            return View(myRentalList);
        }

        [ChildActionOnly]
        public ActionResult RentalCustomerDetails(int customerid)
        {
            DB_Context custContext = new DB_Context();
            return PartialView("RentalCustomerDetails", custContext.Customers.Find(customerid));
        }

        [ChildActionOnly]
        public ActionResult RentalMovieDetails(int movieId)
        {
            DB_Context movieContext = new DB_Context();
            return PartialView("RentalMovieDetails", movieContext.Movies.Find(movieId));
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
                context.Customers.Add(customer);
                context.SaveChanges();
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
                context.Movies.Add(movie);
                context.SaveChanges();

                return RedirectToAction("ListMovies");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateRental()
        {
            List<MovieModel> myMovies = new List<MovieModel>();
            foreach (var item in context.Movies)
            {
                if (!item.Rented) myMovies.Add(item);
            }

            if (myMovies.Count == 0) myMovies.Add(new MovieModel() { MovieId = 0, Name = "NO MOVIES AVAILABLE!!" });
            ViewBag.ListCustomers = context.Customers;
            ViewBag.ListMovies = myMovies;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRental(RentalModel rental)
        {
            if (rental.CustomerId == 0 || rental.MovieId == 0)
            {
                ViewBag.UserMsg = "Both customer and movie must be declared!";
                return View();
            }
            else
            {
                try
                {
                    rental.RentalDate = DateTime.Today.ToShortDateString();
                    rental.LastReturnDate = DateTime.Today.AddDays(context.Movies.Find(rental.MovieId).RentalPeriodDays).ToShortDateString();
                    context.Rentals.Add(rental);
                    context.Movies.Find(rental.MovieId).Rented = true;
                    context.SaveChanges();

                    return RedirectToAction("ListRentals");
                }
                catch
                {
                    return View();
                }

            }
        }


        public ActionResult NewCustomerRental(RentalModel custRental)
        {
            if (TempData != null) TempData["CustomerName"] = context.Customers.Find(custRental.CustomerId).Name;
            List<MovieModel> myMovies = new List<MovieModel>();
            foreach (var item in context.Movies)
            {
                if (!item.Rented) myMovies.Add(item);
            }

            if (myMovies.Count == 0) myMovies.Add(new MovieModel() { MovieId = 0, Name = "NO MOVIES AVAILABLE!!" });
            ViewBag.ListItem = myMovies;
            return View();
        }

        [HttpPost]
        public ActionResult NewCustomerRental(RentalModel custRental, string dummy)
        {
            try
            {
                custRental.RentalDate = DateTime.Today.ToShortDateString();
                custRental.LastReturnDate = DateTime.Today.AddDays(context.Movies.Find(custRental.MovieId).RentalPeriodDays).ToShortDateString();
                context.Rentals.Add(custRental);
                context.Movies.Find(custRental.MovieId).Rented = true;
                context.SaveChanges();

                return RedirectToAction("ListRentals");
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
                using (var ctx = new DB_Context())
                {
                    editMovie = ctx.Movies.Where(m => m.MovieId == movie.MovieId).FirstOrDefault<MovieModel>();
                }

                if (editMovie != null)
                {
                    editMovie = movie;
                }

                //save modified entity using new Context
                using (var ctx = new DB_Context())
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
            bool hasActiveRental = false;

            foreach (var item in context.Rentals)
            {
                if (item.MovieId == movie.MovieId && item.ActualReturnDate == null)
                {
                    hasActiveRental = true;
                }
            }

            ViewBag.HasActiveRental = hasActiveRental;

            return View(movie);
        }
        [HttpPost]
        public ActionResult DeleteMovie(MovieModel movie)
        {
            try
            {
                // Need to instantiate a new context to make remove work
                using (var context = new DB_Context())
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
        public ActionResult EditCustomer(string dummy, CustomerModel customer)
        {
            try
            {
                CustomerModel editCustomer;
                using (DB_Context ctx = new DB_Context())
                {
                    editCustomer = ctx.Customers.Where(i => i.CustomerId == customer.CustomerId).FirstOrDefault<CustomerModel>();
                }
                if (editCustomer != null)
                {
                    editCustomer = customer;
                }
                using (DB_Context newctx = new DB_Context())
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

        public ActionResult DeleteCustomer(string dummy, CustomerModel customer)
        {
            bool hasActiveRental = false;

            foreach (var item in context.Rentals)
            {
                if (item.CustomerId == customer.CustomerId && item.ActualReturnDate == null)
                {
                    hasActiveRental = true;
                }
            }

            ViewBag.HasActiveRental = hasActiveRental;

            return View(customer);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(CustomerModel customer)
        {
            try
            {
                // Need to instantiate a new context to make remove work
                using (var context = new DB_Context())
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

        public ActionResult DeleteRental(RentalModel rental, string dummy)
        {
            rental.ActualReturnDate = DateTime.Today.ToShortDateString();
            MovieModel editMovie;
            using (DB_Context newctx = new DB_Context())
            {
                // Mark entity as modified
                newctx.Entry(rental).State = System.Data.Entity.EntityState.Modified;
                editMovie = newctx.Movies.Where(i => i.MovieId == rental.MovieId).FirstOrDefault<MovieModel>();
                editMovie.Rented = false;
                newctx.Entry(editMovie).State = System.Data.Entity.EntityState.Modified;
                // call SaveChanges
                newctx.SaveChanges();
                return RedirectToAction("ListActiveRentals");
            }
            //return View(rental);
        }

        [HttpPost]
        public ActionResult DeleteRental(RentalModel rental)
        {
            try
            {
                RentalModel editRental;
                MovieModel editMovie;
                using (DB_Context ctx = new DB_Context())
                {
                    editRental = ctx.Rentals.Where(i => i.RentalId == rental.RentalId).FirstOrDefault<RentalModel>();
                    editMovie = ctx.Movies.Where(i => i.MovieId == rental.MovieId).FirstOrDefault<MovieModel>();
                }
                if (editRental != null)
                {
                    editRental = rental;
                    editRental.ActualReturnDate = DateTime.Today.ToShortDateString();
                }

                if (editMovie != null)
                {
                    editMovie.Rented = false;
                }
                using (DB_Context newctx = new DB_Context())
                {
                    // Mark entity as modified
                    newctx.Entry(editRental).State = System.Data.Entity.EntityState.Modified;
                    newctx.Entry(editMovie).State = System.Data.Entity.EntityState.Modified;
                    // call SaveChanges
                    newctx.SaveChanges();
                    return RedirectToAction("ListRentals");
                }
            }
            catch
            {
                return View(rental);
            }

        }

        //[HttpPost]
        //public ActionResult DeleteRental(RentalModel rental)
        //{
        //    try
        //    {
        //        // Need to instantiate a new context to make remove work
        //        using (var context = new DB_Context())
        //        {
        //            // Note: Attatch to the entity:
        //            context.Rentals.Attach(rental);
        //            context.Rentals.Remove(rental);
        //            var nrOfObjectsChanged = context.SaveChanges();
        //            return RedirectToAction("ListRentals");
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}