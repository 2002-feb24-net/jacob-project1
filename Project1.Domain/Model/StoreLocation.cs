using System;
using System.Collections.Generic;

namespace Project1.Domain.Model
{
    /// <summary>
    /// A Store Location Object, has a Location Name, a collection of orders,
    /// and a collection of products.
    /// </summary>
    public partial class StoreLocation
    {
        public StoreLocation()
        {
            Orders = new HashSet<Orders>();
            Product = new HashSet<Product>();
        }
        /// <summary>
        /// A Location has a unique Id that must not be 0.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// A store location has a location name that must not be empty.
        /// </summary>
        public string LocationName { get; set; }
        /// <summary>
        /// The store has a collection of Orders that may be empty.
        /// </summary>

        public virtual ICollection<Orders> Orders { get; set; }
        /// <summary>
        /// The store has a collection of Products that may be empty.
        /// </summary>
        public virtual ICollection<Product> Product { get; set; }
    }
}
