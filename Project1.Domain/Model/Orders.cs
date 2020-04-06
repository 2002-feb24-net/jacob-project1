using System;
using System.Collections.Generic;

namespace Project1.Domain.Model
{
    /// <summary>
    /// An Order object, having a StoreLocation, Customer, Product
    /// OrderTime, Quantity, and CheckOut.
    /// </summary>
    public partial class Orders
    {
        /// <summary>
        /// The Order's Id is unique and must not be 0.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The StoreLocationId is the foreign key that connects to it's respective StoreLocation object.
        /// </summary>
        public int StoreLocationId { get; set; }
        /// <summary>
        /// The CustomerId is the foreign key that connects to it's respective Customer object.
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// The ProductId is the foreign key that connects to it's respective Product object.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// The OrderTime is a string that keeps track of the time when the order is made. The Database takes care of the OrderTime.
        /// </summary>
        public string OrderTime { get; set; }
        /// <summary>
        /// The Quantity is the amount of products the User orders. Quantity must not be 0.
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The CheckOut bool is used to create a cart that contains unconfirmed orders.
        /// </summary>
        public bool CheckOut { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        public virtual StoreLocation StoreLocation { get; set; }
    }
}
