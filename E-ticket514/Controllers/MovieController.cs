using E_ticket514.DataAccess;
using E_ticket514.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_ticket514.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new();
        public IActionResult Index()
        {
            var movies = _dbContext.Movies.Include(e=>e.Cinema).Include(e=>e.Category).Include(e=>e.Images).ToList();
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }

        public IActionResult Details(int Id)
        {
            var movie = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category).Include(e => e.Images)
               .FirstOrDefault(e=>e.Id== Id);
            var actormovies = _dbContext.ActorsMovies.Include(e => e.Actor).Where(e=>e.MovieId== Id).Include(e=>e.Actor).ToList();
            if (movie == null)
            {
                return NotFound();
            }
            var model = new ModelsAndActorMovieVM() { actorMovies = actormovies, Movie = movie };
            return View(model);
        }
        public IActionResult Create()
        {
            var categories = _dbContext.Categories.Select(e=> new SelectListItem() 
            { Text = e.Name , Value= e.Id.ToString() }).ToList();
            var cinemas = _dbContext.Cinemas.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var actors = _dbContext.Actors.Select(e => new SelectListItem()
            { Text = e.FirstName+' '+ e.LastName, Value = e.Id.ToString() }).ToList();

            ViewData["Cinema"] = _dbContext.Cinemas.ToList();
           

            return View(new MovieWithCategories_Cinemas_ActorsVM()
            {
                Actors = actors,
                Categories = categories,
                Cinemas = cinemas
            });
        }
    }
}
