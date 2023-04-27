using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ActorMediaItemAddFormViewModel
    {
        public int ActorId { get; set; }

        public string ActorName { get; set; }

        [Required, StringLength(100)]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Attachment")]
        [DataType(DataType.Upload)]
        public string ContentUpload { get; set; }
    }
}