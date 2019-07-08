using System;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserComment { get; set; }

        public DateTime Date { get; set; }

        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }

        public string CustomerId { get; set; }

        public User Customer { get; set; }
    }
}