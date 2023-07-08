using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class KinshipTies
    {
        #region Attributes
        public int KinshipId { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        internal Task<List<KinshipTies>> get(bool v)
        {
            throw new NotImplementedException();
        }

        internal Task<List<KinshipTies>> save(KinshipTies kinshipTies)
        {
            throw new NotImplementedException();
        }

        internal Task<List<KinshipTies>> delete(int kinshipId, long userId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
