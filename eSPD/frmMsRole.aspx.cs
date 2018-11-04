using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;

namespace eSPD
{
    public partial class frmMsRole : System.Web.UI.Page
    {
        protected string userLoginID = ""; 
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

            if (!IsPostBack)
            {
                pnlForm.Visible = false;
                fillGV(txtFilterName.Text.Trim());
            }

            if (roleid == Konstan.GA || strLoginID.Contains("yudisss") || roleid == Konstan.SYSADMIN)
            {
                pnlGrid.Visible = true;
                gvRole.Visible = true;
            }
            else
            {
                pnlGrid.Visible = false;
                gvRole.Visible = false;
            }

            userLoginID = !String.IsNullOrEmpty(strLoginID) ? strLoginID.Length > 10 ? strLoginID.Substring(0,10).ToString() : strLoginID : "";
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

        private void fillGV(string filter)
        {
            dsSPDDataContext dsp = new dsSPDDataContext();
            if (!String.IsNullOrEmpty(filter)) {
                var query = (from m in dsp.msRoles
                             select new { m.id, m.namaRole, m.status }
                             ).Where(o => o.status == true && o.namaRole.Contains(filter));
                gvRole.DataSource = query.ToList();
            }
            else {
                var query = (from m in dsp.msRoles
                             select new { m.id, m.namaRole, m.status }
                             ).Where(o => o.status == true);
                gvRole.DataSource = query.ToList();
            }
            
            gvRole.DataBind();
            dsp.Dispose();
            gvRole.Visible = true;
        }

        protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRole.PageIndex = e.NewPageIndex;
            fillGV(txtFilterName.Text.Trim());
        }

        protected void fillCmbUser()
        {
            dsSPDDataContext dsp = new dsSPDDataContext();
            var query = (from m in dsp.v_karyawanMappings
                         select new { m.Nrp, m.namaLengkap, m.posisi }
                        ).OrderBy(o=> o.namaLengkap);
            cmbUser.DataSource = query.ToList();
            cmbUser.DataBind();
            dsp.Dispose();
            cmbUser.Visible = true;

        }

        protected void btnTambah_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            pnlGrid.Visible = false;
            hfmode.Value = "add";

            //hdnRoleName.Value = string.Empty;
            //txtRoleName.Enabled = true;
            hdnRoleID.Value = string.Empty;
            //txtNrp.Text = string.Empty;
            //txtNrp.Enabled = false;
            //cmbUser.Items.Clear();
            //cmbUser.Enabled = false;
            txtNrp.Enabled = true;
            fillCmbUser();
            cmbUser.Enabled = true;
            txtPosisi.Enabled = false;
            txtPosisi.Text = string.Empty;
            //RequiredFieldValidator1.Visible = false;
            //RequiredFieldValidator2.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e) 
        {
            clear_form();
            pnlForm.Visible = false;
            pnlGrid.Visible = true;
            fillGV("");
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            Label costId = (Label)gv.FindControl("id");
            msRole role = data.msRoles.Where(f => f.id.ToString() == costId.Text).Single();
            data.msRoles.DeleteOnSubmit(role);
            data.SubmitChanges();
            data.Dispose();
            fillGV(txtFilterName.Text.Trim());
        }
        //protected void lbEdit_Click(object sender, EventArgs e)
        //{
        //    dsSPDDataContext dss = new dsSPDDataContext();
        //    LinkButton link = (LinkButton)sender;
        //    GridViewRow gv = (GridViewRow)(link.NamingContainer);
        //    string nrp = gv.Cells[0].Text;
        //    Label costId = (Label)gv.FindControl("lblIdCost");
        //    //msCost cost = (from k in dss.msCosts where k.costId.ToString().Trim() == costId.Text.Trim() select k).FirstOrDefault();
        //    Label id = (Label)gv.FindControl("id");
        //    var x = (from m in dss.msRoles
        //             where m.id.ToString().Trim() == id.Text.Trim()
        //             select m).FirstOrDefault();
        //    fill_form(ref x);
        //    hfmode.Value = "edit";
        //    pnlForm.Visible = true;
        //    pnlGrid.Visible = false;
        //}

