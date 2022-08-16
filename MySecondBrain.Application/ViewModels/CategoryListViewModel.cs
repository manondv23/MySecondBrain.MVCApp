using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Application.ViewModels
{
    public class CategoryListViewModel
    {

        public IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> Notes { get; set; }

        public List<Infrastructure.DB.Dossier> ListDossiers { get; set; }


        public List<Infrastructure.DB.Tag> Tags { get; set; }

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
                return Tags.Count;
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
