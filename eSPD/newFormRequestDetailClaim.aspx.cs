using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSPD
{
    public partial class newFormRequestDetailClaim : System.Web.UI.Page
    {
        private static dsSPDDataContext ctx = new dsSPDDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }

            string noSPD = Encrypto.Decrypt(Request.QueryString["encrypt"]);
            Id.Value = noSPD;

            if (true)
            {
                
            }
        }
    }
}