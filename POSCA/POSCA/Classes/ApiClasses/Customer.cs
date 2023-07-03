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
        public string Job { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
