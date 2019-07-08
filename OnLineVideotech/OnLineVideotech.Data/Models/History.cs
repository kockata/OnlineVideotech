using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnLineVideotech.Data.Models
{
    public class History
    {
        public History()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]

        public decimal Price { get; set; }

        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }

        public string CustomerId { get; set; }

        public User Customer { get; set; }
    }
}