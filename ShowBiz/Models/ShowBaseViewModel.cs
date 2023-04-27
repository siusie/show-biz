using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ShowBaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required, StringLength(250)]
        [Display(Name = "Added By")]
        public string Coordinator { get; set; }

        [DataType(DataType.MultilineText)]
        public string Premise { get; set; }
    }
}