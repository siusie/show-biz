using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ActorAddViewModel
    {
        [Required, StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Alternate Name")]
        [StringLength(150)]
        public string AlternateName { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Height (m)")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [Range(1, 2.8, ErrorMessage = "Please enter a valid (human) height")]
        public decimal? Height { get; set; }

        [Required, StringLength(250)]
        [Display(Name = "Image URL")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [DataType(DataType.MultilineText)]
        public string Biography { get; set; }
    }
}