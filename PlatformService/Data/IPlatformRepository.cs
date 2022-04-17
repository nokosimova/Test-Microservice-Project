using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepository 
    {
        bool SaveChange();

        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform newPlatform);
    
    }
}