using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowBiz.Models
{
    public class EpisodeAddFormViewModel
    {
        public EpisodeAddFormViewModel()
        {
            SeasonNumber = 1;
            EpisodeNumber = 1;
            AirDate = DateTime.Now;
        }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Season")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please enter a valid number")]
        public int SeasonNumber { get; set; }

        [Display(Name = "Episode")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please enter a valid number")]
        public int EpisodeNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Aired")]
        public DateTime AirDate { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Image")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [DataType(DataType.MultilineText)]
        public string Premise { get; set; }

        [Display(Name = "Genre")]
        public SelectList GenreList { get; set; }

        [Display(Name = "Video Attachment")]
        [DataType(DataType.Upload)]
        public string VideoUpload { get; set; }

        // identifier of associated show
        [Range(1, Int32.MaxValue)]
        public int ShowId { get; set; }

        //name of associated show
        public string ShowName { get; set; }
    }
}