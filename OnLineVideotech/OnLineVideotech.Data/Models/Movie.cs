using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Data.Models
{
    public class Movie
    {
        public Movie()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public DateTime Year { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public string VideoPath { get; set; }

        [Required]
        public string PosterPath { get; set; }

        [Required]
        public string TrailerPath { get; set; }

        [Required]
        public string Summary { get; set; }

        public List<GenreMovie> Genres { get; set; } = new List<GenreMovie>();

        public List<History> Histories { get; set; } = new List<History>();

        public List<Price> Prices { get; set; } = new List<Price>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}