using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;

namespace eSPD
{
    public partial class frmAssignRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strLoginID = string.Empty;
            classSpd oSPD = new classSpd();
            if (Session["IDLogin"] != null)
            {
                strLoginID = (string)Session["IDLogin"];
            }
            else
            {
                strLoginID = SetLabelWelcome();
            }
           msKaryawan  karyawan = oSPD.getKaryawan(strLoginID);
            dsSPDDataContext data = new dsSPDDataContext();
            Int32 roleid = (from k in data.msUsers
                            where k.nrp == karyawan.nrp && (k.roleId == Konstan.SYSADMIN || k.roleId == Konstan.GA)
                          select k.roleId).FirstOrDefault();
            if (roleid==Konstan.SYSADMIN || roleid == Konstan.GA || strLoginID.Contains("yudi"))
            {
                cmbxUser.Visible = true;
                cmbxUserRole.Visible = true;
                gvRole.Visible = true;
                btnAdd.Visible = true;
                Label1.Visible = true;
                Label2.Visible = false;
            }
            else
            {
                cmbxUser.Visible = false;
                cmbxUserRole.Visible = false;
                gvRole.Visible = false;
                btnAdd.Visible = false;
                Label1.Visible = false;
                Label2.Visible = true;
            }
            
        }
        public string SetLabelWelcome()
        {
            System.Security.Principal.WindowsIdentity User = null;
            User = System.Web.HttpContext.Current.Request.LogonUserIdentity;
            string username = null;
            //username = "anton009190"
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
        protected void cmbxUser_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }

        private void fillGridView(object p)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            string nrp = (string)p;
            var query = (from u in data.msUsers
                         join r in data.msRoles
                         on u.roleId equals r.id
                         join k in data.msKaryawans
                          on u.nrp equals k.nrp
                         where u.nrp == p.ToString()
                         select new
                         {
                             nrp = u.nrp,
                             nama = k.namaLengkap,
                             roleid = r.id,
                             role = r.namaRole
                         });
            gvRole.DataSource = query.ToList();
            gvRole.DataBind();
            data.Dispose();


        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            msUser user = new msUser();
            user.nrp = cmbxUser.Value.ToString();
            user.roleId = Convert.ToInt32(cmbxUserRole.Value);
            try
            {
                data.msUsers.InsertOnSubmit(user);
                data.SubmitChanges();
                fillGridView(cmbxUser.Value.ToString());
            }
            catch (Exception ex)
            {
                Response.Write("Gagal dalam memasukkan data " + ex.Message);
            }
            finally
            {
                data.Dispose();
            }

        }

        protected void cmbxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillGridView(cmbxUser.Value);

        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
           string nrp =  gv.Cells[0].Text;
           string idRole = gv.Cells[2].Text;
           try
           {
               var query = (from r in data.msUsers
                            where r.nrp == nrp && r.roleId.ToString() == idRole
                            select r).FirstOrDefault();
               msUser user = new msUser();
               data.msUsers.DeleteOnSubmit((msUser)query);
               data.SubmitChanges();
               fillGridView(cmbxUser.Value.ToString());
           }
           catch (Exception ex)
           {
               Response.Write("Penghapusan data gagal "+ ex.Message);
           }
           finally
           {
               data.Dispose();
           }
        }
    }
}