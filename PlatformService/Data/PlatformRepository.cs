using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _context;
        public PlatformRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform newPlatform)
        {
            if (newPlatform == null)
            {
                throw new ArgumentNullException(nameof(newPlatform));
            }

            _context.Platforms.Add(newPlatform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChange()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}