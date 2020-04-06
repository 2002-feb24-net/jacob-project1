using System;
using System.Collections.Generic;

namespace Project1.Domain.Model
{
    /// <summary>
    /// A Customer Object, having an Id, a FirstName and LastName,
    /// and a collection of orders.
    /// </summary>
    public partial class Customer
    {
        /// <summary>
        /// The default constructor must initialize the orders collection.
        /// </summary>
        public Customer()
        {
            Orders = new HashSet<Orders>();
        }

        /// <summary>
        /// The Customer's ID. Required and must not be 0.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Customer's first name must be required and not empty.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The Customer's last name must be required and not empty.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// The order collection may be empty or contain objects of Orders.
        /// </summary>

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
