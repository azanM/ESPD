using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using eSPD.Core;
using System.Web.Services;
using System.Web.Script.Services;

namespace eSPD
{
    public partial class frmRequestInput : System.Web.UI.Page
    {
        private static dsSPDDataContext ctx = new dsSPDDataContext();
        private static string strID = string.Empty;
        private static classSpd oSPD = new classSpd();
        private static msKaryawan karyawan = new msKaryawan();
        private static msKaryawan karyawanBySekretaris = new msKaryawan();
        public string TextFormatString { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }

            // ixan
            strID = (string)Session["IDLogin"];
            karyawan = oSPD.getKaryawan(strID);
            Session["nrpLogin"] = karyawan.nrp;

            //txtNoSPD.Text = generateNumber("SPD", 5, 0);
            string strNoSpd = string.Empty, strLoginID = string.Empty;
            bool editable = true;
            bool gamode = false;
            if (!IsPostBack)
            {
                cmbCompanyCode.DataBound += new EventHandler(LstData_DataBound);
                txtTglBerangkat.Text = DateTime.Now.ToShortDateString();
                txtTglKembali.Text = DateTime.Now.ToShortDateString();
                if (Session["noSPD"] != null)
                {
                    strNoSpd = (string)Session["noSPD"];
                    Session["noSPD"] = string.Empty;
                }
                if (Session["editable"] != null)
                {
                    editable = (bool)Session["editable"];
                    Session["editable"] = false;
                }
                if (Session["gamode"] != null)
                {
                    gamode = (bool)Session["gamode"];
                    Session["gamode"] = false;
                }
                if (Session["IDLogin"] != null)
                {
                    strLoginID = (string)Session["IDLogin"];
                }

                if (strLoginID.ToLower() == "is08")
                {
                    //!!!!! debug onleee
                    //strLoginID = "Putu005001";
                    strLoginID = "SPD";
                    //strLoginID = "erna009094";
                    Session["IDLogin"] = strLoginID;
                }

                FLDgamode.Value = gamode.ToString().ToLower();


                if (strNoSpd != string.Empty && !editable)
                {
                    fillFormKaryawan(strLoginID);
                    txtNoSPD.Text = strNoSpd;
                    btnSave.Enabled = false;
                    btnSubmit0.Enabled = false;
                    btnReset0.Enabled = true;
                    txtNoSPD_TextChanged(null, null);
                    txtGolongan_TextChanged(null, null);

                }
                if (strNoSpd != string.Empty && editable)
                {
                    fillFormKaryawan(strLoginID);
                    txtNoSPD.Text = strNoSpd;
                    btnSave.Enabled = true;
                    btnSubmit0.Enabled = true;
                    btnReset0.Enabled = true;
                    txtNoSPD_TextChanged(null, null);
                    txtGolongan_TextChanged(null, null);
                    txtTglKembali_TextChanged(null, null);

                }
                if (strNoSpd == string.Empty)
                {
                    fillFormKaryawan(strLoginID);
                    txtGolongan_TextChanged(null, null);

                }

                if (Session["IDLogin"] != null)
                {
                    strLoginID = (string)Session["IDLogin"];
                }


                rdDalamNegeri.Checked = true;
                if (cek_sec(strLoginID))
                {
                    cmbDireksi.Visible = true;
                }
                else
                {
                    cmbDireksi.Visible = false;
                }

                if (gamode)
                {
                    btnSubmit0.Enabled = false;
                }

                cmbGolongan_SelectedIndexChanged(null, null);
            }


            //if (cek_sec(strLoginID))
            //{
            //    cmbDireksi.Visible = true;
            //}
            //else
            //{
            //    cmbDireksi.Visible = false;
            //}
        }

        #region approval rules //cr : 2014-12-9  ixan method

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //tipe
            string lndnValue = string.Empty;
            if (rdDalamNegeri.Checked) lndnValue = "DN";
            if (rbLuarNegeri.Checked) lndnValue = "LN";

            //tipe detail
            string hocbgValue = string.Empty;
            if (rdbHO.Checked) hocbgValue = "HO";
            if (rdbCbg.Checked) hocbgValue = "Cabang";

            //if (karyawanBySekretaris.nrp != null) karyawan = karyawanBySekretaris;

            //if (karyawan.posisi.ToLower().Contains("dept")) karyawan.posisi = "Kadept";
            //else if (karyawan.posisi.ToLower().Contains("Branch Manager")) karyawan.posisi = "BM";
            //else if (karyawan.posisi.ToLower().Contains("div") || karyawan.posisi.ToLower().Contains("Manager")) karyawan.posisi = "Kadiv/OM";
            //else if (karyawan.posisi.ToLower().Contains("presiden")) karyawan.posisi = "Presiden Director";
            //else if (karyawan.posisi.ToLower().Contains("dire")) karyawan.posisi = "Direksi";
            //else karyawan.posisi = "Karyawan";

            var data = (from x in ctx.ApprovalRules
                        join y in ctx.ApprovalStatus
                        on new { X1 = x.RuleID, X2 = txtNoSPD.Text } equals new { X1 = y.RuleID, X2 = y.NoSPD } into aps
                        from y1 in aps.DefaultIfEmpty()
                        where
                              x.Tipe == lndnValue &&
                              x.TipeDetail == hocbgValue &&
                            //(cmbDireksi.Checked == true ? x.Posisi == "Direksi" : x.Posisi != "Direksi") &&
                              x.Golongan == cmbGolongan.SelectedValue &&
                              x.Posisi.Equals(ddlPosisi.SelectedValue)
                        select new
                          {
                              x.Tipe,
                              x.TipeDetail,
                              x.RuleID,
                              x.Posisi,
                              x.IndexLevel,
                              x.Golongan,
                              x.Deskripsi,
                              y1.NrpApproval,
                              y1.Nama,
                              y1.Email
                          }).ToList();

            gvApproval.DataSource = data;
            gvApproval.DataBind();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SelectListItem> GetAtasan(string searchText, string additionalFilter, int page, string posisi)
        {
            List<SelectListItem> data = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(posisi))
            {
                switch (posisi.ToLower())
                {
                    case "bm":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && (p.posisi.Contains("Branch Manager")
                                    || p.posisi.Contains("General Manager"))
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;

                    case "cfat div head":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && p.posisi.Contains("CFAT Div Head")
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;

                    case "dic":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && p.posisi.Contains("Director")
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;

                    case "finance director":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && p.posisi.Contains("Finance Director")
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;

                    case "gm/om/rm":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && (p.posisi.Contains("General Manager")
                                    || p.posisi.Contains("Operation Manager")
                                    || p.posisi.Contains("TRAC Operation Chief")
                                    || p.posisi.Contains("Regional Manager")
                                    || p.posisi.Contains("TRAC Retail Sales Head")
                                    || p.posisi.Contains("TRAC Corporate Sales Head"))
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;

                    case "kadept/am/om":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && p.coCd == karyawan.coCd
                                && (p.posisi.Contains("Dept Head")
                                    || p.posisi.Contains("TRAC Sales Project Manager")
                                    || p.posisi.Contains("TRAC 4x4 Sales Force Manager")
                                    || p.posisi.Contains("TRAC Retail Sales Branch Manager")
                                    || p.posisi.Contains("Account Manager")
                                    || p.posisi.Contains("Operation Manager"))
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;

                    case "kadiv/om":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && p.coCd == karyawan.coCd
                                && (p.posisi.Contains("Div Head")
                                    || p.posisi.Contains("Operation Manager")
                                    || p.posisi.Contains("TRAC Corporate Sales Head")
                                    || p.posisi.Contains("TRAC Operation Chief")
                                    || p.posisi.Contains("TRAC Retail Sales Head"))
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;

                    case "presiden director":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && p.posisi.Contains("President Director")
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();
                        break;
                    case "bm/kadept/am/om":

                        data = (from p in ctx.msKaryawans
                                where
                                p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                && (p.posisi.Contains("Branch Manager")
                                    || p.posisi.Contains("Dept Head")
                                    || p.posisi.Contains("Departement Head")
                                    || p.posisi.Contains("Account Manager")
                                    || p.posisi.Contains("General Manager")
                                    || p.posisi.Contains("Operation Manager"))
                                select new SelectListItem
                                {
                                    id = p.nrp,
                                    text = p.namaLengkap,
                                }).Skip(10 * (page - 1)).Take(10).ToList();

                        break;
                    default:
                        break;
                }
            }
            else
            {
                data = (from p in ctx.msKaryawans
                        where
                        p.namaLengkap.ToLower().Contains(searchText.ToLower())
                        orderby p.namaLengkap
                        select new SelectListItem
                        {
                            id = p.nrp,
                            text = p.namaLengkap,
                        }).Skip(10 * (page - 1)).Take(10).ToList();
            }


