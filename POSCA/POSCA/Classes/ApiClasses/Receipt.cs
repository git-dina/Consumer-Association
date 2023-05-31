﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Receipt
    {
        #region Attributes
        public long ReceiptId { get; set; }
        public string ReceiptStatus { get; set; }
        public bool IsRecieveAll { get; set; }
        public string InvNumber { get; set; }
        public string ReceiptType { get; set; }
        public Nullable<long> LocationId { get; set; }
        public Nullable<long> PurchaseId { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public Nullable<long> SupId { get; set; }
        public string SupInvoiceNum { get; set; }
        public System.DateTime SupInvoiceDate { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }
        public Nullable<decimal> AmountDifference { get; set; }
        public string Notes { get; set; }
        public string SupplierNotes { get; set; }
        public string SupplierPurchaseNotes { get; set; }
        public decimal CoopDiscount { get; set; }
        public decimal DiscountValue { get; set; }


        //////////////////// extra
        public bool IsTransfer { get; set; }
        public Nullable<long> TransferBy { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public bool ISAccountTransfer { get; set; }
        public Nullable<System.DateTime> AccountTransferDate { get; set; }
        public Nullable<long> AccountEntryCode { get; set; }
        public Nullable<long> AccountEntryCodeCustody { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        public string LocationName { get; set; }
        public Supplier supplier { get; set; }
        public List<RecieptDetails> ReceiptDetails { get; set; }
        #endregion
    }
    public class RecieptDetails
    {
        #region Attributes
        public long DetailsId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<long> ReceiptId { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public string ItemNotes { get; set; }
        public Nullable<int> Factor { get; set; }
        public decimal MainCost { get; set; }
        public decimal Cost { get; set; }
        public decimal MainPrice { get; set; }
        public decimal Price { get; set; }
        public int MaxQty { get; set; }
        public int MinQty { get; set; }
        public int FreeQty { get; set; }
        public Nullable<decimal> CoopDiscount { get; set; }
        public Nullable<decimal> ConsumerDiscount { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        //extra
        public decimal Balance { get; set; }
    }
}
