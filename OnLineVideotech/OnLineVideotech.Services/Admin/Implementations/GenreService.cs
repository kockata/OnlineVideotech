using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.ServiceModels;

namespace OnLineVideotech.Services.Admin.Implementations
{
    public class GenreService : BaseService, IBaseService, IGenreService
    {
        public GenreService(OnLineVideotechDbContext db) : base(db)
        {
        }

        public async Task Create(string name)
        {
            Genre genre = new Genre()
            {
                Name = name
            };

            await this.Db.Genres.AddAsync(genre);
            await this.Db.SaveChangesAsync();
        }

        public async Task<GenreServiceModel> FindGenre(Guid id)
        {
            Genre genre = new Genre();

            if (this.Db.Genres.Any(g => g.Id == id))
            {
                genre = await this.Db.Genres.FindAsync(id);
            }

            return new GenreServiceModel
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public async Task<List<GenreServiceModel>> GetAllGenres()
        {
            return await this.Db.Genres
                .Select(x => new GenreServiceModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task UpdateGenre(GenreServiceModel genreServiceModel)
        {
            Genre genre = new Genre
            {
                Id = genreServiceModel.Id,
                Name = genreServiceModel.Name
            };

            this.Db.Genres.Update(genre);
            await this.Db.SaveChangesAsync();
        }

        public async Task Delete(GenreServiceModel genreServiceModel)
        {
            Genre genre = new Genre
            {
                Id = genreServiceModel.Id,
                Name = genreServiceModel.Name
            };

            this.Db.Genres.Remove(genre);
            await this.Db.SaveChangesAsync();
        }

        public async Task<List<Genre>> GetAllGenreForMovie(Guid MovieId)
        {
            List<GenreMovie> movieGenres = await this.Db.GenreMovies
                .Where(x => x.MovieId == MovieId)
                .ToListAsync();

            List<Genre> genres = new List<Genre>();

            foreach (var item in movieGenres)
            {
                genres.Add(await this.Db.Genres.FindAsync(item.GenreId));
            }

            return genres;
        }

        public async Task<List<Guid>> GetAllMoviesForGenre(Guid genreId)
        {
            return await this.Db.GenreMovies
                .Where(x => x.GenreId == genreId)
                .Select(c => c.MovieId)
                .ToListAsync();
        }
    }
}