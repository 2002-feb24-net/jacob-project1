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
        public ActionResult IndexStore(int search)
        {
            IEnumerable<Orders> order2 = Repo.GetOrders();
            IEnumerable<Orders> orders = order2.Where(o => o.StoreLocationId == search);
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

        public ActionResult IndexCustomer(int search)
        {
            //var customer = Repo.GetCustomers(search).First();
            IEnumerable<Orders> order2 = Repo.GetOrders();
            IEnumerable<Orders> orders = order2.Where(o => o.CustomerId == search);
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

        public ActionResult Home()
        {
            return View();
        }

        // GET: Order/Details/5
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
            if (orders == null)
            {
                return NotFound();
            }
            return View(orderModel);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(Repo.GetCustomers(), "Id", "FirstName");
            ViewData["ProductId"] = new SelectList(Repo.GetProducts(), "Id", "Name");
            ViewData["StoreLocationId"] = new SelectList(Repo.GetLocations(), "Id", "LocationName");
            ViewData["DateTime"] = Repo.GetOrders().Last().OrderTime;
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,StoreLocationId,CustomerId,ProductId,OrderTime,Quantity")] OrderViewModel orderModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(orderModel);
                }
                var orders = new Orders
                {
                    ProductId = orderModel.ProductId,
                    StoreLocationId = orderModel.StoreLocationId,
                    CustomerId = orderModel.CustomerId,
                    OrderTime = orderModel.OrderTime,
                    Quantity = orderModel.Quantity
                };
                Repo.AddOrder(orders);
                Repo.Save();

                return RedirectToAction(nameof(Home));
            }
            catch
            {
                return View(orderModel);
            }
        }

        // GET: Order/Edit/5
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    orders.StoreLocationId = orders.StoreLocationId;
                    orders.CustomerId = orderModel.CustomerId;
                    orders.OrderTime = orderModel.OrderTime;
                    orders.Quantity = orderModel.Quantity;
                    Repo.AddOrder(orders);
                    Repo.Save();

                    return RedirectToAction(nameof(Index));
                }
                ViewData["CustomerId"] = new SelectList(Repo.GetCustomers(), "Id", "FirstName");
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Repo.DeleteOrder(id);
                Repo.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
