using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ShowWithInfoViewModel : ShowBaseViewModel
    {
        public ShowWithInfoViewModel()
        {
            Actors = new List<ActorBaseViewModel>();
            Episodes = new List<EpisodeWithShowNameViewModel>();
        }

        [Display(Name = "Cast")]
        public IEnumerable<ActorBaseViewModel> Actors { get; set; }

        [Display(Name = "Episodes")]
        public IEnumerable<EpisodeWithShowNameViewModel> Episodes { get; set; }
    }
}