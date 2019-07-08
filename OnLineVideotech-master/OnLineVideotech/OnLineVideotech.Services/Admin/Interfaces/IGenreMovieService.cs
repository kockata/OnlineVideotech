using System;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Interfaces
{
    public interface IGenreMovieService : IBaseService
    {
        Task Create(Guid movieId, Guid genreId);
    }
}