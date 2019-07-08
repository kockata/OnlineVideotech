using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Web.Areas.Admin.Models.Users
{
    public class AddUserToRoleFormModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}