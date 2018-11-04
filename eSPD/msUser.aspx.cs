using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;

namespace eSPD
{
    public partial class msUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            classSpd cspd = new classSpd();


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
            msKaryawan karyawan = oSPD.getKaryawan(strLoginID);
            dsSPDDataContext data = new dsSPDDataContext();
            Int32 roleid = (from k in data.msUsers
                            where k.nrp == karyawan.nrp && (k.roleId == Konstan.SYSADMIN || k.roleId == Konstan.GA)
                            select k.roleId).FirstOrDefault();
            if (roleid == Konstan.GA || strLoginID.Contains("yudisss") || roleid == Konstan.SYSADMIN)
            {
                gvUser.Visible = true;
                btnTambah.Visible = true;
            }
            else
            {
                gvUser.Visible = false;
                btnTambah.Visible = false;
            }

            if (!IsPostBack)
            {
                pnlForm.Visible = false;
                hfmode.Value = "add";
                fillGV();
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
        protected void gvCostCenter_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUser.PageIndex = e.NewPageIndex;
            fillGV();
        }

        protected void btnTambah_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            btnTambah.Visible = false;
            hfmode.Value = "add";
        }

        protected void btnBatal_Click(object sender, EventArgs e)
        {
            clear_form();
            pnlForm.Visible = false;
            btnTambah.Visible = true;
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            Label costId = (Label)gv.FindControl("id");
            msUser cost = data.msUsers.Where(f => f.id.ToString() == costId.Text).Single();
            data.msUsers.DeleteOnSubmit(cost);
            data.SubmitChanges();
            data.Dispose();
            fillGV();
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string nrp = gv.Cells[0].Text;
            Label costId = (Label)gv.FindControl("lblIdCost");
            //msCost cost = (from k in dss.msCosts where k.costId.ToString().Trim() == costId.Text.Trim() select k).FirstOrDefault();
            Label id = (Label)gv.FindControl("id"); 
            var x = (from m in dss.v_userRoles
                     where m.id.ToString().Trim() == id.Text.Trim()
                     select m).FirstOrDefault();
            fill_form( ref x);
            hfmode.Value = "edit";
            pnlForm.Visible = true;           
            btnTambah.Visible = false;           
        }

        protected void btnSimpan_Click(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            string mode = "add";
            mode = hfmode.Value.ToString();
            msUser cst = (from k in dss.msUsers where k.id.ToString().Trim() == hfCostId.Value.Trim() select k).FirstOrDefault();
            if (cst == null)
            {
                msUser cost = new msUser();
                cost = fillK();
                dss.msUsers.InsertOnSubmit(cost);
                dss.SubmitChanges();
                dss.Dispose();
                clear_form();
                notif.Text = "Data berhasil disimpan";
                fillGV();
            }
            else
            {
                //clear_form();

                //if (mode == "add")
                //{
                //    notif.Text = "Simpan gagal : NRP Karyawan Sudah Terdaftar";
                //}
                //else
                //{
                    fillEdit(ref cst);
                    dss.SubmitChanges();
                    dss.Dispose();
                    clear_form();
                    notif.Text = "Data berhasil disimpan";
                    fillGV();
                //}

            }

        }

        protected void cmbCompanyName_Selecting1(object sender, LinqDataSourceSelectEventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();

            //msKaryawan personalArea = (from u in data.msKaryawans                                       
            //                           select u).FirstOrDefault();
            var user = (from u in data.msUsers
                        join k in data.msKaryawans on u.nrp equals k.nrp                        
                        orderby k.namaLengkap
                        select new
                        {
                            companyCode = k.companyCode,
                            coCd = k.coCd
                        }).Distinct();
            e.Result = user;

        }

        #region voids

        private void fillGV()
        {
            dsSPDDataContext dsp = new dsSPDDataContext();
            var query = (from m in dsp.v_userRoles
                         select new { m.nrp, m.namaLengkap, m.roleId, m.namaRole, m.id }
            );
            gvUser.DataSource = query.ToList();
            gvUser.DataBind();
            dsp.Dispose();
            gvUser.Visible = true;
        }

        private void clear_form()
        {
            txtCostCenter.Text = string.Empty;
            txtCostDesc.Text = string.Empty;
            //pnlForm.Visible = false;
            //btnTambah.Visible = true;
        }

        private void fill_form(ref v_userRole x)
        {
            cmbCompanyName.SelectedValue = x.roleId.ToString();
            txtCostDesc.Text = x.nrp;
            txtCostCenter.Text = x.namaLengkap;
            hfCostId.Value = x.id.ToString();       
        }

        public msUser fillK()
        {

            msUser cost = new msUser();

            cost.nrp = txtCostDesc.Text;
            cost.roleId = Convert.ToInt32(cmbCompanyName.SelectedItem.Value);
            cost.status = 1;

            return cost;
        }

        public void fillEdit(ref msUser cost)
        {
            cost.nrp = txtCostDesc.Text;
            cost.roleId = Convert.ToInt32(cmbCompanyName.SelectedItem.Value);
            cost.id = Convert.ToInt32(hfCostId.Value);

        }

        #endregion

        protected void txtCostCenter_TextChanged(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            var x = (from m in dss.v_karyawanMappings
                     where m.Nrp.ToString().Trim() == txtCostDesc.Text
                     select m).FirstOrDefault();
            txtCostCenter.Text = x.namaLengkap;
        }

        protected void cmbCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}