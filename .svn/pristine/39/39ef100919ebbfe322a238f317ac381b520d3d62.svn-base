using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSPD
{
    public partial class frmReportSLA : System.Web.UI.Page
    {
        private static dsSPDDataContext ctx = new dsSPDDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        void bindFind()
        {
            var month = Convert.ToInt32(ddlBulanKeberangkatan.SelectedItem.Value);
            var query = ctx.sp_GetSLA(month).ToList();
            grvSLA.DataSource = query;
            grvSLA.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            bindFind();
        }
        protected void grvSLA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSLA.PageIndex = e.NewPageIndex;
            bindFind();
        }
    }
}