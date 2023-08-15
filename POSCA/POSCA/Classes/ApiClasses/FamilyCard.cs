using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class FamilyCard
    {
        #region Attributes
        public long FamilyCardId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public Nullable<bool> IsStopped { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public Nullable<long> BoxNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerStatus { get; set; }
        public string CivilNum { get; set; }

        public string AutomatedNumber { get; set; }

        public List<Escort> Escorts { get; set; }
        #endregion

        #region Methods
        #endregion
    }

    public class Escort
    {
        public long EscortId { get; set; }
        public Nullable<long> FamilyCardId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CivilNum { get; set; }
        public string EscortName { get; set; }
        public Nullable<int> KinshipId { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public Nullable<long> BoxNumber { get; set; }
        public Nullable<bool> IsCustomer { get; set; }
        public string KinshipName { get; set; }
    }
}
