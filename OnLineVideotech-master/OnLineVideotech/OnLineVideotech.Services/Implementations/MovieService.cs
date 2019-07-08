using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.ServiceModels;
using OnLineVideotech.Services.Interfaces;
using OnLineVideotech.Services.ServiceModels;

namespace OnLineVideotech.Services.Implementations
{
    public class MovieService : BaseService, IBaseService, IMovieService
    {
        private IPriceService priceService;
        private IUserBalanceService userBalance;
        private IGenreService genreService;

        public MovieService(OnLineVideotechDbContext db,
            IPriceService priceService,
            IUserBalanceService userBalance,
            IGenreService genreService) : base(db)
        {
            this.priceService = priceService;
            this.userBalance = userBalance;
            this.genreService = genreService;
        }

        public async Task<List<MovieServiceModel>> GetMovies()
        {
            List<MovieServiceModel> movieModel = await this.Db.Movies
                .Select(m => new MovieServiceModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Genres = m.Genres,
                    Rating = m.Rating,
                    PosterPath = m.PosterPath,
                    VideoPath = m.VideoPath,
                    TrailerPath = m.TrailerPath,
                    Summary = m.Summary,
                    Year = m.Year
                })
                .ToListAsync();

            return movieModel;
        }

        public async Task<MovieServiceModel> FindMovie(Guid id)
        {
            Movie movie = await this.Db.Movies
                .Include(x => x.Genres)
                    .ThenInclude(p => p.Genre)
                .SingleOrDefaultAsync(m => m.Id == id);

            MovieServiceModel movieModel = new MovieServiceModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Genres = movie.Genres,
                Rating = movie.Rating,
                PosterPath = movie.PosterPath,
                VideoPath = movie.VideoPath,
                TrailerPath = movie.TrailerPath,
                Summary = movie.Summary,
                Year = movie.Year
            };

            movieModel.Prices = await this.priceService.GetAllPricesForMovie(movieModel.Id);

            return movieModel;
        }

        public bool IsPurchased(string userId, Guid movieId)
        {
            return this.Db.Histories.Any(x => x.MovieId == movieId && x.CustomerId == userId);
        }

        public async Task BuyMovie(string userId, Guid movieId, decimal price)
        {
            await this.userBalance.DecreaseBalance(userId, price);

            History history = new History();
            history.Price = price;
            history.CustomerId = userId;
            history.MovieId = movieId;
            history.Date = DateTime.Now;

            await this.Db.Histories.AddAsync(history);
            await this.Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieServiceModel>> GetAllPurchasedMoviesForUser(string userId)
        {
            List<History> userHistories = await this.Db.Histories.Where(x => x.CustomerId == userId).ToListAsync();
            List<MovieServiceModel> movies = new List<MovieServiceModel>();

            foreach (History history in userHistories)
            {
                Movie movie = await this.Db.Movies
                     .FindAsync(history.MovieId);

                MovieServiceModel movieModel = new MovieServiceModel
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Genres = movie.Genres,
                    Rating = movie.Rating,
                    PosterPath = movie.PosterPath,
                    VideoPath = movie.VideoPath,
                    TrailerPath = movie.TrailerPath,
                    Summary = movie.Summary,
                    Year = movie.Year
                };

                movies.Add(movieModel);
            }

            return movies;
        }

        public async Task<MovieFilterServiceModel> FilteredMovies(MovieFilterServiceModel moviesModel)
        {
            MovieFilterServiceModel movieModel = new MovieFilterServiceModel();

            if (moviesModel.MovieName != null)
            {
                movieModel = await SearchWithMovieName(moviesModel.MovieName, movieModel);

                if (movieModel.MovieCollection.Count == 0)
                {
                    return movieModel;
                }
            }

            if (moviesModel.Year != null)
            {
                movieModel = await SearchWithMovieYear(moviesModel.Year, movieModel);

                if (movieModel.MovieCollection.Count == 0)
                {
                    return movieModel;
                }
            }

            if (moviesModel.Genres.Any(x => x.IsSelected))
            {
                movieModel = await SearchWithMovieGenres(moviesModel.Genres, movieModel);
            }

            return movieModel;
        }

        public async Task<MovieFilterServiceModel> SearchWithMovieName(string name, MovieFilterServiceModel movieModel)
        {
            movieModel.MovieCollection = await this.Db.Movies
                    .Include(p => p.Genres)
                    .Where(d => d.Name.Contains(name))
                    .Select(g => new MovieServiceModel
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Rating = g.Rating,
                        PosterPath = g.PosterPath,
                        VideoPath = g.VideoPath,
                        TrailerPath = g.TrailerPath,
                        Summary = g.Summary,
                        Year = g.Year
                    })
                    .ToListAsync();

            return movieModel;
        }

        public async Task<MovieFilterServiceModel> SearchWithMovieYear(string year, MovieFilterServiceModel movieModel)
        {
            try
            {
                if (movieModel.MovieCollection.Count == 0)
                {
                    movieModel.MovieCollection = await this.Db.Movies
                       .Include(p => p.Genres)
                       .Where(d => d.Year.Year == int.Parse(year))
                       .Select(g => new MovieServiceModel
                       {
                           Id = g.Id,
                           Name = g.Name,
                           Rating = g.Rating,
                           PosterPath = g.PosterPath,
                           VideoPath = g.VideoPath,
                           TrailerPath = g.TrailerPath,
                           Summary = g.Summary,
                           Year = g.Year
                       })
                       .ToListAsync();
                }
                else
                {
                    movieModel.MovieCollection = movieModel.MovieCollection
                        .Where(d => d.Year.Year == int.Parse(year))
                        .ToList();
                }
            }
            catch (Exception)
            {
                return movieModel;
            }

            return movieModel;
        }

        public async Task<MovieFilterServiceModel> SearchWithMovieGenres(List<GenreServiceModel> genres, MovieFilterServiceModel movieModel)
        {
            List<MovieServiceModel> actualMovieList = new List<MovieServiceModel>();

            foreach (GenreServiceModel genre in genres)
            {
                if (genre.IsSelected)
                {
                    if (movieModel.MovieCollection.Count == 0)
                    {
                        List<Guid> moviesId = await this.genreService.GetAllMoviesForGenre(genre.Id);

                        foreach (Guid movieId in moviesId)
                        {
                            Movie movie = await this.Db.Movies
                                .Include(p => p.Prices)
                                .Include(b => b.Genres)
                                .SingleOrDefaultAsync(k => k.Id == movieId);


                            if (movieModel.MovieCollection == null || !movieModel.MovieCollection.Any(m => m.Id == movie.Id))
                            {
                                movieModel.MovieCollection.Add(new MovieServiceModel
                                {
                                    Id = movie.Id,
                                    Name = movie.Name,
                                    Rating = movie.Rating,
                                    PosterPath = movie.PosterPath,
                                    VideoPath = movie.VideoPath,
                                    TrailerPath = movie.TrailerPath,
                                    Summary = movie.Summary,
                                    Year = movie.Year
                                });
                            }
                        }
                    }
                    else
                    {
                        foreach (MovieServiceModel movie in movieModel.MovieCollection)
                        {
                            List<Genre> genresList = await this.genreService.GetAllGenreForMovie(movie.Id);                  

                            if (genresList.Any(g => g.Id == genre.Id) && !actualMovieList.Any(m => m.Id == movie.Id))
                            {
                                actualMovieList.Add(movie);
                            }
                        }

                        movieModel.MovieCollection = actualMovieList;
                    }
                }
            }

            return movieModel;
        }
    }
}