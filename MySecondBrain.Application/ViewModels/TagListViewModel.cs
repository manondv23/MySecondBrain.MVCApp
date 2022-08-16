using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Application.ViewModels
{
    public class TagListViewModel
    {
        public List<Infrastructure.DB.Dossier> ListDossiers { get; set; }

        public IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> Notes { get; set; }

        public IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.TagDocument> Tags { get; set; }

        public string query { get; set; }

        public string CustomTitle { get; set; }

        public int CategoriesCount
        {
            get
            {
                return ListDossiers.Count;
            }
        }

        public int TagsCount
        {
            get
            {
                return Tags.ToList().Count;
            }
        }

        public int NotesCount
        {
            get
            {
                return Notes.ToList().Count;
            }
        }
    }
}
