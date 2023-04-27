using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowBiz.Models
{
    public class ShowAddFormViewModel : ShowAddViewModel
    {
        [Display(Name = "Genre")]
        public SelectList GenreList { get; set; }

        [Display(Name = "Actors")]
        public MultiSelectList ActorList { get; set; }

        // adding a show with known context - name of the associated actor
        [Display(Name = "Name")]
        public string ActorName { get; set; }
    }
}