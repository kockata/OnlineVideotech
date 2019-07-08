using OnLineVideotech.Services.Admin.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Interfaces
{
    public interface IMovieManagementService : IBaseService
    {
        Task Create(
            string name,
            DateTime year,
            double rating,
            string videoPath,
            string posterPath,
            string trailerPath,
            string summary,
            List<PriceServiceModel> prices,
            List<GenreServiceModel> genres);

        Task<MovieAdminServiceModel> FindMovie(Guid id);

        void EditMovie(
            Guid id,
            string name,
            DateTime year,
            double rating,
            string videoPath,
            string posterPath,
            string trailerPath,
            string summary,
            List<PriceServiceModel> prices,
            List<GenreServiceModel> genres);

        Task DeleteMovie(Guid id);
    }
}