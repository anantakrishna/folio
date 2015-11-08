using Lucene.Net.Analysis;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Fluent;
using Lucene.Net.Linq.Mapping;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio.Storage
{
    public class RecordRepository : IDisposable
    {
        private readonly LuceneDataProvider provider;
        private readonly IDocumentMapper<Resource> mapper;

        private const Lucene.Net.Util.Version LuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

        public RecordRepository()
            : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData, Environment.SpecialFolderOption.Create), "PureBhakti", "Folio", "records"))
        {
        }

        public RecordRepository(string path)
        {
            this.provider = new LuceneDataProvider(FSDirectory.Open(path), LuceneVersion);
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

        private static ClassMap<Resource> CreateMap()
        {
            var map = new ClassMap<Resource>(LuceneVersion);
            map.Key(r => r.Url).NotAnalyzed();
            map.Property(r => r.Source).NotAnalyzedNoNorms();
            map.Property(r => r.Title);
            map.Property(r => r.Description);
            map.Property(r => r.Type).AnalyzeWith(new KeywordAnalyzer());
            map.Property(r => r.PrimaryLanguage).NotAnalyzedNoNorms();
            map.Property(r => r.PrimaryLanguageType).NotAnalyzedNoNorms();
            map.Property(r => r.SecondaryLanguage).NotAnalyzedNoNorms();
            map.Property(r => r.SecondaryLanguageType).NotAnalyzedNoNorms();
            return map;
        }
    }
}
