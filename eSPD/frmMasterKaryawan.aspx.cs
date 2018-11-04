using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;

namespace eSPD
{
    public partial class frmMasterKaryawan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            classSpd cspd = new classSpd();

            if (!IsPostBack) {
                pnlForm.Visible = false;
                hfmode.Value = "add";
                fillGV();
            }
        }
       

        protected void gvKaryawan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvKaryawan.PageIndex = e.NewPageIndex;
            fillGV();
        }

        protected void btnTambah_Click(object sender, EventArgs e)
        {
            //pnlForm.Visible = true;
            //btnTambah.Visible = false;
            //hfmode.Value = "add";
            dsSPDDataContext dsp = new dsSPDDataContext();
            var query = from c in dsp.trSPDs
                        where !(from o in dsp.trClaims
                                select o.noSPD).Contains(c.noSPD) && ((DateTime.Now.Date - c.tglKembali.Date).TotalDays == 21)
                        select c;            
            //var x = query.Where(t => (DateTime.Now.Date - t.tglKembali.Date).TotalDays == 21);
            foreach (var item in query)
            {
                
            }
        }

        protected void btnBatal_Click(object sender, EventArgs e)
        {
            clear_form();           
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string nrp = gv.Cells[0].Text;
            msKaryawan kr = (from k in data.msKaryawans where k.nrp.Trim() == nrp.Trim() select k).FirstOrDefault();
            kr.statusKaryawan = "Not Active";
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
            msKaryawan kr = (from k in dss.msKaryawans where k.nrp.Trim() == nrp.Trim() select k).FirstOrDefault();
            fill_form(ref kr);
            hfmode.Value = "edit";
            pnlForm.Visible = true;
            txtNRP.ReadOnly = true;
            btnTambah.Visible = false;
          
        }

        protected void btnSimpan_Click(object sender, EventArgs e)
        {
            dsSPDDataContext dss = new dsSPDDataContext();
            string mode = "add";
            mode = hfmode.Value.ToString();
            msKaryawan kr = (from k in dss.msKaryawans where k.nrp.Trim() == txtNRP.Text.Trim() select k).FirstOrDefault();
            if (kr == null)
            {
                msKaryawan krs = new msKaryawan();
                krs = fillK();
                dss.msKaryawans.InsertOnSubmit(krs);
                dss.SubmitChanges();
                dss.Dispose();                
                clear_form();
                notif.Text = "Data berhasil disimpan";
                fillGV();
            }
            else
            {
                //clear_form();

                if (mode == "add")
                {
                    notif.Text = "Simpan gagal : NRP Karyawan Sudah Terdaftar";
                }
                else {
                    fillEdit(ref kr);
                    dss.SubmitChanges();
                    dss.Dispose();
                    clear_form();
                    notif.Text = "Data berhasil disimpan";
                    fillGV();
                }
                
            }

        }


        #region voids

        private void fillGV()
        {
            dsSPDDataContext dsp = new dsSPDDataContext();
            //var query = (from k in dsp.msKaryawans where k.statusKaryawan.Trim() == "Active" select k);
            var query = (from k in dsp.msKaryawans select k);
            gvKaryawan.DataSource = query.ToList();
            gvKaryawan.DataBind();
            dsp.Dispose();
        }

        private void clear_form() {
            txtCocd.Text = string.Empty;
            txtCompanyCode.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtGolongan.Text = string.Empty;
            txtJob.Text = string.Empty;
            txtKdJob.Text = string.Empty;
            txtKdOrg.Text = string.Empty;
            txtKdPA.Text = string.Empty;
            txtKdPosisi.Text = string.Empty;
            txtKdPSA.Text = string.Empty;
            txtNama.Text = string.Empty;
            txtNamaAtasan.Text = string.Empty;
            txtNRP.Text = string.Empty;
            txtNRPAtasan.Text = string.Empty;
            txtOrg.Text = string.Empty;
            txtPA.Text = string.Empty;
            txtPosisi.Text = string.Empty;
            txtPSA.Text = string.Empty;
            txtSubGroup.Text = string.Empty;
            txtTgl.Text = string.Empty;
            txtTglMasuk.Text = string.Empty;
            pnlForm.Visible = false;
            btnTambah.Visible = true;
        }

        private void fill_form(ref msKaryawan kx) {
            txtNRP.Text=kx.nrp;
            txtCocd.Text=kx.coCd;
            txtCompanyCode.Text=kx.companyCode;
            txtEmail.Text=kx.EMail;
            txtGolongan.Text=kx.golongan;
            txtJob.Text=kx.Job;
            txtSubGroup.Text=kx.karyawanSubGroup;
            txtKdJob.Text=kx.kodeJob;
            txtKdOrg.Text=kx.kodeOrganisasiUnit;
            txtKdPA.Text=kx.kodePA;
            txtKdPosisi.Text=kx.kodePosisi;
            txtKdPSA.Text=kx.kodePSubArea;
            txtNamaAtasan.Text=kx.namaAtasan;
            txtNama.Text=kx.namaLengkap;
            txtNRPAtasan.Text=kx.nrpAtasan;
            txtOrg.Text=kx.organisasiUnit;
            txtPA.Text=kx.personelArea;
            txtPosisi.Text=kx.posisi;
            txtPSA.Text=kx.pSubArea;
            txtTglMasuk.Text=DateTime.Parse(kx.startDate.ToString()).ToShortDateString();
            ddlStatus.SelectedValue=kx.statusKaryawan;
            txtTgl.Text = DateTime.Parse(kx.tanggal.ToString()).ToShortDateString();
            pnlForm.Visible = false;
            btnTambah.Visible = true;
        }

        public msKaryawan fillK()
        {

            msKaryawan kx = new msKaryawan();

            kx.nrp = txtNRP.Text;
            kx.coCd = txtCocd.Text;
            kx.companyCode = txtCompanyCode.Text;
            kx.EMail = txtEmail.Text;
            kx.golongan = txtGolongan.Text;
            kx.Job = txtJob.Text;
            kx.karyawanSubGroup = txtSubGroup.Text;
            kx.kodeJob = txtKdJob.Text;
            kx.kodeOrganisasiUnit = txtKdOrg.Text;
            kx.kodePA = txtKdPA.Text;
            kx.kodePosisi = txtKdPosisi.Text;
            kx.kodePSubArea = txtKdPSA.Text;
            kx.namaAtasan = txtNamaAtasan.Text;
            kx.namaLengkap = txtNama.Text;
            kx.nrpAtasan = txtNRPAtasan.Text;
            kx.organisasiUnit = txtOrg.Text;
            kx.personelArea = txtPA.Text;
            kx.posisi = txtPosisi.Text;
            kx.pSubArea = txtPSA.Text;
            kx.startDate = DateTime.Parse(txtTglMasuk.Text);
            kx.statusKaryawan = ddlStatus.SelectedValue.ToString();
            kx.tanggal = DateTime.Parse(txtTgl.Text);
            
            return kx;
        }

        public void fillEdit(ref msKaryawan kx) {
            kx.coCd = txtCocd.Text;
            kx.companyCode = txtCompanyCode.Text;
            kx.EMail = txtEmail.Text;
            kx.golongan = txtGolongan.Text;
            kx.Job = txtJob.Text;
            kx.karyawanSubGroup = txtSubGroup.Text;
            kx.kodeJob = txtKdJob.Text;
            kx.kodeOrganisasiUnit = txtKdOrg.Text;
            kx.kodePA = txtKdPA.Text;
            kx.kodePosisi = txtKdPosisi.Text;
            kx.kodePSubArea = txtKdPSA.Text;
            kx.namaAtasan = txtNamaAtasan.Text;
            kx.namaLengkap = txtNama.Text;
            kx.nrpAtasan = txtNRPAtasan.Text;
            kx.organisasiUnit = txtOrg.Text;
            kx.personelArea = txtPA.Text;
            kx.posisi = txtPosisi.Text;
            kx.pSubArea = txtPSA.Text;
            kx.startDate = DateTime.Parse(txtTglMasuk.Text);
            kx.statusKaryawan = ddlStatus.SelectedValue.ToString();
            kx.tanggal = DateTime.Parse(txtTgl.Text);
        }

        #endregion

    }

    
}