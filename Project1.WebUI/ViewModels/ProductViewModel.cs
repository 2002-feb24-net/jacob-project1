using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project1.WebUI.ViewModels
{
    public class ProductViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        public int StoreLocationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }

        public virtual IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
