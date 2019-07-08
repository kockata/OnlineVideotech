using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OnLineVideotech.Data.Models
{
    public class Role : IdentityRole
    {
        public List<Price> Movies { get; set; } = new List<Price>();
    }
}