using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO;
using QuakeAPI.Exceptions;
using QuakeAPI.Extensions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class LocationService : ILocationService
    {
        private IRepositoryManager _rep;

        private IWebHostEnvironment _env;

        private readonly string LOCATION_PATH;
        private readonly string POSTER_PATH;

        public LocationService(IRepositoryManager rep, IWebHostEnvironment env)
        {
            _rep = rep;
            _env = env;

            LOCATION_PATH = _env.WebRootPath + @"\Locations\";
            POSTER_PATH = _env.WebRootPath + @"\Posters\";
        }

        public async Task<Location> Create(LocationNew location)
        {
            if(await _rep.Location.FindByCondition(x => EF.Functions.Like(x.Name, location.Name), false)
                .FirstOrDefaultAsync() != null)
                throw new BadRequestException("Location with same name already exist.");

            var locationExtension = Path.GetExtension(location.Location.FileName);
            var posterExtenstion = Path.GetExtension(location.Poster.FileName);

            var locationEntity = new Location() 
            {
                Name = location.Name,
                Description = location.Description,
                LocationPath = location.Name + locationExtension,
                PosterPath = location.Name + posterExtenstion 
            };

            try 
            {       
                using (var stream = System.IO.File.Create(LOCATION_PATH + locationEntity.LocationPath))
                {
                    await location.Location.CopyToAsync(stream);
                }

                using (var stream = System.IO.File.Create(POSTER_PATH + locationEntity.PosterPath))
                {
                    await location.Poster.CopyToAsync(stream);
                }

                var entity = _rep.Location.Create(locationEntity);

                await _rep.Save();

                return entity;
            }
            catch
            {
                if(File.Exists(LOCATION_PATH + locationEntity.LocationPath))
                    File.Delete(LOCATION_PATH + locationEntity.LocationPath);

                if(File.Exists(POSTER_PATH + locationEntity.PosterPath))
                    File.Delete(POSTER_PATH + locationEntity.PosterPath);

                throw;
            }
        }

        public async Task Delete(int id)
        {
            var entity = await _rep.Location.FindByCondition(x => x.Id == id, false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Location not found.");

            await _rep.BeginTransaction();

            _rep.Location.Delete(entity);
            await _rep.Save();
            try 
            {
                if(File.Exists(LOCATION_PATH + entity.LocationPath))
                    File.Delete(LOCATION_PATH + entity.LocationPath);

                if(File.Exists(POSTER_PATH + entity.PosterPath))
                    File.Delete(POSTER_PATH + entity.PosterPath);
            } catch(Exception ex) 
            {
                await _rep.Rollback();
                throw new Exception("Failed to delete files", ex);
            }

            await _rep.Commit();
        }

        public async Task<List<Location>> GetAll()
        {
            return await _rep.Location.FindAll(false).ToListAsync();
        }

        public async Task<List<Location>> GetPage(Page page)
        {
            return await _rep.Location.FindAll(false).TakePage(page).ToListAsync();
        }
    }
}