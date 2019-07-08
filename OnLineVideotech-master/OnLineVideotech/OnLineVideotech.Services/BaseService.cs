using OnLineVideotech.Data;
using System.Threading.Tasks;

namespace OnLineVideotech.Services
{
    public class BaseService : IBaseService
    {
        public BaseService(OnLineVideotechDbContext db)
        {
            this.Db = db;
        }

        public OnLineVideotechDbContext Db { get; }
    }
}