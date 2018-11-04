using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eSPD.Extensions
{
    public class SPHelper
    {

        public static DataTable GetApprovalSPDAtasanLangsung(string nrpApprover, string filter, string param)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SPDConnectionString"].ConnectionString;
            return SqlHelper.ExecuteDataset(
                connectionString,
                "sp_GetApprovalSPDAtasanLangsung",
                nrpApprover,
                filter,
                param).Tables[0];
        }
        public static DataTable GetListSPDGA(string filter, string param)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SPDConnectionString"].ConnectionString;
            return SqlHelper.ExecuteDataset(
                connectionString,
                "sp_GetListSPDGA",
                filter,
                param).Tables[0];
        }
        public static DataTable GetApprovalClaimSPDOlehGA(string filter, string param)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SPDConnectionString"].ConnectionString;
            return SqlHelper.ExecuteDataset(
                connectionString,
                "sp_GetApprovalClaimSPDOlehGA",
                filter,
                param).Tables[0];
        }
    }
}