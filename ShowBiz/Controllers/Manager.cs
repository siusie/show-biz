using AutoMapper;
using ShowBiz.Data;
using ShowBiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ShowBiz.Controllers
{
    public class Manager
    {
        private ApplicationDbContext ds = new ApplicationDbContext();

        public IMapper mapper;

        private RequestUser _user;

        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        public Manager()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();
                cfg.CreateMap<Genre, GenreBaseViewModel>();

                cfg.CreateMap<Actor, ActorBaseViewModel>();
                cfg.CreateMap<Actor, ActorWithShowInfoViewModel>();
                cfg.CreateMap<ActorMediaItem, ActorMediaItemBaseViewModel>();
                cfg.CreateMap<ActorMediaItem, ActorMediaItemWithContentViewModel>();
                cfg.CreateMap<ActorAddViewModel, Actor>();

                cfg.CreateMap<Show, ShowBaseViewModel>();
                cfg.CreateMap<Show, ShowWithInfoViewModel>();
                cfg.CreateMap<ShowAddViewModel, Show>();

                cfg.CreateMap<Episode, EpisodeWithShowNameViewModel>();
                cfg.CreateMap<Episode, EpisodeVideoViewModel>();
                cfg.CreateMap<EpisodeAddViewModel, Episode>();
            });

            mapper = config.CreateMapper();
            ds.Configuration.ProxyCreationEnabled = false;
            ds.Configuration.LazyLoadingEnabled = false;
        }


        #region "Get all" use cases

        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(ds.Genres.OrderBy(genre => genre.Name));
        }

        public IEnumerable<ActorBaseViewModel> ActorGetAll()
        {
            return mapper.Map<IEnumerable<Actor>, IEnumerable<ActorBaseViewModel>>(ds.Actors.OrderBy(actor => actor.Name));
        }

        public IEnumerable<ShowBaseViewModel> ShowGetAll()
        {
            return mapper.Map<IEnumerable<Show>, IEnumerable<ShowBaseViewModel>>(ds.Shows.OrderBy(show => show.Name));
        }

        public IEnumerable<EpisodeWithShowNameViewModel> EpisodeGetAll()
        {
            var queryResults = ds.Episodes
                                 .Include("Show")
                                 .OrderBy(episode => episode.Show.Name)
                                 .ThenBy(episode => episode.SeasonNumber)
                                 .ThenBy(episode => episode.EpisodeNumber);

            return mapper.Map<IEnumerable<Episode>, IEnumerable<EpisodeWithShowNameViewModel>>(queryResults);
        }

        #endregion

        #region "Get one" use cases

        public ActorWithShowInfoViewModel ActorGetById(int id)
        {
            var actor = ds.Actors
                          .Include("Shows")
                          .Include("ActorMediaItems")
                          .SingleOrDefault(a => a.Id == id);

            return (actor == null) ? null : mapper.Map<Actor, ActorWithShowInfoViewModel>(actor);
        }

        public ShowWithInfoViewModel ShowGetById(int id)
        {
            var show = ds.Shows
                         .Include("Actors")
                         .Include("Episodes")
                         .SingleOrDefault(s => s.Id == id);

            return (show == null) ? null : mapper.Map<Show, ShowWithInfoViewModel>(show);
        }

        public EpisodeWithShowNameViewModel EpisodeGetById(int id)
        {
            var episode = ds.Episodes
                            .Include("Show")
                            .SingleOrDefault(e => e.Id == id);

            return (episode == null) ? null : mapper.Map<Episode, EpisodeWithShowNameViewModel>(episode);
        }

        public EpisodeVideoViewModel EpisodeVideoGetById(int id)
        {
            var video = ds.Episodes.Find(id);
            return (video == null) ? null : mapper.Map<Episode, EpisodeVideoViewModel>(video);
        }

        public ActorMediaItemWithContentViewModel ActorMediaItemGetById(int id)
        {
            var mediaItem = ds.ActorMediaItems.Find(id);
            return (mediaItem == null) ? null : mapper.Map<ActorMediaItem, ActorMediaItemWithContentViewModel>(mediaItem);
        }

        #endregion

        #region "Add new" use cases

        public ActorWithShowInfoViewModel ActorAdd(ActorAddViewModel newItem)
        {
            var addedItem = ds.Actors.Add(mapper.Map<ActorAddViewModel, Actor>(newItem));
            addedItem.Executive = HttpContext.Current.User.Identity.Name;
            ds.SaveChanges();
            return (addedItem == null) ? null : mapper.Map<Actor, ActorWithShowInfoViewModel>(addedItem);
        }

        public ShowWithInfoViewModel ShowAdd(ShowAddViewModel newItem)
        {
            // find the actor associated with this show
            var actor = ds.Actors.Find(newItem.ActorId);

            /* return null if:
             *      the associated actor doesn't exist;
             *      no actors have been selected
             */
            if (actor == null || newItem.ActorIds.Count() == 0) { return null; }
            else
            {
                // add the new show to the persistent store
                var addedItem = ds.Shows.Add(mapper.Map<ShowAddViewModel, Show>(newItem));

                // validate the incoming collection of actor identifiers
                foreach (var actorId in newItem.ActorIds)
                {
                    var a = ds.Actors.Find(actorId);
                    addedItem.Actors.Add(a);
                }

                addedItem.Coordinator = HttpContext.Current.User.Identity.Name;

                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Show, ShowWithInfoViewModel>(addedItem);
            }
        }

        public EpisodeWithShowNameViewModel EpisodeAdd(EpisodeAddViewModel newItem)
        {
            //  find the show associated with this episode
            var show = ds.Shows.Find(newItem.ShowId);

            if (show == null) { return null; }
            else
            {
                var addedItem = ds.Episodes.Add(mapper.Map<EpisodeAddViewModel, Episode>(newItem));

                // if a video (mp4 format) is included with the incoming object:
                if (newItem.VideoUpload.ContentType == "video/mp4")
                {
                    // extract the bytes from the HttpPostedFile object
                    byte[] videoBytes = new byte[newItem.VideoUpload.ContentLength];
                    newItem.VideoUpload.InputStream.Read(videoBytes, 0, newItem.VideoUpload.ContentLength);

                    // configure the new object's media item properties
                    addedItem.Video = videoBytes;
                    addedItem.VideoContentType = newItem.VideoUpload.ContentType;
                }

                // set the associated item property
                addedItem.Show = show;

                addedItem.Clerk = HttpContext.Current.User.Identity.Name;

                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Episode, EpisodeWithShowNameViewModel>(addedItem);
            }
        }

        public ActorWithShowInfoViewModel ActorMediaItemAdd(ActorMediaItemAddViewModel newItem)
        {
            // find the actor associated with this media item
            var actor = ds.Actors.Find(newItem.ActorId);

            // Boolean to determine whether the incoming media item is one of the following types:
            bool isValidFormat = (newItem.ContentUpload.ContentType == "image/png" ||
                                  newItem.ContentUpload.ContentType == "image/jpeg" ||
                                  newItem.ContentUpload.ContentType == "application/pdf" ||
                                  newItem.ContentUpload.ContentType == "audio/mpeg" ||
                                  newItem.ContentUpload.ContentType == "video/mp4");

            if (actor == null || !isValidFormat) { return null; }
            else
            {
                // create a media item object, add it to the db, and set its properties
                var addedItem = new ActorMediaItem();
                ds.ActorMediaItems.Add(addedItem);
                addedItem.Caption = newItem.Caption;
                addedItem.Actor = actor;

                // handle the uploaded media item
                byte[] mediaItemBytes = new byte[newItem.ContentUpload.ContentLength];
                newItem.ContentUpload.InputStream.Read(mediaItemBytes, 0, newItem.ContentUpload.ContentLength);

                // configure the new object's properties
                addedItem.Content = mediaItemBytes;
                addedItem.ContentType = newItem.ContentUpload.ContentType;

                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Actor, ActorWithShowInfoViewModel>(actor);
            }
        }

        #endregion


        #region Role Claims

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        #endregion

        #region Load Data Methods

        public bool LoadRoles()
        {
            var user = HttpContext.Current.User.Identity.Name;

            var done = false;

            if (ds.RoleClaims.Count() == 0)
            {
                ds.RoleClaims.Add(new RoleClaim { Name = "Admin" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool LoadGenres()
        {
            var done = false;

            if (ds.Genres.Count() == 0)
            {
                ds.Genres.Add(new Genre { Name = "Comedy" });
                ds.Genres.Add(new Genre { Name = "Sci-Fi" });
                ds.Genres.Add(new Genre { Name = "Horror" });
                ds.Genres.Add(new Genre { Name = "Romance" });
                ds.Genres.Add(new Genre { Name = "Action" });
                ds.Genres.Add(new Genre { Name = "Thriller" });
                ds.Genres.Add(new Genre { Name = "Drama" });
                ds.Genres.Add(new Genre { Name = "Mystery" });
                ds.Genres.Add(new Genre { Name = "Crime" });
                ds.Genres.Add(new Genre { Name = "Animation" });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool LoadActors()
        {
            var done = false;

            if (ds.Actors.Count() == 0)
            {
                ds.Actors.Add(new Actor
                {
                    Name = "Ian McKellen",
                    BirthDate = new DateTime(1939, 5, 25),
                    Height = 1.83M,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/SDCC13_-_Ian_McKellen.jpg/800px-SDCC13_-_Ian_McKellen.jpg",
                    Executive = HttpContext.Current.User.Identity.Name
                });
                ds.Actors.Add(new Actor
                {
                    Name = "Keanu Reeves",
                    BirthDate = new DateTime(1964, 9, 2),
                    Height = 1.86M,
                    ImageUrl = "https://m.media-amazon.com/images/M/MV5BNGJmMWEzOGQtMWZkNS00MGNiLTk5NGEtYzg1YzAyZTgzZTZmXkEyXkFqcGdeQXVyMTE1MTYxNDAw._V1_.jpg",
                    Executive = HttpContext.Current.User.Identity.Name
                });
                ds.Actors.Add(new Actor
                {
                    Name = "Sacha Baron Cohen",
                    BirthDate = new DateTime(1971, 10, 13),
                    Height = 1.91M,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Sacha_Baron_Cohen%2C_2011.jpg/800px-Sacha_Baron_Cohen%2C_2011.jpg",
                    Executive = HttpContext.Current.User.Identity.Name
                });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool LoadShows()
        {
            var done = false;

            if (ds.Shows.Count() == 0)
            {
                var sbc = ds.Actors.SingleOrDefault(actor => actor.Name == "Sacha Baron Cohen");
                if (sbc != null)
                {
                    // add first show
                    ds.Shows.Add(new Show
                    {
                        Actors = new Actor[] { sbc },
                        Name = "Da Ali G Show",
                        Genre = "Comedy",
                        ReleaseDate = new DateTime(2000, 3, 30),
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/47/Da_Ali_G_Show_logo.png",
                        Coordinator = HttpContext.Current.User.Identity.Name
                    });

                    // add second show
                    ds.Shows.Add(new Show
                    {
                        Actors = new Actor[] { sbc },
                        Name = "Who Is America?",
                        Genre = "Comedy",
                        ReleaseDate = new DateTime(2018, 7, 15),
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/21/Who_Is_America_Title.svg/1920px-Who_Is_America_Title.svg.png",
                        Coordinator = HttpContext.Current.User.Identity.Name
                    });

                    ds.SaveChanges();
                    done = true;
                }
            }

            return done;
        }

        public bool LoadEpisodes()
        {
            var done = false;

            if (ds.Episodes.Count() == 0)
            {
                var daAliGShow = ds.Shows.SingleOrDefault(s => s.Name == "Da Ali G Show");
                var whoIsAmerica = ds.Shows.SingleOrDefault(s => s.Name == "Who Is America?");

                if (daAliGShow != null && whoIsAmerica != null)
                {
                    ds.Episodes.Add(new Episode
                    {
                        Show = daAliGShow,
                        Name = "Neil Hamilton",
                        SeasonNumber = 1,
                        EpisodeNumber = 1,
                        Genre = daAliGShow.Genre,
                        AirDate = new DateTime(2000, 3, 30),
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BYWZiNWM4MDYtZWFkMy00ZGFmLTk2NWYtNTM2ZTU3NjVmODU1XkEyXkFqcGdeQXVyNzk2MzgzODU@._V1_.jpg",
                        Clerk = HttpContext.Current.User.Identity.Name
                    });

                    ds.Episodes.Add(new Episode
                    {
                        Show = daAliGShow,
                        Name = "Mohamed Al-Fayed",
                        SeasonNumber = 1,
                        EpisodeNumber = 2,
                        Genre = daAliGShow.Genre,
                        AirDate = new DateTime(2000, 4, 7),
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BYTNjMTA4ODEtZWUyMi00OWYxLWE5YTktZjhkNWQyN2Y2ZmI3XkEyXkFqcGdeQXVyNzk2MzgzODU@._V1_.jpg",
                        Clerk = HttpContext.Current.User.Identity.Name
                    });

                    ds.Episodes.Add(new Episode
                    {
                        Show = daAliGShow,
                        Name = "Gail Porter",
                        SeasonNumber = 1,
                        EpisodeNumber = 3,
                        Genre = daAliGShow.Genre,
                        AirDate = new DateTime(2000, 4, 14),
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BNmY3ZTlkNDctM2I4OC00ODA3LTk3OTQtNzUzN2VkZWQwMWFlXkEyXkFqcGdeQXVyNTM3MDMyMDQ@._V1_.jpg",
                        Clerk = HttpContext.Current.User.Identity.Name
                    });

                    ds.Episodes.Add(new Episode
                    {
                        Show = whoIsAmerica,
                        Name = "Episode #1.1",
                        SeasonNumber = 1,
                        EpisodeNumber = 1,
                        Genre = whoIsAmerica.Genre,
                        AirDate = new DateTime(2018, 7, 15),
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BNDgyYTcxNzMtY2MxYi00NGY4LTk4YjktZWM5YWFmMzdkMjhmXkEyXkFqcGdeQXVyNTQ0NjQzNTE@._V1_.jpg",
                        Clerk = HttpContext.Current.User.Identity.Name
                    });

                    ds.Episodes.Add(new Episode
                    {
                        Show = whoIsAmerica,
                        Name = "Episode #1.2",
                        SeasonNumber = 1,
                        EpisodeNumber = 2,
                        Genre = whoIsAmerica.Genre,
                        AirDate = new DateTime(2018, 7, 22),
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BMDJmYmI2YzEtMDZiZC00MWJmLTkzNGUtYmU4MDRjYjIwNzU1XkEyXkFqcGdeQXVyNTQ0NjQzNTE@._V1_.jpg",
                        Clerk = HttpContext.Current.User.Identity.Name
                    });

                    ds.Episodes.Add(new Episode
                    {
                        Show = whoIsAmerica,
                        Name = "Episode #1.3",
                        SeasonNumber = 1,
                        EpisodeNumber = 3,
                        Genre = whoIsAmerica.Genre,
                        AirDate = new DateTime(2018, 7, 29),
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BZjY2YmViNjgtMzRiYy00ZDVmLTk0MDItODFiNGY1MDY3NWE2XkEyXkFqcGdeQXVyNDE2OTU4Njk@._V1_.jpg",
                        Clerk = HttpContext.Current.User.Identity.Name
                    });

                    ds.SaveChanges();
                    done = true;
                }
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveActors()
        {
            var removed = false;
            foreach (var e in ds.Actors)
            {
                ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                removed = true;
            }
            ds.SaveChanges();
            return removed;
        }

        public bool RemoveShows()
        {
            var removed = false;
            foreach (var e in ds.Shows)
            {
                ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                removed = true;
            }
            ds.SaveChanges();
            return removed;
        }

        public bool RemoveEpisodes()
        {
            var removed = false;

            foreach (var e in ds.Episodes)
            {
                ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                removed = true;
            }
            ds.SaveChanges();
            return removed;
        }

    }

    #endregion

    #region RequestUser Class

    // This "RequestUser" class includes many convenient members that make it
    // easier to work with the authenticated user and render user account info.
    // Study the properties and methods, and think about how you could use this class.

    // How to use...
    // In the Manager class, declare a new property named User:
    //    public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value:
    //    User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }

        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }

        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }

        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

    #endregion

}