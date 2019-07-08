using OnLineVideotech.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Services.Admin.ServiceModels
{
    public class MovieAdminServiceModel
    {
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

        public List<GenreServiceModel> Genres { get; set; } 

        public List<PriceServiceModel> Prices { get; set; } 
    }
}