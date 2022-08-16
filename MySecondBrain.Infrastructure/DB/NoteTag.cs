using System;
using System.Collections.Generic;

#nullable disable

namespace MySecondBrain.Infrastructure.DB
{
    public partial class NoteTag
    {
        public int Idnote { get; set; }
        public int Idtag { get; set; }

        public virtual Note IdnoteNavigation { get; set; }
        public virtual Tag IdtagNavigation { get; set; }
    }
}
