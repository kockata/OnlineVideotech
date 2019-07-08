using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.ServiceModels;
using OnLineVideotech.Web.Areas.Admin.Models;
using OnLineVideotech.Web.Areas.Admin.Models.Movies;
using OnLineVideotech.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnLineVideotech.Web.Areas.Admin.Controllers
{
    public class MovieManagementController : BaseAdminController
    {
        private readonly IMovieManagementService moviesService;
        private readonly UserManager<User> userManager;
        private readonly IRoleService roleService;
        private readonly IGenreService genreService;

        public MovieManagementController(
            IMovieManagementService moviesService,
            UserManager<User> userManager,
            IRoleService roleService,
            IGenreService genreService)
        {
            this.moviesService = moviesService;
            this.userManager = userManager;
            this.roleService = roleService;
            this.genreService = genreService;
        }

        public async Task<IActionResult> CreateMovie()
        {
            IEnumerable<Role> roles = await roleService.GetAllRoles();
            IEnumerable<GenreServiceModel> genres = await genreService.GetAllGenres();

            MovieAdminViewModel model = new MovieAdminViewModel();
            model.Prices = new List<PriceServiceModel>();
            model.Genres = new List<GenreServiceModel>();

            foreach (Role role in roles)
            {
                PriceServiceModel priceModel = new PriceServiceModel();
                priceModel.RoleId = role.Id;
                priceModel.Role = role;
                model.Prices.Add(priceModel);
            }

            foreach (GenreServiceModel genre in genres)
            {
                GenreServiceModel genreModel = new GenreServiceModel();
                genreModel.Name = genre.Name;
                genreModel.Id = genre.Id;
                model.Genres.Add(genreModel);
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.moviesService.Create(
                model.Name,
                model.Year,
                model.Rating,
                model.VideoPath,
                model.PosterPath,
                model.TrailerPath,
                model.Summary,
                model.Prices,
                model.Genres);

            TempData.AddSuccessMessage($"Movie '{model.Name}' successfully created");

            return RedirectToAction(nameof(CreateMovie));
        }

        public async Task<IActionResult> EditMovie(Guid id)
        {
            MovieAdminServiceModel movie = await this.moviesService.FindMovie(id);
            MovieAdminViewModel model = new MovieAdminViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Year = movie.Year,
                Rating = movie.Rating,
                VideoPath = movie.VideoPath,
                PosterPath = movie.PosterPath,
                TrailerPath = movie.TrailerPath,
                Summary = movie.Summary,
                Prices = movie.Prices,
                Genres = movie.Genres
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult EditMovie(MovieAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.moviesService.EditMovie(
                model.Id,
                model.Name,
                model.Year,
                model.Rating,
                model.VideoPathEdit,
                model.PosterPath,
                model.TrailerPath,
                model.Summary,
                model.Prices,
                model.Genres);

            TempData.AddSuccessMessage($"Movie '{model.Name}' successfully edited !");

            return RedirectToAction(nameof(EditMovie));
        }

        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            MovieAdminServiceModel movie = await this.moviesService.FindMovie(id);
            DeleteMovieViewModel movieModel = new DeleteMovieViewModel();

            movieModel.MovieName = movie.Name;
            movieModel.Id = movie.Id;

            return View(movieModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovie(DeleteMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //var errors = ModelState.Select(x => x.Value.Errors)
                //           .Where(y => y.Count > 0)
                //           .ToList();

                return View(model);
            }

            await this.moviesService.DeleteMovie(model.Id);

            TempData.AddSuccessMessage($"Movie '{model.MovieName}' successfully deleted !");

            return RedirectToAction("Index", "Movie");
        }
    }
}