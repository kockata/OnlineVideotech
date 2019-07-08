using OnLineVideotech.Data;
using System.Threading.Tasks;

namespace OnLineVideotech.Services
{
    public interface IBaseService
    {
        OnLineVideotechDbContext Db { get; }
    }
}