using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShowBiz.Models
{
    public class ActorMediaItemWithContentViewModel : ActorMediaItemBaseViewModel
    {
        public byte[] Content { get; set; }
    }
}