using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Data
{
    public class ActorMediaItem
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string ContentType { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required, StringLength(100)]
        public string Caption { get; set; }

        [Required]
        public Actor Actor { get; set; }
    }
}