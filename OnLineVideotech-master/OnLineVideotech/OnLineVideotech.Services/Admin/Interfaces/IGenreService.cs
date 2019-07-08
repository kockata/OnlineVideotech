using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Interfaces
{
    public interface IGenreService : IBaseService
    {
        Task Create(string name);

        Task<List<GenreServiceModel>> GetAllGenres();

        Task<GenreServiceModel> FindGenre(Guid id);

        Task UpdateGenre(GenreServiceModel genreServiceModel);

        Task Delete(GenreServiceModel genreServiceModel);

        Task<List<Genre>> GetAllGenreForMovie(Guid MovieId);

        Task<List<Guid>> GetAllMoviesForGenre(Guid genreId);
    }
}