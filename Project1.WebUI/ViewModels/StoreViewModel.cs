using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project1.WebUI.ViewModels
{
    public class StoreViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        public string LocationName { get; set; }

        public virtual IEnumerable<OrderViewModel> Orders { get; set; }
        public virtual IEnumerable<ProductViewModel> Product { get; set; }
    }
}
