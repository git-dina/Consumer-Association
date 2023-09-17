using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class SalesInvoiceDetails
    {
        public long DetailsId { get; set; }
        public string ItemName { get; set; }
        public Nullable<long> InvoiceId { get; set; }
        public string Barcode { get; set; }
        public Nullable<long> ItemUnitId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public Nullable<decimal> Total { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; } = MainWindow.userLogin.UserId;
        public Nullable<long> UpdateUserId { get; set; } = MainWindow.userLogin.UserId;

        //extra
        public int Sequence { get; set; }
    }
}
