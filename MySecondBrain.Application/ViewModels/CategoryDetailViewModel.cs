using System;
using System.Collections.Generic;
using System.Text;

namespace MySecondBrain.Application.ViewModels
{
    public class CategoryDetailViewModel
    {
        public Infrastructure.DB.Dossier Dossier { get; set; }

        public List<Infrastructure.DB.Dossier> ListDossiers { get; set; }



    }
}
