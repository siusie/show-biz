using ShowBiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowBiz.Controllers
{
    public class ShowsController : Controller
    {
        private Manager m = new Manager();

        // GET: Shows
        public ActionResult Index()
        {
            return View(m.ShowGetAll());
        }

        // GET: Shows/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.ShowGetById(id.GetValueOrDefault());
            if (obj == null) return null;
            else return View(obj);
        }

        // GET: Shows/{id}/AddEpisode
        [Authorize(Roles = "Clerk")]
        [Route("Shows/{id}/AddEpisode")]
        public ActionResult AddEpisode(int? id)
        {
            // attempt to fetch the associated show
            var show = m.ShowGetById(id.GetValueOrDefault());

            if (show == null) { return HttpNotFound(); }
            else
            {
                var form = new EpisodeAddFormViewModel();

                form.ShowId = show.Id;
                form.ShowName = show.Name;
                form.GenreList = new SelectList(m.GenreGetAll().Select(genre => genre.Name));

                return View(form);
            }
        }

        // POST: Shows/{id}/AddEpisode
        [Authorize(Roles = "Clerk")]
        [Route("Shows/{id}/AddEpisode")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddEpisode(EpisodeAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            try
            {
                var addedItem = m.EpisodeAdd(newItem);
                if (addedItem == null) return View(newItem);
                else return RedirectToAction("Details", "Episodes", new { id = addedItem.Id });
            }
            catch
            {
                return View(newItem);
            }
        }
    }
}