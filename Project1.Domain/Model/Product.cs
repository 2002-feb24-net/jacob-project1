using System;
using System.Collections.Generic;

namespace Project1.Domain.Model
{
    public partial class Product
    {
        /// <summary>
        /// A Product object, having a StoreLocation, Collection of Orders,
        /// a name, a price and stock.
        /// </summary>
        public Product()
        {
            Orders = new HashSet<Orders>();
        }
        /// <summary>
        /// The Product has an ID that is unique and must not be 0.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Product has a Store Location ID that is a foreign key that matches to its respective StoreLocation.
        /// </summary>
        public int StoreLocationId { get; set; }
        /// <summary>
        /// The Product has a name that must not be empty.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Product has a price that should be a decimal and not empty.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The Product has a stock that represents the amount of products left in stock and must not be empty.
        /// </summary>
        public int Stock { get; set; }

        public virtual StoreLocation StoreLocation { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
