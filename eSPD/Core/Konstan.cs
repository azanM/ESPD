using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSPD.Core
{
    public static class Konstan
    {
        public const Int16 KADEPT = 1;
        public const Int16 ADH = 2;
        public const Int16 GM = 3;
        public const Int16 BM = 5;
        public const Int16 ASH = 6;
        public const Int16 SM = 7;
        public const Int16 OM = 10;
        public const Int16 AM = 11;
        public const Int16 DIREKTUR = 13;
        public const Int16 PRESIDEN_DIREKTUR = 14;
        public const Int16 GA = 17;
        public const Int16 FINANCE = 19;
        public const Int16 KASIR = 20;
        public const Int16 SEKRETARIS = 23;
        public const Int16 SYSADMIN = 24;

        public static string ConnectionString { get { return System.Configuration.ConfigurationManager.ConnectionStrings["SPDConnOrgUnit"].ConnectionString; } }
    }
}