using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project1.WebUI.ViewModels
{
    public class OrderViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        public int StoreLocationId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public byte[] OrderTime { get; set; }
        [Required]
        public int Quantity { get; set; }

        public string StoreName { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
    }
}
