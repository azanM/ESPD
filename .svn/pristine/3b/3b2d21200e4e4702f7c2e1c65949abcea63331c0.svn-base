using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;

namespace eSPD
{
    public partial class frmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public bool AuthenticateUser(string user, string pass)
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://TRAC.ASTRA.CO.ID", user, pass, AuthenticationTypes.Secure);
            try
            {
                //run a search using those credentials.  
                //If it returns anything, then you're authenticated
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                //de.Properties["P.O.Box"].Value;
                //physicalDeliveryOfficeName
                //postOfficeBox
                return true;
            }
            catch
            {
                ;
                //otherwise, it will crash out so return false
                return false;
            }
        }

       
      

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            bool authLog = AuthenticateUser(Login1.UserName.Trim(), Login1.Password.Trim());
            List<string> roles = new List<string>();
            //dsSPDDataContext data = new dsSPDDataContext();
            if (authLog == true)
            {
                Session.Add("UserLog", Login1.UserName.Trim());
                roles.Add("Karyawan");
                msUser user = new msUser();
                //P.O.Box
                Response.Redirect("frmHome.aspx");
            }
          

        }
    }
}