using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class PhoneType
    {
        public int PhoneTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        internal Task<List<PhoneType>> get(bool v)
        {
            throw new NotImplementedException();
        }

        internal Task<List<PhoneType>> save(PhoneType phoneType)
        {
            throw new NotImplementedException();
        }

        internal Task<List<PhoneType>> delete(int phoneTypeId, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
