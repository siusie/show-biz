using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShowBiz.Data
{
    public class Actor
    {
        public Actor()
        {
            Shows = new HashSet<Show>();
            ActorMediaItems = new HashSet<ActorMediaItem>();
        }

        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string AlternateName { get; set; }

        public DateTime? BirthDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Height { get; set; }

        [Required, StringLength(250)]
        public string ImageUrl { get; set; }

        [Required, StringLength(250)]
        public string Executive { get; set; }

        public string Biography { get; set; }

        // entity collections
        public ICollection<Show> Shows { get; set; }

        public ICollection<ActorMediaItem> ActorMediaItems { get; set; }
    }
}