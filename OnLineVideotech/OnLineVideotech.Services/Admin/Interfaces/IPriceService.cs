using OnLineVideotech.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Interfaces
{
    public interface IPriceService : IBaseService
    {
        Task CreatePrice(Guid movieId, string roleId, decimal moviePrice);

        Task<List<Price>> GetAllPricesForMovie(Guid idMovie);
    }
}