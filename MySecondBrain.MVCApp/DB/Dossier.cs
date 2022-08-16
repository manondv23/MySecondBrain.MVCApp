using System;
using System.Collections.Generic;

#nullable disable

namespace MySecondBrain.MVCApp
{
    public partial class Dossier
    {
        public Dossier()
        {
            InverseIddossierParentNavigation = new HashSet<Dossier>();
            Notes = new HashSet<Note>();
        }

        public int Iddossier { get; set; }
        public string Nom { get; set; }
        public int? IddossierParent { get; set; }
        public string UserId { get; set; }

        public virtual Dossier IddossierParentNavigation { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual ICollection<Dossier> InverseIddossierParentNavigation { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
