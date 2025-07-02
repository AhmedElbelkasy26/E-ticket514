using E_ticket514.DataAccess;
using E_ticket514.Models;
using E_ticket514.Repositories.IRepositories;

namespace E_ticket514.Repositories
{
    public class ActorMovieRepository : Repository<ActorMovie>, IActorMovieRepository
    {
        public ActorMovieRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
