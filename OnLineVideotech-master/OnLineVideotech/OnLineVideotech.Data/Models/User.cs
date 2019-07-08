using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Data.Models
{
    public class User : IdentityUser
    {
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string Address { get; set; }

        public List<History> Histories { get; set; } = new List<History>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public UserMoneyBalance Balance { get; set; }
    }
}