using OnLineVideotech.Data.Models;
using System;

namespace OnLineVideotech.Services.Admin.ServiceModels
{
    public class PriceServiceModel
    {
        public Guid Id { get; set; }

        public string RoleId { get; set; }

        public Role Role { get; set; }

        public decimal Price { get; set; }
    }
}