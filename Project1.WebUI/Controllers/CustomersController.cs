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
    public class CustomersController : Controller
    {

        public IProject1Repository Repo { get; }
        public CustomersController(IProject1Repository repo)
        {
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public ActionResult Home()
        {
            return View();
        }
        // GET: Customers
        /// <summary>
        /// Index Action Method displays all the customers in the database based on the search 
        /// input given by the user.
        /// </summary>
        /// <param name="search">The Name provided by the User</param>
        /// <returns>View of customer models </returns>
        public ActionResult Index([FromQuery]string search = null)
        {
            IEnumerable<Customer> customers = Repo.GetCustomers(search);
            IEnumerable<CustomerViewModel> customerModels = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Orders = c.Orders.Select(x => new OrderViewModel())
            });
            return View(customerModels);
        }

        // GET: Customers/Details/5
        /// <summary>
        /// Details Action Method returns the details of a Customer based on their ID.
        /// It displays their first and last name.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View of Customer Model</returns>
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
        /// <summary>
        /// The Create Action Method returns the Create Method View.
        /// It lets the user create new customers with a firstname and last name.
        /// </summary>
        /// <returns>Create View</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        /// <summary>
        /// The Create Action Method binds the user input to create a customer model and saves it to the repo.
        /// </summary>
        /// <param name="customerModel">Customer Model User Input</param>
        /// <returns>View of customer model</returns>
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
        /// <summary>
        /// Edit Action Method allows the user to edit a customers details.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>View model of specified user</returns>
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
        /// <summary>
        /// Edit Action Method posts the changes made to the user to the repository.
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="customerModel">Customer Model</param>
        /// <returns>View of Customer Model</returns>
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
        /// <summary>
        /// The Delete Action Method returns the selected customer model to the view and prompts the user to delete it.
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer Model View</returns>
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
        /// <summary>
        /// The Delete Action Method deletes the selected customer and persists the changes to the database.
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="collection">User input form</param>
        /// <returns>Delete View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, [BindNever]IFormCollection collection)
        {
            try
            {
                Repo.DeleteCustomer(id);
                Repo.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        /// <summary>
        /// The search action method lets the user search the customer repository.
        /// The user can enter a first name, last name, or full name.
        /// </summary>
        /// <returns>Search View</returns>
        public ActionResult Search()
        {
            return View();
        }
        /// <summary>
        /// The SearchFound takes the user input and finds the coresponding customer in the repository.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public ActionResult SearchFound(string fullName)
        {
            IEnumerable<Customer> customers = Repo.GetCustomers(fullName);
            IEnumerable<CustomerViewModel> customerModels = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Orders = c.Orders.Select(x => new OrderViewModel())
            });
            return View(customerModels);
        }
    }
}
