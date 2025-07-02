using E_ticket514.DataAccess;
using E_ticket514.Models;
using E_ticket514.Models.ViewModels;
using E_ticket514.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_ticket514.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new();
        public MovieController( )
        {
            
        }
        public IActionResult Index()
        {
            var movies = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category).Include(e => e.Images).ToList();
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }

        public IActionResult Details(int Id)
        {
            var movie = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category).Include(e => e.Images)
               .FirstOrDefault(e => e.Id == Id);
            var actormovies = _dbContext.ActorsMovies.Include(e => e.Actor).Where(e => e.MovieId == Id).Include(e => e.Actor).ToList();
            if (movie == null)
            {
                return NotFound();
            }
            var model = new ModelsAndActorMovieVM() { actorMovies = actormovies, Movie = movie };
            return View(model);
        }
        public IActionResult Create()
        {
            var categories = _dbContext.Categories.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var cinemas = _dbContext.Cinemas.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var actors = _dbContext.Actors.Select(e => new SelectListItem()
            { Text = e.FirstName + ' ' + e.LastName, Value = e.Id.ToString() }).ToList();

           


            return View(new MovieWithCategories_Cinemas_ActorsVM()
            {
                Actors = actors,
                Categories = categories,
                Cinemas = cinemas
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, List<int> ActorsId, List<IFormFile> imgs)
        {
            ModelState.Remove("Movie.Cinema");
            ModelState.Remove("Movie.Category");
            if (ModelState.IsValid)
            {
                if (!imgs.Any())
                {
                    var categories2 = _dbContext.Categories.Select(e => new SelectListItem()
                    { Text = e.Name, Value = e.Id.ToString() }).ToList();
                    var cinemas2 = _dbContext.Cinemas.Select(e => new SelectListItem()
                    { Text = e.Name, Value = e.Id.ToString() }).ToList();
                    var actors2 = _dbContext.Actors.Select(e => new SelectListItem()
                    { Text = e.FirstName + ' ' + e.LastName, Value = e.Id.ToString() }).ToList();


                    var selectedActors2 = _dbContext.ActorsMovies.Where(e => e.MovieId == movie.Id).ToList();

                    ModelState.AddModelError("Movie.Images", "you must add atleast one image");

                    return View(new MovieWithCategories_Cinemas_ActorsVM()
                    {
                        Actors = actors2,
                        Categories = categories2,
                        Cinemas = cinemas2,
                        Movie = movie,
                        MyActors = selectedActors2
                    });
                }
                List<string> newImgs = new List<string>();
                 foreach (var item in imgs)
                {
                    //hjksfdjghdfsiuoydfsi.png
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await item.CopyToAsync(stream);
                    }
                    newImgs.Add(fileName);
                }
                _dbContext.Movies.Add(movie);
                _dbContext.SaveChanges();
                if (ActorsId.Any())
                {
                    foreach (var item in ActorsId)
                    {
                        _dbContext.ActorsMovies.Add(new ActorMovie() { ActorId = item  , MovieId = movie.Id});
                    }
                }
                if (newImgs.Any())
                {
                    foreach (var item in newImgs)
                    {
                        _dbContext.Images.Add(new() { ImageUrl = item, MovieId = movie.Id });
                    }
                }
                _dbContext.SaveChanges();
                return RedirectToAction("index");

                
            }
            var categories = _dbContext.Categories.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var cinemas = _dbContext.Cinemas.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var actors = _dbContext.Actors.Select(e => new SelectListItem()
            { Text = e.FirstName + ' ' + e.LastName, Value = e.Id.ToString() }).ToList();


            var selectedActors = _dbContext.ActorsMovies.Where(e => e.MovieId == movie.Id).ToList();

            return View(new MovieWithCategories_Cinemas_ActorsVM()
            {
                Actors = actors,
                Categories = categories,
                Cinemas = cinemas ,
                Movie = movie ,
                MyActors = selectedActors
            });
            
        }

        public IActionResult Edit(int id)
        {
            var movie = _dbContext.Movies.FirstOrDefault(e => e.Id == id);
            var categories = _dbContext.Categories.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var cinemas = _dbContext.Cinemas.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var actors = _dbContext.Actors.Select(e => new SelectListItem()
            { Text = e.FirstName + ' ' + e.LastName, Value = e.Id.ToString() }).ToList();

            var selectedActors = _dbContext.ActorsMovies.Where(e => e.MovieId == id).ToList();
            


            return View(new MovieWithCategories_Cinemas_ActorsVM()
            {
                Actors = actors,
                Categories = categories,
                Cinemas = cinemas,
                Movie = movie ,
                MyActors = selectedActors
            });
        }

        [HttpPost]
        public IActionResult Edit(Movie movie, List<int> ActorsId, List<IFormFile> imgs)
        {
            ModelState.Remove("Movie.Cinema");
            ModelState.Remove("Movie.Category");
            if (ModelState.IsValid)
            {
                if (imgs.Any())
                {
                    List<string> newImgs = new List<string>();
                    // delete old images 
                    var oldImgs = _dbContext.Images.Where(e=>e.MovieId == movie.Id).ToList();
                    foreach (var item in oldImgs)
                    {
                        // delete from database
                        _dbContext.Images.Remove(item);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", item.ImageUrl);
                        //delete from server
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    foreach (var item in imgs)
                    {
                        //hjksfdjghdfsiuoydfsi.png
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                             item.CopyTo(stream);
                        }
                        newImgs.Add(fileName);
                    }
                    //save new images
                    foreach (var item in newImgs)
                    {
                        _dbContext.Images.Add(new() { ImageUrl = item, MovieId = movie.Id });
                    }
                }
                if (ActorsId.Any())
                {
                    var oldActors = _dbContext.ActorsMovies.Where(e => e.MovieId == movie.Id);
                    //delete ols actors
                    foreach (var item in oldActors)
                    {
                        _dbContext.ActorsMovies.Remove(item);
                    }
                    foreach (var item in ActorsId)
                    {
                        _dbContext.ActorsMovies.Add(new() { ActorId = item, MovieId = movie.Id });
                    }
                }
                _dbContext.SaveChanges();
                return RedirectToAction("index");
            }

            
            var categories = _dbContext.Categories.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var cinemas = _dbContext.Cinemas.Select(e => new SelectListItem()
            { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var actors = _dbContext.Actors.Select(e => new SelectListItem()
            { Text = e.FirstName + ' ' + e.LastName, Value = e.Id.ToString() }).ToList();

            ViewData["Cinema"] = _dbContext.Cinemas.ToList();


            return View(new MovieWithCategories_Cinemas_ActorsVM()
            {
                Actors = actors,
                Categories = categories,
                Cinemas = cinemas,
                Movie = movie
            });
        }
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.FirstOrDefault(e => e.Id == id);
            // delete old images 
            var oldImgs = _dbContext.Images.Where(e => e.MovieId == movie.Id).ToList();
            foreach (var item in oldImgs)
            {
                // delete from database
                _dbContext.Images.Remove(item);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", item.ImageUrl);
                //delete from server
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                

            }
            var oldActors = _dbContext.ActorsMovies.Where(e => e.MovieId == movie.Id);
            //delete ols actors
            foreach (var item in oldActors)
            {
                _dbContext.ActorsMovies.Remove(item);
            }
            _dbContext.Movies.Remove(movie);
            _dbContext.SaveChanges();
            return RedirectToAction("index");
        }

    }
}
