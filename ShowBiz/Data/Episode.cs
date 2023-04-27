using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Data
{
    public class Episode
    {
        public Episode()
        {
            SeasonNumber = 1;
            EpisodeNumber = 1;
            AirDate = DateTime.Now;
        }

        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int SeasonNumber { get; set; }

        [Range(1, int.MaxValue)]
        public int EpisodeNumber { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        [Required]
        public DateTime AirDate { get; set; }

        [Required, StringLength(250)]
        public string ImageUrl { get; set; }

        [Required, StringLength(250)]
        public string Clerk { get; set; }

        public int ShowId { get; set; }

        public string Premise { get; set; }

        // media item properties
        [StringLength(200)]
        public string VideoContentType { get; set; }

        public byte[] Video { get; set; }

        // navigation property
        [Required]
        public Show Show { get; set; }
    }
}