        protected void btnSimpan_Click(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            string mode = "add";
            mode = hfmode.Value.ToString();
            if (mode == "add")
            {
                msRole cst = (from k in dss.msRoles
                              where k.namaRole.ToString().Trim() == txtPosisi.Text.Trim()
                              select k).FirstOrDefault();
                if (cst == null)
                {
                    msRole role = new msRole();
                    //role.id = Convert.ToInt32(hdnRoleID.Value);
                    role.namaRole = txtPosisi.Text.Trim();
                    role.status = true;
                    role.dibuatOleh = userLoginID;
                    role.dibuatTanggal = DateTime.Now;
                    role.diubahOleh = userLoginID;
                    role.diubahTanggal = DateTime.Now;

                    dss.msRoles.InsertOnSubmit(role);
                    dss.SubmitChanges();
                    dss.Dispose();
                    //clear_form();
                    notif.Text = "Data berhasil disimpan";
                    //fillGV("");
                }
                else {
                    notif.Text = "Nama Role atau Posisi sudah terdaftar";
                }
            }
            ////mode edit gadipake
            //else if (mode == "edit")
            //{

            //    msRole cst = (from k in dss.msRoles
            //                  where k.namaRole.ToString().Trim() == txtPosisi.Text.Trim()
            //                  select k).FirstOrDefault();
            //    if (cst == null)
            //    {
            //        msRole role = new msRole();
            //        //role.id = Convert.ToInt32(hdnRoleID.Value);
            //        role.namaRole = txtPosisi.Text.Trim();
            //        role.status = true;
            //        role.dibuatOleh = userLoginID;
            //        role.dibuatTanggal = DateTime.Now;
            //        role.diubahOleh = userLoginID;
            //        role.diubahTanggal = DateTime.Now;

            //        dss.msRoles.InsertOnSubmit(role);
            //        dss.SubmitChanges();
            //        dss.Dispose();
            //        notif.Text = "Data berhasil disimpan";
            //    }
            //    else
            //    {
            //        cst.id = Convert.ToInt32(hdnRoleID.Value);
            //        cst.namaRole = txtPosisi.Text.Trim();
            //        cst.diubahOleh = userLoginID;
            //        cst.diubahTanggal = DateTime.Now;
            //        dss.SubmitChanges();
            //        dss.Dispose();
            //        notif.Text = "Data berhasil disimpan";
            //    }
            //}

        }

        private void clear_form()
        {
            //txtRoleName.Text = string.Empty;
            hdnRoleID.Value = string.Empty;
            txtNrp.Text = string.Empty;
            txtPosisi.Text = string.Empty;
        }

        private void fill_form(ref msRole x)
        {
            //txtRoleName.Text = x.namaRole;
            hdnRoleID.Value = x.id.ToString();
            txtNrp.Text = string.Empty;
            fillCmbUser();
            txtPosisi.Text = x.namaRole;

            //txtRoleName.Enabled = false;
            txtNrp.Enabled = true;
            cmbUser.Enabled = true;
            txtPosisi.Enabled = false;
            //RequiredFieldValidator1.Visible = true;
            //RequiredFieldValidator2.Visible = true;
        }

        protected void txtNrp_TextChanged(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            var x = (from m in dss.v_karyawanMappings
                     where m.Nrp.ToString().Trim() == txtNrp.Text
                     select m).FirstOrDefault();
            if (x != null)
            {
                fillCmbUser();
                cmbUser.Items.FindByValue(x.Nrp.ToString()).Selected = true;
                txtPosisi.Text = x.posisi;
            }
        }

        protected void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            var userName = cmbUser.SelectedItem.Text;
            dsSPDDataContext dss = new dsSPDDataContext();
            var x = (from m in dss.v_karyawanMappings
                     where m.namaLengkap.ToString().Trim() == userName
                     select m).FirstOrDefault();
            if (x != null)
            {
                txtNrp.Text = x.Nrp.ToString();
                txtPosisi.Text = x.posisi;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            fillGV(txtFilterName.Text.Trim());
        }

    }
}