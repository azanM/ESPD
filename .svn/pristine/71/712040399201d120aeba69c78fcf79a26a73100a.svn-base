using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSPD
{
    public partial class frmUserManual : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = this.MapPath("UserManual/");
            //path = path.Replace("/", "/");
            string namafile = "User Manual E-SPD 2015.pdf";
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + namafile);
            string pr = path + namafile;
            Response.WriteFile(pr);
            Response.End();
            Response.Close();
        }
    }
}