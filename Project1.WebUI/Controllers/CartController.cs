using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Project1.Domain.Model;
using Project1.Domain.Interfaces;
using Project1.WebUI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Project1.WebUI.Controllers
{
    public class CartController : Controller
    {

        public IProject1Repository Repo { get; }
        public CartController(IProject1Repository repo)
        {
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        /// <summary>
        /// The Home Action for Cart Controller
        /// </summary>
        /// <returns>Home view</returns>
        public ActionResult Home()
        {
            return View();
        }
        /// <summary>
        /// The CheckOut Page that lists the orders based on the customers id and the checkout bool
        /// </summary>
        /// <returns>orderModels</returns>
        public ActionResult Index()
        {
            if(HttpContext.Request.Cookies["user_id"] == null)
            {
                return Redirect("NoLogin");
            }
            var id = Int32.Parse(HttpContext.Request.Cookies["user_id"]);
            IEnumerable<Orders> orders = Repo.GetOrders().Where(o => !o.CheckOut && o.CustomerId == id);
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
        /// Takes the Orders and updates them to the repo.
        /// The method also changes the stock of the products based on the quantity of the order.
        /// </summary>
        /// <returns>CheckedOut View</returns>
        public ActionResult CheckedOut()
        {
            var id = Int32.Parse(HttpContext.Request.Cookies["user_id"]);
            Orders orders;
            Product product;
            try
            {
                while ((orders = Repo.GetOrders().First(o => !o.CheckOut && o.CustomerId == id)) != null)
                {
                    product = Repo.GetProductById(orders.ProductId);
                    product.Stock = product.Stock - orders.Quantity;
                    Repo.UpdateProduct(product);
                    Repo.Save();
                    orders.CheckOut = true;
                    Repo.UpdateOrder(orders);
                    Repo.Save();
                }
            }
            catch
            {
                return View();
            }
            return View();
        }
        /// <summary>
        /// In case of the Customer not being logged in, The view will switch to a default please log in message.
        /// </summary>
        /// <returns>NoLogin view</returns>
        public ActionResult NoLogIn()
        {
            return View();
        }
    }
}
