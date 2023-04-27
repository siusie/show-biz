using ShowBiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowBiz.Controllers
{
    public class ActorsController : Controller
    {
        private Manager m = new Manager();

        // GET: Actors
        public ActionResult Index()
        {
            return View(m.ActorGetAll());
        }

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.ActorGetById(id.GetValueOrDefault());
            var photos = new List<ActorMediaItemBaseViewModel>();
            var documents = new List<ActorMediaItemBaseViewModel>();
            var audio = new List<ActorMediaItemBaseViewModel>();
            var videos = new List<ActorMediaItemBaseViewModel>();

            if (obj == null) return HttpNotFound();
            else
            {
                if (obj.ActorMediaItems != null)
                {
                    foreach (var mediaItem in obj.ActorMediaItems)
                    {
                        switch (mediaItem.ContentType)
                        {
                            case "image/jpeg":
                            case "image/png":
                                photos.Add(mediaItem);
                                break;
                            case "application/pdf":
                                documents.Add(mediaItem);
                                break;
                            case "audio/mpeg":
                                audio.Add(mediaItem);
                                break;
                            case "video/mp4":
                                videos.Add(mediaItem);
                                break;
                        }
                    }

                    obj.Photos = photos.OrderBy(p => p.Caption);
                    obj.Documents = documents.OrderBy(d => d.Caption);
                    obj.AudioClips = audio.OrderBy(a => a.Caption);
                    obj.VideoClips = videos.OrderBy(v => v.Caption);

                }

                return View(obj);
            }
        }

        // GET: Actors/Create
        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            return View(new ActorAddViewModel());
        }

        // POST: Actors/Create
        [HttpPost]
        [Authorize(Roles = "Executive")]
        [ValidateInput(false)]
        public ActionResult Create(ActorAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            try
            {
                var addedItem = m.ActorAdd(newItem);
                if (addedItem == null) return View(newItem);
                else return RedirectToAction("Details", new { id = addedItem.Id });
            }
            catch
            {
                return View(newItem);
            }
        }

        // GET: Actors/{id}/AddShow
        [Authorize(Roles = "Coordinator")]
        [Route("Actors/{id}/AddShow")]
        public ActionResult AddShow(int? id)
        {
            // locate the associated actor
            var actor = m.ActorGetById(id.GetValueOrDefault());

            if (actor == null) { return HttpNotFound(); }
            else
            {
                var form = new ShowAddFormViewModel();

                form.ActorId = actor.Id;
                form.ActorName = actor.Name;
                form.GenreList = new SelectList(m.GenreGetAll().Select(genre => genre.Name));
                form.ActorList = new MultiSelectList(
                    items: m.ActorGetAll(),
                    dataValueField: "Id",
                    dataTextField: "Name",
                    selectedValues: new List<int> { actor.Id }
                    );

                return View(form);
            }
        }

        // POST: Actors/{id}/AddShow
        [Authorize(Roles = "Coordinator")]
        [Route("Actors/{id}/AddShow")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddShow(ShowAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            try
            {
                var addedItem = m.ShowAdd(newItem);
                if (addedItem == null) return RedirectToAction("AddShow", new { id = newItem.ActorId });
                else return RedirectToAction("Details", "Shows", new { id = addedItem.Id });
            }
            catch
            {
                return RedirectToAction("AddShow", new { id = newItem.ActorId });
            }
        }

        // GET: Actors/5/AddMediaItem
        [Authorize(Roles = "Executive")]
        [Route("Actors/{id}/AddMediaItem")]
        public ActionResult AddMediaItem(int? id)
        {
            // locate the associated actor
            var actor = m.ActorGetById(id.GetValueOrDefault());

            if (actor == null) { return HttpNotFound(); }
            else
            {
                var form = new ActorMediaItemAddFormViewModel();
                form.ActorId = actor.Id;
                form.ActorName = actor.Name;
                return View(form);
            }
        }

        // POST: Actors/5/AddMediaItem
        [HttpPost]
        [Authorize(Roles = "Executive")]
        [Route("Actors/{id}/AddMediaItem")]
        public ActionResult AddMediaItem(int? id, ActorMediaItemAddViewModel newItem)
        {
            if (!ModelState.IsValid && id.GetValueOrDefault() == newItem.ActorId)
            {
                return View(newItem);
            }

            try
            {
                var addedItem = m.ActorMediaItemAdd(newItem);
                if (addedItem == null) return RedirectToAction("Details", new { id = addedItem.Id });
                else return RedirectToAction("Details", new { id = addedItem.Id });
            }
            catch
            {
                return RedirectToAction("AddMediaItem", new { id = newItem.ActorId });
            }
        }

        [Route("Actors/MediaItem/{id}")]
        public ActionResult MediaItemDownload(int? id)
        {
            var mediaItem = m.ActorMediaItemGetById(id.GetValueOrDefault());

            if (mediaItem == null) { return HttpNotFound(); }
            else
            {
                if (mediaItem.ContentType != "application/pdf")
                {
                    return File(mediaItem.Content, mediaItem.ContentType);
                }
                else
                {
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        // Assemble the file name + extension
                        FileName = $"actor-document.pdf",
                        // Force the media item to be saved (not viewed)
                        Inline = false
                    };
                    // Add the content disposition header to the response
                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return File(mediaItem.Content, mediaItem.ContentType);
                }
            }
        }
    }
}