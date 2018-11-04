using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{
    System.Security.Principal.WindowsIdentity user;
    #region Handlers  
    protected void Page_Load(object sender, EventArgs e)
    {
        
        //lbLogout.Attributes.Add("onclick", "window.close();");
       //.Attributes.Add("onclick", "javascript:window.close();");
       // MembershipUser user = Membership.GetUser();
        //if (user == null)
        //{
           // Response.Redirect(@"~\frmLogin.aspx");
            //ucLogin1.Visible = true;
            //pobChangePwd.Visible = false;
            //pobResetPassword.Visible = false;
        //}
        //else
        //{
            //pobChangePwd.PopUpTitle = "Change Password";
            //pobChangePwd.PopUpUrl = "~/Pages/Popup/PopupChangePassword.aspx";
            //pobChangePwd.Visible = true;
            //ucLogin1.Visible = false;
       // }

       // if (Request.IsAuthenticated)
       // {
           // List<msUserItem> u = null;
            //Label lbl = (Label)LoginView1.FindControl("lblLogin");
            //if (user.UserName.ToLower() != "admin2011")
            //{
               // u = CORREO.Core.msUserItem.GetByFilter("", "", "", "", "", user.UserName, "", "namalengkap", "Asc", 1, 10, "");
                //lbl.Text = u[0].namaLengkap;
           // }
            //else
              //  lbl.Text = "admin2011";
           // lbLogout.Visible = true;
       // }
       // else
          //  lbLogout.Visible = false;

        if (!IsPostBack)
        {
        }
    }
    #endregion    

    protected void imb_Click(object sender, ImageClickEventArgs e)
    {
        string Page = Request.QueryString["P"] == null ? "" : Request.QueryString["P"];
        ImageButton imb = (ImageButton)sender;
        switch (imb.ID)
        {           
            case "imbLogOut":
                FormsAuthentication.SignOut();
                Response.Redirect(@"~\frmHome.aspx");
                break;           
        }
    }

    protected void lbLogout_Click(object sender, EventArgs e)
    {
        user = System.Web.HttpContext.Current.Request.LogonUserIdentity;
        LinkButton lb = (LinkButton)sender;
        if (user == null) { lb.Attributes.Add("onclick", "window.close();"); }
        Session.Abandon();
        FormsAuthentication.SignOut();
       // Response.Redirect(@"~\frmLogin.aspx");
        Response.Write("<script language='javascript'> { self.close() }</script>");
    }
    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        Response.Redirect(@"~\Pages/Common/ChangePassword.aspx");
    }
    protected void lbtLogout_Click(object sender, EventArgs e)
    {
        user = System.Web.HttpContext.Current.Request.LogonUserIdentity;
        if (user == null) { Response.Redirect(@"~\frmHome.aspx"); }
        Session.Abandon();
        FormsAuthentication.SignOut();
        Response.Redirect(@"~\frmHome.aspx");
        
    }
}
