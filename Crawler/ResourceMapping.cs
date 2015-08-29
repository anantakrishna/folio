using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAzure.Table.EntityConverters.TypeData;

namespace Folio
{
    public class ResourceMapping : EntityTypeMap<Resource>
    {
        public ResourceMapping()
        {
            this.PartitionKey(r => r.Source)
                .RowKey(r => r.Id);
        }
    }
}
