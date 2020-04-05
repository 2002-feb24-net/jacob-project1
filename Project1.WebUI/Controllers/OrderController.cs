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
            return View();
        }
        public ActionResult CustomerSearch()
        {
            return View();
        }
        // GET: Order
        public ActionResult IndexStore([FromQuery]string search = null)
        {
            var storeLocations = Repo.GetLocations(search).First();
            IEnumerable<Orders> orders = Repo.GetOrders().Where(o=>o.StoreLocationId == storeLocations.Id);
            IEnumerable<OrderViewModel> orderModels = orders.Select(c => new OrderViewModel
            {
                Id = c.Id,
                ProductId = c.ProductId,
                StoreLocationId = c.StoreLocationId,
                CustomerId = c.CustomerId,
                OrderTime = c.OrderTime,
                Quantity = c.Quantity
            });
            return View(orderModels);
        }

        public ActionResult IndexCustomer([FromQuery]string search = null)
        {
            var customer = Repo.GetCustomers(search).First();
            IEnumerable<Orders> orders = Repo.GetOrders().Where(o => o.CustomerId == customer.Id);
            IEnumerable<OrderViewModel> orderModels = orders.Select(c => new OrderViewModel
            {
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

                return RedirectToAction(nameof(OrderController.Details),
                    "Order", new { id = orderModel.Id });
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
