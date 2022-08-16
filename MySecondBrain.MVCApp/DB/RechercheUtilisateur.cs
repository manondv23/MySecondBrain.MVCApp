using System;
using System.Collections.Generic;

#nullable disable

namespace MySecondBrain.MVCApp
{
    public partial class RechercheUtilisateur
    {
        public int IdrechercheUtilisateur { get; set; }
        public string UserId { get; set; }
        public string MotsCles { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}
