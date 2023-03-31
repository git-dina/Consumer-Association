using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class ItemGeneralization
    {
        public long Id { get; set; }
        public Nullable<long> ItemId { get; set; }
        public Nullable<int> GeneralizationYear { get; set; }
        public Nullable<int> GeneralizationNo { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
    }
}
