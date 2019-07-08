using System;
using System.Linq;
using System.Threading.Tasks;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Interfaces;
using OnLineVideotech.Services.ServiceModels;

namespace OnLineVideotech.Services.Implementations
{
    public class UserBalanceService : BaseService, IBaseService, IUserBalanceService
    {
        public UserBalanceService(OnLineVideotechDbContext db) : base(db)
        {
        }

        public async Task AddAmount(UserBalanceServiceModel userModel, string userId)
        {
            if (this.Db.UserMoneyBalance.Any(u => u.UserId == userId))
            {
                UserMoneyBalance userMoneyBalance = await this.Db.UserMoneyBalance.FindAsync(userModel.Id);              
                UserBalanceServiceModel userBalanceServiceModel = GetUserBalance(userId);

                userMoneyBalance.Balance = userModel.Balance + userBalanceServiceModel.Balance;

                this.Db.UserMoneyBalance.Update(userMoneyBalance);
            }
            else
            {
                this.Db.UserMoneyBalance.Add(
                new UserMoneyBalance
                {
                    Balance = userModel.Balance,
                    UserId = userId
                });
            }

            await this.Db.SaveChangesAsync();
        }

        public async Task DecreaseBalance(string userId, decimal amount)
        {
            UserMoneyBalance userMoneyBalance = this.Db.UserMoneyBalance.Single(x => x.UserId == userId);
            userMoneyBalance.Balance = userMoneyBalance.Balance - amount;

            this.Db.UserMoneyBalance.Update(userMoneyBalance);
            await this.Db.SaveChangesAsync();
        }

        public UserBalanceServiceModel GetUserBalance(string userId)
        {
            UserMoneyBalance userBalance = this.Db.UserMoneyBalance
                .SingleOrDefault(x => x.UserId == userId);

            UserBalanceServiceModel userModel = new UserBalanceServiceModel();

            if (userBalance != null)
            {
                userModel.Balance = userBalance.Balance;
                userModel.Id = userBalance.Id;
            }
            else
            {
                userModel.Balance = Decimal.Zero;

                return userModel;
            }

            return userModel;
        }
    }
}