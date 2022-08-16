using System;
using System.Collections.Generic;

#nullable disable

namespace MySecondBrain.Infrastructure.DB
{
    public partial class Tag
    {
        public Tag()
        {
            NoteTags = new HashSet<NoteTag>();
        }

        public int Idtag { get; set; }
        public string Nom { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<NoteTag> NoteTags { get; set; }

      
    }
}
