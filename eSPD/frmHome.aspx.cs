using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using eSPD.Core;
using eSPD.Extensions;
using System.Data;
using System.Configuration;

namespace eSPD
{
    public partial class frmHome : System.Web.UI.Page
    {
        private string strStatusSPD = string.Empty;
        //debug only
        private string strLoginID = string.Empty;
        public string LoginID
        {
            get
            {
                object o = Request.QueryString["LoginID"];
                if ((o != null))
                    return Convert.ToString(o);
                else
                    return UserID();
            }
            set { Request.QueryString["LoginID"] = value; }
        }

        public string SetLabelWelcome()
        {
            System.Security.Principal.WindowsIdentity User = null;
            User = System.Web.HttpContext.Current.Request.LogonUserIdentity;
            string username = null;
            username = User.Name;
            for (int i = 0; i <= username.Length - 1; i++)
            {
                if (username[i] == '\\')
                {
                    username = username.Remove(0, i + 1);
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            return username;
        }

        private string UserID()
        {
            System.Security.Principal.WindowsIdentity User = null;
            User = System.Web.HttpContext.Current.Request.LogonUserIdentity;
            string UID = null;
            UID = User.Name;
            for (int i = 0; i <= UID.Length - 1; i++)
            {
                if (UID[i] == '\\')
                {
                    UID = UID.Remove(0, i + 1);
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            return UID;
        }

        msKaryawan karyawan = new msKaryawan();

        string errorLogin = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new dsSPDDataContext())
                {
                    if (!Page.IsPostBack)
                    {
                        //strLoginID = SetLabelWelcome();//production

                        //strLoginID = "benny00002704";
                        //strLoginID = "titin008704";
                        //strLoginID = "siska90000308";
                        //strLoginID = "HP";
                        strLoginID = ConfigurationManager.AppSettings["DevstrLoginID"].ToString();//development
                        Session["IDLogin"] = strLoginID;
                        classSpd oSPD = new classSpd();
                        karyawan = oSPD.getKaryawan(strLoginID);
                        Session["nrpLogin"] = karyawan.nrp;

                        if (string.IsNullOrEmpty(karyawan.nrp))
                        {
                            errorLogin += "User (karyawan) tidak ditemukan, session login kosong, silahkan close browser anda, lalu login ulang.";
                        }

                        msUser sekretaris = (from u in ctx.msUsers
                                             where u.nrp == karyawan.nrp && u.roleId == Konstan.SEKRETARIS
                                             select u).FirstOrDefault();
                        if (sekretaris != null)
                        {
                            Session["sekretaris"] = true;
                        }
                        else Session["sekretaris"] = false;

                    }
                }

                ////debug
                //txtLogin.Visible = true;
                //if (Page.IsPostBack) txtLogin_TextChanged(null, null);
            }
            catch (Exception)
            {
                // sorry broo saya cuman ngakal2in crash loginnya soalnya minta login ke server laen -_-"
                bool recIt = ApplicationPoolRecycle.RecycleCurrentApplicationPool();
                HttpRuntime.UnloadAppDomain();
                Response.Redirect("~/FormError.aspx?e=Applikasi sedang merefresh, silahkan close browser anda, lalu login ulang.");
            }
            finally
            {
                ////debug only
                //if (Page.IsPostBack)
                //{
                //    Response.Redirect("~/newFormHome.aspx");
                //}

                //production
                if (!string.IsNullOrEmpty(errorLogin))
                {
                    Response.Redirect("~/FormError.aspx?e=" + errorLogin);
                }
                else
                {
                    Response.Redirect("~/newFormHome.aspx");
                }
            }
        }

        protected void txtLogin_TextChanged(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                if (String.IsNullOrEmpty(txtLogin.Text))
                {
                    txtLogin.Text = SetLabelWelcome();
                }
                strLoginID = txtLogin.Text;
                Session["IDLogin"] = strLoginID;
                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(strLoginID);
                Session["nrpLogin"] = karyawan.nrp;
                dsSPDDataContext data = new dsSPDDataContext();
                msUser sekretaris = (from u in data.msUsers
                                     where u.nrp == karyawan.nrp && u.roleId == Konstan.SEKRETARIS
                                     select u).FirstOrDefault();
                if (sekretaris != null)
                {
                    Session["sekretaris"] = true;
                }
                else Session["sekretaris"] = false;
            }
        }
    }
}