using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Project1.Domain.Model;
using Project1.Domain.Interfaces;
using Project1.WebUI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project1.WebUI.Controllers
{
    public class OrderController : Controller
    {
        public IProject1Repository Repo { get; }

        public OrderController(IProject1Repository repo)
        {
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        /// <summary>
        /// Login Action Method that returns the login page.
        /// Lets the user login based on their full name and saves their user id into their cookies.
        /// </summary>
        /// <returns>view</returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// LoginConfirmed action method takes the input from the user and checks if there are any users with the same matching name.
        /// If it does, then the method saves the user id in the user's cookies and redirects to the create order method.
        /// </summary>
        /// <param name="name">The Full name input from the user</param>
        /// <returns>Redirects to Create or redirects to login on error</returns>
        public ActionResult LoginConfirmed(string name)
        {
            var customers = Repo.GetCustomers();
            if(customers.Any(c=>c.FirstName.Trim(' ')+" "+c.LastName.Trim(' ') == name))
            {
                ViewData["Error"] = "";
                string id = customers.First(c => c.FirstName + " " + c.LastName == name).Id.ToString();
                HttpContext.Response.Cookies.Append("user_id", id);
                return RedirectToAction("Create");
            }
            else
            {
                ViewData["Error"] = "Invalid Name. Please Try Again.";
                return RedirectToAction("Login");
            }
        }
        /// <summary>
        /// Store Search Action Method lets the user search the Database for a store location
        /// and display its respective order history
        /// </summary>
        /// <returns>View of locationModels</returns>
        public ActionResult StoreSearch()
        {
            IEnumerable<StoreLocation> locations = Repo.GetLocations();
            IEnumerable<StoreViewModel> locationModels = locations.Select(c => new StoreViewModel
            {
                Id = c.Id,
                LocationName = c.LocationName,
                Orders = c.Orders.Select(x => new OrderViewModel()),
                Product = c.Product.Select(x => new ProductViewModel())
            });
            return View(locationModels);
        }
        /// <summary>
        /// Customer Search Action Method lets the user search the Database for a customer
        /// and displays the customers order history.
        /// </summary>
        /// <returns>View of customer models</returns>
        public ActionResult CustomerSearch()
        {
            IEnumerable<Customer> customers = Repo.GetCustomers();
            IEnumerable<CustomerViewModel> customerModels = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Orders = c.Orders.Select(x => new OrderViewModel())
            });
            return View(customerModels);
        }
        // GET: Order
        /// <summary>
        /// IndexStore gets the order history of the selected store.
        /// </summary>
        /// <param name="search">user input of the store id</param>
        /// <returns>View of order models</returns>
        public ActionResult IndexStore(int search)
        {
            IEnumerable<Orders> order2 = Repo.GetOrders();
            IEnumerable<Orders> orders = order2.Where(o => o.StoreLocationId == search && o.CheckOut);
            IEnumerable<OrderViewModel> orderModels = orders.Select(c => new OrderViewModel
            {
                CustomerName = c.Customer.FirstName + " " + c.Customer.LastName,
                StoreName = c.StoreLocation.LocationName,
                ProductName = c.Product.Name,
                Id = c.Id,
                ProductId = c.ProductId,
                StoreLocationId = c.StoreLocationId,
                CustomerId = c.CustomerId,
                OrderTime = c.OrderTime,
                Quantity = c.Quantity
            });
            return View(orderModels);
        }
        // GET: Order
        /// <summary>
        /// IndexCustomer gets the order history of the selected store.
        /// </summary>
        /// <param name="search">User input of the customers id</param>
        /// <returns>View of order models</returns>
        public ActionResult IndexCustomer(int search)
        {
            IEnumerable<Orders> order2 = Repo.GetOrders();
            IEnumerable<Orders> orders = order2.Where(o => o.CustomerId == search && o.CheckOut);
            IEnumerable<OrderViewModel> orderModels = orders.Select(c => new OrderViewModel
            {
                CustomerName = c.Customer.FirstName + " " + c.Customer.LastName,
                StoreName = c.StoreLocation.LocationName,
                ProductName = c.Product.Name,
                Id = c.Id,
                ProductId = c.ProductId,
                StoreLocationId = c.StoreLocationId,
                CustomerId = c.CustomerId,
                OrderTime = c.OrderTime,
                Quantity = c.Quantity
            });
            return View(orderModels);
        }
        /// <summary>
        /// Default Action Method for the Order Controller.
        /// Sends the User to the home page for Orders
        /// </summary>
        /// <returns>Home view</returns>
        public ActionResult Home()
        {
            return View();
        }

        // GET: Order/Details/5
        /// <summary>
        /// Details Action Method displays the details of an order and its respective attributes based on the order id.
        /// </summary>
        /// <param name="id">The Order ID</param>
        /// <returns>View of an order model</returns>
        public ActionResult Details(int id)
        {
            Orders orders = Repo.GetOrderById(id);

            var orderModel = new OrderViewModel
            {
                Id = orders.Id,
                ProductId = orders.ProductId,
                StoreLocationId = orders.StoreLocationId,
                CustomerId = orders.CustomerId,
                OrderTime = orders.OrderTime,
                Quantity = orders.Quantity
            };
            if (orders.Id == 0)
            {
                return NotFound();
            }
            return View(orderModel);
        }

        // GET: Order/Create
        /// <summary>
        /// The Create Action Method that lets the user create new Order entities.
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Create()
        {
            ViewData["error"] = HttpContext.Request.Cookies["error"];
            if (HttpContext.Request.Cookies["user_id"] == null)
            {
                return Redirect("LogIn");
            }
            ViewData["CustomerId"] = HttpContext.Request.Cookies["user_id"];
            ViewData["ProductId"] = new SelectList(Repo.GetProducts(), "Id", "Name", "Stock");
            ViewData["StoreLocationId"] = new SelectList(Repo.GetLocations(), "Id", "LocationName");
            ViewData["DateTime"] = Repo.GetOrders().Last().OrderTime;
            return View();
        }

        // POST: Order/Create
        /// <summary>
        /// The Create Action Method Overloads with Binds for a OrderViewModel that is added to the database.
        /// The order has a false checkout bool until the user checksout with the CartController.
        /// </summary>
        /// <param name="orderModel">User Input for creating an order</param>
        /// <returns>Returns to the order home page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,StoreLocationId,CustomerId,ProductId,OrderTime,Quantity")] OrderViewModel orderModel)
        {
            try
            {
                var test = Repo.GetProductById(orderModel.ProductId);
                if (!ModelState.IsValid)
                {
                    return View(orderModel);
                }
                if(test.StoreLocationId != orderModel.StoreLocationId)
                {
                    string str = test.Name + " is not available at the selected store.";
                    HttpContext.Response.Cookies.Append("error", str);
                    return Redirect("Create");
                }
                else if(orderModel.Quantity > test.Stock)
                {
                    string str = "Quantity entered is too big. There are only " +test.Stock +" "+ test.Name + " left in stock.";
                    HttpContext.Response.Cookies.Append("error", str);
                    return Redirect("Create");
                }
                var orders = new Orders
                {
                    ProductId = orderModel.ProductId,
                    StoreLocationId = orderModel.StoreLocationId,
                    CustomerId = Int32.Parse(HttpContext.Request.Cookies["user_id"]),
                    OrderTime = orderModel.OrderTime,
                    Quantity = orderModel.Quantity,
                    CheckOut = false
                };
                Repo.AddOrder(orders);
                Repo.Save();
                HttpContext.Response.Cookies.Append("error", "");
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                return View(orderModel);
            }
        }

        // GET: Order/Edit/5
        /// <summary>
        /// The Edit ActionMethod edits the order based on user input.
        /// </summary>
        /// <param name="id">The Order ID</param>
        /// <returns>View of order model</returns>
        public ActionResult Edit(int id)
        {
            Orders orders = Repo.GetOrderById(id);
            var orderModel = new OrderViewModel
            {
                ProductId = orders.ProductId,
                StoreLocationId = orders.StoreLocationId,
                CustomerId = orders.CustomerId,
                OrderTime = orders.OrderTime,
                Quantity = orders.Quantity
            };
            ViewData["CustomerId"] = new SelectList(Repo.GetCustomers(), "Id", "FirstName");
            ViewData["ProductId"] = new SelectList(Repo.GetProducts(), "Id", "Name");
            ViewData["StoreLocationId"] = new SelectList(Repo.GetLocations(), "Id", "LocationName");
            return View(orderModel);
        }

        // POST: Order/Edit/5
        /// <summary>
        /// The Edit Action Method Overload is binding the changes made by the user and saving the changes
        /// to the repository.
        /// </summary>
        /// <param name="id">The Order ID</param>
        /// <param name="orderModel">The User Input Order Changes</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,StoreLocationId,CustomerId,ProductId,OrderTime,Quantity")] OrderViewModel orderModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Orders orders = Repo.GetOrderById(id);
                    orders.ProductId = orderModel.ProductId;
                    orders.StoreLocationId = orderModel.StoreLocationId;
                    orders.CustomerId = orderModel.CustomerId;
                    orders.OrderTime = orderModel.OrderTime;
                    orders.Quantity = orderModel.Quantity;
                    Repo.AddOrder(orders);
                    Repo.Save();

                    return RedirectToAction(nameof(Index));
                }
                ViewData["ProductId"] = new SelectList(Repo.GetProducts(), "Id", "Name");
                ViewData["StoreLocationId"] = new SelectList(Repo.GetLocations(), "Id", "LocationName");
                return View(orderModel);
            }
            catch (Exception)
            {
                return View(orderModel);
            }
        }

        // GET: Order/Delete/5
        /// <summary>
        /// The Delete Action Method prompts the user to delete the selected action method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            Orders orders = Repo.GetOrderById(id);
            var orderModel = new OrderViewModel
            {
                Id = orders.Id,
                ProductId = orders.ProductId,
                StoreLocationId = orders.StoreLocationId,
                CustomerId = orders.CustomerId,
                OrderTime = orders.OrderTime,
                Quantity = orders.Quantity
            };
            return View(orderModel);
        }

        // POST: Order/Delete/5
        /// <summary>
        /// The DeleteConfirmed Method tells the user that the deletion is complete and deletes specified order.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>View of Confirmation</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Repo.DeleteOrder(id);
                Repo.Save();

                return RedirectToAction("Home");
            }
            catch
            {
                return View();
            }
        }
    }
}
