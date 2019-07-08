using OnLineVideotech.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnLineVideotech.Services.ServiceModels
{
    public class MovieServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Year { get; set; }

        public double Rating { get; set; }

        public string VideoPath { get; set; }

        public string PosterPath { get; set; }

        public string TrailerPath { get; set; }

        public string Summary { get; set; }

        public int NumLinesSummary { get; set; }

        public decimal Price { get; set; }

        public bool IsPurchased { get; set; }

        public HistoryServiceModel History { get; set; }

        public List<GenreMovie> Genres { get; set; } = new List<GenreMovie>();

        public List<Price> Prices { get; set; } = new List<Price>();

        public List<CommentServiceModel> Comments { get; set; } = new List<CommentServiceModel>();

        [Required]
        [StringLength(2000, MinimumLength = 3)]
        public string Comment { get; set; }
    }
}