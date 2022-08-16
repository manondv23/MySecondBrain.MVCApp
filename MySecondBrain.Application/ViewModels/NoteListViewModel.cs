using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySecondBrain.Application.ViewModels;

namespace MySecondBrain.Application.ViewModels
{
    public class NoteListViewModel
    {

        public IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> Notes { get; set; }
        public string query { get; set; }

        public List<Infrastructure.DB.Dossier> ListDossiers { get; set; }

        public List<Infrastructure.DB.Tag> Tags { get; set; }

        public PaginatedList<Infrastructure.ElasticSearch.IndexDocuments.NoteDocument> PaginatedNotes { get; set; }

        public string dossier { get; set; }

        public string CustomTitle { get; set; }
        public int NotesCount
        {
            get
            {
                return Notes.Count();
            }
        }

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

    }

    public class PaginatedList<T> : List<T>
    {

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }

    }

    }
