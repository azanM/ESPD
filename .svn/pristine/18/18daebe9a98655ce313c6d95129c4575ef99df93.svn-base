using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;

namespace eSPD
{
    public partial class frmMsPlafonGolongan : System.Web.UI.Page
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

            if (roleid == Konstan.GA || strLoginID.Contains("yudisss") || strLoginID.Contains("Syam005812") || roleid == Konstan.SYSADMIN)
            {
                pnlGrid.Visible = true;
                gvRole.Visible = true;
            }
            else
            {
                pnlGrid.Visible = false;
                gvRole.Visible = false;
            }

            userLoginID = !String.IsNullOrEmpty(strLoginID) ? strLoginID.Length > 10 ? strLoginID.Substring(0, 10).ToString() : strLoginID : "";
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
            if (!String.IsNullOrEmpty(filter))
            {

                var query = (from m in dsp.msGolonganPlafons
                             join n in dsp.msPlafons on m.idPlafon equals n.id
                             select new
                            {
                                id = m.id,
                                jenisSPD = m.jenisSPD,
                                wilayah = m.wilayah,
                                golongan = m.golongan,
                                Plafon = n.plafon,
                                harga = m.harga,
                                deskripsi = m.deskripsi,
                                status = m.status.Equals("AKTIF") ? "Aktif" : "Tidak Aktif",
                                idPlafon = m.idPlafon

                            }).Where(o => o.golongan.Contains(filter));
                gvRole.DataSource = query.ToList();
            }
            else
            {
                var query = from m in dsp.msGolonganPlafons
                            join n in dsp.msPlafons on m.idPlafon equals n.id
                            select new
                             {
                                 id = m.id,
                                 jenisSPD = m.jenisSPD,
                                 wilayah = m.wilayah,
                                 golongan = m.golongan,
                                 plafon = n.plafon,
                                 harga = m.harga,
                                 deskripsi = m.deskripsi,
                                 status = m.status.Equals("AKTIF") ? "Aktif" : "Tidak Aktif",
                                 idPlafon = m.idPlafon
                             };
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


        protected void btnTambah_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            pnlGrid.Visible = false;
            hfmode.Value = "add";
            hdnRoleID.Value = string.Empty;
            txtHarga.Text = string.Empty;
            txtNrp.Text = string.Empty;
            txtNrp.Enabled = true;
            cmbUser.Enabled = true;
            ddlJenisSPD.Enabled = true;
            ddlWilayah.Enabled = true;
            ddlGolongan.Enabled = true;
            ddlPlafon.Enabled = true;
            txtHarga.Enabled = true;


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear_form();
            pnlForm.Visible = false;
            pnlGrid.Visible = true;
            fillGV("");
        }
        protected void lbEdit_Click(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            Label id = (Label)gv.FindControl("id");
            Label idPlafon = (Label)gv.FindControl("idPlafon");
            string jenisSPD = gv.Cells[1].Text;
            string wilayah = gv.Cells[2].Text;
            string golongan = gv.Cells[3].Text;
            string plafon = gv.Cells[4].Text;
            string harga = gv.Cells[5].Text;
            string deskripsi = gv.Cells[6].Text;
            string status = gv.Cells[7].Text.ToUpper();
            var x = (from m in dss.msGolonganPlafons
                     where m.id.ToString().Trim() == id.Text.Trim()
                     select m).FirstOrDefault();
            fill_form(ref x, jenisSPD, wilayah, golongan, idPlafon.Text, status);
            hfmode.Value = "edit";

            pnlForm.Visible = true;
            pnlGrid.Visible = false;
        }

        protected void btnSimpan_Click(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            string mode = "add";
            mode = hfmode.Value.ToString();
            if (mode == "add")
            {
                msGolonganPlafon cst = (from k in dss.msGolonganPlafons
                                        where k.id.ToString().Trim() == hdnRoleID.Value
                                        select k).FirstOrDefault();
                if (cst == null)
                {
                    msGolonganPlafon role = new msGolonganPlafon();
                    role.jenisSPD = ddlJenisSPD.SelectedValue.ToString();
                    role.wilayah = ddlWilayah.SelectedValue.ToString();
                    role.golongan = ddlGolongan.SelectedValue.ToString();
                    role.idPlafon = Convert.ToInt32(ddlPlafon.SelectedValue.ToString());
                    role.harga = Convert.ToInt32(txtHarga.Text);
                    role.deskripsi = txtNrp.Text.Trim();
                    role.status = cmbUser.SelectedValue.ToString() == "AKTIF" ? "aktif" : "tidak aktif";
                    role.dibuatOleh = userLoginID;
                    role.dibuatTanggal = DateTime.Now;
                    role.diubahOleh = userLoginID;
                    role.diubahTanggal = DateTime.Now;

                    dss.msGolonganPlafons.InsertOnSubmit(role);
                    dss.SubmitChanges();
                    dss.Dispose();
                    //clear_form();
                    notif.Text = "Data berhasil disimpan";
                    //fillGV("");
                }
                else
                {
                    notif.Text = "Data sudah terdaftar";
                }
            }
            ////mode edit gadipake
            else if (mode == "edit")
            {
                msGolonganPlafon cst = (from k in dss.msGolonganPlafons
                                        where k.id.ToString().Trim() == hdnRoleID.Value
                                        select k).FirstOrDefault();
                cst.id = Convert.ToInt32(hdnRoleID.Value);
                cst.jenisSPD = ddlJenisSPD.SelectedValue.ToString();
                cst.wilayah = ddlWilayah.SelectedValue.ToString();
                cst.golongan = ddlGolongan.SelectedValue.ToString();
                cst.idPlafon = Convert.ToInt32(ddlPlafon.SelectedValue.ToString());
                cst.harga = Convert.ToInt32(txtHarga.Text);
                cst.deskripsi = txtNrp.Text.Trim();
                cst.diubahOleh = userLoginID;
                cst.diubahTanggal = DateTime.Now;
                cst.status = cmbUser.SelectedValue.ToString() == "AKTIF" ? "aktif" : "tidak aktif";
                dss.SubmitChanges();
                dss.Dispose();
                notif.Text = "Data berhasil disimpan";
                //}
            }
            fillGV(txtFilterName.Text.Trim());
        }

        private void clear_form()
        {
            hdnRoleID.Value = string.Empty;
            txtNrp.Text = string.Empty;
        }

        private void fill_form(ref msGolonganPlafon x, string jenisSPD, string wilayah, string golongan, string idPlafon, string status)
        {
            hdnRoleID.Value = x.id.ToString();
            txtNrp.Text = x.deskripsi;
            cmbUser.SelectedValue = status;
            ddlJenisSPD.SelectedValue = jenisSPD;
            ddlWilayah.SelectedValue = wilayah;
            ddlGolongan.SelectedValue = golongan;
            ddlPlafon.SelectedValue = idPlafon;
            txtHarga.Text = x.harga.ToString();
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            fillGV(txtFilterName.Text.Trim());
        }

    }
}