using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Customer
    {
        #region Attributes
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string InvoiceName { get; set; }
        public string Gender { get; set; }
        public string FundNumber { get; set; }
        public string CivilNum { get; set; }
        public string FamilyCard { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<int> JobId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<long> BoxNumber { get; set; }
        public decimal CurrentPurchses { get; set; }
        public string CustomerStatus { get; set; }
        public string MemberNature { get; set; }
        public Nullable<int> SessionNumber { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<int> ReceiptVoucherNumber { get; set; }
        public Nullable<System.DateTime> ReceiptVoucherDate { get; set; }
        public int JoiningSharesCount { get; set; }
        public int SharesCount { get; set; }
        public bool CalculateEarnings { get; set; }
        public bool IsBlocked { get; set; }
        public string IBAN { get; set; }
        public Nullable<int> BankId { get; set; }
        public bool PrintNameOnInv { get; set; }
        public bool RegisteredInMinistry { get; set; }
        public bool StopOnPOS { get; set; }
        public bool DataCompleted { get; set; }
        public string Notes { get; set; }
        public bool IsArchived { get; set; }
        public bool FamilyCardHolder { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        public CustomerAddress customerAddress { get; set; }

        public List<CustomerDocument> customerDocuments { get; set; }

        #endregion

        #region Methods
        internal Task<List<Customer>> get(bool v)
        {
            throw new NotImplementedException();
        }

       

       

        internal Task<List<Customer>> save(Customer customer)
        {
            throw new NotImplementedException();
        }

        internal Task<List<Customer>> delete(long bankId, long userId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class CustomerAddress
    {
        public long CustomerId { get; set; }
        public string AutomtedNumber { get; set; }
        public Nullable<int> AreaId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public Nullable<int> Floor { get; set; }
        public string AvenueNumber { get; set; }
        public string Plot { get; set; }
        public string MailBox { get; set; }
        public string PostalCode { get; set; }
        public string Employer { get; set; }
        public string Guardian { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumber2 { get; set; }
        public string WorkAddress { get; set; }
        public Nullable<int> KinshipId { get; set; }
    }

    public class CustomerDocument
    {
        public long DocumentId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string DocName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
    }
}
