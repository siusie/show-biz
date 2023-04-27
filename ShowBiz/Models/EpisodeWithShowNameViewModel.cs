using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class EpisodeWithShowNameViewModel : EpisodeBaseViewModel
    {
        [Display(Name = "Show Name")]
        public string ShowName { get; set; }

        [Display(Name = "Video")]
        public string VideoContentType { get; set; }
    }
}