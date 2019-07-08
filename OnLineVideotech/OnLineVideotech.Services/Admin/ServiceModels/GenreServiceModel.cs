using System;

namespace OnLineVideotech.Services.Admin.ServiceModels
{
    public class GenreServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}