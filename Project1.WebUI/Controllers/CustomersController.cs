using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project1.Domain.Model;
using Project1.Domain.Interfaces;
using Project1.WebUI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Project1.WebUI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Project1Context _context;

        public IProject1Repository Repo { get; }
        public CustomersController(IProject1Repository repo)
        {
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            Customer customer = Repo.GetCustomerById(id);

            var customerModel = new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Orders = customer.Orders.Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderTime = o.OrderTime,
                    Quantity = o.Quantity,
                }),
            };
            if (customer == null)
            {
                return NotFound();
            }
            return View(customerModel);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("FirstName,LastName")] CustomerViewModel customerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customer = new Customer
                    {
                        FirstName = customerModel.FirstName,
                        LastName = customerModel.LastName,
                    };
                    Repo.AddCustomer(customer);
                    Repo.Save();

                    return RedirectToAction(nameof(Index));
                }
                return View(customerModel);
            }
            catch
            {
                return View(customerModel);
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            Customer customer = Repo.GetCustomerById(id);
            var viewModel = new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            };
            return View(viewModel);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("FirstName,LastName")] CustomerViewModel customerModel)
        {
            try
            {
                //Server side validation
                if (ModelState.IsValid)
                {
                    Customer customer = Repo.GetCustomerById(id);
                    customer.FirstName = customerModel.FirstName;
                    customer.LastName = customerModel.LastName;
                    Repo.UpdateCustomer(customer);
                    Repo.Save();

                    return RedirectToAction(nameof(Index));
                }
                return View(customerModel);
            }
            catch (Exception)
            {
                return View(customerModel);
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            Customer customer = Repo.GetCustomerById(id);
            var customerModel = new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Orders = customer.Orders.Select(x => new OrderViewModel())
            };
            return View(customerModel);
        }

        // POST: Restaurant/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, [BindNever]IFormCollection collection)
        {
            try
            {
                Repo.DeleteCustomer(id);
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
