﻿
using System.Linq;
using Project1.Domain.Model;
namespace Project1.DataAccess
{
    public static class Mapper
    {
        //get the location entity
        public static StoreLocation MapStoreLocationWithOrdersAndProduct(StoreLocation storeLocation)
        {
            return new StoreLocation
            {
                Id = storeLocation.Id,
                LocationName = storeLocation.LocationName,
                Orders = storeLocation.Orders.Select(Map).ToList(),
                Product = storeLocation.Product.Select(Map).ToList()
            };
        }
        //get the customer entity
        public static Customer MapCustomerWithOrders(Customer customer)
        {
            return new Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Orders = customer.Orders.Select(Map).ToList()
            };
        }
        //get the orders entity
        public static Orders Map(Orders order)
        {
            return new Orders
            {
                Id = order.Id,
                StoreLocationId = order.StoreLocationId,
                CustomerId = order.CustomerId,
                ProductId = order.ProductId,
                OrderTime = order.OrderTime,
                Quantity = order.Quantity, 
                CheckOut = order.CheckOut,
                StoreLocation = order.StoreLocation,
                Customer = order.Customer,
                Product = order.Product
            };
        }
        //get the product entity
        public static Product Map(Product product)
        {
            return new Product
            {
                Id = product.Id,
                StoreLocationId = product.StoreLocationId,
                StoreLocation = product.StoreLocation,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Orders = product.Orders.Select(Map).ToList()
            };
        }
    }
}
