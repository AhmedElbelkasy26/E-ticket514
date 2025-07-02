using E_ticket514.DataAccess;
using E_ticket514.Models;
using E_ticket514.Repositories.IRepositories;

namespace E_ticket514.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