            return data;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static SelectListItem NamaAtasan(string nrpAtasan)
        {
            var data = (from p in ctx.v_atasans
                        where p.nrp == nrpAtasan
                        select new SelectListItem
                        {
                            id = p.nrp,
                            text = p.namaLengkap
                        }).FirstOrDefault();

            return data;
        }

        public List<string> ApprovalValidation(object sender, EventArgs e)
        {
            List<string> errorMessage = new List<string>();
            for (int i = 0; i < gvApproval.Rows.Count; i++)
            {
                var atasan = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txNrpApproval");
                var descr = (Label)gvApproval.Rows[i].Cells[0].FindControl("lblDeskripsi");
                if (string.IsNullOrEmpty(atasan.Text)) errorMessage.Add("Dimohon untuk mengisi approval atasan :" + descr.Text);
            }


            return errorMessage;
        }

        public bool ApprovalInsert(object sender, EventArgs e)
        {
            var sessionn = FLDgamode.Value;

            if (sessionn.Equals("true"))
            {
                return true;
            }
            try
            {
                var noSpd = txtNoSPD.Text;

                var existing = ctx.ApprovalStatus.Where(o => o.NoSPD == noSpd).ToList();

                if (existing.Count > 0)
                {
                    ctx.ApprovalStatus.DeleteAllOnSubmit(existing);
                    ctx.SubmitChanges();
                }

                List<ApprovalStatus> listAppS = new List<ApprovalStatus>();

                for (int i = 0; i < gvApproval.Rows.Count; i++)
                {
                    ApprovalStatus appSnew = new ApprovalStatus();

                    var ruleId = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txRuleID");
                    var atasan = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txNrpApproval");
                    var index = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txIndexLevel");

                    var atasanData = (from p in ctx.msKaryawans
                                      where p.nrp == atasan.Text
                                      select p).FirstOrDefault();

                    appSnew.NoSPD = noSpd;
                    appSnew.RuleID = Convert.ToInt32(ruleId.Text);
                    appSnew.NrpApproval = atasan.Text;
                    appSnew.Nama = atasanData.namaLengkap;
                    appSnew.Email = atasanData.EMail;
                    appSnew.IndexLevel = Convert.ToInt32(index.Text);

                    listAppS.Add(appSnew);
                }

                // set first index level for claim email
                var atasanPertama = listAppS.OrderBy(o => o.IndexLevel).FirstOrDefault();
                nrpAtasanFirst.Value = atasanPertama.NrpApproval;
                emailAtasanFirst.Value = atasanPertama.Email;

                ctx.ApprovalStatus.InsertAllOnSubmit(listAppS);
                ctx.SubmitChanges();

                if (listAppS.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception x)
            {
                string error = x.Message.ToString();
                return false;
            }
        }

        public class SelectListItem
        {
            public string id { get; set; }
            public string text { get; set; }
        }
        #endregion

        void LstData_DataBound(object sender, EventArgs e)
        {
            // The code below will insert the item at 0'th index of the dropdownlist.
            cmbCompanyCode.Items.Insert(cmbCompanyCode.Items.Count, new ListItem("LAIN - LAIN", "0"));
            //cmbCompanyCode.Items.Insert(7, "Lain-Lain");
        }

        private void fillFormKaryawan(string strLoginID)
        {
            classSpd oSPD = new classSpd();
            msKaryawan karyawan = new msKaryawan(); ;
            //untuk non karyawan
            if (strLoginID.ToUpper() == "SPD")
            {
                txtGolongan.Text = "III";
                txtNrp.Text = "99999999";
                lblEmail.Visible = true;
                txtEmail.Visible = true;
                txtJabatan.Enabled = true;
                cmbGolongan.SelectedValue = "III";
                txtGolongan.Text = "III";
                //cmbGolongan.Enabled = false;
                //txtComCode.Enabled = true;
                //txtPA.Enabled = true;
                //txtPsa.Enabled = true;
                txtNamaLengkap.Enabled = true;
                txtJabatan.Enabled = true;
           
            }
            else
            {
                karyawan = oSPD.getKaryawan(strLoginID);
                txtNrp.Text = karyawan.nrp;
                txtNamaLengkap.Text = karyawan.namaLengkap;
                cmbGolongan.Enabled = true;
                if (string.IsNullOrEmpty(karyawan.golongan))
                {
                    karyawan.golongan = "I";
                }
                else
                {
                    karyawan.golongan = karyawan.golongan.Trim();
                    if (string.IsNullOrEmpty(karyawan.golongan))
                    {
                        karyawan.golongan = "I";
                    }
                }
                //txtGolongan.Text = karyawan.golongan;
                //cmbGolongan.Text = karyawan.golongan;
                //cmbGolongan.SelectedValue = karyawan.golongan;
                //cmbGolongan.Items.FindByValue(karyawan.golongan).Selected = true;
                txtJabatan.Text = karyawan.Job;
                txtComCode.Text = karyawan.companyCode;
                txtPA.Text = karyawan.personelArea;
                txtPsa.Text = karyawan.pSubArea;
             }
            txtGolongan_TextChanged(null, null);


        }


        private void GetFormVal(string mnrp)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            //msKaryawan karyawan = new msKaryawan();

            karyawanBySekretaris = (from q in data.msKaryawans
                                    where q.nrp == mnrp
                                    select q).FirstOrDefault();

            txtNrp.Text = karyawanBySekretaris.nrp;
            txtNamaLengkap.Text = karyawanBySekretaris.namaLengkap;
            //karena golongan tidak lagi diisi, maka diisi manual. kalau nntinya golongan sudah terisi di DB, bisa make kode dibawah ini
            //if (txtGolongan.Visible == true)
            //{
            //    txtGolongan.Text = karyawan.golongan;
            //}
            //else {
            //    cmbGolongan.Text = karyawan.golongan;
            //}           

            //if (string.IsNullOrEmpty(karyawanBySekretaris.golongan))
            //{
            //    karyawanBySekretaris.golongan = "I";
            //}
            //else
            //{
            //    karyawanBySekretaris.golongan = karyawanBySekretaris.golongan.Trim();
            //    if (string.IsNullOrEmpty(karyawanBySekretaris.golongan))
            //    {
            //        karyawanBySekretaris.golongan = "I";
            //    }
            //}
            //txtGolongan.Text = "I"; //set awal do
            //cmbGolongan.Items.FindByValue(txtGolongan.Text).Selected = true;
            txtJabatan.Text = karyawanBySekretaris.Job;
            txtComCode.Text = karyawanBySekretaris.companyCode;
            txtPA.Text = karyawanBySekretaris.personelArea;
            txtPsa.Text = karyawanBySekretaris.pSubArea;
            //txtGolongan_TextChanged(null, null);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> errorMessage = ApprovalValidation(sender, e);

            if (errorMessage.Count > 0)
            {
                string errorString = "Dimohon semua approval atasan diisi";
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"" + errorString + "\")</SCRIPT>");
            }
            else
            {
                if (Session["IDLogin"] == null)
                {
                    Response.Redirect("frmHome.aspx");
                }
                else
                {

                    dsSPDDataContext data = new dsSPDDataContext();
                    trSPD SPD = new trSPD();
                    msKaryawan karyawan = new msKaryawan();
                    btnSave.Enabled = true;
                    if (FLDgamode.Value.ToString().ToLower() == "false")
                    {
                        btnSubmit0.Enabled = true;
                    }

                    if (txtNoSPD.Text == string.Empty)
                    {

                        getFromForm(ref SPD);
                        if (chkUbah.Checked)
                        {
                            karyawan = (from k in data.msKaryawans
                                        where k.nrp == SPD.nrp
                                        select k).FirstOrDefault();
                            karyawan.golongan = cmbGolongan.Text;
                        }

                        //cr : ikhsan 2014-02-03 change methode generate number
                        //SPD.noSPD = generateNumber("SPD", 5, 0);
                        SPD.noSPD = ctx.sp_GenerateNoSpd().FirstOrDefault().number;

                        txtNoSPD.Text = SPD.noSPD;
                        //cr : 2014-12-9  ixan method
                        bool approvalMethod = ApprovalInsert(sender, e);
                        if (approvalMethod)
                        {
                            // set approval 1 level
                            SPD.nrpAtasan = nrpAtasanFirst.Value;
                            SPD.isSubmit = null;
                            SPD.isApproved = null;

                            data.trSPDs.InsertOnSubmit(SPD);
                            data.SubmitChanges();

                            lblStat.Text = "SPD berhasil di-Save";
                        }
                        else
                        {
                            //lblStat.Text = "SPD gagal di-Save, pastikan cara inputnya sesuai urutan dari atas kebawah, dan approval atasan semuanya harus terisi.";
                            lblStat.Text = "Error Save Approval, Pastikan semua approval atasan diisi";
                            txtNoSPD.Text = string.Empty;
                        }
                    }
                    else
                    {
                        SPD = (from p in data.trSPDs
                               where p.noSPD == txtNoSPD.Text.Trim()
                               select p).FirstOrDefault();
                        getFromForm(ref SPD);
                        ////CR : ian 2014-12-9 modify cabang/ho ambil dari login user
                        //var userInfo = (from k in data.msKaryawans
                        //                where k.nrp == SPD.nrp
                        //                select k).FirstOrDefault();
                         ////end CR
                        if (chkUbah.Checked)
                        {
                            karyawan = (from k in data.msKaryawans
                                        where k.nrp == SPD.nrp
                                        select k).FirstOrDefault();
                            karyawan.golongan = cmbGolongan.Text;
                        }

                        //cr : 2014-12-9  ixan method
                        bool approvalMethod = ApprovalInsert(sender, e);

                        if (approvalMethod)
                        {
                            SPD.nrpAtasan = nrpAtasanFirst.Value;
                            data.SubmitChanges();
                            lblStat.Text = "SPD berhasil di-Save";
                        }
                        else
                        {
                            lblStat.Text = "SPD gagal di-Save, pastikan cara inputnya sesuai urutan dari atas kebawah, dan approval atasan semuanya harus terisi.";
                        }
                    }
                }
            }


        }

        public string generateNumber(string prefix, int length, int alreadyExist)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            string strId = prefix + "/" + DateTime.Today.Year + "/" + DateTime.Today.Month + "/";
            // hitung jumlah row
            int jumlah = (from p in data.trSPDs
                          where p.noSPD.StartsWith(prefix + "/" + DateTime.Today.Year + "/" + DateTime.Today.Month + "/")
                          select p.noSPD).Count() + 1 + alreadyExist;
            //sesuaikan dengan panjang akhiran belakang
            for (int i = 1; i <= length - jumlah.ToString().Length; i++)
            {
                strId += "0";
            }

            //nambal nomor serilal
            strId += jumlah.ToString();
            //cek apakah no sudah ada
            int hasil = (from p in data.trSPDs
                         where p.noSPD == strId
                         select p.noSPD).Count();
            if (hasil > 0)
            {
                strId = generateNumber(prefix, length, alreadyExist + 1);
            }
            return strId;

        }
        private void getFromForm(ref trSPD SPD)
        {
            //CR : ian 2014-12-9 changed, cabang/ho ambil dari login user
            if (rdbHO.Checked)
            {
                SPD.asal = "HO";
            }
            else
                SPD.asal = "Cabang";
            //SPD.asal = hdfHOCabang.Value;
            //end CR


            // tanggal\datetime dibuat dan diubah oleh
            DateTime tgl = DateTime.Now;
            SPD.diubahTanggal = tgl;
            SPD.diubahOleh = txtNrp.Text;
            if (string.IsNullOrEmpty(SPD.noSPD))
            {
                SPD.dibuatTanggal = tgl;
                SPD.dibuatOleh = txtNrp.Text;
            }

            SPD.noSPD = txtNoSPD.Text;
            SPD.nrp = txtNrp.Text;
            SPD.namaLengkap = txtNamaLengkap.Text;
            SPD.idGolongan = cmbGolongan.SelectedValue; //txtGolongan.Text;
            SPD.posisi = ddlPosisi.SelectedValue;
            SPD.jabatan = txtJabatan.Text;
            SPD.isHotel = cbHotel.Checked;
            // SPD.divisi = txtDivisi.Text;
            //SPD.departemen = txtDepartemen.Text;

            if (txtTempatTujuanLain.Text != string.Empty)
            {
                SPD.tempatTujuanLain = txtTempatTujuanLain.Text;
            }

            if (txtTempatTujuanLain.Text != string.Empty && (cmbCompanyCode.SelectedValue == string.Empty && cmbTempatTujuan.SelectedValue == string.Empty && cmbSubArea.SelectedValue == string.Empty))
            {
                SPD.tempatTujuanLain = txtTempatTujuanLain.Text;
            }
            else
            {

                SPD.coCdTujuan = cmbCompanyCode.SelectedValue;
                SPD.companyCodeTujuan = cmbCompanyCode.SelectedItem.Text;
                SPD.kodePATujuan = cmbTempatTujuan.SelectedValue;
                SPD.personelAreaTujuan = cmbTempatTujuan.SelectedItem.Text;
                SPD.kodePSubAreaTujuan = cmbSubArea.SelectedValue;
                SPD.pSubAreaTujuan = cmbSubArea.SelectedItem.Text;
                // else SPD.idCabangTujuan = cmbTempatTujuan.SelectedValue;
            }
            //if (txtKeperluanLain.Text != string.Empty)
            //{
            //    SPD.keperluanLain = txtKeperluanLain.Text;
            //}
            //else 
            SPD.idKeperluan = Convert.ToInt32(cmbKeperluan.SelectedValue);
            SPD.ketKeperluan = txtKetKeperluan.Text;

            if (txtKeterangan.Text != string.Empty)
            {
                SPD.keterangan = txtKeterangan.Text;
            }

            if (rbDicarikan.Checked)
            {
                SPD.tiket = "Dicarikan";
            }
            else SPD.tiket = "Sendiri";
            SPD.tglBerangkat = Convert.ToDateTime(txtTglBerangkat.Text);
            SPD.jamBerangkat = cmbJamBerangkat0.Text;
            SPD.menitBerangkat = cmbMenitBerangkat0.Text;
            SPD.tglKembali = Convert.ToDateTime(txtTglKembali.Text);
            SPD.jamKembali = cmbJamKembali.Text;
            SPD.menitKembali = cmbMenitKembali.Text;
            if (txtAngkutanLain.Text != string.Empty)
            {
                SPD.angkutanLain = txtAngkutanLain.Text;
            }
            else SPD.idAngkutan = Convert.ToInt32(cmbAngkutan.SelectedValue);

            if (rbDisediakan.Checked)
            {
                SPD.penginapan = "Disediakan";
            }
            else
            {
                SPD.penginapan = "Tidak Disediakan";
                SPD.nrpApprovalGA = cmbxTujuan.SelectedItem.Value.ToString();
            }
            //SPD.nrpAtasan = cmbxAtasan.SelectedItem.Value.ToString();
            if (cmbCompanyCode.SelectedValue == "0")
                SPD.nrpApprovalTujuan = cmbxTujuan.SelectedItem.Value.ToString();
            else
                SPD.nrpApprovalTujuan = cmbxTujuan.SelectedItem.Value.ToString();
            SPD.uangMuka = txtUangMuka.Text;

            if (txtCostCenter.SelectedItem != null)
            {
                SPD.costCenter = txtCostCenter.SelectedItem.Value.ToString();
            }
            else
            {
                SPD.costCenter = txtCostCenter.Text;
            }


            SPD.NoHP = txtNoHp.Text;
            SPD.ketKeperluan = txtKetKeperluan.Text;

            if (FLDgamode.Value.ToString().ToLower() == "false")
            {
                SPD.status = "Save"; //"1-Save";
            }


            if (rdDalamNegeri.Checked)
            {
                SPD.Tujuan = "Dalam Negeri";
                SPD.WilayahTujuan = "Seluruh Indonesia";
            }
            else
            {
                SPD.Tujuan = "Luar Negeri";
                SPD.WilayahTujuan = ddlWilayah.SelectedItem.Text;
            }
            SPD.email = txtEmail.Text;
            SPD.posisi = ddlPosisi.SelectedValue;


        }
        private void SetForm(trSPD SPD)
        {
            if (SPD.asal.ToLower() == "ho")
            {
                rdbHO.Checked = true;
            }
            else if (SPD.asal.ToLower() == "cabang")
            {
                rdbCbg.Checked = true;
            }
            //hdfHOCabang.Value = SPD.asal;

            txtNrp.Text = SPD.nrp;
            txtNamaLengkap.Text = SPD.namaLengkap;
            txtGolongan.Text = SPD.idGolongan.ToString();
            cmbGolongan.SelectedValue = SPD.idGolongan.ToString();
            txtGolongan_TextChanged(null, null);
            txtJabatan.Text = SPD.jabatan;

            //txtDivisi.Text = SPD.divisi;
            //txtDepartemen.Text = SPD.departemen;

            if (SPD.tempatTujuanLain != null)
            {
                txtTempatTujuanLain.Text = SPD.tempatTujuanLain;
                //}


                //if (SPD.tempatTujuanLain != null && SPD.coCdTujuan == null && SPD.kodePATujuan == null && SPD.kodePSubAreaTujuan == null)
                //{
                //txtTempatTujuanLain_TextChanged(null, null);
                cmbCompanyCode.DataBind();
                cmbCompanyCode.SelectedValue = SPD.coCdTujuan;
                cmbCompanyCode_SelectedIndexChanged(null, null);
            }
            else
            {
                //cmbCompanyCode.DataBind();
                //cmbxTujuan.DataBind();
                cmbCompanyCode.SelectedValue = SPD.coCdTujuan;
                TujuanHid.Value = SPD.kodePATujuan.ToString();
                SaHid.Value = SPD.kodePSubAreaTujuan.ToString();

                if (SPD.coCdTujuan != string.Empty)
                {
                    //cmbTempatTujuan.DataBind();
                    int num = cmbCompanyCode.Items.Count;

                }

                if (cmbTempatTujuan.Items.Count > 0)
                {
                    cmbTempatTujuan.SelectedValue = SPD.kodePATujuan.ToString();

                    //cmbSubArea.DataBind();
                }
                if (cmbSubArea.Items.Count > 0)
                {
                    cmbSubArea.SelectedValue = SPD.kodePSubAreaTujuan.ToString();
                }

            }
            // cmbTempatTujuan.SelectedValue=SPD.idCabangTujuan;
            //if (SPD.keperluanLain != null)
            //txtKeperluanLain.Text = SPD.keperluanLain;
            //else

            cmbKeperluan.SelectedValue = SPD.idKeperluan.ToString();
            txtKetKeperluan.Text = SPD.ketKeperluan;
            txtKeterangan.Text = SPD.keterangan;

            if (SPD.tiket.ToLower() == "dicarikan")
            {
                rbDicarikan.Checked = true;
            }
            else if (SPD.tiket.ToLower() == "sendiri")
            {
                rbSendiri.Checked = true;
            }
            txtTglBerangkat.Text = SPD.tglBerangkat.ToShortDateString();
            cmbJamBerangkat0.SelectedValue = SPD.jamBerangkat.Trim();
            cmbMenitBerangkat0.SelectedValue = SPD.menitBerangkat.Trim();
            txtTglKembali.Text = SPD.tglKembali.ToShortDateString(); ;
            cmbJamKembali.SelectedValue = SPD.jamKembali;
            cmbJamKembali.SelectedValue = SPD.menitKembali;

            #region cr : ian enable angkutan lain2
            cmbAngkutan.DataBind();
            cmbAngkutan.SelectedValue = SPD.idAngkutan.ToString();
            cmbAngkutan_SelectedIndexChanged(null, null);

            //if (SPD.angkutanLain != null)
            //    txtAngkutanLain.Text = SPD.angkutanLain;
            //else
            //    cmbAngkutan.SelectedValue = SPD.idAngkutan.ToString();
            #endregion
            
            if (SPD.penginapan.ToLower() == "disediakan")
            {
                rbDisediakan.Checked = true;
            }
            else if (SPD.penginapan.ToLower() == "tidak disediakan")
            {
                rbTidakDisediakan.Checked = true;
            }
            txtUangMuka.Text = SPD.uangMuka;
            //cmbxAtasan.Value = SPD.nrpAtasan.Trim();
            cmbxTujuan.SelectedValue = SPD.nrpApprovalTujuan.Trim();
            ViewState["cmbxTujuan"] = SPD.nrpApprovalTujuan.Trim();
            txtNoHp.Text = SPD.NoHP;
            txtCostCenter.Text = SPD.costCenter;
            txtKetKeperluan.Text = SPD.ketKeperluan;

            if (SPD.Tujuan.ToLower() == "dalam negeri")
            {
                rdDalamNegeri.Checked = true;
            }
            else
            {
                rbLuarNegeri.Checked = true;
                ddlWilayah.SelectedValue = SPD.WilayahTujuan;
            }
            txtEmail.Text = SPD.email;
            cbHotel.Checked = SPD.isHotel == null ? false : SPD.isHotel.Value;
            ddlPosisi.SelectedValue = SPD.posisi;


            btnUpdate_Click(null, null);
        }
        private void clearForm()
        {
            try
            {
                rdbHO.Checked = true;
                //txtComCode.Text = string.Empty;
                txtNoSPD.Text = string.Empty;
                //txtPA.Text = string.Empty;
                // txtPsa.Text = string.Empty;
                //txtNrp.Text =string.Empty;
                //txtNamaLengkap.Text = string.Empty;
                //txtGolongan.Text = string.Empty;
                // txtJabatan.Text = string.Empty;
                //txtDivisi.Text = string.Empty;
                //txtDepartemen.Text = string.Empty;
                txtTempatTujuanLain.Text = string.Empty;
                cmbTempatTujuan.SelectedIndex = 0;
                cmbCompanyCode.SelectedIndex = 0;
                cmbSubArea.SelectedIndex = 0;
                txtTempatTujuanLain.Text = string.Empty;
                cmbKeperluan.SelectedIndex = 0;
                rbDicarikan.Checked = true;
                txtTglBerangkat.Text = DateTime.Now.ToShortDateString();
                cmbJamBerangkat0.SelectedIndex = 0;
                cmbMenitBerangkat0.SelectedIndex = 0;
                txtTglKembali.Text = DateTime.Now.ToShortDateString();
                cmbJamKembali.SelectedIndex = 0;
                cmbJamKembali.SelectedIndex = 0;
                txtAngkutanLain.Text = string.Empty;
                cmbAngkutan.SelectedIndex = 0;
                rbDisediakan.Checked = true;
                txtUangMuka.Text = string.Empty;
                //txtMakan.Text = string.Empty;
                //txtTransportasi.Text = string.Empty;
                //txtUangSaku.Text = string.Empty;
                txtNoHp.Text = string.Empty;
                //  txtCostCenter.Text = string.Empty;
                txtKetKeperluan.Text = string.Empty;
                btnSave.Enabled = true;
                btnSubmit0.Enabled = false;

                txtEmail.Text = string.Empty;

            }
            catch (Exception) { }


        }

        private void setIntoForm(ref trSPD SPD)
        {
            txtNrp.Text = SPD.nrp;
        }

        protected void btnSubmit0_Click(object sender, EventArgs e)
        {
            List<string> errorMessage = ApprovalValidation(sender, e);

            if (errorMessage.Count > 0)
            {
                string errorString = string.Empty;

                foreach (var item in errorMessage)
                {
                    errorString += item + "\n";
                }

                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"" + errorString + "\")</SCRIPT>");
            }
            else
            {
                if (Session["IDLogin"] == null)
                {
                    Response.Redirect("frmHome.aspx");
                }
                else
                {
                    if (txtNoSPD.Text != string.Empty)
                    {
                        if (txtNrp.Text != string.Empty)
                        {
                            dsSPDDataContext datas = new dsSPDDataContext();
                            trSPD spd = (from p in datas.trSPDs
                                         where p.noSPD == txtNoSPD.Text
                                         select p).FirstOrDefault();
                            msKaryawan kary = new msKaryawan();
                            if (txtNrp.Text == "99999999")
                            {
                                kary.EMail = spd.email;
                                kary.nrp = txtNrp.Text;
                                kary.namaLengkap = spd.namaLengkap;
                                kary.golongan = "III";
                                kary.Job = spd.jabatan;
                                kary.posisi = spd.jabatan;
                                kary.coCd = "0100";
                                kary.kodePSubArea = "0001";
                                kary.kodePA = "0001";

                            }
                            else
                            {
                                kary = (from kar in datas.msKaryawans
                                        where kar.nrp == spd.nrp
                                        select kar).FirstOrDefault();
                            }

                            try
                            {

                                //cr : 2014-12-9  ixan method
                                bool approvalMethod = ApprovalInsert(sender, e);
                                if (approvalMethod)
                                {
                                    // set approval 1 level
                                    spd.status = "Menunggu Approval " + datas
                                        .ApprovalStatus
                                        .FirstOrDefault(o => o.NoSPD == spd.noSPD && o.IndexLevel == 1)
                                        .ApprovalRule.Deskripsi;

                                    spd.isSubmit = true;
                                    spd.isSubmitDate = DateTime.Now;
                                    spd.nrpAtasan = nrpAtasanFirst.Value;
                                    datas.SubmitChanges();
                                    btnSubmit0.Enabled = false;
                                    btnSave.Enabled = false;
                                    lblStat.Text = "SPD berhasil di-Submit";

                                    EmailCore.ApprovalSPD(nrpAtasanFirst.Value, spd.noSPD, "1", spd);

                                    //classSpd oSpd = new classSpd();
                                    //oSpd.sendMail(spd, "Atasan", kary);
                                }
                                else
                                {
                                    lblStat.Text = "SPD gagal di-Submit, cek koneksi pada internet, atau dicoba lagi input data dengan benar sesuai urutan dari atas ke bawah, dan data atasan harus terisi.";
                                }

                            }
                            catch (Exception ex)
                            {
                                Response.Write(ex.Message);
                            }
                            finally
                            {
                                datas.Dispose();
                            }
                        }
                        else
                            Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"Simpan SPD terlebih dahulu\")</SCRIPT>");
                    }
                }
            }
        }

        protected void txtNoSPD_TextChanged(object sender, EventArgs e)
        {
            setForm();
        }

        private void setForm()
        {
            dsSPDDataContext data = new dsSPDDataContext();
            trSPD spd = (from p in data.trSPDs
                         where p.noSPD.Equals(txtNoSPD.Text)
                         select p).FirstOrDefault();
            SetForm(spd);
        }

        protected void rbTidakDisediakan_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void rbDisediakan_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void btnReset0_Click(object sender, EventArgs e)
        {
            //clearForm();
            // Page_Load(null, null);
            Response.Redirect("frmRequestInput.aspx");
        }


        protected void txtGolongan_TextChanged(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            string jenis = "Dalam Negeri";
            string wilayah = ddlWilayah.SelectedValue;
            if (rbLuarNegeri.Checked)
            {
                jenis = "Luar Negeri";
            }
            try
            {
                if (txtNoSPD.Text == string.Empty)
                {
                    var query = from p in data.msGolonganPlafons
                                where p.golongan.Equals(txtGolongan.Text.Trim()) && p.jenisSPD == jenis
                                select p;
                    //List<msGolonganPlafon> plafons = query.ToList<msGolonganPlafon>();
                    List<msGolonganPlafon> plafon = query.Where(p => p.idPlafon == 1).ToList<msGolonganPlafon>();
                    if (plafon.Count > 0) txtMakan.Text = plafon.First().harga.ToString();
                    plafon = query.Where(p => p.idPlafon == 2).ToList<msGolonganPlafon>();
                    if (plafon.Count > 0) txtUangSaku.Text = plafon.First().harga.ToString();
                    plafon = query.Where(p => p.idPlafon == 3).ToList<msGolonganPlafon>();
                    if (plafon.Count > 0) txtTransportasi.Text = plafon.First().deskripsi.ToString();
                }
                else
                {

                    trSPD spd = (from p in data.trSPDs
                                 where p.noSPD.Equals(txtNoSPD.Text)
                                 select p).FirstOrDefault();
                    wilayah = spd.WilayahTujuan;

                    var query = from p in data.msGolonganPlafons
                                where p.golongan.Equals(txtGolongan.Text.Trim()) && p.jenisSPD == jenis && p.wilayah == wilayah
                                select p;
                    //List<msGolonganPlafon> plafons = query.ToList<msGolonganPlafon>();
                    List<msGolonganPlafon> plafon = query.Where(p => p.idPlafon == 1).ToList<msGolonganPlafon>();
                    if (plafon.Count > 0) txtMakan.Text = plafon.First().harga.ToString();
                    plafon = query.Where(p => p.idPlafon == 2).ToList<msGolonganPlafon>();
                    if (plafon.Count > 0) txtUangSaku.Text = plafon.First().harga.ToString();
                    plafon = query.Where(p => p.idPlafon == 3).ToList<msGolonganPlafon>();
                    if (plafon.Count > 0) txtTransportasi.Text = plafon.First().deskripsi.ToString();
                }


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void cmbTempatTujuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaHid.Value = string.Empty;

            if (cmbSubArea.SelectedValue != string.Empty && cmbTempatTujuan.SelectedValue != string.Empty && cmbCompanyCode.SelectedValue != string.Empty)
            {
                Validate5.Enabled = false;
                L5.Visible = false;
            }
            else
            {
                Validate4.Enabled = true;
                L4.Visible = true;
                Validate1.Enabled = true;
                L1.Visible = true;
                Validate2.Enabled = true;
                L2.Visible = true;
            }
        }

        protected void cmbCompanyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCompanyCode.SelectedValue == "0")
            {
                cmbTempatTujuan.Enabled = false;
                cmbSubArea.Enabled = false;
                L4.Visible = false;
                Validate4.Visible = false;
                L2.Visible = false;
                Validate2.Visible = false;

                txtTempatTujuanLain.Visible = true;
                lblTmpLain.Visible = true;
                L5.Visible = true;
                Validate5.Visible = true;

                if (IsPostBack)
                {
                    cmbSubArea.SelectedValue = "";
                    txtTempatTujuanLain.Text = "";
                    ldAppTujuan.DataBind();
                    cmbxTujuan.DataBind();
                }

            }
            else
            {
                cmbTempatTujuan.Enabled = true;
                cmbSubArea.Enabled = true;
                L4.Visible = true;
                Validate4.Visible = true;
                L2.Visible = true;
                Validate2.Visible = true;

                txtTempatTujuanLain.Visible = false;
                lblTmpLain.Visible = false;
                L5.Visible = false;
                Validate5.Visible = false;

                cmbxTujuan.SelectedValue = "";
                cmbxTujuan.Enabled = true;
            }

            TujuanHid.Value = string.Empty;
            //cmbTempatTujuan_SelectedIndexChanged(null, null);
        }

        protected void LinqDSSubCabang_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //string tempatTujuan = "";
            if (txtNoSPD.Text != string.Empty && SaHid.Value != string.Empty)
            {


                dsSPDDataContext data = new dsSPDDataContext();
                trSPD spd = (from p in data.trSPDs
                             where p.noSPD.Equals(txtNoSPD.Text)
                             select p).FirstOrDefault();
                //if (spd.kodePATujuan == "TRAC")
                //{ tempatTujuan = "TRAC LEASING"; }
                //else tempatTujuan = spd.kodePATujuan;
                var user = (from k in data.v_karyawanTracs
                            where k.kodePA == spd.kodePATujuan
                            select new
                            {
                                kodePSubArea = k.kodePSubArea,
                                pSubArea = k.pSubArea,
                                kodePA = k.kodePA
                            }).Distinct();
                e.Result = user;

                cmbSubArea.SelectedValue = SaHid.Value.ToString();
            }
            else
            {
                dsSPDDataContext data = new dsSPDDataContext();

                //if (cmbTempatTujuan.SelectedValue == "1510")
                //{
                //    tempatTujuan = "1520";
                //}
                //else tempatTujuan = cmbTempatTujuan.SelectedValue;
                var user = (from k in data.v_karyawanTracs
                            where k.kodePA == cmbTempatTujuan.SelectedValue
                            select new
                            {
                                kodePSubArea = k.kodePSubArea,
                                pSubArea = k.pSubArea,
                                kodePA = k.kodePA
                            }).Distinct();
                e.Result = user;
            }

            txtTempatTujuanLain_TextChanged(null, null);
        }

        protected void txtTglKembali_TextChanged(object sender, EventArgs e)
        {
            TimeSpan Jumlahhari = (Convert.ToDateTime(txtTglKembali.Text) - Convert.ToDateTime(txtTglBerangkat.Text));
            //if (Jumlahhari.Days < 2)
            //{
            //    txtUangMuka.Text = string.Empty;
            //    txtUangMuka.Enabled = false;
            //}
            //else
            //    txtUangMuka.Enabled = true;
            //if (Convert.ToDateTime(txtTglKembali.Text) < Convert.ToDateTime(txtTglBerangkat.Text))
            //{
            //    txtTglKembali.Text = txtTglBerangkat.Text;
            //}


            if (Jumlahhari.Days >= 5)
            {
                txtUangMuka.Enabled = true;
            }
            else
            {
                txtUangMuka.Enabled = false;
            }

        }

        protected void txtTglBerangkat_TextChanged(object sender, EventArgs e)
        {
            TimeSpan Jumlahhari = (Convert.ToDateTime(txtTglKembali.Text) - Convert.ToDateTime(txtTglBerangkat.Text));
            //if (Jumlahhari.Days < 2)
            //{
            //    txtUangMuka.Text = string.Empty;
            //    txtUangMuka.Enabled = false;
            //}
            //else
            //    txtUangMuka.Enabled = true;
            //if (txtTglKembali.Text == string.Empty)
            //{
            //    txtTglKembali.Text = txtTglBerangkat.Text;
            //}

            if (Jumlahhari.Days >= 5)
            {
                txtUangMuka.Enabled = true;
            }
            else
            {
                txtUangMuka.Enabled = false;
            }
        }

        protected void LinqDSNRPTujuan_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

        }

        protected void cmbTempatTujuan_TextChanged(object sender, EventArgs e)
        {
            cmbSubArea.DataBind();
        }
        protected void cmbSubArea_TextChanged(object sender, EventArgs e)
        {

        }
        protected void cmbTempatTujuan_DataBound(object sender, EventArgs e)
        {
            if (Session["noSPD"] == null || (string)Session["noSPD"] == "")
            {
                cmbTempatTujuan.Items.Insert(0, new ListItem("", string.Empty));

            }
        }

        protected void cmbSubArea_DataBound(object sender, EventArgs e)
        {
            if (Session["noSPD"] == null || (string)Session["noSPD"] == "")
            {
                cmbSubArea.Items.Insert(0, new ListItem("", string.Empty));
            }
        }



        protected void cmbxAtasan_DataBound(object sender, EventArgs e)
        {
            //Request Pengecualian penambahan approval atasan
            //if (txtNrp.Text != "99999999")
            //{
            //    if (txtPsa.Text.Contains("AFTER SALES JKT"))
            //    {
            //        cmbxAtasan.Items.Add("MOHAMMAD SAHRI", "146");
            //    }
            //    if (txtJabatan.Text.Contains("Division Head"))
            //    {
            //        cmbxAtasan.Items.Add("NI LUH WAYAN PURNIWATI", "76");
            //    }
            //    if (txtJabatan.Text.Contains("General Manager"))
            //    {
            //        cmbxAtasan.Items.Add("NENENG LATIFAH", "296");
            //    }
            //}
        }

        protected void cmbAtasan_DataBound(object sender, EventArgs e)
        {
            if (Session["noSPD"] == null || (string)Session["noSPD"] == "")
            {
                //cmbAtasan.Items.Insert(0, new ListItem("", string.Empty));
            }
        }

        protected void cmbAppTujuan_DataBound(object sender, EventArgs e)
        {
            if (Session["noSPD"] == null || (string)Session["noSPD"] == "")
            {
                //cmbAppTujuan.Items.Insert(0, new ListItem("", string.Empty));
            }
        }

        protected void rbLuarNegeri_CheckedChanged(object sender, EventArgs e)
        {
            plafomLuarNegeri();
            ddlWilayah.Enabled = true;
            btnUpdate_Click(sender, e);
        }

        protected void rdDalamNegeri_CheckedChanged(object sender, EventArgs e)
        {
            txtGolongan_TextChanged(null, null);
            ddlWilayah.Enabled = false;
            btnUpdate_Click(sender, e);
        }

        protected void ddlWilayah_SelectedIndexChanged(object sender, EventArgs e)
        {
            plafomLuarNegeri();
        }

        private void plafomLuarNegeri()
        {
            dsSPDDataContext data = new dsSPDDataContext();
            string jenis = "Dalam Negeri";
            if (rbLuarNegeri.Checked)
            {
                jenis = "Luar Negeri";
            }
            try
            {
                var query = from p in data.msGolonganPlafons
                            where p.golongan.Equals(txtGolongan.Text.Trim()) && p.jenisSPD == jenis && p.wilayah == ddlWilayah.SelectedItem.Text
                            select p;
                //List<msGolonganPlafon> plafons = query.ToList<msGolonganPlafon>();
                List<msGolonganPlafon> plafon = query.Where(p => p.idPlafon == 1).ToList<msGolonganPlafon>();
                if (plafon.Count > 0) txtMakan.Text = plafon.First().harga.ToString();
                plafon = query.Where(p => p.idPlafon == 2).ToList<msGolonganPlafon>();
                if (plafon.Count > 0) txtUangSaku.Text = plafon.First().harga.ToString();
                plafon = query.Where(p => p.idPlafon == 3).ToList<msGolonganPlafon>();
                if (plafon.Count > 0) txtTransportasi.Text = plafon.First().deskripsi.ToString();
            }
            catch (Exception ex)
            {
                //debug
                Response.Write(ex.Message);
            }
        }

        private void plafomDalamNegeri()
        {
            dsSPDDataContext data = new dsSPDDataContext();
            string jenis = "Dalam Negeri";
            if (rbLuarNegeri.Checked)
            {
                jenis = "Luar Negeri";
            }
            try
            {
                var query = from p in data.msGolonganPlafons
                            where p.golongan.Equals(txtGolongan.Text.Trim()) && p.jenisSPD == jenis
                            select p;
                //List<msGolonganPlafon> plafons = query.ToList<msGolonganPlafon>();
                List<msGolonganPlafon> plafon = query.Where(p => p.idPlafon == 1).ToList<msGolonganPlafon>();
                if (plafon.Count > 0) txtMakan.Text = plafon.First().harga.ToString();
                plafon = query.Where(p => p.idPlafon == 2).ToList<msGolonganPlafon>();
                if (plafon.Count > 0) txtUangSaku.Text = plafon.First().harga.ToString();
                plafon = query.Where(p => p.idPlafon == 3).ToList<msGolonganPlafon>();
                if (plafon.Count > 0) txtTransportasi.Text = plafon.First().deskripsi.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void ldsUser_Selecting1(object sender, LinqDataSourceSelectEventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();

            var user = (from u in data.msUsers
                        join k in data.msKaryawans on u.nrp equals k.nrp
                        where u.roleId <= 14
                        orderby k.namaLengkap
                        select new
                        {
                            namaLengkap = k.namaLengkap,
                            nrp = k.nrp
                        }).Distinct();
            e.Result = user;

        }

        protected void ldAppAtasan_Selecting1(object sender, LinqDataSourceSelectEventArgs e)
        {
            //dsSPDDataContext data = new dsSPDDataContext();

            //msKaryawan personalArea = (from u in data.msKaryawans
            //                           where u.nrp == txtNrp.Text
            //                           select u).FirstOrDefault();

            //var user = (from k in data.v_atasans
            //            where k.kodePA == personalArea.kodePA
            //            orderby k.namaLengkap
            //            select new
            //            {
            //                namaLengkap = k.namaLengkap,
            //                nrp = k.nrp,
            //            }).Distinct();
            //e.Result = user;

            e.Result = ApprovalAtasan();

        }
        protected void ldAppTujuan_Selecting1(object sender, LinqDataSourceSelectEventArgs e)
        {

            //dsSPDDataContext data = new dsSPDDataContext();
            //var user = (from k in data.v_atasans
            //            where k.kodePSubArea == cmbSubArea.SelectedValue
            //            orderby k.namaLengkap
            //            select new
            //            {
            //                namaLengkap = k.namaLengkap,
            //                nrp = k.nrp,
            //                kodePSubArea = k.kodePSubArea
            //            }).Distinct();

            //e.Result = user;

            dsSPDDataContext data = new dsSPDDataContext();
            var user = (object)null;
            if (txtNoSPD.Text != string.Empty && SaHid.Value != string.Empty)
            {
                if (cmbCompanyCode.SelectedItem.Text == "LAIN - LAIN")
                {
                    user = (from k in data.v_atasan_tujuans
                            //where k.kodePSubArea == cmbSubArea.SelectedValue
                            where k.nrp != txtNrp.Text
                            orderby k.namaLengkap
                            select new
                            {
                                namaLengkap = k.namaLengkap,
                                nrp = k.nrp,
                                kodePSubArea = k.kodePSubArea
                            }).Distinct();
                    e.Result = user;
                }
                else
                {
                    user = (from k in data.v_atasan_tujuans
                            //where k.kodePSubArea == SaHid.Value
                            where k.nrp != txtNrp.Text
                            orderby k.namaLengkap
                            select new
                            {
                                namaLengkap = k.namaLengkap,
                                nrp = k.nrp,
                                kodePSubArea = k.kodePSubArea
                            }).Distinct();
                }
                e.Result = user;
                cmbxTujuan.SelectedValue = Convert.ToString(ViewState["cmbxTujuan"]);
            }
            else
            {
                if (cmbCompanyCode.SelectedItem.Text == "LAIN - LAIN")
                {
                    user = (from k in data.v_atasan_tujuans
                            //where k.kodePSubArea == cmbSubArea.SelectedValue
                            //where k.nrp != txtNrp.Text
                            orderby k.namaLengkap
                            select new
                            {
                                namaLengkap = k.namaLengkap,
                                nrp = k.nrp,
                                kodePSubArea = k.kodePSubArea
                            }).Distinct();
                    ldAppTujuan.Where = "";
                    ldAppTujuan.WhereParameters.Clear();
                    e.Result = user;
                }
                else
                {
                    user = (from k in data.v_atasan_tujuans
                            //where k.kodePSubArea == cmbSubArea.SelectedValue
                            where k.nrp != txtNrp.Text
                            orderby k.namaLengkap
                            select new
                            {
                                namaLengkap = k.namaLengkap,
                                nrp = k.nrp,
                                kodePSubArea = k.kodePSubArea
                            }).Distinct();
                    e.Result = user;
                }
            }


        }
        protected void chkUbah_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUbah.Checked)
            {
                txtGolongan.Visible = false;
                cmbGolongan.Visible = true;
            }
            else
            {
                txtGolongan.Visible = true;
                cmbGolongan.Visible = false;

            }
        }

        protected void cmbGolongan_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtGolongan.Text = cmbGolongan.SelectedValue;
            if (rdDalamNegeri.Checked)
            {
                plafomDalamNegeri();
            }
            else
            {
                plafomLuarNegeri();
            }

            ////CR : ian 2014-12-10
            //if (rdDalamNegeri.Checked)
            //{
            //    if (!string.IsNullOrEmpty(hdfHOCabang.Value))
            //    {
            //        createDynamicControl(cmbGolongan.SelectedItem.Value);
            //    }
            //}
            //else
            //{ 

            //}
            ////end CR
            btnUpdate_Click(sender, e);
        }

        protected void ddlPosisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
        }

        protected void IdsSec_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();

            var user = (from u in data.msUsers
                        join k in data.msKaryawans on u.nrp equals k.nrp
                        where u.roleId == 14 || u.roleId == 13
                        orderby k.namaLengkap
                        select new
                        {
                            namaLengkap = k.namaLengkap,
                            nrp = k.nrp
                        }).Distinct();
            e.Result = user;
            // cmbDireksi_CheckedChanged(null, null);
            //txtNrp.Text = ddlDireksi.Items[0].Value;
            //ddlDireksi_SelectedIndexChanged(null, null);
        }

        private bool cek_sec(string LoginID)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            classSpd oSPD = new classSpd();
            msKaryawan karyawan = new msKaryawan(); ;
            string t_nrp = oSPD.getKaryawan(LoginID).nrp;

            var user = (from u in data.msUsers
                        join k in data.msKaryawans on u.nrp equals k.nrp
                        where u.roleId == 23 && k.nrp == t_nrp
                        orderby k.namaLengkap
                        select new
                        {
                            namaLengkap = k.namaLengkap,
                            nrp = k.nrp
                        }).Distinct();
            if (user.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        protected void cmbDireksi_CheckedChanged(object sender, EventArgs e)
        {

            if (cmbDireksi.Checked)
            {
                ddlDireksi.Visible = true;
                txtNamaLengkap.Visible = false;

                if (ddlDireksi.SelectedItem != null)
                {
                    //txtNrp.Text = ddlDireksi.SelectedItem.Value.ToString();
                    ddlDireksi_SelectedIndexChanged(sender, e);
                }

                ValDir.Visible = true;
                ValidateDir.Enabled = true;

            }
            else
            {
                ddlDireksi.Visible = false;
                txtNamaLengkap.Visible = true;
                //txtNrp.Text = ddlDireksi.SelectedItem.Value.ToString();
                string strLoginID = (string)Session["IDLogin"];
                fillFormKaryawan(strLoginID);

                ValDir.Visible = false;
                ValidateDir.Enabled = false;

            }

            btnUpdate_Click(sender, e);
        }

        protected void ddlDireksi_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtNamaLengkap.Text = ddlDireksi.SelectedItem.Text.ToString();
            //txtNrp.Text = ddlDireksi.SelectedItem.Value.ToString();
            GetFormVal(ddlDireksi.SelectedItem.Value.ToString());
            btnUpdate_Click(sender, e);
        }

        protected void cmbSubArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubArea.SelectedValue != string.Empty && cmbTempatTujuan.SelectedValue != string.Empty && cmbCompanyCode.SelectedValue != string.Empty)
            {
                Validate5.Enabled = false;
                L5.Visible = false;

            }
            else
            {
                Validate4.Enabled = true;
                L4.Visible = true;
                Validate1.Enabled = true;
                L1.Visible = true;
                Validate2.Enabled = true;
                L2.Visible = true;
            }
            ldAppTujuan.DataBind();
            cmbxTujuan.DataBind();
        }

        protected void txtTempatTujuanLain_TextChanged(object sender, EventArgs e)
        {
            //if (txtTempatTujuanLain.Text != null && txtTempatTujuanLain.Text != "")
            //{
            //    Validate1.Enabled = false;
            //    L1.Visible = false;
            //    Validate2.Enabled = false;
            //    L2.Visible = false;
            //    Validate4.Enabled = false;
            //    L4.Visible = false;
            //    Validate5.Enabled = false;
            //    L5.Visible = false;
            //}
            //else
            //{
            //    Validate4.Enabled = true;
            //    L4.Visible = true;
            //    Validate1.Enabled = true;
            //    L1.Visible = true;
            //    Validate2.Enabled = true;
            //    L2.Visible = true;
            //    Validate5.Enabled = true;
            //    L5.Visible = true;
            //}

            //if (cmbCompanyCode.SelectedValue == "0")
            //{
            //    if (cmbxAtasan.SelectedItem != null)
            //    {
            //        //cmbxTujuan.Value = cmbxAtasan.SelectedItem.Text;
            //        //cmbxTujuan.Enabled = false;

            //    }
            //    else
            //    {
            //        cmbxTujuan.Value = "";
            //        cmbxTujuan.Enabled = true;
            //    }
            //}
            //else
            //{
            //    cmbxTujuan.Value = "";
            //    cmbxTujuan.Enabled = true;
            //}
            //if (cmbSubArea.SelectedValue != string.Empty && cmbTempatTujuan.SelectedValue != string.Empty && cmbCompanyCode.SelectedValue != string.Empty)
            //{
            //    Validate5.Enabled = false;
            //    L5.Visible = false;
            //}
            //else
            //{
            //    Validate4.Enabled = true;
            //    L4.Visible = true;
            //    Validate1.Enabled = true;
            //    L1.Visible = true;
            //    Validate2.Enabled = true;
            //    L2.Visible = true;
            //}


        }

        protected void cmbKeperluan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKeperluan.SelectedValue == "6")
            {
                L12.Visible = true;
                Validate12.Enabled = true;
            }
            else
            {
                L12.Visible = false;
                Validate12.Enabled = false;
            }
        }

        protected void cmbAngkutan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAngkutan.SelectedValue == "5")
            {
                lblAngkutanLain.Visible = true;
                txtAngkutanLain.Visible = true;
                ValLain.Visible = true;
                ValidateLain.Visible = true;
            }
            else
            {
                lblAngkutanLain.Visible = false;
                txtAngkutanLain.Visible = false;
                ValLain.Visible = false;
                ValidateLain.Visible = false;
            }
        }

        protected void LinqDSCabang_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (txtNoSPD.Text != string.Empty && TujuanHid.Value != string.Empty)
            {
                dsSPDDataContext data = new dsSPDDataContext();
                trSPD spd = (from p in data.trSPDs
                             where p.noSPD.Equals(txtNoSPD.Text)
                             select p).FirstOrDefault();

                var user = (from k in data.v_karyawanTracs
                            where k.coCd == spd.coCdTujuan
                            select new
                            {
                                leasingRental = k.leasingRental,
                                kodePA = k.kodePA,
                                coCd = k.coCd
                            }).Distinct();
                e.Result = user;

                cmbTempatTujuan.SelectedValue = TujuanHid.Value.ToString();
            }
            else
            {
                dsSPDDataContext data = new dsSPDDataContext();
                var user = (from k in data.v_karyawanTracs
                            where k.coCd == cmbCompanyCode.SelectedValue
                            select new
                            {
                                leasingRental = k.leasingRental,
                                kodePA = k.kodePA,
                                coCd = k.coCd
                            }).Distinct();
                e.Result = user;
            }
        }

        protected void cmbxAtasan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCompanyCode.SelectedItem.Value == "0")
            {
                //cmbxTujuan.Value = cmbxAtasan.SelectedItem.Text;
            }

        }

        private object ApprovalAtasan()
        {
            dsSPDDataContext data = new dsSPDDataContext();
            var user = (object)null;


            msKaryawan personalArea = (from u in data.msKaryawans
                                       where u.nrp == txtNrp.Text
                                       select u).FirstOrDefault();

            if (txtNrp.Text == "99999999")
            {
                user = (from k in data.v_atasans
                        //where k.kodePA == personalArea.kodePA
                        orderby k.namaLengkap
                        select new
                        {
                            namaLengkap = k.namaLengkap,
                            nrp = k.nrp,
                        }).Distinct();

            }
            else
            {
                user = (from k in data.v_atasans
                        //where k.kodePA == personalArea.kodePA
                        orderby k.namaLengkap
                        select new
                        {
                            namaLengkap = k.namaLengkap,
                            nrp = k.nrp,
                        }).Distinct();
            }

            return user;
        }

        protected void ldsApproval1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = GetAtasan();
        }

        private object GetAtasan()
        {
            dsSPDDataContext data = new dsSPDDataContext();
            var user = (object)null;

            msKaryawan personalArea = (from u in data.msKaryawans
                                       where u.nrp == txtNrp.Text
                                       select u).FirstOrDefault();

            if (txtNrp.Text == "99999999")
            {
                user = (from k in data.v_atasans
                        where k.coCd == personalArea.coCd
                                && !k.posisi.Contains("director")
                        //&& k.kodePA == personalArea.kodePA
                        //&& k.kodePSubArea == personalArea.kodePSubArea
                        orderby k.namaLengkap
                        select new
                        {
                            namaLengkap = k.namaLengkap,
                            nrp = k.nrp,
                        }).Distinct();

            }
            else
            {
                user = (from k in data.v_atasans
                        where k.coCd == personalArea.coCd
                                && !k.posisi.Contains("director")
                        //&& k.kodePA == personalArea.kodePA
                        //&& k.kodePSubArea == personalArea.kodePSubArea
                        orderby k.namaLengkap
                        select new
                        {
                            namaLengkap = k.namaLengkap,
                            nrp = k.nrp,
                        }).Distinct();
            }

            return user;
        }

        protected void rdbHO_CheckedChanged(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
        }

        protected void rdbCbg_CheckedChanged(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
        }

        protected void ddlDireksi_DataBound(object sender, EventArgs e)
        {
            txtNamaLengkap.Text = ddlDireksi.SelectedItem.Text;
            txtNrp.Text = ddlDireksi.SelectedItem.Value;
        }

        /// <summary>
        /// NOT USED IAN KASELA
        /// </summary>
        /// <param name="golongan"></param>
        /// 
        #region not used
        //private void createDynamicControl(string golongan)
        //{
        //    if (cmbGolongan.SelectedItem.Value == "I"
        //        || cmbGolongan.SelectedItem.Value == "II"
        //        || cmbGolongan.SelectedItem.Value == "III")
        //    {
        //        ////remove all control at placeholder
        //        //foreach (Control ctrl in pnlLabelDynamic.Controls)
        //        //{
        //        //    ctrl.Controls.Remove(ctrl);
        //        //}

        //        //create label+ddl (kadept, kadiv, cfat div head) //HOGol1-3
        //        Label labelKadept = new Label();
        //        Label labelKadiv = new Label();
        //        Label labelCfatDivHead = new Label();
        //        ASPxComboBox ddlKadept = new ASPxComboBox();
        //        ASPxComboBox ddlKadiv = new ASPxComboBox();
        //        ASPxComboBox ddlCfatDivHead = new ASPxComboBox();

        //        // Label
        //        phLabelDynamic.Controls.Add(new LiteralControl("<table style='height:100%;vertical-align: middle;'"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<td>"));
        //        labelKadept.Text = "Kadept";
        //        labelKadept.ID = "lblKadept";
        //        phLabelDynamic.Controls.Add(labelKadept);
        //        phLabelDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<td>"));
        //        labelKadiv.Text = "Kadiv";
        //        labelKadiv.ID = "lblKadiv";
        //        phLabelDynamic.Controls.Add(labelKadiv);
        //        phLabelDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<td>"));
        //        labelCfatDivHead.Text = "CFAT Div Head";
        //        labelCfatDivHead.ID = "lblCfatDivHead";
        //        phLabelDynamic.Controls.Add(labelCfatDivHead);
        //        phLabelDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</table>"));

        //        // ComboBox
        //        phDdlDynamic.Controls.Add(new LiteralControl("<table style='height:100%;'"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<td>"));
        //        ddlKadept.ID = "cmbxApprovalKadept";
        //        ddlKadept.DataSourceID = "ldsApproval1";
        //        ddlKadept.TextField = "namaLengkap";
        //        ddlKadept.ValueField = "nrp";
        //        ddlKadept.EnableIncrementalFiltering = true;
        //        phDdlDynamic.Controls.Add(ddlKadept);
        //        phDdlDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<td>"));
        //        ddlKadiv.ID = "cmbxApprovalKadiv";
        //        ddlKadiv.DataSourceID = "ldsApproval1";
        //        ddlKadiv.TextField = "namaLengkap";
        //        ddlKadiv.ValueField = "nrp";
        //        ddlKadiv.EnableIncrementalFiltering = true;
        //        phDdlDynamic.Controls.Add(ddlKadiv);
        //        phDdlDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<td>"));
        //        ddlCfatDivHead.ID = "cmbxApprovalCfatDivHead";
        //        ddlCfatDivHead.DataSourceID = "ldsApproval1";
        //        ddlCfatDivHead.TextField = "namaLengkap";
        //        ddlCfatDivHead.ValueField = "nrp";
        //        ddlCfatDivHead.EnableIncrementalFiltering = true;
        //        phDdlDynamic.Controls.Add(ddlCfatDivHead);
        //        phDdlDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</table>"));

        //    }
        //    else if (cmbGolongan.SelectedItem.Value == "IV"
        //        || cmbGolongan.SelectedItem.Value == "V"
        //        || cmbGolongan.SelectedItem.Value == "VI")
        //    {
        //        ////remove all control at placeholder
        //        //foreach (Control ctrl in pnlLabelDynamic.Controls)
        //        //{
        //        //    ctrl.Controls.Remove(ctrl);
        //        //}

        //        //create label+ddl (kadept, kadiv, dic, finance direktur) //HOGol4-6
        //        Label labelKadept = new Label();
        //        Label labelKadiv = new Label();
        //        Label labelDic = new Label();
        //        Label labelFinanceDirektur = new Label();
        //        ASPxComboBox ddlKadept = new ASPxComboBox();
        //        ASPxComboBox ddlKadiv = new ASPxComboBox();
        //        ASPxComboBox ddlDic = new ASPxComboBox();
        //        ASPxComboBox ddlFinanceDirektur = new ASPxComboBox();

        //        //Label
        //        phLabelDynamic.Controls.Add(new LiteralControl("<table style='height:100%;vertical-align: middle;'"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<td>"));
        //        labelKadept.Text = "Kadept";
        //        labelKadept.ID = "lblKadept";
        //        phLabelDynamic.Controls.Add(labelKadept);
        //        phLabelDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<td>"));
        //        labelKadiv.Text = "Kadiv";
        //        labelKadiv.ID = "lblKadiv";
        //        phLabelDynamic.Controls.Add(labelKadiv);
        //        phLabelDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<td>"));
        //        labelDic.Text = "DIC";
        //        labelDic.ID = "lblDic";
        //        phLabelDynamic.Controls.Add(labelDic);
        //        phLabelDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("<td>"));
        //        labelFinanceDirektur.Text = "Finance Direktur";
        //        labelFinanceDirektur.ID = "lblFinanceDirektur";
        //        phLabelDynamic.Controls.Add(labelFinanceDirektur);
        //        phLabelDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phLabelDynamic.Controls.Add(new LiteralControl("</table>"));

        //        // ComboBox
        //        phDdlDynamic.Controls.Add(new LiteralControl("<table style='height:100%;'"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<td>"));
        //        ddlKadept.ID = "cmbxApprovalKadept";
        //        ddlKadept.DataSourceID = "ldsApproval1";
        //        ddlKadept.TextField = "namaLengkap";
        //        ddlKadept.ValueField = "nrp";
        //        ddlKadept.EnableIncrementalFiltering = true;
        //        phDdlDynamic.Controls.Add(ddlKadept);
        //        phDdlDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<td>"));
        //        ddlKadiv.ID = "cmbxApprovalKadiv";
        //        ddlKadiv.DataSourceID = "ldsApproval1";
        //        ddlKadiv.TextField = "namaLengkap";
        //        ddlKadiv.ValueField = "nrp";
        //        ddlKadiv.EnableIncrementalFiltering = true;
        //        phDdlDynamic.Controls.Add(ddlKadiv);
        //        phDdlDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<td>"));
        //        ddlDic.ID = "cmbxApprovalDic";
        //        ddlDic.DataSourceID = "ldsApproval1";
        //        ddlDic.TextField = "namaLengkap";
        //        ddlDic.ValueField = "nrp";
        //        ddlDic.EnableIncrementalFiltering = true;
        //        phDdlDynamic.Controls.Add(ddlDic);
        //        phDdlDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("<td>"));
        //        ddlFinanceDirektur.ID = "cmbxApprovalFinanceDirektur";
        //        ddlFinanceDirektur.DataSourceID = "ldsApproval1";
        //        ddlFinanceDirektur.TextField = "namaLengkap";
        //        ddlFinanceDirektur.ValueField = "nrp";
        //        ddlFinanceDirektur.EnableIncrementalFiltering = true;
        //        phDdlDynamic.Controls.Add(ddlFinanceDirektur);
        //        phDdlDynamic.Controls.Add(new LiteralControl("</td>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</tr>"));
        //        phDdlDynamic.Controls.Add(new LiteralControl("</table>"));
        //    }
        //}

        #endregion
    }
}