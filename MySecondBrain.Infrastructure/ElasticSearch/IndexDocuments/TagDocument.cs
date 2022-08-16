using MySecondBrain.Infrastructure.DB;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Infrastructure.ElasticSearch.IndexDocuments
{
    [ElasticsearchType(IdProperty = nameof(TagDocument.TagId))]
    public class TagDocument
    {
        [Keyword(Name = nameof(TagId))]
        public int TagId { get; set; }

        [Text(Name = nameof(TagName))]
        public string TagName { get; set; }

        [Keyword(Name = nameof(TagUserId))]
        public string TagUserId { get; set; }


    }
}