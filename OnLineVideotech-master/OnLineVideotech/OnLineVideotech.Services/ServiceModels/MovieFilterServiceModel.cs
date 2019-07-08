using OnLineVideotech.Services.Admin.ServiceModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Services.ServiceModels
{
    public class MovieFilterServiceModel
    {
        public List<MovieServiceModel> MovieCollection { get; set; } = new List<MovieServiceModel>();

        [Display(Name = "Movie name")]
        public string MovieName { get; set; }

        public string Year { get; set; }

        public List<GenreServiceModel> Genres { get; set; } = new List<GenreServiceModel>();
    }
}