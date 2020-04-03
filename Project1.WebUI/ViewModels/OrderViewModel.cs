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
        [Required]
        public int ProductId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public byte[] OrderTime { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
