using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_ticket514.Models.ViewModels
{
    public class MovieWithCategories_Cinemas_ActorsVM
    {
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Cinemas { get; set; }
        public List<SelectListItem> Actors { get; set; }
        public Movie Movie { get; set; }
    }
}
