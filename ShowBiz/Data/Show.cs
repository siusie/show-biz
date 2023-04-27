﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Data
{
    public class Show
    {
        public Show()
        {
            ReleaseDate = DateTime.Now;
            Actors = new HashSet<Actor>();
            Episodes = new HashSet<Episode>();
        }

        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required, StringLength(250)]
        public string ImageUrl { get; set; }

        [Required, StringLength(250)]
        public string Coordinator { get; set; }

        public string Premise { get; set; }

        // entity collections
        public ICollection<Actor> Actors { get; set; }

        public ICollection<Episode> Episodes { get; set; }
    }
}