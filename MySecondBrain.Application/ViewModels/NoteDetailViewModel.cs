using System;
using System.Collections.Generic;
using System.Text;

namespace MySecondBrain.Application.ViewModels
{
    public class NoteDetailViewModel
    {
        public Infrastructure.DB.Note Note { get; set; }

        public IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> Notes { get; set; }
        public List<Infrastructure.DB.Dossier> ListDossiers { get; set; }

        public string newDossierName { get; set; }

        public string newTagName { get; set; }

        public List<Infrastructure.DB.Tag> Tags { get; set; }

        public string dossier { get; set; }

        public List<int> CheckedTags { get; set; }

        public List<Infrastructure.DB.Tag> TagsOfNote { get; set; }

    }

    
}
