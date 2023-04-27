using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowBiz.Controllers
{
    public class EpisodesController : Controller
    {
        private Manager m = new Manager();

        // GET: Episodes
        public ActionResult Index()
        {
            return View(m.EpisodeGetAll());
        }

        // GET: Episodes/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.EpisodeGetById(id.GetValueOrDefault());
            if (obj == null) return null;
            else return View(obj);
        }

        // GET: /Episodes/Video/{id}
        [Route("Episodes/Video/{id}")]
        public ActionResult Video(int? id)
        {
            var video = m.EpisodeVideoGetById(id.GetValueOrDefault());
            if (video == null) return null;
            else return File(video.Video, video.VideoContentType);
        }
    }
}