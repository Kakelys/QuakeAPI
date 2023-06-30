using QuakeAPI.Data.Models;
using QuakeAPI.DTO;

namespace QuakeAPI.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAll();
        Task<Location> Create(LocationNew location);
        Task Delete(int id);
    }
}