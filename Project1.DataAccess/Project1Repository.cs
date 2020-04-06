using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NLog;
using Project1.Domain.Interfaces;
using Project1.Domain.Model;
namespace Project1.DataAccess
{
    public class Project1Repository : IProject1Repository, IDisposable
    {
        private readonly Project1Context _dbContext;

        private static readonly ILogger s_logger = LogManager.GetCurrentClassLogger();

        //initializes a new Project0 Repository given an available Database
        public Project1Repository(Project1Context dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        //Returns collection of Locations
        public IEnumerable<StoreLocation> GetLocations(string search = null)
        {
            IQueryable<StoreLocation> items = _dbContext.StoreLocation
                .Include(r => r.Product).Include(o => o.Orders).AsNoTracking();
            if (search != null)
            {
                items = items.Include(o => o.Orders).Include(p => p.Product).Where(r => r.LocationName.Contains(search));
            }
            return items.Select(Mapper.MapStoreLocationWithOrdersAndProduct);
        }

        //get location by id
        public StoreLocation GetLocationsById(int id)
        {
            return Mapper.MapStoreLocationWithOrdersAndProduct(_dbContext.StoreLocation.Include(o => o.Orders).Include(p => p.Product).First(l => l.Id == id));
        }

        //Add a location
        public void AddLocation(StoreLocation storeLocation)
        {
            if (storeLocation.Id != 0)
            {
                //Identity insert will not allow us to change the Id
                s_logger.Warn($"Location to be added has an ID ({storeLocation.Id}) already: ignoring.");
            }
            s_logger.Info($"Adding location");
            StoreLocation entity = Mapper.MapStoreLocationWithOrdersAndProduct(storeLocation);
            entity.Id = 0;
            _dbContext.Add(entity);
        }

        //Delete a Location by Id
        public void DeleteLocation(int locationId)
        {
            s_logger.Info($"Deleting location with ID {locationId}");
            StoreLocation entity = _dbContext.StoreLocation.Find(locationId);
            _dbContext.Remove(entity);
        }

        //Update a Location
        public void UpdateLocation(StoreLocation storeLocation)
        {
            s_logger.Info($"Updating Location with ID {storeLocation.Id}");
            StoreLocation currentEntity = _dbContext.StoreLocation.Include(o => o.Orders).Include(p => p.Product).First(r => r.Id == storeLocation.Id);
            StoreLocation newEntity = Mapper.MapStoreLocationWithOrdersAndProduct(storeLocation);
            //This marks only the changed properties as modified

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        //Returns collection of Customers
        public IEnumerable<Customer> GetCustomers(string search = null)
        {
            IQueryable<Customer> items = _dbContext.Customer
                .Include(r => r.Orders).AsNoTracking();
            if (search != null)
            {
                items = items.Include(o => o.Orders).Where(r => r.FirstName == search || r.LastName == search || r.FirstName+" "+r.LastName == search);
            }
            return items.Select(Mapper.MapCustomerWithOrders);
        }

        //get Customer by id
        public Customer GetCustomerById(int id)
        {
            return Mapper.MapCustomerWithOrders(_dbContext.Customer.Include(o => o.Orders).First(c => c.Id == id));
        }

        //Add a Customer
        public void AddCustomer(Customer customer)
        {
            if (customer.Id != 0)
            {
                //Identity insert will not allow us to change the Id
                s_logger.Warn($"Customer to be added has an ID ({customer.Id}) already: ignoring.");
            }
            s_logger.Info($"Adding customer");
            Customer entity = Mapper.MapCustomerWithOrders(customer);
            entity.Id = 0;
            _dbContext.Add(entity);
        }

        //Delete a Customer by Id
        public void DeleteCustomer(int customerId)
        {
            s_logger.Info($"Deleting Customer with ID {customerId}");
            Customer entity = _dbContext.Customer.Find(customerId);
            _dbContext.Remove(entity);
        }

        //Update a Customer
        public void UpdateCustomer(Customer customer)
        {
            s_logger.Info($"Updating Customer with ID {customer.Id}");
            Customer currentEntity = _dbContext.Customer.Include(r => r.Orders).First(c => c.Id == (customer.Id));
            Customer newEntity = Mapper.MapCustomerWithOrders(customer);
            //This marks only the changed properties as modified

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }
        //Returns collection of Orders
        public IEnumerable<Orders> GetOrders(string search = null)
        {
            IQueryable<Orders> items = _dbContext.Orders.Include(o=>o.StoreLocation).Include(o => o.Customer).Include(o => o.Product);
            if (search != null)
            {
                items = items.Where(r => r.Id == Int32.Parse(search));
            }
            return items.Select(Mapper.Map);
        }

        //get Order by id
        public Orders GetOrderById(int id)
        {
            return Mapper.Map(_dbContext.Orders.Include(o => o.StoreLocation).Include(o => o.Customer).Include(o => o.Product).First(o=>o.Id == id));
        }

        //Add a Order
        public void AddOrder(Orders order)
        {
            if (order.Id != 0)
            {
                //Identity insert will not allow us to change the Id
                s_logger.Warn($"Order to be added has an ID ({order.Id}) already: ignoring.");
            }
            s_logger.Info($"Adding Order");
            Orders entity = Mapper.Map(order);
            entity.Id = 0;
            _dbContext.Add(entity);
        }

        //Delete a Order by Id
        public void DeleteOrder(int orderId)
        {
            s_logger.Info($"Deleting Order with ID {orderId}");
            Orders entity = _dbContext.Orders.First(c=>c.Id==orderId);
            _dbContext.Remove(entity);
        }

        //Update a Order
        public void UpdateOrder(Orders order)
        {
            s_logger.Info($"Updating Order with ID {order.Id}");
            Orders currentEntity = _dbContext.Orders.Include(o => o.StoreLocation).Include(o => o.Customer).Include(o => o.Product).First(c => c.Id == order.Id);
            Orders newEntity = Mapper.Map(order);
            //This marks only the changed properties as modified

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }
        //Returns collection of Products
        public IEnumerable<Product> GetProducts(string search = null)
        {
            IQueryable<Product> items = _dbContext.Product;
            if (search != null)
            {
                items = items.Include(o => o.Orders).Where(r => r.Id == Int32.Parse(search));
            }
            return items.Select(Mapper.Map);
        }

        //get Product by id
        public Product GetProductById(int id)
        {
            return Mapper.Map(_dbContext.Product.Include(o => o.Orders).First(p => p.Id == id));
        }

        //Add a Product
        public void AddProduct(Product product)
        {
            if (product.Id != 0)
            {
                //Identity insert will not allow us to change the Id
                s_logger.Warn($"Product to be added has an ID ({product.Id}) already: ignoring.");
            }
            s_logger.Info($"Adding Order");
            Product entity = Mapper.Map(product);
            entity.Id = 0;
            _dbContext.Add(entity);
        }

        //Delete a Product by Id
        public void DeleteProduct(int productId)
        {
            s_logger.Info($"Deleting Order with ID {productId}");
            Product entity = _dbContext.Product.Find(productId);
            _dbContext.Remove(entity);
        }

        //Update a Product
        public void UpdateProduct(Product product)
        {
            s_logger.Info($"Updating Product with ID {product.Id}");
            Product currentEntity = _dbContext.Product.Include(o => o.Orders).First(p=>p.Id == product.Id);
            Product newEntity = Mapper.Map(product);
            //This marks only the changed properties as modified

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }
        //Persisting changes to database
        public void Save()
        {
            s_logger.Info("Saving changes to the database");
            _dbContext.SaveChanges();
        }

        //Provided by the Restaurant review example
        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
