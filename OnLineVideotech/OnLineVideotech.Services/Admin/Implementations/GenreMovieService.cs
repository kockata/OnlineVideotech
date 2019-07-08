using System;
using System.Threading.Tasks;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;

namespace OnLineVideotech.Services.Admin.Implementations
{
    public class GenreMovieService : BaseService, IBaseService, IGenreMovieService
    {
        public GenreMovieService(OnLineVideotechDbContext db) : base(db)
        {
        }

        public async Task Create(Guid movieId, Guid genreId)
        {
            GenreMovie genreMovie = new GenreMovie()
            {
                GenreId = genreId,
                MovieId = movieId
            };

            await this.Db.GenreMovies.AddAsync(genreMovie);
        }
    }
}