using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CompanySettings
    {
        #region Attributs
        public long SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }
        #endregion
    }
}
