using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnLineVideotech.Data.Models
{
    public class Price
    {
        public Price()
        {
            this.Id = Guid.NewGuid();
        }
   
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }
    
        public string RoleId { get; set; }

        public Role Role { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MoviePrice { get; set; }
    }
}