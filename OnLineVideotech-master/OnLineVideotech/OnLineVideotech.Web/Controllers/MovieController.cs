using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.ServiceModels;
using OnLineVideotech.Services.Interfaces;
using OnLineVideotech.Services.ServiceModels;
using OnLineVideotech.Web.Infrastructure;
using OnLineVideotech.Web.Infrastructure.Extensions;
using OnLineVideotech.Web.Models;

namespace OnLineVideotech.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IMovieService movieService;
        private readonly IUserBalanceService userBalanceService;
        private readonly ICommentService commentService;
        private readonly IHistoryService historyService;
        private readonly IGenreService genreService;

        public MovieController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMovieService movieService,
            IUserBalanceService userBalanceService,
            ICommentService commentService,
            IHistoryService historyService,
            IGenreService genreService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.movieService = movieService;
            this.userBalanceService = userBalanceService;
            this.commentService = commentService;
            this.historyService = historyService;
            this.genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            MovieFilterServiceModel movieFilterServiceModel = new MovieFilterServiceModel();
            List<MovieServiceModel> movies = await this.movieService.GetMovies();
            List<GenreServiceModel> genres = await genreService.GetAllGenres();

            movieFilterServiceModel.MovieCollection = movies;

            if (User.Identity.IsAuthenticated)
            {
                User user = await userManager.GetUserAsync(HttpContext.User);
                IList<string> roles = await userManager.GetRolesAsync(user);
                string role = roles.SingleOrDefault();              

                if (role == null)
                {
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.RegularUser);

                    role = GlobalConstants.RegularUser;
                }

                foreach (MovieServiceModel movie in movies)
                {
                    MovieServiceModel movieModel = await this.movieService.FindMovie(movie.Id);

                    movie.IsPurchased = this.movieService.IsPurchased(user.Id, movie.Id);

                    movie.Price = movieModel.Prices.SingleOrDefault(x => x.Role.Name == role).MoviePrice;
                }

                foreach (GenreServiceModel genre in genres)
                {
                    GenreServiceModel genreModel = new GenreServiceModel();
                    genreModel.Name = genre.Name;
                    genreModel.Id = genre.Id;
                    movieFilterServiceModel.Genres.Add(genreModel);
                }              
            }

            return View(movieFilterServiceModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(MovieFilterServiceModel movieFilter)
        {
            MovieFilterServiceModel movieFilterServiceModel = await this.movieService.FilteredMovies(movieFilter);
            List<GenreServiceModel> genres = await genreService.GetAllGenres();

            if (User.Identity.IsAuthenticated)
            {
                User user = await userManager.GetUserAsync(HttpContext.User);
                IList<string> roles = await userManager.GetRolesAsync(user);
                string role = roles.SingleOrDefault();

                if (role == null)
                {
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.RegularUser);

                    role = GlobalConstants.RegularUser;
                }

                if (movieFilterServiceModel.MovieCollection != null)
                {
                    foreach (MovieServiceModel movie in movieFilterServiceModel.MovieCollection)
                    {
                        MovieServiceModel movieModel = await this.movieService.FindMovie(movie.Id);

                        movie.IsPurchased = this.movieService.IsPurchased(user.Id, movie.Id);

                        movie.Price = movieModel.Prices.SingleOrDefault(x => x.Role.Name == role).MoviePrice;
                    }
                }
                else
                {
                    movieFilterServiceModel.MovieCollection = new List<MovieServiceModel>();
                }

                foreach (GenreServiceModel genre in genres)
                {
                    GenreServiceModel genreModel = new GenreServiceModel();
                    genreModel.Name = genre.Name;
                    genreModel.Id = genre.Id;
                    movieFilterServiceModel.Genres.Add(genreModel);
                }
            }

            return View(movieFilterServiceModel);
        }

        public async Task<IActionResult> MovieDetails(Guid id)
        {
            MovieServiceModel movieModel = await this.movieService.FindMovie(id);

            movieModel.NumLinesSummary = Regex.Matches(movieModel.Summary, "\r\n").Count;
            movieModel.NumLinesSummary = movieModel.NumLinesSummary + movieModel.NumLinesSummary + 1;

            movieModel.Comments = await this.commentService.GetAllCommentsForMovie(movieModel.Id);
            movieModel.Comments = movieModel.Comments
                .OrderByDescending(x => x.Date)
                .ThenBy(p => p.Date.Hour)
                .ThenBy(c => c.Date.Minute)
                .ThenBy(s => s.Date.Second)
                .ToList();           

            if (User.Identity.IsAuthenticated)
            {
                User user = await userManager.GetUserAsync(HttpContext.User);

                movieModel.History = await this.historyService.GetHistoryForMovieAboutUser(user.Id, id);

                IList<string> roles = await userManager.GetRolesAsync(user);

                foreach (string role in roles)
                {
                    movieModel.Price = movieModel.Prices.SingleOrDefault(x => x.Role.Name == role).MoviePrice;
                }

                movieModel.IsPurchased = this.movieService.IsPurchased(user.Id, movieModel.Id);
            }

            return View(movieModel);
        }

        [Authorize]
        public IActionResult BuyMovie(Guid id)
        {
            BuyMovieViewModel buyMovieViewModel = new BuyMovieViewModel();
            buyMovieViewModel.MovieId = id;

            return View(buyMovieViewModel);
        }

        [Authorize]
        public async Task<IActionResult> PayWithCard(Guid id)
        {
            BuyMovieViewModel buyMovieViewModel = await GetPaymentModel(id);

            return View(buyMovieViewModel);
        }

        [Authorize]
        public async Task<IActionResult> PayFromBalance(Guid id)
        {
            BuyMovieViewModel buyMovieViewModel = await GetPaymentModel(id);

            return View(buyMovieViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PayFromBalance(BuyMovieViewModel buyMovieViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(buyMovieViewModel);
            }

            User user = await userManager.GetUserAsync(HttpContext.User);

            UserBalanceServiceModel userBalance = this.userBalanceService.GetUserBalance(user.Id);

            if (userBalance.Balance < buyMovieViewModel.Price || userBalance.Balance == 0)
            {
                TempData.AddErrorMessage("You don't have enough money in your account !");

                return RedirectToAction(nameof(PayFromBalance), new { id = buyMovieViewModel.MovieId });
            }

            await this.movieService.BuyMovie(user.Id, buyMovieViewModel.MovieId, buyMovieViewModel.Price);

            TempData.AddSuccessMessage($"Your purchase was successful!");

            return RedirectToAction(nameof(MovieDetails), new { id = buyMovieViewModel.MovieId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Library()
        {
            User user = await userManager.GetUserAsync(HttpContext.User);

            IEnumerable<MovieServiceModel> movies = await this.movieService.GetAllPurchasedMoviesForUser(user.Id);

            return View(movies);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(MovieServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                //var errors = ModelState.Select(x => x.Value.Errors)
                //           .Where(y => y.Count > 0)
                //           .ToList();

                return RedirectToAction(nameof(MovieDetails), new { id = model.Id });
            }

            User user = await userManager.GetUserAsync(HttpContext.User);

            await this.commentService.AddCommentForMovie(model.Comment, user.Id, model.Id);

            return RedirectToAction(nameof(MovieDetails), new { id = model.Id });
        }

        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            CommentServiceModel commentModel = await this.commentService.FindComment(id);

            return View(commentModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteComment(CommentServiceModel commentModel)
        {
            if (!ModelState.IsValid)
            {
                //var errors = ModelState.Select(x => x.Value.Errors)
                //           .Where(y => y.Count > 0)
                //           .ToList();

                return RedirectToAction(nameof(MovieDetails), new { id = commentModel.MovieId });
            }

            await this.commentService.DeleteComment(commentModel.Id);

            TempData.AddSuccessMessage($"Comment successfully deleted !");

            return RedirectToAction(nameof(MovieDetails), new { id = commentModel.MovieId });
        }    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<BuyMovieViewModel> GetPaymentModel(Guid id)
        {
            MovieServiceModel movieModel = await this.movieService.FindMovie(id);

            User user = await userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await userManager.GetRolesAsync(user);
            string role = roles.SingleOrDefault();

            movieModel.Price = movieModel.Prices.SingleOrDefault(x => x.Role.Name == role).MoviePrice;

            UserBalanceServiceModel userBalanceModel = new UserBalanceServiceModel();
            userBalanceModel = this.userBalanceService.GetUserBalance(user.Id);

            BuyMovieViewModel buyMovieViewModel = new BuyMovieViewModel();
            buyMovieViewModel.Balance = userBalanceModel.Balance;
            buyMovieViewModel.MovieId = id;
            buyMovieViewModel.Price = movieModel.Price;
            buyMovieViewModel.MovieName = movieModel.Name;

            return buyMovieViewModel;
        }
    }
}