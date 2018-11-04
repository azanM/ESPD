using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;
using AjaxControlToolkit;

namespace eSPD
{
    public partial class frmClaimInput : System.Web.UI.Page
    {
        dsSPDDataContext ctx = new dsSPDDataContext();
        classSpd oSPD = new classSpd();
        msKaryawan karyawan = new msKaryawan();
        //private string strNrp = "";

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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmClaimApproval.aspx");
            }
            karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

            visibleFile(false);
            string strNoSpd = string.Empty, strRole = string.Empty;
            bool editable = true;
            bool gamode = false;
            if (!IsPostBack)
            {
                resetCHK(false);
                txtHotel.Enabled = false;
                strNoSpd = (string)Session["noSPD"];
                Session["noSPDc"] = Session["noSPD"];
                Session["noSPD"] = string.Empty;
                strRole = (string)Session["Role"];
                editable = (bool)Session["editable"];
                if (Session["gamode"] != null)
                {
                    gamode = (bool)Session["gamode"];
                    Session["gamode"] = false;
                }

                FLDgamode.Value = gamode.ToString().ToLower();

                if (strRole == "GA")
                {
                    txtSTMakan.Enabled = true;
                    txtSTUangSk.Enabled = true;
                    txtSTMakanDLR.Enabled = true;
                    txtSTUangSkDLR.Enabled = true;
                    txtLaundry.Enabled = true;
                    resetCHK(true);
                }

                //jika di trClaim ada
                if (strNoSpd != string.Empty && !editable)
                {
                    txtNoSPD.Text = strNoSpd;
                    btnSave.Enabled = false;
                    btnSubmit.Enabled = false;
                    btnReset.Enabled = false;
                    txtNoSPD_TextChanged(null, null);
                    if (gamode)
                    {

                        //if (Convert.ToInt32(lblJumlahhari.Text) >= 3)
                        if (Convert.ToInt32(lblJumlahhari.Text) >= 5)
                        {
                            //txtUangMuka.Text = string.Empty;
                            txtUangMuka.Enabled = true;
                        }
                        else
                        {
                            txtUangMuka.Enabled = false;
                        }
                    }



                }

                //jika di trClaim tidak ada
                if (strNoSpd != string.Empty && editable)
                {
                    txtNoSPD.Text = strNoSpd;
                    btnSave.Enabled = true;
                    btnSubmit.Enabled = true;
                    txtNoSPD_TextChanged(null, null);
                    if (gamode)
                    {

                        //if (Convert.ToInt32(lblJumlahhari.Text) >= 3)
                        if (Convert.ToInt32(lblJumlahhari.Text) >= 5)
                        {
                            //txtUangMuka.Text = string.Empty;
                            txtUangMuka.Enabled = true;
                        }
                        else
                        {
                            txtUangMuka.Enabled = false;
                        }
                    }

                }
                //GA bisa mengedit uang saku dan uang makan

                dsSPDDataContext data = new dsSPDDataContext();
                trClaim claim = (from c in data.trClaims
                                 where c.noSPD.Equals(txtNoSPD.Text)
                                 select c).FirstOrDefault();
                if (claim == null)
                {
                    btnSubmit.Enabled = false;
                }

                //trSPD spd = new trSPD();
                //trSPD query = (from p in data.trSPDs
                //               where p.noSPD.Equals(txtNoSPD.Text)
                //               select p).FirstOrDefault();

                //msGolonganPlafon golPlaf = new msGolonganPlafon();
                //msGolonganPlafon query2 = (from a in data.msGolonganPlafons
                //                           where a.idPlafon.Equals(6)
                //                           select a).FirstOrDefault();
                //if (query != null && query.penginapan == "Tidak Disediakan")
                //{
                //    //txtHotel.ReadOnly = false;
                //    //lblJumlahhari * query2.har
                //    //txtHotel.Text = 
                //}



                ////ix cr
                //#region set penginapan
                //try
                //{
                //    if (claim == null)
                //    {
                //        var hasil = (from q in data.msGolonganPlafons
                //                     where q.golongan.Equals(query.idGolongan) && q.jenisSPD.ToLower().Equals(query.Tujuan) && q.wilayah.ToLower().Equals(query.WilayahTujuan)
                //                     select q);

                //        if (query.isHotel == false)
                //        {
                //            TimeSpan Jumlahhari = (query.tglKembali - query.tglBerangkat);
                //            TimeSpan tambahan = TimeSpan.FromDays(1);
                //            Jumlahhari += tambahan;

                //            var jumlahPenginapan = hasil.FirstOrDefault(o => o.idPlafon == 6) != null ? hasil.FirstOrDefault(o => o.idPlafon == 6).harga.Value * Jumlahhari.Days : 0;

                //            if (jumlahPenginapan >= 5)
                //            {
                //                txtUangMuka.ReadOnly = true;
                //            }

                //            txtHotel.Text = jumlahPenginapan.ToString();
                //        }
                //        else { txtHotel.Text = "0"; txtHotel.ReadOnly = true; }
                //    }
                //}
                //catch (Exception) { }
                //#endregion


                if (gamode)
                {
                    btnSubmit.Enabled = false;

                }

                //if (claim.isSubmit == null)
                //{
                //    btnSubmit.Enabled = true;
                //}

            }
        }

        //visible file upload, if editable make it true else false, not equals with file download
        void visibleFile(bool isvisible)
        {
            //tiketUpload, penginapan, laundry, komunikasi, airporttax, bbm, tol, taxi, parkir
            tiketUpload.Visible = isvisible;
            penginapanUpload.Visible = isvisible;
            laundryUpload.Visible = isvisible;
            komunikasiUpload.Visible = isvisible;
            airporttaxUpload.Visible = isvisible;
            bbmUpload.Visible = isvisible;
            tolUpload.Visible = isvisible;
            taxiUpload.Visible = isvisible;
            parkirUpload.Visible = isvisible;

            // download file
        }

        private void getFromForm(ref trClaim claim)
        {
            claim.noSPD = txtNoSPD.Text;
            claim.biayaMakan = int.Parse(txtSTMakan.Text);
            claim.uangSaku = int.Parse(txtSTUangSk.Text);
            claim.tiket = int.Parse(txtTiket.Text);
            claim.tiket_cek = chkTiket.Checked;
            claim.hotel = int.Parse(txtHotel.Text);
            claim.hotel_cek = chkPenginapan.Checked;
            claim.BBM = int.Parse(txtBBM.Text);
            claim.BBM_cek = chkBBM.Checked;
            claim.tol = int.Parse(txtTol.Text);
            claim.tol_cek = chkTol.Checked;
            claim.taxi = int.Parse(txtTaxi.Text);
            claim.taxi_cek = chkTaxi.Checked;
            claim.airportTax = int.Parse(txtAirPortTax.Text);
            claim.airportTax_cek = chkAirportTax.Checked;
            claim.laundry = int.Parse(txtLaundry.Text);
            claim.laundry_cek = chkLaundry.Checked;
            claim.parkir = int.Parse(txtParkir.Text);
            claim.parkir_cek = chkParkir.Checked;
            claim.ketLainLain = txtLainlainDetail.Text;
            claim.biayaLainLain = int.Parse(txtLainlain.Text);
            claim.total = int.Parse(txtTotal.Text);
            claim.nrpAtasan = lblAtasan.Text.Trim();
            claim.komunikasi = int.Parse(txtKomunikasi.Text);
            claim.komunikasi_cek = chkKomunikasi.Checked;
            claim.kurs = int.Parse(kurs.Text);
            //claim.status = "10-Claim Simpan";


        }

        private void setIntoForm(trClaim claim)
        {
            txtNoSPD.Text = claim.noSPD;
            txtSTMakan.Text = claim.biayaMakan.ToString();
            txtSTUangSk.Text = claim.uangSaku.ToString();
            txtTiket.Text = claim.tiket.ToString();
            chkTiket.Checked = claim.tiket_cek == null ? false : Convert.ToBoolean(claim.tiket_cek);
            txtHotel.Text = claim.hotel.ToString();
            chkPenginapan.Checked = claim.hotel_cek == null ? false : Convert.ToBoolean(claim.hotel_cek);
            txtBBM.Text = claim.BBM.ToString();
            chkBBM.Checked = claim.BBM_cek == null ? false : Convert.ToBoolean(claim.BBM_cek);
            txtTol.Text = claim.tol.ToString();
            chkTol.Checked = claim.tol_cek == null ? false : Convert.ToBoolean(claim.tol_cek);
            txtTaxi.Text = claim.taxi.ToString();
            chkTaxi.Checked = claim.taxi_cek == null ? false : Convert.ToBoolean(claim.taxi_cek);
            txtAirPortTax.Text = claim.airportTax.ToString();
            chkAirportTax.Checked = claim.airportTax_cek == null ? false : Convert.ToBoolean(claim.airportTax_cek);
            txtLaundry.Text = claim.laundry.ToString();
            chkLaundry.Checked = claim.laundry_cek == null ? false : Convert.ToBoolean(claim.laundry_cek);
            txtParkir.Text = claim.parkir.ToString();
            chkParkir.Checked = claim.parkir_cek == null ? false : Convert.ToBoolean(claim.parkir_cek);
            txtLainlain.Text = claim.ketLainLain;
            txtLainlain.Text = claim.biayaLainLain.ToString();
            txtTotal.Text = claim.total.ToString();
            lblAtasan.Text = claim.nrpAtasan.Trim();
            txtKomunikasi.Text = claim.komunikasi.ToString();
            chkKomunikasi.Checked = claim.komunikasi_cek == null ? false : Convert.ToBoolean(claim.komunikasi_cek);
            kurs.Text = claim.kurs.ToString();



        }

        private void clearForm()
        {
            //txtNoSPD.Text = "";
            //txtSTMakan.Text = "0";
            //txtSTUangSk.Text = "0";
            txtTiket.Text = "0";
            txtHotel.Text = "0";
            txtBBM.Text = "0";
            txtTol.Text = "0";
            txtTaxi.Text = "0";
            txtAirPortTax.Text = "0";
            txtLaundry.Text = "0";
            txtParkir.Text = "0";
            txtLainlain.Text = "0";
            txtLainlain.Text = "0";
            // txtTotal.Text = "0";
            txtKomunikasi.Text = "0";

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["noSPDc"] == null)
            {
                Response.Redirect("frmClaimApproval.aspx");
            }
            else
            {
                using (dsSPDDataContext data = new dsSPDDataContext())
                {
                    trClaim claim = new trClaim();
                    if (!string.IsNullOrEmpty(txtNoSPD.Text))
                    {
                        trSPD spd = data.trSPDs.FirstOrDefault(o => o.noSPD == txtNoSPD.Text);
                        if (spd != null)
                        {
                            if (spd.isApproved == true && spd.isCancel == null)
                            {
                                try
                                {
                                    claim = (from c in data.trClaims
                                                    where c.noSPD == txtNoSPD.Text
                                                    select c).FirstOrDefault();

                                    bool flagCeklisControl = false;

                                    if (claim == null)
                                    {
                                        trClaim claimNew = new trClaim();
                                        getFromForm(ref claimNew);
                                        claimNew.status = "Saved";
                                        claimNew.dibuatOleh = karyawan.nrp;
                                        claimNew.dibuatTanggal = DateTime.Now;
                                        data.trClaims.InsertOnSubmit(claimNew);
                                        data.SubmitChanges();
                                        if (claimNew.tiket_cek == true || claimNew.hotel_cek == true || claimNew.laundry_cek == true || claimNew.komunikasi_cek == true || claimNew.airportTax_cek == true || claimNew.BBM_cek == true || claimNew.tol_cek == true || claimNew.taxi_cek == true || claimNew.parkir_cek == true)
                                        {
                                            flagCeklisControl = true;
                                        }
                                    }
                                    else
                                    {
                                        getFromForm(ref claim);
                                        if (FLDgamode.Value.ToString().ToLower() == "false")
                                        {
                                            claim.status = "Saved";
                                        }
                                        claim.diubahOleh = karyawan.nrp;
                                        claim.diubahTanggal = DateTime.Now;
                                        data.SubmitChanges();
                                        if (claim.tiket_cek == true || claim.hotel_cek == true || claim.laundry_cek == true || claim.komunikasi_cek == true || claim.airportTax_cek == true || claim.BBM_cek == true || claim.tol_cek == true || claim.taxi_cek == true || claim.parkir_cek == true)
                                        {
                                            flagCeklisControl = true;
                                        }
                                    }

                                    #region SendMail Checking Data
                                    if (flagCeklisControl)
                                    {
                                        msKaryawan kary = new msKaryawan();
                                        if (spd.nrp == "99999999")
                                        {
                                            kary.EMail = spd.email;
                                            kary.nrp = spd.nrp;
                                            kary.namaLengkap = spd.namaLengkap;
                                            kary.golongan = "III";
                                            kary.Job = spd.jabatan;
                                            kary.posisi = spd.jabatan;
                                            kary.coCd = "1";
                                            kary.kodePSubArea = "1";
                                            kary.kodePA = "1";

                                        }
                                        else
                                        {
                                            kary = (from kar in data.msKaryawans
                                                    where kar.nrp == spd.nrp
                                                    select kar).First();
                                        }
                                        classSpd oSpd = new classSpd();
                                        oSpd.sendMailClaim(claim, kary, spd);

                                    }
                                    #endregion

                                    btnSave.Enabled = true;
                                    if (FLDgamode.Value.ToString().ToLower() == "false") btnSubmit.Enabled = true;
                                    lblKet.Text = "Claim berhasil di-Save";
                                }
                                catch (Exception ex)
                                {
                                    Response.Write(ex.Message);
                                }
                            }
                            else
                            {
                                lblKet.Text = "SPD belum di approve atau direject atau juga dicancel";
                            }
                        }
                        else
                        {
                            lblKet.Text = "SPD tidak ditemukan";
                        }
                    }
                    else
                    {
                        lblKet.Text = "Isi no spd";
                    }

                }

            }
        }
        protected void txtNoSPD_TextChanged(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            trSPD spd = new trSPD();
            try
            {

                trClaim claimQ = (from p in data.trClaims
                                  where p.noSPD.Equals(txtNoSPD.Text)
                                  select p).FirstOrDefault();
                trSPD query = (from p in data.trSPDs
                               where p.noSPD.Equals(txtNoSPD.Text)
                               select p).FirstOrDefault();
                query.penginapan = "Disediakan";
                lblAtasan.Text = query.nrpAtasan;

                if (query != null)
                {
                    txtNamaLengkap.Text = query.namaLengkap;
                    txtTglBerangkat.Text = query.tglBerangkat.ToShortDateString();
                    txtTglKembali.Text = query.tglKembali.ToShortDateString();
                    ddlJamBerangkat0.SelectedValue = query.jamBerangkat;
                    ddlMenitBerangkat0.SelectedValue = query.menitBerangkat;
                    ddlJamKembali.SelectedValue = query.jamKembali;
                    ddlMenitKembali.SelectedValue = query.menitKembali;
                    txtUangMuka.Text = query.uangMuka != string.Empty ? query.uangMuka : "0";


                    kurs.Text = "1";

                    txtKeperluan.Text = query.idKeperluan == 6 ? query.ketKeperluan : getKeperluan(int.Parse(query.idKeperluan.ToString()));


                }


                msGolonganPlafon golongan = new msGolonganPlafon();
                var hasil = (from q in data.msGolonganPlafons
                             where q.golongan.Equals(query.idGolongan) && q.jenisSPD.ToLower().Equals(query.Tujuan) && q.wilayah.ToLower().Equals(query.WilayahTujuan)
                             select q);
                TimeSpan Jumlahhari = (query.tglKembali - query.tglBerangkat);
                TimeSpan tambahan = TimeSpan.FromDays(1);
                Jumlahhari += tambahan;

                lblJumlahhari.Text = Jumlahhari.Days.ToString();


                try
                {
                    var jumlahPenginapan = hasil.FirstOrDefault(o => o.idPlafon == 6) != null ? hasil.FirstOrDefault(o => o.idPlafon == 6).harga.Value * Jumlahhari.Days : 0;
                    
                    #region CR ian 2015-03-02
                    if (query.isHotel == false)
                    {
                        txtHotel.Text = jumlahPenginapan.ToString(); txtHotel.Enabled = true;
                    }
                    else { txtHotel.Text = "0"; txtHotel.Enabled = false; }
                    #endregion

                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                List<msGolonganPlafon> plafon = hasil.Where(p => p.idPlafon == 1).ToList<msGolonganPlafon>();
                int makan = 0;
                if (plafon.Count > 0 && (query.idKeperluan != 2 || query.idKeperluan != 1))
                {
                    makan = isNull((int)plafon.First().harga);
                }
                txtMakan.Text = makan.ToString();
                //msGolonganPlafon plafon2 = hasil.Where(p => p.idPlafon == 2).FirstOrDefault();
                plafon = hasil.Where(p => p.idPlafon == 2).ToList<msGolonganPlafon>();
                int UangSaku = 0;
                if (plafon.Count > 0)
                {
                    UangSaku = isNull((int)plafon.First().harga);
                }

                txtUangSaku.Text = UangSaku.ToString();

                plafon = hasil.Where(p => p.idPlafon == 7).ToList<msGolonganPlafon>();
                int laundri = 0;
                if (plafon.Count > 0)
                {
                    laundri = isNull((int)plafon.First().harga);
                    txtJmlHariLaundri.Text = (laundri * (Jumlahhari.Days - 2)).ToString();
                    if (plafon.First().deskripsi == "Aktual")
                    {
                        txtJmlHariLaundri.Text = "Aktual";
                    }
                }
                if (int.Parse(query.jamKembali) > 12)
                {
                    txtSTMakan.Text = (makan * Jumlahhari.Days * int.Parse(kurs.Text)).ToString();
                }
                else
                    txtSTMakan.Text = (makan * (Jumlahhari.Days - 1) * int.Parse(kurs.Text)).ToString();
                txtSTUangSk.Text = (UangSaku * Jumlahhari.Days * int.Parse(kurs.Text)).ToString();


                if (Jumlahhari.Days < 3)
                {
                    txtLaundry.Text = "0";
                    txtLaundry.Enabled = false;
                }


                if (claimQ != null)
                {
                    setIntoForm(claimQ);
                    if (claimQ.kurs > 0)
                    {
                        kurs.Text = claimQ.kurs.ToString();
                    }
                    else
                    {
                        kurs.Text = (int.Parse(txtSTUangSk.Text) / (Jumlahhari.Days * UangSaku)).ToString();
                    }

                    if (claimQ.status.Split('-')[0] == "10")
                    {
                        btnSave.Enabled = true;
                        btnSubmit.Enabled = true;
                    }
                }

                int total = int.Parse(txtSTMakan.Text) + int.Parse(txtSTUangSk.Text) + int.Parse(txtTiket.Text) + int.Parse(txtHotel.Text) + int.Parse(txtBBM.Text) + int.Parse(txtTol.Text);
                total += int.Parse(txtTaxi.Text) + int.Parse(txtAirPortTax.Text) + int.Parse(txtLaundry.Text) + int.Parse(txtParkir.Text) + int.Parse(txtLainlain.Text) + int.Parse(txtKomunikasi.Text);
                txtTotal.Text = total.ToString();
                //if (spd.penginapan == "Disediakan")
                //{
                //    txtHotel.Enabled = false;
                //}
                //else txtHotel.Enabled = true;
                if (spd.tiket == "Dicarikan")
                {
                    txtTiket.Enabled = false;
                }
                int penyelesaian = total - int.Parse(txtUangMuka.Text);
                txtPenyelesaian.Text = penyelesaian.ToString();

                FLDTujuan.Value = query.Tujuan;

                if (query.Tujuan == "Luar Negeri")
                {
                    LKurs.Visible = true;
                    kurs.Visible = true;
                    ldlr.Visible = true;
                    txtAirPortTaxDLR.Visible = true;
                    txtAirPortTax.Enabled = false;
                    txtBBMDLR.Visible = true;
                    txtBBM.Enabled = false;
                    txtHotelDLR.Visible = true;
                    txtHotel.Enabled = false;
                    txtKomunikasiDLR.Visible = true;
                    txtKomunikasi.Enabled = false;
                    txtLainlainDLR.Visible = true;
                    txtLainlain.Enabled = false;
                    txtLaundryDLR.Visible = true;
                    txtLaundry.Enabled = false;
                    txtParkirDLR.Visible = true;
                    txtParkir.Enabled = false;
                    txtPenyelesaianDLR.Visible = true;
                    txtPenyelesaian.Enabled = false;
                    txtSTMakanDLR.Visible = true;
                    txtSTMakan.Enabled = false;
                    txtSTUangSkDLR.Visible = true;
                    txtSTUangSk.Enabled = false;
                    txtTaxiDLR.Visible = true;
                    txtTaxi.Enabled = false;
                    txtTiketDLR.Visible = true;
                    txtTiket.Enabled = false;
                    txtTolDLR.Visible = true;
                    txtTol.Enabled = false;
                    txtTotalDLR.Visible = true;
                    txtUangMukaDLR.Visible = true;
                }
                else
                {
                    LKurs.Visible = false;
                    kurs.Visible = false;
                    ldlr.Visible = false;
                    txtAirPortTaxDLR.Visible = false;
                    txtBBMDLR.Visible = false;
                    txtHotelDLR.Visible = false;
                    txtKomunikasiDLR.Visible = false;
                    txtLainlainDLR.Visible = false;
                    txtLaundryDLR.Visible = false;
                    txtParkirDLR.Visible = false;
                    txtPenyelesaianDLR.Visible = false;
                    txtSTMakanDLR.Visible = false;
                    txtSTUangSkDLR.Visible = false;
                    txtTaxiDLR.Visible = false;
                    txtTiketDLR.Visible = false;
                    txtTolDLR.Visible = false;
                    txtTotalDLR.Visible = false;
                    txtUangMukaDLR.Visible = false;
                }

                GetDolar();

            }
            catch (Exception ex)
            {
                //clearForm();
                Response.Write(ex.Message);
            }

        }

        string getKeperluan(int noKep)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            var kep = (from k in data.msKeperluans
                       where k.id == noKep
                       select k).FirstOrDefault();

            return kep.keperluan.ToString();
        }

        private int isNull(int p)
        {
            if (p == null)
            {
                return 0;
            }
            else
            {
                return p;
            }

        }

        protected void txtSTMakan_TextChanged(object sender, EventArgs e)
        {
            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();

            GetDolar();
        }

        protected void txtSTUangSk_TextChanged(object sender, EventArgs e)
        {
            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();

            GetDolar();
        }

        protected void txtTiket_TextChanged(object sender, EventArgs e)
        {

            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();

            GetDolar();
        }

        private int getPenyelesaian(int total)
        {
            int penyelesaian = total - int.Parse(txtUangMuka.Text);
            return penyelesaian;
        }

        private int getTotal()
        {
            count_kurs();
            int total = int.Parse(txtSTMakan.Text) + int.Parse(txtSTUangSk.Text) + int.Parse(txtTiket.Text) + int.Parse(txtHotel.Text) + int.Parse(txtBBM.Text) + int.Parse(txtTol.Text);
            total += int.Parse(txtTaxi.Text) + int.Parse(txtAirPortTax.Text) + int.Parse(txtLaundry.Text) + int.Parse(txtParkir.Text) + int.Parse(txtLainlain.Text) + int.Parse(txtKomunikasi.Text);
            return total;
        }

        void count_kurs()
        {
            int nkurs = int.Parse(kurs.Text);
            int hari = int.Parse(lblJumlahhari.Text);
            int makan = int.Parse(txtSTMakanDLR.Text);
            int saku = int.Parse(txtSTUangSkDLR.Text);
            int back = int.Parse(ddlJamKembali.SelectedValue);


            //if (back > 12) 
            //    txtSTMakan.Text =(nkurs * hari * makan).ToString();
            //else
            //    txtSTMakan.Text = (nkurs * (hari-1) * makan).ToString();
            //txtSTUangSk.Text = (nkurs * hari * saku).ToString();


            if (FLDTujuan.Value.Trim().ToLower() == "luar negeri")
            {
                int tiket = int.Parse(txtTiketDLR.Text);
                int hotel = int.Parse(txtHotelDLR.Text);
                int bbm = int.Parse(txtBBMDLR.Text);
                int tol = int.Parse(txtTolDLR.Text);
                int taxi = int.Parse(txtTaxiDLR.Text);
                int tax = int.Parse(txtAirPortTaxDLR.Text);
                int laundry = int.Parse(txtLaundryDLR.Text);
                int parkir = int.Parse(txtParkirDLR.Text);
                int lain = int.Parse(txtLainlainDLR.Text);
                int kom = int.Parse(txtKomunikasiDLR.Text);
                //int total = int.Parse(txtTotalDLR.Text);
                int muka = int.Parse(txtUangMukaDLR.Text);
                //int selesai = int.Parse(txtPenyelesaianDLR.Text);

                txtSTMakan.Text = (makan * nkurs).ToString();
                txtSTUangSk.Text = (saku * nkurs).ToString();
                txtTiket.Text = (tiket * nkurs).ToString();
                txtHotel.Text = (hotel * nkurs).ToString();
                txtBBM.Text = (bbm * nkurs).ToString();
                txtTol.Text = (tol * nkurs).ToString();
                txtTaxi.Text = (taxi * nkurs).ToString();
                txtAirPortTax.Text = (tax * nkurs).ToString();
                txtLaundry.Text = (laundry * nkurs).ToString();
                txtParkir.Text = (parkir * nkurs).ToString();
                txtLainlain.Text = (lain * nkurs).ToString();
                txtKomunikasi.Text = (kom * nkurs).ToString();
                //txtTotalDLR.Text = (total / nkurs).ToString();
                //txtUangMuka.Text = (muka * nkurs).ToString();
                //txtPenyelesaianDLR.Text = (selesai / nkurs).ToString();



            }



        }

        void GetDolar()
        {
            int nkurs = int.Parse(kurs.Text);
            int makan = int.Parse(txtSTMakan.Text);
            int saku = int.Parse(txtSTUangSk.Text);
            int tiket = int.Parse(txtTiket.Text);
            int hotel = int.Parse(txtHotel.Text);
            int bbm = int.Parse(txtBBM.Text);
            int tol = int.Parse(txtTol.Text);
            int taxi = int.Parse(txtTaxi.Text);
            int tax = int.Parse(txtAirPortTax.Text);
            int laundry = int.Parse(txtLaundry.Text);
            int parkir = int.Parse(txtParkir.Text);
            int lain = int.Parse(txtLainlain.Text);
            int kom = int.Parse(txtKomunikasi.Text);
            int total = int.Parse(txtTotal.Text);
            int muka = int.Parse(txtUangMuka.Text);
            int selesai = int.Parse(txtPenyelesaian.Text);

            if (nkurs > 0)
            {
                txtSTMakanDLR.Text = (makan / nkurs).ToString();
                txtSTUangSkDLR.Text = (saku / nkurs).ToString();
                txtTiketDLR.Text = (tiket / nkurs).ToString();
                txtHotelDLR.Text = (hotel / nkurs).ToString();
                txtBBMDLR.Text = (bbm / nkurs).ToString();
                txtTolDLR.Text = (tol / nkurs).ToString();
                txtTaxiDLR.Text = (taxi / nkurs).ToString();
                txtAirPortTaxDLR.Text = (tax / nkurs).ToString();
                txtLaundryDLR.Text = (laundry / nkurs).ToString();
                txtParkirDLR.Text = (parkir / nkurs).ToString();
                txtLainlainDLR.Text = (lain / nkurs).ToString();
                txtKomunikasiDLR.Text = (kom / nkurs).ToString();
                txtTotalDLR.Text = (total / nkurs).ToString();
                txtUangMukaDLR.Text = (muka / nkurs).ToString();
                txtPenyelesaianDLR.Text = (selesai / nkurs).ToString();

            }
        }

        protected void txtHotel_TextChanged(object sender, EventArgs e)
        {

            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void txtBBM_TextChanged(object sender, EventArgs e)
        {
            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void txtTol_TextChanged(object sender, EventArgs e)
        {
            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void txtTaxi_TextChanged(object sender, EventArgs e)
        {
            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void txtAirPortTax_TextChanged(object sender, EventArgs e)
        {

            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void txtLaundry_TextChanged(object sender, EventArgs e)
        {
            if (txtJmlHariLaundri.Text != "Aktual")
            {
                if (int.Parse(txtLaundry.Text) > int.Parse(txtJmlHariLaundri.Text))
                {
                    txtLaundry.Text = txtJmlHariLaundri.Text;
                }
            }

            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void txtParkir_TextChanged(object sender, EventArgs e)
        {
            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void txtSTLainlain_TextChanged(object sender, EventArgs e)
        {

            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            #region submit old
            if (Session["noSPDc"] == null)
            {
                Response.Redirect("frmClaimApproval.aspx");
            }
            else
            {

                if (txtNoSPD.Text != string.Empty)
                {
                    dsSPDDataContext data = new dsSPDDataContext();
                    trClaim claim = data.trClaims.FirstOrDefault(o => o.noSPD == txtNoSPD.Text);
                    trSPD spd = data.trSPDs.FirstOrDefault(o => o.noSPD == claim.noSPD);

                    if (spd.isApproved == true && spd.isCancel == null)
                    {
                        getFromForm(ref claim);

                        try
                        {
                            btnSubmit.Enabled = false;
                            btnSave.Enabled = false;
                            claim.nrpAtasan = spd.nrpAtasan;
                            claim.status = "Menunggu approval atasan";
                            claim.isSubmit = true;
                            claim.isSubmitDate = DateTime.Now;
                            claim.diubahOleh = karyawan.nrp;
                            claim.diubahTanggal = DateTime.Now;

                            msKaryawan kary = new msKaryawan();
                            if (spd.nrp == "99999999")
                            {
                                kary.EMail = spd.email;
                                kary.nrp = spd.nrp;
                                kary.namaLengkap = spd.namaLengkap;
                                kary.golongan = "III";
                                kary.Job = spd.jabatan;
                                kary.posisi = spd.jabatan;
                                kary.coCd = "1";
                                kary.kodePSubArea = "1";
                                kary.kodePA = "1";

                            }
                            else
                            {
                                kary = (from kar in data.msKaryawans
                                        where kar.nrp == spd.nrp
                                        select kar).First();
                            }
                            data.SubmitChanges();


                            EmailCore.ApprovalClaim(claim.nrpAtasan, spd.noSPD, spd, "Atasan", claim.status);

                            lblKet.Text = "Claim berhasil di Submit";
                            data.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);

                        }
                    }

                }
            }
            #endregion
        }

        protected void txtKomunikasi_TextChanged(object sender, EventArgs e)
        {

            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        protected void kurs_TextChanged(object sender, EventArgs e)
        {
            int total = getTotal();
            txtTotal.Text = total.ToString();

            int penyelesaian = getPenyelesaian(total);
            txtPenyelesaian.Text = penyelesaian.ToString();
            GetDolar();
        }

        private void resetCHK(bool stat)
        {
            chkTiket.Visible = stat;
            chkPenginapan.Visible = stat;
            chkLaundry.Visible = stat;
            chkKomunikasi.Visible = stat;
            chkAirportTax.Visible = stat;
            chkBBM.Visible = stat;
            chkTol.Visible = stat;
            chkTaxi.Visible = stat;
            chkParkir.Visible = stat;

        }

        protected void BtnDownload(object sender, EventArgs e)
        {

        }

        #region upload file
        protected void tiketUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(tiketUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument cannot be less than the second");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void penginapanUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(penginapanUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void laundryUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(laundryUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void komunikasiUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(komunikasiUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void airporttaxUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(airporttaxUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void bbmUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(bbmUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void tolUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(tolUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void taxiUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(taxiUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void parkirUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                contentUpload uploaded = uploaderFile(parkirUpload);
                if (!string.IsNullOrEmpty(uploaded.fileName) && uploaded.errorMessage == null)
                {
                }
                else
                {
                    throw new ArgumentException("the first argument fail");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public contentUpload uploaderFile(AsyncFileUpload fileUpload)
        {
            List<string> errorMessage = new List<string>();
            List<string> fileExt = new List<string>();

            contentUpload cu = new contentUpload();

            fileExt.Add(".jpg");
            fileExt.Add(".png");

            if (fileUpload.PostedFile.ContentLength >= 100000)
                errorMessage.Add("file terlalu besar, maximal 100 kb");

            if (!fileExt.Contains(System.IO.Path.GetExtension(fileUpload.FileName)))
                errorMessage.Add("hanya file jpg atau png yang diperbolehkan");

            if (errorMessage.Count == 0)
            {
                fileUpload.SaveAs(Server.MapPath("Files/") +
                    Guid.NewGuid().ToString() +
                    System.IO.Path.GetExtension(fileUpload.FileName));

                cu.fileName = Server.MapPath("Files/") +
                    Guid.NewGuid().ToString() +
                    System.IO.Path.GetExtension(fileUpload.FileName);
            }

            return cu;
        }

        public class contentUpload
        {
            public List<string> errorMessage { get; set; }
            public string fileName { get; set; }
        }

        #endregion
    }
}