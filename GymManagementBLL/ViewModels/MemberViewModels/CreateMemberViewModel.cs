using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$"  , ErrorMessage = "Name Can contain only letters And Spaces " )]
        [StringLength(50 , MinimumLength = 2 , ErrorMessage = "Name Must be between 2 And 50 Char")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Email Is Invalid Format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Phone Is Invalid Format")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|012|011|015)\d{8}$" , ErrorMessage = "Phone Number Must Be Valid Egyptian PhoneNumber")]

        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Data Of Birth Is Required")]
        [DataType(DataType.Date)]

        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender Is Required ")]

        public Gender Gender { get; set; }


        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1,9000 , ErrorMessage = "Building Number Must Be between 1 And 9000")]
        public int BuildingNumber {  get; set; }


        [Required(ErrorMessage = "Street is Required")]
        [StringLength(30 , MinimumLength = 2 , ErrorMessage = "Street  Must Be between 2 And 30")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can contain only letters And Spaces ")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "HealthRecord Is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
    }
    
}
