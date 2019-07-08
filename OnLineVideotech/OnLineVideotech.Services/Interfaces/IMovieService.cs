using OnLineVideotech.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Interfaces
{
    public interface IMovieService : IBaseService
    {
        Task<List<MovieServiceModel>> GetMovies();

        Task<MovieServiceModel> FindMovie(Guid id);

        Task BuyMovie(string userId, Guid movieId, decimal price);

        bool IsPurchased(string userId, Guid movieId);

        Task<IEnumerable<MovieServiceModel>> GetAllPurchasedMoviesForUser(string userId);

        Task<MovieFilterServiceModel> FilteredMovies(MovieFilterServiceModel moviesModel);
    }
}