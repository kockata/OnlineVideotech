using OnLineVideotech.Services.Admin.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Interfaces
{
    public interface IAdminUserService : IBaseService
    {
        Task<IEnumerable<AdminUserListingServiceModel>> AllAsync();
    }
}