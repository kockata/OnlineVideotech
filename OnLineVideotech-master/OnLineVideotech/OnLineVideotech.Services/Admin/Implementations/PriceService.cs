using Microsoft.EntityFrameworkCore;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Implementations
{
    public class PriceService : BaseService, IBaseService, IPriceService
    {
        private IRoleService roleService;

        public PriceService(OnLineVideotechDbContext db, IRoleService roleService) : base(db)
        {
            this.roleService = roleService;
        }

        public async Task CreatePrice(Guid movieId, string roleId, decimal moviePrice)
        {
            Price price = new Price
            {
               MovieId = movieId,
               RoleId = roleId,
               MoviePrice = moviePrice
            };

            await this.Db.AddAsync(price);           
        }

        public async Task<List<Price>> GetAllPricesForMovie(Guid idMovie)
        {
            List<Price> prices = await Db.Prices
                .Include(x => x.Role)
                .Where(p => p.MovieId == idMovie)
                .ToListAsync();

            return prices;
        }
    }
}