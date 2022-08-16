using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecondBrain.Application.ViewModels
{
    public class TagDetailViewModel
    {

        public Infrastructure.DB.Tag Tag { get; set; }

        public IEnumerable<Infrastructure.ElasticSearch.IndexDocuments.TagDocument> Tags { get; set; }
    }
}
