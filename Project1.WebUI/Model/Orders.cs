using System;
using System.Collections.Generic;

namespace Project1.WebUI.Model
{
    public partial class Orders
    {
        public int Id { get; set; }
        public int StoreLocationId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string OrderTime { get; set; }
        public int Quantity { get; set; }
        public bool? CheckOut { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        public virtual StoreLocation StoreLocation { get; set; }
    }
}
