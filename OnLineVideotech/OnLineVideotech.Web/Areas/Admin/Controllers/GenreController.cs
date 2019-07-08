using Microsoft.AspNetCore.Mvc;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.ServiceModels;
using OnLineVideotech.Web.Areas.Admin.Models.Genres;
using OnLineVideotech.Web.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnLineVideotech.Web.Areas.Admin.Controllers
{
    public class GenreController : BaseAdminController
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public async Task<IActionResult> Add()
        {
            GenreViewModel model = new GenreViewModel();
            model.Genres = await this.genreService.GetAllGenres();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(GenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Genres = await this.genreService.GetAllGenres();

            if (model.Genres.Any(x => x.Name.ToLower() == model.Name.ToLower()))
            {
                TempData.AddErrorMessage($"Genre with name '{model.Name}' already exists !");
            }
            else
            {
                await this.genreService.Create(model.Name);
                TempData.AddSuccessMessage($"Genre '{model.Name}' successfully created !");
            }

            return RedirectToAction(nameof(Add));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GenreServiceModel genre = await this.genreService.FindGenre(id);
            GenreViewModel genreViewModel = new GenreViewModel
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return View(genreViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Genres = await this.genreService.GetAllGenres();

            if (model.Genres.Any(x => x.Name == model.Name))
            {
                TempData.AddErrorMessage($"Genre with name '{model.Name}' already exists !");

                return RedirectToAction(nameof(Edit));
            }
            else
            {
                GenreServiceModel genreServiceModel = new GenreServiceModel
                {
                    Id = model.Id,
                    Name = model.Name
                };

                await this.genreService.UpdateGenre(genreServiceModel);

                TempData.AddSuccessMessage($"Genre '{model.Name}' successfully updated !");

                return RedirectToAction(nameof(Add));
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Add));
            }

            GenreServiceModel genre = await this.genreService.FindGenre(id);

            if (genre.Name == null)
            {
                return RedirectToAction(nameof(Add));
            }

            GenreViewModel genreViewModel = new GenreViewModel
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return View(genreViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(GenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            GenreServiceModel genreServiceModel = new GenreServiceModel
            {
                Id = model.Id,
                Name = model.Name
            };

            await this.genreService.Delete(genreServiceModel);

            TempData.AddSuccessMessage($"Genre '{model.Name}' successfully deleted !");

            return RedirectToAction(nameof(Add));
        }
    }
}