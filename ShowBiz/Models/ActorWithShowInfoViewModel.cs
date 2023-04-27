using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ActorWithShowInfoViewModel : ActorBaseViewModel
    {
        public ActorWithShowInfoViewModel()
        {
            Shows = new List<ShowBaseViewModel>();
            ActorMediaItems = new List<ActorMediaItemBaseViewModel>();
            Photos = new List<ActorMediaItemBaseViewModel>();
            Documents = new List<ActorMediaItemBaseViewModel>();
            AudioClips = new List<ActorMediaItemBaseViewModel>();
            VideoClips = new List<ActorMediaItemBaseViewModel>();
        }

        [Display(Name = "Appeared In")]
        public IEnumerable<ShowBaseViewModel> Shows { get; set; }

        public IEnumerable<ActorMediaItemBaseViewModel> ActorMediaItems { get; set; }

        public IEnumerable<ActorMediaItemBaseViewModel> Photos { get; set; }

        public IEnumerable<ActorMediaItemBaseViewModel> Documents { get; set; }

        [Display(Name = "Audio Clips")]
        public IEnumerable<ActorMediaItemBaseViewModel> AudioClips { get; set; }

        [Display(Name = "Video Clips")]
        public IEnumerable<ActorMediaItemBaseViewModel> VideoClips { get; set; }
    }
}