using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        [Required(ErrorMessage = "Plan Name Is Required")]
        [StringLength(50,ErrorMessage ="Plan Name Must Be Less Than 51 Char" )]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description Name Is Required")]
        [StringLength(200, MinimumLength = 5 ,  ErrorMessage = "Description Name Must Be Between Than 5 And 200 Char")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "DurationDays Name Is Required")]
        [Range(1, 365 , ErrorMessage = " DurationDays Must be Between 1 And 365 ")]
        public int DurationDays {  get; set; }

        [Required(ErrorMessage = "Price Name Is Required")]
        [Range(0.1, 10000, ErrorMessage = " Price Must be Between 0.1 And 10000 ")]
        public decimal Price {  get; set; }
    }
}
