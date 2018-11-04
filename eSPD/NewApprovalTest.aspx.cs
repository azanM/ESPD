using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;
using System.Web.Services;
using System.Web.Script.Services;

namespace eSPD
{
    public partial class NewApprovalTest : System.Web.UI.Page
    {
        private static readonly dsSPDDataContext ctx = new dsSPDDataContext();
        private static string strID = string.Empty;
        private static classSpd oSPD = new classSpd();
        private static msKaryawan karyawan = new msKaryawan();
        protected void Page_Load(object sender, EventArgs e)
        {
            strID = (string)Session["IDLogin"];
            karyawan = oSPD.getKaryawan(strID);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //tipe
            string lndnValue = string.Empty;
            if (rbLN.Checked) lndnValue = "LN";
            if (rbDN.Checked) lndnValue = "DN";

            //tipe detail
            string hocbgValue = string.Empty;
            if (rdbHO.Checked) hocbgValue = "HO";
            if (rdbCbg.Checked) hocbgValue = "Cabang";

            //level (golongan)
            //posisi
            //           


            var data = (from o in ctx.ApprovalRules
                         where 
                         o.Tipe == lndnValue &&
                         o.TipeDetail == hocbgValue &&
                         //if direksi checked true
                         (cbDireksi.Checked == true ? o.Golongan == "D" : o.Golongan == cmbGolongan.SelectedValue)
                         select o).ToList();

            gvApproval.DataSource = data;
            gvApproval.DataBind();
        }


        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<SelectListItem> GetAtasan(string searchText, string additionalFilter)
        //{
        //    var data = (from p in ctx.v_atasans
        //                where p.coCd == karyawan.coCd
        //                        && (additionalFilter.ToLower() == "true" ? p.posisi.Contains("director") : !p.posisi.Contains("director"))
        //                        && p.namaLengkap.ToLower().Contains(searchText.ToLower())
        //                orderby p.namaLengkap
        //                select new SelectListItem
        //                {
        //                    Value = p.nrp,
        //                    Text = p.namaLengkap,
        //                }).Distinct().ToList();

        //    return data;
        //}


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SelectListItem> GetAtasan(string prefixText, int count, string contextKey)
        {
            var data = (from p in ctx.v_atasans
                        where p.coCd == karyawan.coCd
                                && (contextKey.ToLower() == "true" ? p.posisi.Contains("director") : !p.posisi.Contains("director"))
                                && p.namaLengkap.ToLower().Contains(prefixText.ToLower())
                        orderby p.namaLengkap
                        select new SelectListItem
                        {
                            Value = p.nrp,
                            Text = p.namaLengkap,
                        }).Distinct().ToList();

            return data;
        }

        public class SelectListItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

    }
}