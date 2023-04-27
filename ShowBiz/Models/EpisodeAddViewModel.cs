using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class EpisodeAddViewModel
    {
        public string Name { get; set; }

        public int SeasonNumber { get; set; }

        public int EpisodeNumber { get; set; }

        public DateTime AirDate { get; set; }

        public string ImageUrl { get; set; }

        public string Premise { get; set; }

        // identifier of associated show
        public int ShowId { get; set; }

        public string Genre { get; set; }

        public HttpPostedFileBase VideoUpload { get; set; }
    }
}