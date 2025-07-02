using E_ticket514.DataAccess;
using E_ticket514.Models;
using E_ticket514.Repositories.IRepositories;

namespace E_ticket514.Repositories
{
    public class ActorRepository : Repository<Actor> , IActorRepository
    {
        public ActorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
