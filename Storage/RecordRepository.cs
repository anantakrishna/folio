using Lucene.Net.Linq;
using Lucene.Net.Linq.Fluent;
using Lucene.Net.Linq.Mapping;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Folio.Storage
{
    public class RecordRepository : IDisposable, IResourceRepository
    {
        private readonly LuceneDataProvider provider;
        private readonly IDocumentMapper<Resource> mapper;

        private const Lucene.Net.Util.Version LuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

        public RecordRepository(Directory directory)
        {
            this.provider = new LuceneDataProvider(directory, LuceneVersion);
            this.mapper = CreateMap().ToDocumentMapper();
        }

        public void Dispose()
        {
            provider.Dispose();
        }

        public void Add(IEnumerable<Resource> records)
        {
            using (var session = provider.OpenSession<Resource>(mapper))
            {
                session.Add(records.ToArray());
            }
        }

        public IQueryable<Resource> Query
        {
            get
            {
                return provider.AsQueryable(mapper);
            }
            
        }

        public IQueryable<Resource> ByDateTag(DateTag tag, DateTag.Matching matching)
        {
            var term = new Lucene.Net.Index.Term("DateTag", tag);
            Query query;
            switch (matching)
            {
                case DateTag.Matching.Exact:
                    query = new TermQuery(term);
                    break;

                case DateTag.Matching.Inclusive:
                default:
                    query = new PrefixQuery(term);
                    break;
            }
            return provider.AsQueryable(mapper).Where(query);
        }

        private static ClassMap<Resource> CreateMap()
        {
            var map = new ClassMap<Resource>(LuceneVersion);
            map.Key(r => r.Url).NotAnalyzedNoNorms();
            map.Property(r => r.Source).NotAnalyzedNoNorms();
            map.Property(r => r.Title);
            map.Property(r => r.Description);
            map.Property(r => r.Type).NotAnalyzedNoNorms();
            map.Property(r => r.PrimaryLanguage).NotAnalyzedNoNorms();
            map.Property(r => r.PrimaryLanguageType).NotAnalyzedNoNorms();
            map.Property(r => r.SecondaryLanguage).NotAnalyzedNoNorms();
            map.Property(r => r.SecondaryLanguageType).NotAnalyzedNoNorms();
            map.Property(r => r.DateTags).ToField("DateTag");
            map.Property(r => r.Text).Analyzed().Stored().WithTermVector.PositionsAndOffsets();
            return map;
        }
    }
}
