using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ShowAddViewModel
    {
        public ShowAddViewModel()
        {
            ReleaseDate = DateTime.Now;
            ActorIds = new List<int>();
        }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required, StringLength(250)]
        [Display(Name = "Image")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        [DataType(DataType.MultilineText)]
        public string Premise { get; set; }

        // identifier of associated actor
        [Range(1, Int32.MaxValue)]
        public int ActorId { get; set; }

        public IEnumerable<int> ActorIds { get; set; }
    }
}