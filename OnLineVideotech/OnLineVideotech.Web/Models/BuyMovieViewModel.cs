using System;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Web.Models
{
    public class BuyMovieViewModel
    {
        [Required]
        public decimal Balance { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        public decimal Price { get; set; }

        public string MovieName { get; set; }
    }
}