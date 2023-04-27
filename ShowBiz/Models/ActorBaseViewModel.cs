using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ActorBaseViewModel : ActorAddViewModel
    {
        public int Id { get; set; }
        
        [Display(Name="Added By")]
        public string Executive { get; set; }
    }
}