using OnLineVideotech.Services.Admin.ServiceModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Web.Areas.Admin.Models
{
    public class MovieAdminViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "Movie name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Year { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        [Display(Name = "Video path")]
        public string VideoPath { get; set; }

        [Display(Name = "Video path")]
        public string VideoPathEdit { get; set; }

        [Required]
        [Display(Name = "Poster address")]
        public string PosterPath { get; set; }

        [Required]
        [Display(Name = "Trailer address")]
        public string TrailerPath { get; set; }

        [Required]
        public string Summary { get; set; }

        public List<PriceServiceModel> Prices { get; set; }

        public List<GenreServiceModel> Genres { get; set; }
    }
}