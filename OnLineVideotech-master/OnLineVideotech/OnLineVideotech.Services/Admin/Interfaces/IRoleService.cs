using OnLineVideotech.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Interfaces
{
    public interface IRoleService : IBaseService
    {
        Task<IEnumerable<Role>> GetAllRoles();

        Task<Role> FindRole(string roleId);

        Task<string> GetUserRole(string userId);
    }
}