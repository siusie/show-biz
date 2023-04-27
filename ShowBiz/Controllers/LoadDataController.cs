using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowBiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoadDataController : Controller
    {
        // Reference to the manager object
        Manager m = new Manager();

        [AllowAnonymous]
        public ActionResult Roles()
        {
            ViewBag.Result = m.LoadRoles() ? "Role data was loaded" : "Role data has already been loaded!";
            return View("result");
        }

        public ActionResult Genres()
        {
            ViewBag.Result = m.LoadGenres() ? "Genre data was loaded" : "Genre data has already been loaded!";
            return View("result");
        }

        public ActionResult Actors()
        {
            ViewBag.Result = m.LoadActors() ? "Actor data was loaded" : "Actor data has already been loaded!";
            return View("result");
        }

        public ActionResult Shows()
        {
            ViewBag.Result = m.LoadShows() ? "Show data was loaded" : "Show data has already been loaded!";
            return View("result");
        }

        public ActionResult Episodes()
        {
            ViewBag.Result = m.LoadEpisodes() ? "Episode data was loaded" : "Episode data has already been loaded!";
            return View("result");
        }

        public ActionResult Remove()
        {
            if (m.RemoveData())
            {
                return Content("data has been removed");
            }
            else
            {
                return Content("could not remove data");
            }
        }

        public ActionResult RemoveDatabase()
        {
            if (m.RemoveDatabase())
            {
                return Content("database has been removed");
            }
            else
            {
                return Content("could not remove database");
            }
        }

        public ActionResult RemoveActors()
        {
            ViewBag.Result = m.RemoveActors() ? "Actor data was removed" : "(done)";
            return View("result");
        }

        public ActionResult RemoveShows()
        {
            ViewBag.Result = m.RemoveShows() ? "Show data was removed" : "(done)";
            return View("result");
        }

        public ActionResult RemoveEpisodes()
        {
            ViewBag.Result = m.RemoveEpisodes() ? "Episode data was removed" : "(done)";
            return View("result");
        }

    }
}