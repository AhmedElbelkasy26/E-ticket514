using E_ticket514.DataAccess;
using E_ticket514.Models;
using E_ticket514.Repositories.IRepositories;

namespace E_ticket514.Repositories
{
    public class ImageRepository : Repository<MovieImage>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
