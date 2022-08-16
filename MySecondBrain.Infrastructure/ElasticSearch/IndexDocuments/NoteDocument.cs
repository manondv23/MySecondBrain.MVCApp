using MySecondBrain.Infrastructure.DB;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Infrastructure.ElasticSearch.IndexDocuments
{
    [ElasticsearchType(IdProperty = nameof(NoteDocument.NoteId))]
    public class NoteDocument
    {
        [Keyword(Name = nameof(NoteId))]
        public int NoteId { get; set; }

        [Text(Name = nameof(NoteTitle))]
        public string NoteTitle { get; set; }

        [Text(Name = nameof(NoteDescription))]
        public string NoteDescription { get; set; }

        [Text(Name = nameof(NoteContenuMarkdown))]
        public string NoteContenuMarkdown { get; set; }

        [Keyword(Name = nameof(NoteDossierId))]
        public int NoteDossierId { get; set; }

        [Date(Name = nameof(NoteDateCreation))]
        public DateTime NoteDateCreation { get; set; }

        [Keyword(Name = nameof(NoteUserId))]
        public string NoteUserId { get; set; }


    }
}