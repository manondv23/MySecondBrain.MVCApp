using System;
using System.Collections.Generic;

#nullable disable

namespace MySecondBrain.MVCApp
{
    public partial class Note
    {
        public Note()
        {
            NoteTags = new HashSet<NoteTag>();
        }

        public int Idnote { get; set; }
        public int Iddossier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContenuMarkdown { get; set; }
        public DateTime? DateCreation { get; set; }
        public string UserId { get; set; }
        public string Author { get; set; }

        public virtual Dossier IddossierNavigation { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual ICollection<NoteTag> NoteTags { get; set; }
    }
}
