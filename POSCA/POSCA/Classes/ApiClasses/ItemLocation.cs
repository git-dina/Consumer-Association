using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class ItemLocation
    {
        #region Attribute
        public long ItemLocationId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public Nullable<long> LocationId { get; set; }
        public Nullable<long> LocationName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        #region extra attributes
        public int Min_Qty { get; set; }
        public int Max_Qty { get; set; }
        #endregion
        #endregion
    }
}
