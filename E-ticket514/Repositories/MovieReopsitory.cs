using E_ticket514.DataAccess;
using E_ticket514.Models;
using E_ticket514.Repositories.IRepositories;

namespace E_ticket514.Repositories
{
    public class MovieReopsitory : Repository<Movie>, IMovieRepository
    {
        public MovieReopsitory(ApplicationDbContext context) : base(context)
        {
        }
    }
}
