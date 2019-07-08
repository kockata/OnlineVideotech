using OnLineVideotech.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<List<HistoryServiceModel>> GetHistoryForMovie(Guid movieId);

        Task<HistoryServiceModel> GetHistoryForMovieAboutUser(string userId, Guid movieId);

        Task<List<HistoryServiceModel>> GetHistory();
    }
}
