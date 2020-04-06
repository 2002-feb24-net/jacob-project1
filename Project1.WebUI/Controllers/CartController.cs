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

        public ActionResult Home()
        {
            return View();
        }
        // Checkout
        public ActionResult Index()
        {
            if(HttpContext.Request.Cookies["user_id"] == null)
            {
                return Redirect("Cart/NoLogin");
            }
            var id = Int32.Parse(HttpContext.Request.Cookies["user_id"]);
            IEnumerable<Orders> orders = Repo.GetOrders().Where(o => o.CheckOut == false && o.CustomerId == id);
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
        public ActionResult CheckedOut()
        {
            var orders = Repo.GetOrders().Where(o => o.CheckOut == false);
            foreach (var item in orders)
            {
                item.CheckOut = true;
                Repo.UpdateOrder(item);
            }
            Repo.Save();
            return View();
        }

        public ActionResult NoLogIn()
        {
            return View();
        }
    }
}
