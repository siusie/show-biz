using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class EpisodeBaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Season")]
        public int SeasonNumber { get; set; }

        [Display(Name = "Episode")]
        public int EpisodeNumber { get; set; }

        public string Genre { get; set; }

        [Display(Name = "Date Aired"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AirDate { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Added By")]
        public string Clerk { get; set; }

        [DataType(DataType.MultilineText)]
        public string Premise { get; set; }
    }
}