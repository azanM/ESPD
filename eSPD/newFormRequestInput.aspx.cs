using eSPD.Core;
using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace eSPD
{
    public partial class newFormRequestInput : System.Web.UI.Page
    {
        private static msKaryawan karyawan = new msKaryawan();
        private static classSpd oSPD = new classSpd();
        private static string strID = string.Empty;
        List<string> errorMessageHidden = new List<string>();
        private static List<string> atasanArr = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }

            //start change TAS
            strID = (string)Session["IDLogin"];
            //strID = "tracdrc";
            //end change TAS

            karyawan = oSPD.getKaryawan(strID);
            Session["nrpLogin"] = karyawan.nrp;

            if (string.IsNullOrEmpty(karyawan.nrp)) Response.Redirect("frmHome.aspx");

            // jika sekertaris
            isSec(karyawan.nrp);

            if (!IsPostBack)
            {
                setInitial(karyawan);
                LoadDdlCompanyTujuan();
                BindDropdownProvinsi();
            }

            if (IsPostBack)
            {
                approvalViewState();
            }

            enableUangMukaAndAlasan();
            //tujuanLain(txTempatTujuanLain.Enabled);

        }

        protected void BindDropdownProvinsi()
        {
            SqlCommand cmd = new SqlCommand("Select Id, Propinsi from msPropinsi where RowStatus = 1", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
            cmd.Connection.Open();
            SqlDataReader Reader;
            Reader = cmd.ExecuteReader();
            listProvinsi.DataSource = Reader;
            listProvinsi.DataValueField = "Id";
            listProvinsi.DataTextField = "Propinsi";
            listProvinsi.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        protected void provinsi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtHarga.Text = String.Empty;
            string Golongan = ddlGolongan.SelectedValue;
            ListItem item = new ListItem("select one", "0");
            SqlCommand cmd = new SqlCommand("Select Id, Lokasi from Hardship where Golongan = '" + Golongan + "' and RowStatus = 1 and PropinsiId = " + int.Parse(listProvinsi.SelectedValue)+"", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
            cmd.Connection.Open();
            SqlDataReader Reader;
            Reader = cmd.ExecuteReader();
            listLokasiProvinsi.DataSource = Reader;
            listLokasiProvinsi.DataValueField = "Id";
            listLokasiProvinsi.DataTextField = "Lokasi";
            listLokasiProvinsi.DataBind();
            listLokasiProvinsi.Items.Insert(0, item);
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        protected void lokasi_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            txtHarga.Text = String.Empty;
            string Golongan = ddlGolongan.SelectedValue;
            SqlCommand cmd = new SqlCommand("Select Harga from Hardship where Id = " + int.Parse(listLokasiProvinsi.SelectedValue) + "", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
            cmd.Connection.Open();
            SqlDataReader Reader;
            Reader = cmd.ExecuteReader();       
            while (Reader.Read())
            {
                txtHarga.Text = Reader["Harga"].ToString();
            }
        }

        protected void ddlSelfOrDirect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDireksi.Visible = false;
            rvDireksi.Enabled = false;
            ddDireksi.Visible = false;

            if (ddlSelfOrDirect.SelectedValue == "0") setDirect();
            else setInitial(karyawan);
        }

        protected void ddlDireksi_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                if (ddlSelfOrDirect.SelectedValue == "0" && !string.IsNullOrWhiteSpace(ddlDireksi.SelectedValue))
                {
                    var direksi = ctx.msKaryawans.FirstOrDefault(o => o.nrp == ddlDireksi.SelectedValue);
                    setInitial(direksi);
                }
                else
                {
                    setInitial(karyawan);
                }
            }
        }

        protected void ddlAsal_SelectedIndexChanged(object sender, EventArgs e)
        {
            setContent();
            setApproval();
        }

        protected void ddlGolongan_SelectedIndexChanged(object sender, EventArgs e)
        {
            setContent();
            setApproval();
        }

        protected void ddlPosisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            setContent();
            setApproval();
        }

        protected void ddlTujuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlWilayahTujuan.Items.Clear();
            ddlWilayahTujuan.Items.Add(new ListItem(" - Select One - ", "", true));
            ddlWilayahTujuan.AppendDataBoundItems = true;
            setContent();
            setApproval();
        }

        protected void ddlWilayahTujuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            setContent();
        }

        protected void ddlCompanyTujuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPersonalAreaTujuan.Items.Clear();
            ddlPersonalAreaTujuan.Items.Add(new ListItem(" - Select One - ", "", true));
            ddlPersonalAreaTujuan.AppendDataBoundItems = true;

            ddlPSubAreaTujuan.Items.Clear();
            ddlPSubAreaTujuan.Items.Add(new ListItem(" - Select One - ", "", true));
            ddlPSubAreaTujuan.AppendDataBoundItems = true;

            //if (ddlCompanyTujuan.SelectedValue == "0")
            //    tujuanLain(true);
            //else
            //    tujuanLain(false);

            if (ddlCompanyTujuan.SelectedValue == "0")
            {
                ddlPersonalAreaTujuan.Items.Clear();
                ddlPSubAreaTujuan.Items.Clear();

                ddlPersonalAreaTujuan.Enabled = false;
                rvPersonalAreaTujuan.Enabled = false;
                ddlPSubAreaTujuan.Enabled = false;
                rvPSubAreaTujuan.Enabled = false;
                txTempatTujuanLain.Enabled = true;
                rvTempatTujuanLain.Enabled = true;
            }
            else
            {
                ddlPersonalAreaTujuan.Enabled = true;
                ddlPSubAreaTujuan.Enabled = true;
                txTempatTujuanLain.Enabled = false;
            }

            //ChangeApprovalTujuan(); //try TO DO OrgUnit UnCommand
        }

        protected void ddlPersonalAreaTujuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPSubAreaTujuan.Items.Clear();
            ddlPSubAreaTujuan.Items.Add(new ListItem(" - Select One - ", "", true));
            ddlPSubAreaTujuan.AppendDataBoundItems = true;

            //ChangeApprovalTujuan(); //try TO DO OrgUnit UnCommand
        }

        protected void txtTglBerangkat_TextChanged(object sender, EventArgs e)
        {
            enableUangMukaAndAlasan();
            setDisabledHotelAndNotLessThan();
        }

        protected void txtTglKembali_TextChanged(object sender, EventArgs e)
        {
            enableUangMukaAndAlasan();
            setDisabledHotelAndNotLessThan();

            txtTglExp.Text = Convert.ToDateTime(txtTglKembali.Text).AddDays(15).ToShortDateString();

            DateTime today = DateTime.Now;
            DateTime answer = today.AddDays(36);
        }

        protected void ddlAngkutan_SelectedIndexChanged(object sender, EventArgs e)
        {
            angkutanLain(false);
            if (ddlAngkutan.SelectedValue == "5") angkutanLain(true);
        }

        protected void GvApproval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            int iRow = e.Row.DataItemIndex;

            TextBox txt = e.Row.Cells[1].FindControl("txNrpApproval") as TextBox;
            TextBox txtDesc = e.Row.Cells[1].FindControl("txDesc") as TextBox;
            if (txt != null)
                txt.Attributes.Add("data-index", iRow.ToString());
            if (txtDesc != null)
                txtDesc.Attributes.Add("data-index-desc", iRow.ToString());
        }

        // try TO DO OrgUnit UnCommand
        //protected void ddlApprovalTujuan_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ChangeApprovalTujuan();
        //}

        // try TO DO OrgUnit UnCommand
        //private void ChangeApprovalTujuan()
        //{
        //    DateTime now = DateTime.Now;

        //    ddlApprovalTujuan.Items.Clear();
        //    ddlApprovalTujuan.Items.Add(new ListItem(" - Select One - ", "", true));
        //    ddlApprovalTujuan.AppendDataBoundItems = true;

        //    List<ListItem> approvalTujuans = new List<ListItem>();

        //    if (string.IsNullOrEmpty(ddlCompanyTujuan.SelectedValue) || string.IsNullOrEmpty(ddlPersonalAreaTujuan.SelectedValue) || string.IsNullOrEmpty(ddlPSubAreaTujuan.SelectedValue))
        //    {
        //        return;
        //    }

        //    using (var ctx = new dsSPDDataContext())
        //    {
        //        string[] TracArr = new string[2];

        //        approvalTujuans = (from mk in ctx.msKaryawans
        //                           join ou in ctx.vwOrgUnits on mk.nrp equals ou.Nrp.ToString()
        //                           where mk.companyCode.Equals(ddlCompanyTujuan.SelectedItem.Text)
        //                               && mk.personelArea.Equals(ddlPersonalAreaTujuan.SelectedItem.Text)
        //                               && mk.kodePSubArea.Equals(ddlPSubAreaTujuan.SelectedValue)
        //                               && ou.PosChief != "0"
        //                               && (ou.PosStart <= now && ou.PosEnd >= now)
        //                               && (ou.JobStart <= now && ou.JobEnd >= now)
        //                               && (ou.JobStart <= now && ou.JobEnd >= now)
        //                               && (ou.PerStart <= now && ou.PerEnd >= now)
        //                               && (ou.CcStart <= now && ou.CcEnd >= now)
        //                           select new ListItem
        //                           {
        //                               Value = mk.nrp,
        //                               Text = mk.namaLengkap + '-' + mk.nrp
        //                           }).Distinct().ToList();

        //        if (approvalTujuans.Count == 0)
        //        {
        //            approvalTujuans = (from mk in ctx.msKaryawans
        //                               join ou in ctx.vwOrgUnits on mk.nrp equals ou.Nrp.ToString()
        //                               where mk.companyCode == ddlCompanyTujuan.SelectedItem.Text
        //                                    && mk.personelArea == ddlPersonalAreaTujuan.SelectedItem.Text
        //                                    && ou.PosChief != "0"
        //                                    && (ou.PosStart <= now && ou.PosEnd >= now)
        //                                    && (ou.JobStart <= now && ou.JobEnd >= now)
        //                                    && (ou.JobStart <= now && ou.JobEnd >= now)
        //                                    && (ou.PerStart <= now && ou.PerEnd >= now)
        //                                    && (ou.CcStart <= now && ou.CcEnd >= now)
        //                               select new ListItem
        //                               {
        //                                   Value = mk.nrp,
        //                                   Text = mk.namaLengkap + '-' + mk.nrp
        //                               }).Distinct().ToList();

        //            if (approvalTujuans.Count == 0)
        //            {
        //                approvalTujuans = (from mk in ctx.msKaryawans
        //                                   join ou in ctx.vwOrgUnits on mk.nrp equals ou.Nrp.ToString()
        //                                   where mk.companyCode == ddlCompanyTujuan.SelectedItem.Text
        //                                        && ou.PosChief != "0"
        //                                        && (ou.PosStart <= now && ou.PosEnd >= now)
        //                                        && (ou.JobStart <= now && ou.JobEnd >= now)
        //                                        && (ou.JobStart <= now && ou.JobEnd >= now)
        //                                        && (ou.PerStart <= now && ou.PerEnd >= now)
        //                                        && (ou.CcStart <= now && ou.CcEnd >= now)
        //                                   //&& int.Parse(ou.JobAbbreviation.Split('_')[0]) >= 90
        //                                   select new ListItem
        //                                   {
        //                                       Value = mk.nrp,
        //                                       Text = mk.namaLengkap + '-' + mk.nrp
        //                                   }).Distinct().ToList();
        //            }
        //        }
        //    }

        //    ddlApprovalTujuan.DataSource = approvalTujuans;
        //    ddlApprovalTujuan.DataTextField = "Text";
        //    ddlApprovalTujuan.DataValueField = "Value";
        //    ddlApprovalTujuan.DataBind();
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //lblGoLive.Visible = false;
            //if (Convert.ToDateTime(txtTglBerangkat.Text) < Convert.ToDateTime("12/22/2017"))
            //{
            //    lblGoLive.Visible = true;
            //    return;
            //}

            //if (Convert.ToDateTime(txtTglBerangkat.Text) < DateTime.Now.Date)
            //{
            //    lblBackDate.Visible = true;
            //    return;
            //}

            //if (Convert.ToDateTime(txtTglKembali.Text) < DateTime.Now.Date)
            //{
            //    lblBackDate.Visible = true;
            //    return;
            //}

            using (var ctx = new dsSPDDataContext())
            {
                List<Approval> Approvals = (List<Approval>)ViewState["Approval"];

                trSPD spd = new trSPD();

                spd.asal = ddlAsal.SelectedValue;
                spd.nrp = txNrp.Value;
                spd.namaLengkap = txNamaLengkap.Text;
                if(ddlSubGolongan.Visible == true)
                {
                    string strSubGolongan = ddlGolongan.SelectedValue.Trim() + ddlSubGolongan.SelectedValue.Trim();
                    spd.idGolongan = strSubGolongan;
                }
                else
                {
                    spd.idGolongan = ddlGolongan.SelectedValue;
                }
                spd.jabatan = txJabatan.Text;
                spd.email = txEmail.Text;
                spd.coCdTujuan = ddlCompanyTujuan.SelectedValue;
                spd.companyCodeTujuan = ddlCompanyTujuan.SelectedItem.Text;

                if (ddlCompanyTujuan.SelectedValue != "0")
                {
                    spd.kodePATujuan = ddlPersonalAreaTujuan.SelectedValue;
                    spd.personelAreaTujuan = ddlPersonalAreaTujuan.SelectedItem.Text;
                    spd.kodePSubAreaTujuan = ddlPSubAreaTujuan.SelectedValue;
                    spd.pSubAreaTujuan = ddlPSubAreaTujuan.SelectedItem.Text;
                    //spd.nrpApprovalTujuan = ddlApprovalTujuan.SelectedValue; //try TO DO OrgUnit UnCommand
                }
                else
                {
                    spd.personelAreaTujuan = ddlCompanyTujuan.SelectedItem.Text;
                    spd.pSubAreaTujuan = ddlCompanyTujuan.SelectedItem.Text;
                    //spd.nrpApprovalTujuan = Approvals.FirstOrDefault().NrpApproval; //try TO DO OrgUnit UnCommand
                }

                spd.nrpApprovalTujuan = ddlApprovalTujuan.SelectedValue;
                spd.Tujuan = ddlTujuan.SelectedItem.Text;
                spd.WilayahTujuan = ddlWilayahTujuan.SelectedValue;
                spd.NoHP = txNoPonsel.Text;
                spd.tempatTujuanLain = txTempatTujuanLain.Text;
                spd.idKeperluan = Convert.ToInt32(ddlKeperluan.SelectedValue);
                spd.ketKeperluan = txKetKeperluan.Text;
                spd.tiket = "Dicarikan";
                spd.tglBerangkat = Convert.ToDateTime(txtTglBerangkat.Text);
                spd.jamBerangkat = ddlJamBerangkat.SelectedValue;
                spd.menitBerangkat = ddlMenitBerangkat.SelectedValue;
                spd.tglKembali = Convert.ToDateTime(txtTglKembali.Text);
                spd.jamKembali = ddlJamKembali.SelectedValue;
                spd.menitKembali = ddlMenitKembali.SelectedValue;
                spd.tglExpired = Convert.ToDateTime(txtTglExp.Text);
                spd.Alasan = txAlasan.Text;
                spd.idAngkutan = Convert.ToInt32(ddlAngkutan.SelectedValue);
                spd.angkutanLain = txAngkutanLain.Text;
                spd.penginapan = ddlPenginapan.SelectedValue;

                //update tunjangan kejauhan 9 oktober 2018
                if (txtHarga.Text != "")
                {
                    spd.HardshipID = int.Parse(listLokasiProvinsi.SelectedValue);
                    //spd.ProvinsiLokasi = listProvinsi.SelectedItem.Text + "-" + listLokasiProvinsi.SelectedItem.Text;
                }//
                

                if (ddlPenginapan.SelectedValue.ToLower().Contains("disediakan"))
                {
                    spd.isHotel = true;
                }
                else
                {
                    spd.isHotel = false;
                }

                // first Index Sequence approval, di assign pas even submit
                var dataAtasan = (from a in ctx.msKaryawans
                                  where a.nrp == karyawan.nrp
                                  select a).FirstOrDefault();

                spd.nrpAtasan = dataAtasan.nrpAtasan == null ? string.Empty : dataAtasan.nrpAtasan;
                if (txUangMuka.Text != "" || txUangMuka.Text != "" || txUangMuka.Text != string.Empty)
                {
                    spd.statusUM = "pending";
                }

                spd.uangMuka = txUangMuka.Text;
                spd.costCenter = ddlCostCenter.SelectedValue;
                spd.keterangan = txKeterangan.Text;
                spd.status = "Save";
                spd.dibuatOleh = txNrp.Value;
                spd.dibuatTanggal = DateTime.Now;
                spd.posisi = ddlPosisi.SelectedValue;

                #region gak kepake
                //spd.datetimeClose = txNoSPD.Text;
                //spd.nrpApprovalGA = txNoSPD.Text;
                //spd.diubahOleh = txNoSPD.Text;
                //spd.diubahTanggal = txNoSPD.Text;
                //spd.isApproved = txNoSPD.Text;
                //spd.isCancel = txNoSPD.Text;
                //spd.isSubmit = txNoSPD.Text;
                //spd.idCabangTujuan = txNoSPD.Text;
                //spd.keperluanLain = txKetKeperluan.Text;
                //spd.isSubmitDate = txNoSPD.Text;
                //spd.isApprovedDate = txNoSPD.Text;
                //spd.isCancelDate = txNoSPD.Text;
                //spd.idCabangTujuan = txNoSPD.Text;
                #endregion

                if (Approvals.Count() == 0)
                {
                    errorMessageHidden.Add("Tidak ada approval atasan.");
                }

                if (!validateDate())
                {
                    errorMessageHidden.Add("Waktu berangkat harus lebih kecil nilainya, dari pada waktu kembali.");
                }

                if (errorMessageHidden.Count() == 0)
                {
                    try
                    {
                        spd.noSPD = ctx.sp_GenerateNoSpd().FirstOrDefault().number; // generate number belakangan
                        ctx.trSPDs.InsertOnSubmit(spd);
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        errorMessageHidden.Add("Error Save SPD, ketika save spd header");
                        errorMessageHidden.Add(ex.Message);

                        if (ctx.trSPDs.FirstOrDefault(o => o.noSPD == spd.noSPD) != null)
                        {
                            ctx.trSPDs.DeleteOnSubmit(spd);
                            ctx.SubmitChanges();
                        }

                    }
                }

                if (errorMessageHidden.Count() == 0)
                {
                    // method insert approval
                    List<ApprovalStatus> newApprovalList = (from p in Approvals
                                                            select new ApprovalStatus
                                                            {
                                                                NoSPD = spd.noSPD,
                                                                RuleID = p.RuleID,
                                                                NrpApproval = p.NrpApproval,
                                                                Nama = p.Nama,
                                                                Email = p.Email,
                                                                IndexLevel = p.IndexLevel
                                                            }).ToList();

                    // add adh approval
                    var ADH = (from p in ctx.msKaryawans where p.nrp == ddlApprovalADH.SelectedValue select p).FirstOrDefault();
                    ApprovalStatus approvalADH = new ApprovalStatus();
                    approvalADH.NoSPD = spd.noSPD;
                    approvalADH.RuleID = ctx.ApprovalRules.Where(x => x.Deskripsi.Contains("ADH")).First().RuleID;
                    approvalADH.NrpApproval = ADH.nrp;
                    approvalADH.Nama = ADH.namaLengkap;
                    approvalADH.Email = ADH.EMail;
                    approvalADH.IndexLevel = 0;
                    newApprovalList.Insert(0, approvalADH);

                    try
                    {
                        ctx.ApprovalStatus.InsertAllOnSubmit(newApprovalList);
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        errorMessageHidden.Add("Error Save Approval");
                        errorMessageHidden.Add(ex.Message);

                        ctx.trSPDs.DeleteOnSubmit(spd);
                        ctx.SubmitChanges();
                    }
                    finally
                    {
                        txNoSPD.Text = spd.noSPD;
                        pnlSuccess.Visible = true;
                        pnlError.Visible = false;
                        lblSuccess.Text = "SPD Berhasil di Save, No SPD : " + spd.noSPD;
                        btnSave.Enabled = false;
                        btnSubmit.Enabled = true;
                        btnReset.Disabled = true;
                    }
                }

                if (errorMessageHidden.Count() > 0)
                {
                    errorMessage.DataSource = errorMessageHidden;
                    errorMessage.DataBind();

                    pnlError.Visible = true;
                    pnlSuccess.Visible = false;
                }
            }

            if (lblBackDate.Visible == true)
                lblBackDate.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                var spd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == txNoSPD.Text);

                //var firstApproval = ctx.ApprovalStatus.FirstOrDefault(o => o.NoSPD == txNoSPD.Text && o.IndexLevel.Value == 1);//sebelum ada ADH
                var firstApproval = ctx.ApprovalStatus.FirstOrDefault(o => o.NoSPD == txNoSPD.Text && o.IndexLevel.Value == 0);

                if (spd == null)
                {
                    errorMessageHidden.Add("SPD tidak ditemukan");
                }
                if (firstApproval == null)
                {
                    errorMessageHidden.Add("Approval pertama tidak ditemukan");
                }

                if (errorMessageHidden.Count() == 0)
                {
                    spd.diubahOleh = karyawan.nrp;
                    spd.diubahTanggal = DateTime.Now;
                    spd.isSubmitDate = DateTime.Now;
                    spd.nrpAtasan = firstApproval.NrpApproval;
                    spd.status = "Menunggu approval " + firstApproval.ApprovalRule.Deskripsi;
                    spd.isSubmit = true;

                    try
                    {
                        ctx.SubmitChanges();

                        // send email atasan
                        EmailCore.ApprovalSPD(firstApproval.NrpApproval, txNoSPD.Text, firstApproval.IndexLevel.Value.ToString(), spd);
                    }
                    catch (Exception ex)
                    {
                        errorMessageHidden.Add("Error Submit SPD, gangguan teknis");
                        errorMessageHidden.Add(ex.Message);
                    }
                    finally
                    {
                        pnlSuccess.Visible = true;
                        pnlError.Visible = false;
                        lblSuccess.Text = "SPD Berhasil di Submit, No SPD : " + spd.noSPD + ", status spd saat ini adalah " + spd.status;
                        btnSave.Enabled = false;
                        btnSubmit.Enabled = false;

                        btnReset.Disabled = false;
                    }
                }

                if (errorMessageHidden.Count() > 0)
                {
                    errorMessage.DataSource = errorMessageHidden;
                    errorMessage.DataBind();

                    pnlError.Visible = true;
                    pnlSuccess.Visible = false;
                }
            }
        }

        void setInitial(msKaryawan karyawan)
        {
            string StringQueryApproval = string.Empty;
            txNrp.Value = karyawan.nrp;

            if (karyawan.nrp.Equals("99999999"))
            {
                txNamaLengkap.ReadOnly = false;
                txEmail.ReadOnly = false;
                txJabatan.ReadOnly = false;
                ddlGolongan.SelectedValue = "III";
                rvNamaLengkap.Enabled = true;
                rvEmail.Enabled = true;
                rvJabatan.Enabled = true;
            }
            else
            {
                ddlGolongan.SelectedValue = karyawan.payscalegroup == null ? "" : karyawan.payscalegroup.Trim();

                if (karyawan.payscalegroup.Trim() == "A" || karyawan.payscalegroup.Trim() == "C" || karyawan.payscalegroup.Trim() == "S")
                {
                    ddlSubGolongan.Visible = true;
                    LoadDdlSubGolongan(karyawan.payscalegroup.Trim());
                }

                //perubahan 25 september 2017 filter atasan untuk golongan IV

                StringQueryApproval = "select nrp, namaLengkap from v_atasan_tujuan where nrp != "+txNrp.Value+"";
                if (karyawan.payscalegroup == "IV")
                {
                    StringQueryApproval = "select nrp, namaLengkap from v_atasan_tujuan where organisasiunit like '%BOARD OF DIRECTOR%' and nrp != "+txNrp.Value+"";
                }
                SqlCommand cmd = new SqlCommand(StringQueryApproval, new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
                cmd.Connection.Open();
                SqlDataReader listApprovalTujuan;
                listApprovalTujuan = cmd.ExecuteReader();
                ddlApprovalTujuan.DataSource = listApprovalTujuan;
                ddlApprovalTujuan.DataValueField = "nrp";
                ddlApprovalTujuan.DataTextField = "namaLengkap";
                ddlApprovalTujuan.DataBind();
                cmd.Connection.Close();
                cmd.Dispose();

                LoadApprovalADH();
           
            }

            txNamaLengkap.Text = karyawan.namaLengkap;
            txEmail.Text = karyawan.EMail;
            txJabatan.Text = karyawan.Job;
            txCompanyCode.Text = karyawan.companyCode;
            if (karyawan.companyCode == "PT. SERASI AUTORAYA" || karyawan.companyCode == "PT SERASI AUTORAYA")
            {
                if(karyawan.personelArea.Trim() == "HEAD OFFICE" || karyawan.personelArea.Trim() == "Head Office" || karyawan.personelArea.Trim() == "Trac Head Office" || karyawan.personelArea.Trim() == "TRAC HEAD OFFICE")
                {
                    ddlAsal.SelectedValue = "HO";
                }
                else
                {
                    ddlAsal.SelectedValue = "Cabang";
                }
            }
            else
            {
                ddlAsal.SelectedValue = "Cabang";
            }
            txPersonalArea.Text = karyawan.personelArea;
            txPersonalSubArea.Text = karyawan.pSubArea;

            // ddlGolongan_SelectedIndexChanged(null, null);
        }

        private void LoadApprovalADH()
        {
            /*
                select msADH.nrp, msKaryawan.namaLengkap+ + '-'+msCost.costDesc as CostCenter from msADH 
                inner join msCost on msADH.costcenterId = msCost.costId 
                inner join msKaryawan on msADH.nrp = msKaryawan.nrp
                where RowStatus = 1
            */
            string Query = "select distinct msADH.nrp, msKaryawan.namaLengkap from msADH inner join msCost on msADH.costcenterId = msCost.costId inner join msKaryawan on msADH.nrp = msKaryawan.nrp where RowStatus = 1";
            SqlCommand cmd = new SqlCommand(Query, new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
            cmd.Connection.Open();
            SqlDataReader listApprovalADH;
            listApprovalADH = cmd.ExecuteReader();
            ddlApprovalADH.DataSource = listApprovalADH;
            ddlApprovalADH.DataValueField = "nrp";
            ddlApprovalADH.DataTextField = "namaLengkap";
            ddlApprovalADH.DataBind();
            cmd.Connection.Close();
            cmd.Dispose();

        }
        private void LoadDdlSubGolongan(string golongan)
        {
            if(golongan == "A")
            {
                ddlSubGolongan.Items.Clear();
                ddlSubGolongan.Items.Add(new ListItem("- Select One -"));
                ddlSubGolongan.Items.Add(new ListItem("0"));
                ddlSubGolongan.Items.Add(new ListItem("1"));
                ddlSubGolongan.Items.Add(new ListItem("2"));

                ddlSubGolongan.AppendDataBoundItems = true;
            }
            else if(golongan == "C")
            {
                ddlSubGolongan.Items.Clear();
                ddlSubGolongan.Items.Add(new ListItem("- Select One -"));
                ddlSubGolongan.Items.Add(new ListItem("0"));
                ddlSubGolongan.Items.Add(new ListItem("1"));
                ddlSubGolongan.Items.Add(new ListItem("2"));
                ddlSubGolongan.Items.Add(new ListItem("3"));

                ddlSubGolongan.AppendDataBoundItems = true;
            }
            else if (golongan == "S")
            {
                ddlSubGolongan.Items.Clear();
                ddlSubGolongan.Items.Add(new ListItem("- Select One -"));
                ddlSubGolongan.Items.Add(new ListItem("0"));
                ddlSubGolongan.Items.Add(new ListItem("1"));
                ddlSubGolongan.Items.Add(new ListItem("2"));
                ddlSubGolongan.Items.Add(new ListItem("3"));

                ddlSubGolongan.AppendDataBoundItems = true;
            }
        }

        void approvalViewState()
        {
            using (var ctx = new dsSPDDataContext())
            {
                List<Approval> Approvals = new List<Approval>();
                try
                {
                    for (int i = 0; i < gvApproval.Rows.Count; i++)
                    {
                        Approval newApproval = new Approval();

                        var NrpApproval = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txNrpApproval");
                        var IndexLevel = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txIndexLevel");
                        var Deskripsi = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txDesc");
                        var RuleID = (TextBox)gvApproval.Rows[i].Cells[1].FindControl("txRuleID");

                        if (!string.IsNullOrEmpty(NrpApproval.Text))
                        {
                            var atasanData = (from p in ctx.msKaryawans
                                              where p.nrp == NrpApproval.Text
                                              select p).FirstOrDefault();

                            newApproval.Email = atasanData.EMail;
                            newApproval.Nama = atasanData.namaLengkap;
                        }

                        newApproval.NrpApproval = NrpApproval.Text;
                        newApproval.IndexLevel = Convert.ToInt32(IndexLevel.Text); ;
                        newApproval.Deskripsi = Deskripsi.Text;
                        newApproval.RuleID = Convert.ToInt32(RuleID.Text); ;

                        Approvals.Add(newApproval);
                    }
                    ViewState.Remove("Approval");
                    ViewState.Clear();
                    ViewState.Add("Approval", Approvals);
                    gvApproval.DataSource = ViewState["Approval"];
                    gvApproval.DataBind();
                }
                catch (Exception)
                {
                    // nothing
                    ViewState.Clear();
                    Approvals.RemoveRange(0, 9999);
                    pnlApproval.Visible = true;
                    pnlRequiredAppval.Visible = false;
                    setApproval();
                }

                pnlApproval.Visible = false;
                pnlRequiredAppval.Visible = true;

                if (Approvals.Count > 0)
                {
                    pnlApproval.Visible = true;
                    pnlRequiredAppval.Visible = false;
                }
            }
        }

        void setContent()
        {
            string golongan = "";

            if (ddlGolongan.SelectedValue == "A" || ddlGolongan.SelectedValue == "C" || ddlGolongan.SelectedValue == "S")
            {
                ddlSubGolongan.Visible = true;
                string strSubGolongan = ddlGolongan.SelectedValue.Trim() + ddlSubGolongan.SelectedValue.Trim();
                if (strSubGolongan == "A0" || strSubGolongan == "A1" || strSubGolongan == "C0" || strSubGolongan == "C1" || strSubGolongan == "C2" || strSubGolongan == "C3" || strSubGolongan == "S0" || strSubGolongan == "S1" || strSubGolongan == "S2")
                {
                    golongan = "III";
                }
                else if (strSubGolongan == "A2" || strSubGolongan == "S3")
                {
                    golongan = "IV";
                }

                using (var ctx = new dsSPDDataContext())
                {
                    // plafon
                    var data = from p in ctx.msGolonganPlafons
                               where p.golongan.Equals(golongan.Trim()) && p.jenisSPD.Equals(ddlTujuan.SelectedItem.Text)
                               select p;

                    var plafon = data.Where(p => p.idPlafon == 1).ToList();

                    if (plafon.Count > 0) txBiayaMakan.Text = plafon.First().harga == null ? "" : plafon.First().harga.ToString();

                    plafon = data.Where(p => p.idPlafon == 2).ToList();

                    if (plafon.Count > 0) txUangSaku.Text = plafon.First().harga == null ? "" : plafon.First().harga.ToString();

                    plafon = data.Where(p => p.idPlafon == 3).ToList();

                    if (plafon.Count > 0) txTransportasi.Text = plafon.First().deskripsi == null ? "" : plafon.First().deskripsi;
                }
            }
            else
            {
                using (var ctx = new dsSPDDataContext())
                {
                    // plafon
                    var data = from p in ctx.msGolonganPlafons
                               where p.golongan.Equals(ddlGolongan.SelectedValue.Trim()) && p.jenisSPD.Equals(ddlTujuan.SelectedItem.Text)
                               select p;

                    var plafon = data.Where(p => p.idPlafon == 1).ToList();

                    if (plafon.Count > 0) txBiayaMakan.Text = plafon.First().harga == null ? "" : plafon.First().harga.ToString();

                    plafon = data.Where(p => p.idPlafon == 2).ToList();

                    if (plafon.Count > 0) txUangSaku.Text = plafon.First().harga == null ? "" : plafon.First().harga.ToString();

                    plafon = data.Where(p => p.idPlafon == 3).ToList();

                    if (plafon.Count > 0) txTransportasi.Text = plafon.First().deskripsi == null ? "" : plafon.First().deskripsi;
                }
            }


        }

        void setApproval()
        {
            if (ddlGolongan.SelectedValue == "A" || ddlGolongan.SelectedValue == "C" || ddlGolongan.SelectedValue == "S")
            {
                string strSubGolongan = ddlGolongan.SelectedValue.Trim() + ddlSubGolongan.SelectedValue.Trim();
                if (strSubGolongan == "A0" || strSubGolongan == "A1" || strSubGolongan == "C0" || strSubGolongan == "C1" || strSubGolongan == "C2" || strSubGolongan == "C3" || strSubGolongan == "S0" || strSubGolongan == "S1" || strSubGolongan == "S2")
                {
                    using (var ctx = new dsSPDDataContext())
                    {
                        ViewState.Remove("Approval");
                        ViewState.Clear();
                        var dataApproval = (from x in ctx.ApprovalRules
                                            join y in ctx.ApprovalStatus
                                            on new { X1 = x.RuleID, X2 = txNoSPD.Text } equals new { X1 = y.RuleID, X2 = y.NoSPD } into aps
                                            from y1 in aps.DefaultIfEmpty()
                                            where
                                                  x.Tipe == ddlTujuan.SelectedValue &&
                                                  x.TipeDetail == ddlAsal.SelectedValue &&
                                                  x.Golongan == "III" &&
                                                  x.Posisi.Equals(ddlPosisi.SelectedValue)
                                            select new Approval
                                            {
                                                NrpApproval = y1.NrpApproval,
                                                Nama = y1.Nama,
                                                IndexLevel = x.IndexLevel,
                                                Deskripsi = x.Deskripsi,
                                                Email = y1.Email,
                                                RuleID = x.RuleID
                                            }).ToList();

                        ViewState.Add("Approval", dataApproval);

                        gvApproval.DataSource = dataApproval;
                        gvApproval.DataBind();

                        pnlApproval.Visible = false;
                        pnlRequiredAppval.Visible = true;

                        if (dataApproval.Count > 0)
                        {
                            pnlApproval.Visible = true;
                            pnlRequiredAppval.Visible = false;
                        }
                    }
                }
                else if (strSubGolongan == "A2" || strSubGolongan == "S3")
                {
                    using (var ctx = new dsSPDDataContext())
                    {
                        ViewState.Remove("Approval");
                        ViewState.Clear();
                        var dataApproval = (from x in ctx.ApprovalRules
                                            join y in ctx.ApprovalStatus
                                            on new { X1 = x.RuleID, X2 = txNoSPD.Text } equals new { X1 = y.RuleID, X2 = y.NoSPD } into aps
                                            from y1 in aps.DefaultIfEmpty()
                                            where
                                                  x.Tipe == ddlTujuan.SelectedValue &&
                                                  x.TipeDetail == ddlAsal.SelectedValue &&
                                                  x.Golongan == "IV" &&
                                                  x.Posisi.Equals(ddlPosisi.SelectedValue)
                                            select new Approval
                                            {
                                                NrpApproval = y1.NrpApproval,
                                                Nama = y1.Nama,
                                                IndexLevel = x.IndexLevel,
                                                Deskripsi = x.Deskripsi,
                                                Email = y1.Email,
                                                RuleID = x.RuleID
                                            }).ToList();

                        ViewState.Add("Approval", dataApproval);

                        gvApproval.DataSource = dataApproval;
                        gvApproval.DataBind();

                        pnlApproval.Visible = false;
                        pnlRequiredAppval.Visible = true;

                        if (dataApproval.Count > 0)
                        {
                            pnlApproval.Visible = true;
                            pnlRequiredAppval.Visible = false;
                        }
                    }
                }
            }
            else
            {
                using (var ctx = new dsSPDDataContext())
                {
                    ViewState.Remove("Approval");
                    ViewState.Clear();
                    var dataApproval = (from x in ctx.ApprovalRules
                                        join y in ctx.ApprovalStatus
                                        on new { X1 = x.RuleID, X2 = txNoSPD.Text } equals new { X1 = y.RuleID, X2 = y.NoSPD } into aps
                                        from y1 in aps.DefaultIfEmpty()
                                        where
                                              x.Tipe == ddlTujuan.SelectedValue &&
                                              x.TipeDetail == ddlAsal.SelectedValue &&
                                              x.Golongan == ddlGolongan.SelectedValue &&
                                              x.Posisi.Equals(ddlPosisi.SelectedValue)
                                        select new Approval
                                        {
                                            NrpApproval = y1.NrpApproval,
                                            Nama = y1.Nama,
                                            IndexLevel = x.IndexLevel,
                                            Deskripsi = x.Deskripsi,
                                            Email = y1.Email,
                                            RuleID = x.RuleID
                                        }).ToList();

                    ViewState.Add("Approval", dataApproval);

                    gvApproval.DataSource = dataApproval;
                    gvApproval.DataBind();

                    pnlApproval.Visible = false;
                    pnlRequiredAppval.Visible = true;

                    if (dataApproval.Count > 0)
                    {
                        pnlApproval.Visible = true;
                        pnlRequiredAppval.Visible = false;
                    }
                }
            }
        }

        List<ListValueApproverModel> DataInput = new List<ListValueApproverModel>();
        public List<ListValueApproverModel> getApproveAtasan(string[] nrpArr)
        {
            var ctxOrg = new dsSPDDataContext(Konstan.ConnectionString);
            using (var ctx = new dsSPDDataContext())
            {
                var _OuRep = (from a in ctxOrg.OrgUnits
                              where nrpArr.Contains(a.Nrp.ToString())
                              select new
                              {
                                  a.OuRep
                              }).AsEnumerable().Select(x => string.Format("{0}", x.OuRep)).ToArray();

                string[] _OuRepArr = _OuRep;

                var atasan = (from a in ctxOrg.OrgUnits.AsEnumerable()
                              where
                                _OuRepArr.Contains(a.OuId)
                                && int.Parse(a.JobAbbreviation.Split('_')[0]) >= 90
                                && a.OuEnd >= DateTime.Now
                              select new
                              {
                                  a.Nrp
                              }).AsEnumerable().Select(x => string.Format("{0}", x.Nrp)).ToArray();

                string[] atasanArr = atasan;
                if (atasan.Length > 0)
                {
                    DataInput = (from a in ctx.msKaryawans
                                 where atasanArr.Contains(a.nrp)
                                 select new ListValueApproverModel
                                 {
                                     ID = a.nrp,
                                     Text = a.namaLengkap + " - " + a.nrp,
                                     Email = a.EMail
                                 }).ToList();
                }
                else
                {
                    getApproveAtasan(atasanArr);
                }

                return DataInput;
            }
        }

        public class ListValueApproverModel
        {
            public string ID { get; set; }
            public string Text { get; set; }
            public string NRP { get; set; }
            public string Email { get; set; }
            public string Position { get; set; }
            public string OuId { get; set; }
            public string NamaLengkap { get; set; }
            public string JobAbservation { get; set; }
            public string IndexLevel { get; set; }
            public string Desckripsi { get; set; }
            public string RuleID { get; set; }
            public string ReportUIID { get; set; }
            public string ApproverRule { get; set; }
        }

        //void tujuanLain(bool enable)
        //{
        //    ddlPersonalAreaTujuan.Enabled = !enable;
        //    rvPersonalAreaTujuan.Enabled = !enable;
        //    ddlPSubAreaTujuan.Enabled = !enable;
        //    rvPSubAreaTujuan.Enabled = !enable;
        //    txTempatTujuanLain.Enabled = enable;
        //    rvTempatTujuanLain.Enabled = enable;

        //    if (enable)
        //    {
        //        ddlPersonalAreaTujuan.Items.Clear();
        //        ddlPSubAreaTujuan.Items.Clear();
        //    }
        //}

        void angkutanLain(bool enable)
        {
            txAngkutanLain.Enabled = enable;
            rvAngkutanLain.Enabled = enable;
        }

        void enableUangMukaAndAlasan()
        {
            if (!string.IsNullOrEmpty(txtTglBerangkat.Text) && !string.IsNullOrEmpty(txtTglKembali.Text))
            {
                TimeSpan Jumlahhari = (Convert.ToDateTime(txtTglKembali.Text) - Convert.ToDateTime(txtTglBerangkat.Text));
                if (Jumlahhari.Days >= 5)
                {
                    txUangMuka.ReadOnly = false;
                }
                else
                {
                    txUangMuka.ReadOnly = true;
                }

                if (Jumlahhari.Days > 0)
                {
                    txAlasan.Enabled = true;
                    rvAlasan.Enabled = true;
                    reAlasan.Enabled = true;
                }
                else
                {
                    txAlasan.Enabled = false;
                    rvAlasan.Enabled = false;
                    reAlasan.Enabled = false;
                }
            }
        }

        void isSec(string nrp)
        {
            using (var ctx = new dsSPDDataContext())
            {
                var user = (from u in ctx.msUsers
                            join k in ctx.msKaryawans on u.nrp equals k.nrp
                            where u.roleId == 23 && k.nrp == nrp
                            orderby k.namaLengkap
                            select new
                            {
                                namaLengkap = k.namaLengkap,
                                nrp = k.nrp
                            }).Distinct();
                if (user.Count() > 0)
                {
                    pnlSekertaris.Visible = true;
                    rvSelfOrDirect.Enabled = true;
                }
                else
                {
                    pnlSekertaris.Visible = false;
                    rvSelfOrDirect.Enabled = false;
                }
            }
        }

        void setDirect()
        {
            using (var ctx = new dsSPDDataContext())
            {
                ddlDireksi.Visible = true;
                rvDireksi.Enabled = true;
                ddDireksi.Visible = true;

                ddlDireksi.Items.Clear();

                List<SelectListItem> direksi = new List<SelectListItem>();
                direksi.Add(new SelectListItem() { id = "", text = " - Select One - " });

                direksi.AddRange((from p in ctx.msUsers
                                  join k in ctx.msKaryawans on p.nrp equals k.nrp
                                  where p.roleId == 14 || p.roleId == 13
                                  orderby k.namaLengkap
                                  select new SelectListItem
                                  {
                                      id = k.nrp,
                                      text = k.namaLengkap
                                  }).Distinct().ToList());
                //direksi.AddRange((from p in ctx.msKaryawans
                //                  where p.job.Contains("Sera director")
                //                  orderby p.namaLengkap
                //                  select new SelectListItem
                //                  {
                //                      id = p.nrp,
                //                      text = p.namaLengkap
                //                  }).Distinct().ToList());

                ddlDireksi.DataSource = direksi;

                ddlDireksi.DataValueField = "id";
                ddlDireksi.DataTextField = "text";

                ddlDireksi.DataBind();
                ddlDireksi.SelectedValue = string.Empty;
            }
        }

        // disable hotel jika tanggal sama, validasi tanggal berangkat tidak boleh lebih kecil dari pada kembali
        void setDisabledHotelAndNotLessThan()
        {
            DateTime? berangkat = thisDateTime(txtTglBerangkat.Text, "00"), kembali = thisDateTime(txtTglKembali.Text, "00");

            ddlPenginapan.SelectedValue = "Disediakan";
            ddlPenginapan.Enabled = true;

            if (berangkat == kembali)
            {
                ddlPenginapan.SelectedValue = "Tidak disediakan";
                ddlPenginapan.Enabled = false;
            }

            if (berangkat > kembali) lblDateLessThan.Visible = true;
            else lblDateLessThan.Visible = false;
        }

        public bool validateDate()
        {
            DateTime? berangkat = DateTime.Now, kembali = berangkat;
            if (!string.IsNullOrEmpty(txtTglBerangkat.Text) &&
                !string.IsNullOrEmpty(ddlJamBerangkat.SelectedValue) &&
                !string.IsNullOrEmpty(txtTglKembali.Text) &&
                !string.IsNullOrEmpty(ddlJamKembali.SelectedValue))
            {
                berangkat = thisDateTime(txtTglBerangkat.Text, ddlJamBerangkat.SelectedValue);
                kembali = thisDateTime(txtTglKembali.Text, ddlJamKembali.SelectedValue);
            }
            if (kembali <= berangkat) return false;
            else return true;
        }

        private DateTime? thisDateTime(string date, string time)
        {
            try
            {
                DateTime newDate = Convert.ToDateTime(date);
                double newTime = Convert.ToDouble(time);
                return new DateTime(newDate.Year, newDate.Month, newDate.Day, 0, 0, 0).AddHours(newTime);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //============== try TO DO OrgUnit Command ============== start
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SelectListItem> GetAtasan(string searchText, string additionalFilter, int page, string posisi, string tujuan)
        {
            using (var ctx = new dsSPDDataContext())
            {
                List<SelectListItem> data = new List<SelectListItem>();

                if (!string.IsNullOrEmpty(posisi))
                {
                    ////cr 2015-04-14 ian : maintain posisi from ApprovalPosition
                    //var positionDesc = (from p in ctx.ApprovalPositions
                    //            where p.PositionMatrix == posisi.ToLower()
                    //            select p.PositionDescription).ToList();
                    ////end

                    //perubahan 27 september 2018     
                    //karyawan.coCd = karyawan.coCd != null ? karyawan.coCd : String.Empty;                
                    SqlCommand cmd = new SqlCommand("sp_GetAtasan", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
                    cmd.Parameters.AddWithValue("@namaLengkap", searchText.ToLower());
                    cmd.Parameters.AddWithValue("@position", posisi.ToLower());
                    cmd.Parameters.AddWithValue("@nrp", karyawan.nrp);
                    cmd.Parameters.AddWithValue("@tujuan", tujuan);
                    cmd.Parameters.AddWithValue("@coCd", karyawan.coCd);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection.Open();
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SelectListItem item = new SelectListItem();
                        item.id = reader["nrp"].ToString();
                        item.text = reader["namaLengkap"].ToString() + '-' + reader["nrp"].ToString();
                        data.Add(item);
                    }
                    if (data != null)
                    {
                        data = data.Skip(10 * (page - 1)).Take(10).ToList();
                    }
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();//
                    
                    /*
                    switch (posisi.ToLower())
                    {
                        case "bm":
                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                    && (p.Job.Contains("Branch Manager")
                                        || p.Job.Contains("General Manager")
                                        || p.posisi.Contains("Branch Manager")
                                        || p.posisi.Contains("General Manager")
                                        || p.namaLengkap.Contains("BENHARD HUMISAR")
                                        || p.nrp.Contains("551")
                                        )
                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.namaLengkap + '-' + p.nrp,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();

                            break;

                        case "cfat div head":
                            //hanya pak hasan
                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                    && p.namaLengkap.Contains("HASAN KUNTARTO TEDJASUTISNO")
                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.namaLengkap + '-' + p.nrp,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();

                            break;

                        case "dic":

                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                    && p.Job.Contains("Director") || p.posisi.Contains("Director")
                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.namaLengkap + '-' + p.nrp,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();

                            break;

                        case "finance director":


                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                    && p.namaLengkap.Contains("YUDAS TADEUS GO WIE LIEN") 

                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.namaLengkap + '-' + p.nrp,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();

                            break;

                        case "gm/om/rm":

                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                    && (p.Job.Contains("General Manager")
                                        || p.Job.Contains("Operation Manager")
                                        || p.Job.Contains("TRAC Operation Chief")
                                        || p.Job.Contains("Regional Manager")
                                        || p.Job.Contains("TRAC Retail Sales Head")
                                        || p.Job.Contains("TRAC Corporate Sales Head")
                                        || p.Job.Contains("SERA COO Rental Unit")
                                        || p.Job.Contains("SERA Corp IST Div Head")
                                        || p.posisi.Contains("General Manager")
                                        || p.posisi.Contains("Operation Manager")
                                        || p.posisi.Contains("TRAC Operation Chief")
                                        || p.posisi.Contains("Regional Manager")
                                        || p.posisi.Contains("TRAC Retail Sales Head")
                                        || p.posisi.Contains("TRAC Corporate Sales Head")
                                        || p.posisi.Contains("SERA COO Rental Unit")
                                        || p.posisi.Contains("IBID COO")
                                        || p.posisi.Contains("SMM Director")
                                        || p.posisi.Contains("COO Car Auction")
                                        || p.namaLengkap.Contains("RADEN DEFFI CHALID")
                                        || p.namaLengkap.Contains("SUKMA FITRI ASTUTI")
                                        || p.namaLengkap.Contains("HADI WINARTO")
                                        || p.namaLengkap.Contains("HALOMOAN FISCHER")
                                        || p.namaLengkap.Contains("DADDY DOXA MANURUNG")
                                        || p.namaLengkap.Contains("QODARUL MUJIB")
                                        || p.namaLengkap.Contains("Irfan Irawan")
                                        || p.namaLengkap.Contains("NICOLAS PAHALA HUTABARAT")
                                        || p.namaLengkap.Contains("TRI EDI MULYONO HERLAMBANG")
                                        || p.namaLengkap.Contains("ANANG YUNIANTO")
                                        || p.nrp.Contains("324")
                                        || p.namaLengkap.Contains("SETIAWAN ADI SAPUTRO")
                                        || p.namaLengkap.Contains("MAS TEDDY ASTRA MANGKUBRATA")
                                        || p.namaLengkap.Contains("BENHARD HUMISAR")

                                        )
                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.namaLengkap + '-' + p.nrp,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();

                            break;

                        case "kadept/am/om":

                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower())
                                    && p.nrp != karyawan.nrp
                                    && p.coCd == karyawan.coCd
                                    && (p.Job.Contains("Dept Head")
                                        || p.Job.Contains("TRAC Sales Project Manager")
                                        || p.Job.Contains("TRAC 4x4 Sales Force Manager")
                                        || p.Job.Contains("TRAC Retail Sales Branch Manager")
                                        || p.Job.Contains("Account Manager")
                                        || p.Job.Contains("Operation Manager")
                                        || p.Job.Contains("Dept Hea")
                                        || p.posisi.Contains("Dept Head")
                                        || p.posisi.Contains("TRAC Sales Project Manager")
                                        || p.posisi.Contains("TRAC 4x4 Sales Force Manager")
                                        || p.posisi.Contains("TRAC Retail Sales Branch Manager")
                                        || p.posisi.Contains("Account Manager")
                                        || p.posisi.Contains("Operation Manager")
                                         || p.posisi.Contains("SMM Director")
                                        //cr 2015-04-14 add kadept for renata
                                        || p.namaLengkap.Contains("RENATA INDRIANA")
                                        //end cr
                                        || p.namaLengkap.Contains("SUSANTI ELIANA")
                                        || p.namaLengkap.Contains("SUKMA FITRI ASTUTI")
                                        || p.namaLengkap.Contains("ROIRIL")
                                        || p.namaLengkap.Contains("FARIS PRIYANTO")
                                        || p.namaLengkap.Contains("FRANSISCA YOE YAUW SIEK")
                                        || p.namaLengkap.Contains("ZAKI YAMANI")
                                        || p.namaLengkap.Contains("NURAINI ROBIAH")
                                        || p.namaLengkap.Contains("STEFANUS HANDOJO KUSUMADJAJA")
                                        || p.namaLengkap.Contains("DESTI INDRIYASARI")
                                        || p.namaLengkap.Contains("HASAN KUNTARTO TEDJASUTISNO")

                                        )
                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.namaLengkap + '-' + p.nrp,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();
                            break;
                        case "kadiv/om":

                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                    && p.coCd == karyawan.coCd
                                    && (p.Job.Contains("Div Head")
                                        || p.Job.Contains("Operation Manager")
                                        || p.Job.Contains("TRAC Corporate Sales Head")
                                        || p.Job.Contains("TRAC Operation Chief")
                                        || p.Job.Contains("TRAC Retail Sales Head")
                                        || p.posisi.Contains("Div Head")
                                        || p.posisi.Contains("Operation Manager")
                                        || p.posisi.Contains("TRAC Corporate Sales Head")
                                        || p.posisi.Contains("TRAC Operation Chief")
                                        || p.posisi.Contains("TRAC Retail Sales Head")
                                        //add kadiv for Hadi winarto
                                        || p.namaLengkap.Contains("HADI WINARTO")
                                        || p.namaLengkap.Contains("FERDINAND MATONDANG"))
                                        || p.namaLengkap.Contains("YUDAS TADEUS GO WIE LIEN") ||
                                        p.Job.Contains("Chief") || p.posisi.Contains("Chief")
                                        || p.namaLengkap.Contains("HASAN KUNTARTO TEDJASUTISNO")
                                        || p.nrp.Contains("324")

                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.nrp + '-' + p.namaLengkap,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();
                            break;
                        case "presiden director":
                            if(tujuan == "Luar Negeri")
                                data = (from p in ctx.msKaryawans
                                        where
                                        p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                        && (p.Job.Contains("SERA President Director") || p.posisi.Contains("SERA President Director"))
                                        && p.nrp == "3641"
                                        select new SelectListItem
                                        {
                                            id = p.nrp,
                                            text = p.namaLengkap + '-' + p.nrp,
                                        }).Skip(10 * (page - 1)).Take(10).ToList();
                            else
                                data = (from p in ctx.msKaryawans
                                        where
                                        p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                        && p.Job.Contains("President Director") || p.posisi.Contains("President Director")
                                        select new SelectListItem
                                        {
                                            id = p.nrp,
                                            text = p.namaLengkap + '-' + p.nrp,
                                        }).Skip(10 * (page - 1)).Take(10).ToList();
                            break;
                        case "bm/kadept/am/om":

                            data = (from p in ctx.msKaryawans
                                    where
                                    p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                                    && (p.Job.Contains("Branch Manager")
                                        || p.Job.Contains("Dept Head")
                                        || p.Job.Contains("Departement Head")
                                        || p.Job.Contains("Account Manager")
                                        || p.Job.Contains("General Manager")
                                        || p.Job.Contains("Operation Manager")
                                        || p.posisi.Contains("Branch Manager")
                                        || p.posisi.Contains("Dept Head")
                                        || p.posisi.Contains("Departement Head")
                                        || p.posisi.Contains("Account Manager")
                                        || p.posisi.Contains("General Manager")
                                        || p.posisi.Contains("Operation Manager")
                                        || p.posisi.Contains("Branch Head")
                                        || p.posisi.Contains("SMM Director")
                                        //cr 2015-04-14 add kadept for renata
                                        || p.namaLengkap.Contains("RENATA INDRIANA")
                                        || p.namaLengkap.Contains("CATHARINA HERLINA")
                                        || p.namaLengkap.Contains("DADDY DOXA MANURUNG")
                                        || p.namaLengkap.Contains("SUKMA FITRI ASTUTI")
                                        || p.namaLengkap.Contains("BENHARD HUMISAR")
                                        || p.namaLengkap.Contains("ROIRIL")
                                        || p.namaLengkap.Contains("FARIS PRIYANTO")
                                        || p.namaLengkap.Contains("HALOMOAN FISCHER")
                                        || p.namaLengkap.Contains("PETERSON")
                                        || p.namaLengkap.Contains("STEFANUS HANDOJO KUSUMADJAJA")
                                        || p.namaLengkap.Contains("DESTI INDRIYASARI")
                                        || p.nrp.Contains("551")
                                        //end cr
                                        //kadept for SUSANTI ELIANA 17-02-2016
                                        )
                                    select new SelectListItem
                                    {
                                        id = p.nrp,
                                        text = p.namaLengkap + '-' + p.nrp,
                                    }).Skip(10 * (page - 1)).Take(10).ToList();

                            break;
                        default:
                            break;
                    }*/
                }
                else
                {
                    data = (from p in ctx.msKaryawans
                            where
                            p.namaLengkap.ToLower().Contains(searchText.ToLower()) && p.nrp != karyawan.nrp
                            orderby p.namaLengkap
                            select new SelectListItem
                            {
                                id = p.nrp,
                                text = p.namaLengkap + '-' + p.nrp,
                            }).Skip(10 * (page - 1)).Take(10).ToList();
                }

                return data;
            }
        }
        //============== try TO DO OrgUnit Command ============== end

        //============== try TO DO OrgUnit UnCommand ============== start
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<SelectListItem> GetAtasan(string searchText, string additionalFilter, int page, string posisi, int index, string nrp, string prefPosition, string tujuan)
        //{
        //    DateTime now = DateTime.Now;

        //    using (var ctx = new dsSPDDataContext())
        //    {
        //        if (karyawan.nrp.Equals("99999999"))
        //        {
        //            return (from a in ctx.msKaryawans
        //                    join b in ctx.vwOrgUnits on a.nrp equals b.Nrp.ToString()
        //                    where
        //                        a.namaLengkap.ToLower().Contains(searchText.ToLower()) && a.nrp != karyawan.nrp
        //                        && b.PosChief != "0"
        //                        && (b.OuStart <= now && b.OuEnd >= now)
        //                        && (b.PosStart <= now && b.PosEnd >= now)
        //                        && (b.JobStart <= now && b.JobEnd >= now)
        //                        && (b.JobStart <= now && b.JobEnd >= now)
        //                        && (b.PerStart <= now && b.PerEnd >= now)
        //                        && (b.CcStart <= now && b.CcEnd >= now)
        //                    select new SelectListItem
        //                    {
        //                        id = a.nrp,
        //                        text = a.namaLengkap + '-' + a.nrp,
        //                    }).Skip(10 * (page - 1)).Take(10).ToList();
        //        }

        //        string _nrp = string.Empty;

        //        switch (posisi.ToLower())
        //        {
        //            case "finance director":
        //                return (from mp in ctx.msPositions
        //                        join ou in ctx.vwOrgUnits on mp.PositionId equals ou.PosId
        //                        join mk in ctx.msKaryawans on ou.Nrp.ToString() equals mk.nrp
        //                        where
        //                            mp.ApprovalRule == "Finance Director"
        //                            && (ou.OuStart <= now && ou.OuEnd >= now)
        //                            && (ou.PosStart <= now && ou.PosEnd >= now)
        //                            && (ou.JobStart <= now && ou.JobEnd >= now)
        //                            && (ou.JobStart <= now && ou.JobEnd >= now)
        //                            && (ou.PerStart <= now && ou.PerEnd >= now)
        //                            && (ou.CcStart <= now && ou.CcEnd >= now)
        //                        select new SelectListItem
        //                        {
        //                            id = mk.nrp,
        //                            text = mk.namaLengkap + '-' + mk.nrp,
        //                        }).Skip(10 * (page - 1)).Take(10).ToList();

        //            case "cfat div head":
        //                return (from mp in ctx.msPositions
        //                        join ou in ctx.vwOrgUnits on mp.PositionId equals ou.PosId
        //                        join mk in ctx.msKaryawans on ou.Nrp.ToString() equals mk.nrp
        //                        where
        //                            mp.ApprovalRule == "CFAT Div Head"
        //                            && (ou.OuStart <= now && ou.OuEnd >= now)
        //                            && (ou.PosStart <= now && ou.PosEnd >= now)
        //                            && (ou.JobStart <= now && ou.JobEnd >= now)
        //                            && (ou.JobStart <= now && ou.JobEnd >= now)
        //                            && (ou.PerStart <= now && ou.PerEnd >= now)
        //                            && (ou.CcStart <= now && ou.CcEnd >= now)
        //                        select new SelectListItem
        //                        {
        //                            id = mk.nrp,
        //                            text = mk.namaLengkap + '-' + mk.nrp,
        //                        }).Skip(10 * (page - 1)).Take(10).ToList();

        //            default:
        //                if (index > 0 && string.IsNullOrEmpty(nrp))
        //                    return new List<SelectListItem>();

        //                List<string> nrps = new List<string>()
        //                {
        //                    karyawan.nrp
        //                };

        //                if (!string.IsNullOrEmpty(nrp))
        //                    nrps[0] = nrp;

        //                atasanArr = new List<string>();

        //                return GetApproveAtasan(nrps, page, prefPosition, index);
        //        }
        //    }
        //}
        //============== try TO DO OrgUnit UnCommand ============== end

        public static List<SelectListItem> GetApproveAtasan(List<string> nrps, int page, string posisi, int index)
        {
            List<SelectListItem> datas = new List<SelectListItem>();
            DateTime now = DateTime.Now;

            using (var ctx = new dsSPDDataContext())
            {
                //get data OuName
                var OuName = (from a in ctx.vwOrgUnits
                              where
                                nrps.Contains(a.Nrp.ToString())
                                && (a.OuStart <= now && a.OuEnd >= now)
                                && (a.PosStart <= now && a.PosEnd >= now)
                                && (a.JobStart <= now && a.JobEnd >= now)
                                && (a.JobStart <= now && a.JobEnd >= now)
                                && (a.PerStart <= now && a.PerEnd >= now)
                                && (a.CcStart <= now && a.CcEnd >= now)
                              select new
                              {
                                  a.OuName
                              }).FirstOrDefault();

                if (OuName != null)
                {
                    //cek OuName SERA Director atau bukan jika iya, maka dropdown menampilkan nrp dan nama finance director
                    if (OuName.OuName == "SERA Director")
                    {
                        return (from mp in ctx.msPositions
                                join ou in ctx.vwOrgUnits on mp.PositionId equals ou.PosId
                                join mk in ctx.msKaryawans on ou.Nrp.ToString() equals mk.nrp
                                where
                                    mp.ApprovalRule == "Finance Director"
                                    && (ou.OuStart <= now && ou.OuEnd >= now)
                                    && (ou.PosStart <= now && ou.PosEnd >= now)
                                    && (ou.JobStart <= now && ou.JobEnd >= now)
                                    && (ou.JobStart <= now && ou.JobEnd >= now)
                                    && (ou.PerStart <= now && ou.PerEnd >= now)
                                    && (ou.CcStart <= now && ou.CcEnd >= now)
                                select new SelectListItem
                                {
                                    id = mk.nrp,
                                    text = mk.namaLengkap + '-' + mk.nrp,
                                }).Skip(10 * (page - 1)).Take(10).ToList();
                    }
                }

                //jika ouname bukan Finance Director
                var _Ou = (from a in ctx.vwOrgUnits
                           where
                                nrps.Contains(a.Nrp.ToString())
                                && (a.OuStart <= now && a.OuEnd >= now)
                                && (a.PosStart <= now && a.PosEnd >= now)
                                && (a.JobStart <= now && a.JobEnd >= now)
                                && (a.JobStart <= now && a.JobEnd >= now)
                                && (a.PerStart <= now && a.PerEnd >= now)
                                && (a.CcStart <= now && a.CcEnd >= now)
                           select new
                           {
                               a.OuId
                                ,
                               a.OuRep
                           }).FirstOrDefault();

                var _OuIdData = (from a in ctx.vwOrgUnits
                                 where
                                    a.OuId == _Ou.OuId
                                    && a.PosChief != "0"
                                    && !nrps.Contains(a.Nrp.ToString())
                                    && (a.OuStart <= now && a.OuEnd >= now)
                                    && (a.PosStart <= now && a.PosEnd >= now)
                                    && (a.JobStart <= now && a.JobEnd >= now)
                                    && (a.JobStart <= now && a.JobEnd >= now)
                                    && (a.PerStart <= now && a.PerEnd >= now)
                                    && (a.CcStart <= now && a.CcEnd >= now)
                                 select new
                                 {
                                     a.Nrp
                                 }).AsEnumerable().Select(x => string.Format("{0}", x.Nrp)).ToList();

                if (_OuIdData.Count == 0)
                {
                    var _OuRepData = (from a in ctx.vwOrgUnits
                                      where
                                        a.OuId == _Ou.OuRep
                                        && a.PosChief != "0"
                                        && !nrps.Contains(a.Nrp.ToString())
                                        && (a.OuStart <= now && a.OuEnd >= now)
                                        && (a.PosStart <= now && a.PosEnd >= now)
                                        && (a.JobStart <= now && a.JobEnd >= now)
                                        && (a.JobStart <= now && a.JobEnd >= now)
                                        && (a.PerStart <= now && a.PerEnd >= now)
                                        && (a.CcStart <= now && a.CcEnd >= now)
                                      select new
                                      {
                                          a.Nrp
                                      }).AsEnumerable().Select(x => string.Format("{0}", x.Nrp)).ToList();
                    datas = (from a in ctx.msKaryawans
                             where _OuRepData.Contains(a.nrp)
                             select new SelectListItem
                             {
                                 id = a.nrp,
                                 text = a.namaLengkap + " - " + a.nrp
                             }).Skip(10 * (page - 1)).Take(10).ToList();

                    if (datas.Count > 0)
                    {
                        return datas;
                    }
                    else
                    {
                        var dataJob = (from a in ctx.vwOrgUnits
                                       where
                                           nrps.Contains(a.Nrp.ToString())
                                           && (a.OuStart <= now && a.OuEnd >= now)
                                           && (a.PosStart <= now && a.PosEnd >= now)
                                           && (a.JobStart <= now && a.JobEnd >= now)
                                           && (a.JobStart <= now && a.JobEnd >= now)
                                           && (a.PerStart <= now && a.PerEnd >= now)
                                           && (a.CcStart <= now && a.CcEnd >= now)
                                       select new
                                       {
                                           a.JobAbbreviation
                                       }).FirstOrDefault();

                        if (int.Parse(dataJob.JobAbbreviation.Split('_')[0]) == 120)
                        {
                            datas = (from a in ctx.msKaryawans
                                     where nrps.Contains(a.nrp)
                                     select new SelectListItem
                                     {
                                         id = a.nrp,
                                         text = a.namaLengkap + " - " + a.nrp
                                     }).Skip(10 * (page - 1)).Take(10).ToList();
                        }

                        return datas;
                    }
                }
                else
                {
                    datas = (from a in ctx.msKaryawans
                             where _OuIdData.Contains(a.nrp)
                             select new SelectListItem
                             {
                                 id = a.nrp,
                                 text = a.namaLengkap + " - " + a.nrp
                             }).Skip(10 * (page - 1)).Take(10).ToList();

                    return datas;
                }
            }
        }

        public class SelectListItem
        {
            public string id { get; set; }
            public string text { get; set; }
        }

        [Serializable]
        public class Approval
        {
            public string NrpApproval { get; set; }
            public string Nama { get; set; }
            public int? IndexLevel { get; set; }
            public string Deskripsi { get; set; }
            public string Email { get; set; }
            public int RuleID { get; set; }
        }

        private void LoadDdlCompanyTujuan()
        {
            ddlCompanyTujuan.Items.Clear();
            ddlCompanyTujuan.Items.Add(new ListItem("- Select One -"));
            ddlCompanyTujuan.Items.Add(new ListItem("LAIN - LAIN", "0"));

            ddlCompanyTujuan.AppendDataBoundItems = true;

            using (var ctx = new dsSPDDataContext())
            {
                var data = (from a in ctx.msKaryawans
                            select new
                            {
                                a.coCd,
                                a.companyCode
                            }).Distinct().ToList();

                ddlCompanyTujuan.DataSource = data;
                ddlCompanyTujuan.DataTextField = "companyCode";
                ddlCompanyTujuan.DataValueField = "coCd";
                ddlCompanyTujuan.DataBind();
            }
        }

        protected void ddlSubGolongan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string golongan = "";
            string strSubGolongan = ddlGolongan.SelectedValue.Trim() + ddlSubGolongan.SelectedValue.Trim();
            if (strSubGolongan == "A0" || strSubGolongan == "A1" || strSubGolongan == "C0" || strSubGolongan == "C1" || strSubGolongan == "C2" || strSubGolongan == "C3" || strSubGolongan == "S0" || strSubGolongan == "S1" || strSubGolongan == "S2")
            {
                golongan = "III";
            }
            else if (strSubGolongan == "A2" || strSubGolongan == "S3")
            {
                golongan = "IV";
            }

            using (var ctx = new dsSPDDataContext())
            {
                // plafon
                var data = from p in ctx.msGolonganPlafons
                           where p.golongan.Equals(golongan.Trim()) && p.jenisSPD.Equals(ddlTujuan.SelectedItem.Text)
                           select p;

                var plafon = data.Where(p => p.idPlafon == 1).ToList();

                if (plafon.Count > 0) txBiayaMakan.Text = plafon.First().harga == null ? "" : plafon.First().harga.ToString();

                plafon = data.Where(p => p.idPlafon == 2).ToList();

                if (plafon.Count > 0) txUangSaku.Text = plafon.First().harga == null ? "" : plafon.First().harga.ToString();

                plafon = data.Where(p => p.idPlafon == 3).ToList();

                if (plafon.Count > 0) txTransportasi.Text = plafon.First().deskripsi == null ? "" : plafon.First().deskripsi;
            }

            setApproval();
        }
    }
}