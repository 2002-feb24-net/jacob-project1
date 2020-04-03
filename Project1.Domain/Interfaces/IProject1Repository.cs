using Project1.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain.Interfaces
{
    public interface IProject1Repository : IDisposable
    {
        //Get all locations
        IEnumerable<StoreLocation> GetLocations(string search = null);

        //Get a location by id
        StoreLocation GetLocationsById(int id);

        //Add a new store location
        void AddLocation(StoreLocation storeLocation);
        
        //Delete a location by its id
        void DeleteLocation(int locationId);
        //Update location with new name, order, or products
        void UpdateLocation(StoreLocation storeLocation);

        //Get all customers
        IEnumerable<Customer> GetCustomers(string search = null);

        //Get a customer by id
        Customer GetCustomerById(int id);

        //Add a new customer
        void AddCustomer(Customer customer);

        //Delete a customer by its id
        void DeleteCustomer(int customerId);
        //update customer with new name or order
        void UpdateCustomer(Customer customer);

        //Get all orders
        IEnumerable<Orders> GetOrders(string search = null);

        //Get an order by id
        Orders GetOrderById(int id);

        //Add a new order
        void AddOrder(Orders order);

        //Delete a order by its id
        void DeleteOrder(int orderId);
        //update order with new time or quantity
        void UpdateOrder(Orders order);

        //Get all Products
        IEnumerable<Product> GetProducts(string search = null);

        //Get a Product by id
        Product GetProductById(int id);

        //Add a new Product
        void AddProduct(Product product);

        //Delete a Product by its id
        void DeleteProduct(int productId);
        //update Product with new stock, name, or price
        void UpdateProduct(Product product);
        //Persist changes to data base
        void Save();
    }
}
