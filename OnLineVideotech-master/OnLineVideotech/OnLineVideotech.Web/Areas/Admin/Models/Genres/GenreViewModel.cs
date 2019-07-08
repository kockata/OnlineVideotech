using OnLineVideotech.Services.Admin.ServiceModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Web.Areas.Admin.Models.Genres
{
    public class GenreViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name="Genre name")]
        public string Name { get; set; }

        public List<GenreServiceModel> Genres { get; set; }
    }
}