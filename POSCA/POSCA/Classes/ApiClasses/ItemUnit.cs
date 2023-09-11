using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes
{
    public class ItemUnit
    {
        #region Attributes
        public long ItemUnitId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public Nullable<long> UnitId { get; set; }
        public string Barcode { get; set; }
        public string BarcodeType { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public decimal Cost { get; set; }
        public decimal SalePrice { get; set; }
        public int Factor { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        //extra
        public string ItemName { get; set; }
        #endregion

    }
}
