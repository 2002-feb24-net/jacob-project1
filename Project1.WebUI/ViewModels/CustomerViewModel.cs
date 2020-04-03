using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project1.WebUI.ViewModels
{
    public class CustomerViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public virtual IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
