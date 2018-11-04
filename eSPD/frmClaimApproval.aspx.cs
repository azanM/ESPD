using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;
using System.Data;
using eSPD.Extensions;

namespace eSPD
{
    public partial class frmClaimApproval : System.Web.UI.Page
    {
        private string strStatusSPD = string.Empty;
        msKaryawan karyawan = null;
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

            Session["noSPD"] = string.Empty;
            Session["editable"] = false;
            Session["Role"] = string.Empty;
            classSpd oSPD = new classSpd();
            string strLoginID = string.Empty;
            if (Session["IDLogin"] != null)
            {
                strLoginID = (string)Session["IDLogin"];
            }
            else
            {
                strLoginID = SetLabelWelcome();
            }
            //!!!!!debug onleee
            if (strLoginID.ToLower() == "asus")
            {
                //strLoginID = "Syam005812";
               // strLoginID = "Putu005001";
               //strLoginID = "yulia009582";
               // strLoginID = "fanie009601";
                //strLoginID = "spd";
            }

            //if (strLoginID.ToLower() == "yudi90000146")
            //{
            //    // strLoginID = "Putu005001";
            //    strLoginID = "yulia009582";
            //    // strLoginID = "fanie009601";
            //    //strLoginID = "spd";
            //}
            if (strLoginID.ToLower() == "is07")
            {
                // strLoginID = "Putu005001";
                // strLoginID = "yulia009582";
            }

            // strLoginID = "yulia009582";
                karyawan = oSPD.getKaryawan(strLoginID);
            lblUserName.Text = strLoginID;
            lblNRP.Text = karyawan.nrp;


            if (!IsPostBack)
            {
                try
                {
                    btnFind_Click(null, null);
                    btnFindGA_Click(null, null);
                    btnFindFinance_Click(null, null);
                    btnFindKasir_Click(null, null);
                    pnlSPDPerorangan.Visible = true;
                    pnlClaimPerorangan.Visible = true;
                    dsSPDDataContext data = new dsSPDDataContext();
                    msUser sekretaris = (from u in data.msUsers
                                         where u.nrp == karyawan.nrp && u.roleId == Konstan.SEKRETARIS
                                         select u).FirstOrDefault();
                    if (sekretaris != null)
                    {
                        Session["sekretaris"] = true;
                        FindPibadiSekretaris();
                        listfindSekretaris();

                    }
                    else
                    {
                        Session["sekretaris"] = false;
                        btnFindPribadi_Click(null, null);
                        btnListFind_Click(null, null);
                    }

                    DropDownList2_SelectedIndexChanged(null, null);

                }
                catch (Exception q)
                {
                    Response.Write(q.Message);
                }
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
        protected void btnFind_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();

            if (txtTglBerangkat.Text == "" || txtTglBerangkat.Text == null)
            {
                var query = from claim in data.trClaims //.AsEnumerable()
                            join spd in data.trSPDs //.AsEnumerable()
                                on claim.noSPD equals spd.noSPD
                            where claim.nrpAtasan.Trim() == karyawan.nrp
                                //&& claim.status.Split('-')[0] == "11"
                                && spd.isApproved == true
                                && claim.isSubmit == true
                                && claim.isApprovedAtasan == null
                                && claim.isApprovedGA == null
                                && claim.isApprovedFinance == null
                                && claim.isCancel == null
                            select new
                            {
                                noSPD = claim.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                tglKembali = spd.tglKembali.ToShortDateString(),
                                uangMakan = claim.biayaMakan,
                                uangSaku = claim.uangSaku,
                                Tiket = claim.tiket,
                                Hotel = claim.hotel,
                                BBM = claim.BBM,
                                Tol = claim.tol,
                                Taxi = claim.taxi,
                                AirportTax = claim.airportTax,
                                laundry = claim.laundry,
                                Parkir = claim.parkir,
                                Lain = claim.biayaLainLain,
                                Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                status = claim.status //.Split('-')[1]
                            };
                gvViewClaim.DataSource = query.ToList();
                gvViewClaim.DataBind();

                if (!query.Any())
                    pnlAtasan.Visible = false;
                else
                    pnlAtasan.Visible = true;

                data.Dispose();
            }

            else
            {
                DateTime str = Convert.ToDateTime(txtTglBerangkat.Text);
                var query = from claim in data.trClaims //.AsEnumerable()
                            join spd in data.trSPDs //.AsEnumerable()
                                on claim.noSPD equals spd.noSPD
                            where claim.nrpAtasan.Trim() == karyawan.nrp
                                //&& claim.status.Split('-')[0] == "11"
                                && spd.isApproved == true
                                && claim.isSubmit == true
                                && claim.isApprovedAtasan == null
                                && claim.isApprovedGA == null
                                && claim.isApprovedFinance == null
                                && claim.isCancel == null
                                && spd.tglBerangkat == str
                            select new
                            {
                                noSPD = claim.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                tglKembali = spd.tglKembali.ToShortDateString(),
                                uangMakan = claim.biayaMakan,
                                uangSaku = claim.uangSaku,
                                Tiket = claim.tiket,
                                Hotel = claim.hotel,
                                BBM = claim.BBM,
                                Tol = claim.tol,
                                Taxi = claim.taxi,
                                AirportTax = claim.airportTax,
                                laundry = claim.laundry,
                                Parkir = claim.parkir,
                                Lain = claim.biayaLainLain,
                                Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                status = claim.status //.Split('-')[1]
                            };
                gvViewClaim.DataSource = query.ToList();
                gvViewClaim.DataBind();

                if (!query.Any())
                    pnlAtasan.Visible = false;
                else
                    pnlAtasan.Visible = true;

                data.Dispose();
            }



            #region nitip
            //nitip taro biar kalo sewaktu waktu dibutuhkan tinggal ambil

            //<asp:BoundField HeaderText="Uang Makan" DataField="uangMakan"/>
            //<asp:BoundField HeaderText="Uang Saku" DataField="uangSaku"/>
            //<asp:BoundField HeaderText="Tiket" DataField="Tiket"/>
            //<asp:BoundField HeaderText="Hotel" DataField="Hotel"/>
            //<asp:BoundField HeaderText="BBM" DataField="BBM"/>
            //<asp:BoundField HeaderText="Tol" DataField="Tol"/>
            //<asp:BoundField HeaderText="Taxi" DataField="Taxi"/>
            //<asp:BoundField HeaderText="Airport Tax" DataField="AirportTax"/>
            //<asp:BoundField HeaderText="Laundry" DataField="Laundry"/>
            //<asp:BoundField HeaderText="Parkir" DataField="Parkir"/>
            //<asp:BoundField HeaderText="Lain-Lain" DataField="Lain"/>
            #endregion

        }

        protected void lbDetail_Click(object sender, EventArgs e)
        {
            Session["editable"] = false;
            DetailClick(sender);
        }

        protected void lbSetuju_Click(object sender, EventArgs e)
        {
            #region changed
            //string status = "16-Claim Approve (Atasan)";
            //akanUbahStatus(sender, status);
            //btnFind_Click(null, null);
            //historyApproval(sender, status);
            #endregion

            //cr : 2015-01-08 ian
            try
            {
                var lb = (Control)sender;
                GridViewRow row = (GridViewRow)lb.NamingContainer;

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

                string noSPD = row.Cells[0].Text;
                string nrpApproval = karyawan.nrp;
                string emailApproval = karyawan.EMail;
                string action = "approve";
                string claimApprove = "atasan";

                ClaimApprovalUrl claimApprovalUrl = new ClaimApprovalUrl();
                lblStat.Text = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //bool approvalMethod = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //if (approvalMethod)
                //{
                //    lblStat.Text = noSPD + " berhasil di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
                //else
                //{
                //    lblStat.Text = noSPD + " gagal di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            btnFind_Click(null, null);
        }
        private void historyApproval(object sender, string status)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmClaimApproval.aspx");
            }
            else
            {
                string strLoginID = (string)Session["IDLogin"];
                classSpd oSPD = new classSpd();
                msKaryawan karyawan = new msKaryawan();
                karyawan = oSPD.getKaryawan(strLoginID);

                LinkButton link = (LinkButton)sender;
                GridViewRow gv = (GridViewRow)(link.NamingContainer);
                dsSPDDataContext data = new dsSPDDataContext();
                string strNoSpd = gv.Cells[0].Text;
                string strNRP = oSPD.getKaryawan(Session["IDLogin"].ToString()).nrp;
                string strStatus = status;

                //var role = (from msUser in data.msUsers.AsEnumerable()
                //            where msUser.nrp == karyawan.nrp
                //            select msUser).First();
                //var role = (from msRole in data.msRoles.AsEnumerable()
                //            where msRole.namaRole == karyawan.posisi
                //            select msRole).First();

                //string strRole = Convert.ToString(role.id);
                string strRole = "";
                var role = (from msRole in data.msRoles.AsEnumerable()
                            where msRole.namaRole == karyawan.posisi
                            select msRole).FirstOrDefault();

                if (role != null) { strRole = Convert.ToString(role.id); }
                else
                {
                    var user = (from msUser in data.msUsers.AsEnumerable()
                                where msUser.nrp == karyawan.nrp
                                select msUser).FirstOrDefault();
                    if (user != null) { strRole = Convert.ToString(user.roleId); }
                    else { strRole = "1"; }
                }
                DateTime dateApproval = DateTime.Now;

                trApprovalHistory AppHistory = new trApprovalHistory();
                AppHistory.noSPD = strNoSpd;
                AppHistory.nrpApprover = strNRP;
                AppHistory.statusApproval = strStatus;
                AppHistory.idRole = strRole;
                AppHistory.approvalDatetime = dateApproval;
                data.trApprovalHistories.InsertOnSubmit(AppHistory);
                data.SubmitChanges();
                data.Dispose();
            }
        }
        protected void lbTolak_Click(object sender, EventArgs e)
        {
            #region changed
            //strStatusSPD = "12-SPD Tolak (Atasan)";
            //akanUbahStatus(sender, "12-Claim Tolak (Atasan)");
            //btnFind_Click(null, null);
            //historyApproval(sender, strStatusSPD);
            #endregion

            //cr : 2015-01-09 ian
            try
            {
                var lb = (Control)sender;
                GridViewRow row = (GridViewRow)lb.NamingContainer;

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

                string noSPD = row.Cells[0].Text;
                string nrpApproval = karyawan.nrp;
                string emailApproval = karyawan.EMail;
                string action = "reject";
                string claimApprove = "atasan";

                ClaimApprovalUrl claimApprovalUrl = new ClaimApprovalUrl();
                lblStat.Text = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //bool approvalMethod = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //if (approvalMethod)
                //{
                //    lblStat.Text = noSPD + " berhasil di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
                //else
                //{
                //    lblStat.Text = noSPD + " gagal di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            btnFind_Click(null, null);
        }
        private void akanUbahStatus(object sender, string status)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmClaimApproval.aspx");
            }
            else
            {
                LinkButton link = (LinkButton)sender;
                GridViewRow gv = (GridViewRow)(link.NamingContainer);
                string strNoSpd = gv.Cells[0].Text;
                try
                {
                    ubahStatus(strNoSpd, status);
                    lblNoSPD.Text = strNoSpd;
                    lblStatus.Text = status.Trim();
                    int intStatus = int.Parse(status.Split('-')[0]);
                    if (intStatus == 8 || intStatus == 9 || intStatus == 12 || intStatus == 13)
                    {
                        pnlClaimPerorangan.Visible = true;
                        //  bPanel1 = Panel1.Visible;
                        //  bPanel2 = Panel2.Visible;
                        //   bPanel5 = Panel5.Visible;
                        if (intStatus == 8 || intStatus == 9)
                        {
                            lblRevisi.Visible = true;
                            lblTolak.Visible = false;
                        }
                        else
                        {
                            lblRevisi.Visible = false;
                            lblTolak.Visible = true;

                        }
                        pnlAtasan.Visible = false;
                        Panel5.Visible = false;
                        txtRevisi.Focus();

                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }
        private void ubahStatus(string strNoSpd, string p)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            trClaim claimQ = (from claim in data.trClaims
                              where claim.noSPD == strNoSpd
                              select claim).FirstOrDefault();
            claimQ.status = p;
            trSPD spdQ = (from spd in data.trSPDs
                          where spd.noSPD == strNoSpd
                          select spd).FirstOrDefault();
            spdQ.status = p;
            data.SubmitChanges();
            trSPD oSpd = (from spd in data.trSPDs
                                                where spd.noSPD == strNoSpd
                                                select spd).First();
            msKaryawan kary = new msKaryawan();
            if (oSpd.nrp == "99999999")
            {
                kary.EMail = oSpd.email;
                kary.nrp = oSpd.nrp;
                kary.namaLengkap = oSpd.namaLengkap;
                kary.golongan = "III";
                kary.Job = oSpd.jabatan;
                kary.posisi = oSpd.jabatan;
                kary.coCd = "0001";
                kary.kodePSubArea = "0001";
                kary.kodePA = "0001";

            }
            else
            {
                kary = (from kar in data.msKaryawans
                        where kar.nrp == oSpd.nrp
                        select kar).First();
            }
            //update status spd mengikuti yang di claim
            oSpd.status = claimQ.status;
            classSpd oClassSPD = new classSpd();
            switch (oSpd.status.Split('-')[0])
            {
                case "11":
                    oClassSPD.sendMail(oSpd, "Atasan", kary);
                    break;
                case "13":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    break;
                case "14":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    break;
                case "15":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    break;
                case "16":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    oClassSPD.sendMail(oSpd, "GA", kary);
                    break;
                case "17":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    oClassSPD.sendMail(oSpd, "finance", kary);
                    break;
                case "18":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    break;
                case "19":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    break;
                case "20":
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    break;
                case "26":
                    // oClassSPD.sendMail(oSpd, "kasir", kary);
                    oClassSPD.sendMail(oSpd, "Pembuat", kary);
                    break;
            }
            data.Dispose();


        }
        protected void lbRevisi_Click(object sender, EventArgs e)
        {
            strStatusSPD = "18-Claim Perlu Revisi (Atasan)";
            akanUbahStatus(sender, "18-Claim Perlu Revisi (Atasan)");
            btnFind_Click(null, null);
            historyApproval(sender, strStatusSPD);

        }

        protected void lbRevisiFinance_Click(object sender, EventArgs e)
        {
            strStatusSPD = "25-Finance Revisi";
            akanUbahStatus(sender, "25-Finance Revisi");
            btnFindFinance_Click(null, null);
            historyApproval(sender, strStatusSPD);

        }

        protected void lbDetailGA_Click(object sender, EventArgs e)
        {
            Session["editable"] = true;
            Session["gamode"] = true;
            Session["Role"] = "GA";
            DetailClick(sender);

        }

        private void DetailClick(object sender)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            Session["noSPD"] = strNoSpd;
            Response.Redirect("frmClaimInput.aspx");
        }

        protected void lbSetujuGA_Click(object sender, EventArgs e)
        {
            //cr : 2015-01-09 ian
            try
            {
                var lb = (Control)sender;
                GridViewRow row = (GridViewRow)lb.NamingContainer;

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

                string noSPD = row.Cells[0].Text;
                string nrpApproval = karyawan.nrp;
                string emailApproval = karyawan.EMail;
                string action = "approve";
                string claimApprove = "ga";

                ClaimApprovalUrl claimApprovalUrl = new ClaimApprovalUrl();
                lblStat2.Text = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //bool approvalMethod = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //if (approvalMethod)
                //{
                //    lblStat2.Text = noSPD + " berhasil di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
                //else
                //{
                //    lblStat2.Text = noSPD + " gagal di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            #region changed
            //string status = "17-Claim Approve (GA)";
            //akanUbahStatus(sender, status);
            //historyApproval(sender, status);
            #endregion
            btnFindGA_Click(null, null);
        }

        protected void lbListGACancel_Click(object sender, EventArgs e)
        {
            //cr : 2015-01-09 ian
            try
            {
                var lb = (Control)sender;
                GridViewRow row = (GridViewRow)lb.NamingContainer;

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

                string noSPD = row.Cells[0].Text;
                string nrpApproval = karyawan.nrp;
                string emailApproval = karyawan.EMail;
                string action = "cancel";
                string claimApprove = "ga";

                ClaimApprovalUrl claimApprovalUrl = new ClaimApprovalUrl();
                lblStat.Text = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);

                //bool approvalMethod = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //if (approvalMethod)
                //{
                //    lblStat2.Text = noSPD + " berhasil di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
                //else
                //{
                //    lblStat2.Text = noSPD + " gagal di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            #region changed
            //string status = "29-SPD Cancel";
            //akanUbahStatus(sender, status);
            //historyApproval(sender, status);
            #endregion

            btnFindGA_Click(null, null);
        }

        protected void lbTolakGA_Click(object sender, EventArgs e)
        {
            //cr : 2015-01-09 ian
            try
            {
                var lb = (Control)sender;
                GridViewRow row = (GridViewRow)lb.NamingContainer;

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

                string noSPD = row.Cells[0].Text;
                string nrpApproval = karyawan.nrp;
                string emailApproval = karyawan.EMail;
                string action = "reject";
                string claimApprove = "ga";

                ClaimApprovalUrl claimApprovalUrl = new ClaimApprovalUrl();
                lblStat2.Text = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);

                //bool approvalMethod = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //if (approvalMethod)
                //{
                //    lblStat2.Text = noSPD + " berhasil di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
                //else
                //{
                //    lblStat2.Text = noSPD + " gagal di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            #region changed
            //strStatusSPD = "15-Claim Tolak (GA)";
            //akanUbahStatus(sender, "15-Claim Tolak (GA)");
            //historyApproval(sender, strStatusSPD);
            #endregion

            btnFind_Click(null, null);
        }

        protected void lbRevisiGA_Click(object sender, EventArgs e)
        {
            //strStatusSPD = "19-Claim Perlu Revisi (GA)";
            //akanUbahStatus(sender, "19-Claim Perlu Revisi (GA)");
            //btnFindGA_Click(null, null);
            Session["editable"] = true;
            Session["gamode"] = true;
            Session["Role"] = "GA";
            DetailClick(sender);
        }

        protected void lbDetailFinance_Click(object sender, EventArgs e)
        {
            Session["editable"] = false;
            DetailClick(sender);
        }

        protected void lbDetailPribadi_Click(object sender, EventArgs e)
        {

            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            dsSPDDataContext data = new dsSPDDataContext();
            trClaim claimQ= (from c in data.trClaims
                             where c.noSPD == strNoSpd
                       select c).SingleOrDefault();
            //if (claimQ != null && (claimQ.status.Split('-')[0] == "18" || claimQ.status.Split('-')[0] == "19" || claimQ.status.Split('-')[0] == "10"))
            if (claimQ != null && (claimQ.isSubmit == null || claimQ.isSubmit == false))
            {
                Session["editable"] = true;
            }
            else
                Session["editable"] = false;
            DetailClick(sender);
        }

        protected void lbSetujuFinance_Click(object sender, EventArgs e)
        {
            //cr : 2015-01-09 ian
            try
            {
                var lb = (Control)sender;
                GridViewRow row = (GridViewRow)lb.NamingContainer;

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

                string noSPD = row.Cells[0].Text;
                string nrpApproval = karyawan.nrp;
                string emailApproval = karyawan.EMail;
                string action = "approve";
                string claimApprove = "finance";

                ClaimApprovalUrl claimApprovalUrl = new ClaimApprovalUrl();
                lblStat3.Text = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);

                //bool approvalMethod = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //if (approvalMethod)
                //{
                //    lblStat3.Text = noSPD + " berhasil di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
                //else
                //{
                //    lblStat3.Text = noSPD + " gagal di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            #region changed
            //string status = "26-Finance Approve";
            //akanUbahStatus(sender, status);
            //historyApproval(sender, status);
            #endregion

            btnFindFinance_Click(null, null);
        }
        protected void lbTolakFinance_Click(object sender, EventArgs e)
        {
            //cr : 2015-01-09 ian
            try
            {
                var lb = (Control)sender;
                GridViewRow row = (GridViewRow)lb.NamingContainer;

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(Session["IDLogin"].ToString());

                string noSPD = row.Cells[0].Text;
                string nrpApproval = karyawan.nrp;
                string emailApproval = karyawan.EMail;
                string action = "reject";
                string claimApprove = "finance";

                ClaimApprovalUrl claimApprovalUrl = new ClaimApprovalUrl();
                lblStat3.Text = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);

                //bool approvalMethod = claimApprovalUrl.ChangeStatus(noSPD, action, nrpApproval, claimApprove);
                //if (approvalMethod)
                //{
                //    lblStat3.Text = noSPD + " berhasil di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
                //else
                //{
                //    lblStat3.Text = noSPD + " gagal di" + action + " oleh " + nrpApproval + " " + emailApproval;
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            #region changed
            //string status = "27-Finance Tolak";
            //akanUbahStatus(sender, status);
            //historyApproval(sender, status);
            #endregion

            btnFindFinance_Click(null, null);
        }

        protected void btnFindGA_Click(object sender, EventArgs e)
        {
            try
            {
                dsSPDDataContext data = new dsSPDDataContext();
                //cek apakah user GA atau tidak
                msKaryawan gaAsal = (from k in data.msKaryawans
                                     join u in data.msUsers on k.nrp equals u.nrp
                                     where u.roleId == 17 && u.nrp == lblNRP.Text
                                     select k
                                     ).SingleOrDefault();
                if (gaAsal != null)
                {
                    string filter = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.SelectedItem.Value;
                    string param = txtTglBerangkatGA.Text;
                    DataTable dataTable = SPHelper.GetApprovalClaimSPDOlehGA(filter, param);
                    gvViewClaimGA.DataSource = dataTable;
                    gvViewClaimGA.DataBind();
                    if (dataTable.Rows.Count > 0) pnlGA.Visible = true;
                    else pnlGA.Visible = false;

                    #region change
                    //if (txtTglBerangkatGA.Text == "" || txtTglBerangkatGA.Text == null)
                    //{
                    //    var query = (from claim in data.trClaims.AsEnumerable()
                    //                 join spd in data.trSPDs.AsEnumerable()
                    //                     on claim.noSPD equals spd.noSPD
                    //                 join k in data.msKaryawans
                    //                     on spd.nrp equals k.nrp
                    //                 where //(claim.status.Split('-')[0] == "16" || claim.status.Split('-')[0] == "25")
                    //                    claim.isApprovedAtasan == true
                    //                    && claim.isApprovedGA == null
                    //                 select new
                    //                 {
                    //                     noSPD = claim.noSPD,
                    //                     nrp = spd.nrp,
                    //                     namaLengkap = spd.namaLengkap,
                    //                     cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                    //                     tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                    //                     tglKembali = spd.tglKembali.ToShortDateString(),
                    //                     uangMakan = claim.biayaMakan,
                    //                     uangSaku = claim.uangSaku,
                    //                     Tiket = claim.tiket,
                    //                     Hotel = claim.hotel,
                    //                     BBM = claim.BBM,
                    //                     Tol = claim.tol,
                    //                     Taxi = claim.taxi,
                    //                     AirportTax = claim.airportTax,
                    //                     laundry = claim.laundry,
                    //                     Parkir = claim.parkir,
                    //                     Lain = claim.biayaLainLain,
                    //                     Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                    //                     status = claim.status.Split('-')[1]
                    //                 });
                    //    gvViewClaimGA.DataSource = query.ToList();
                    //    gvViewClaimGA.DataBind();
                    //    if (!query.Any())
                    //    {
                    //        pnlGA.Visible = false;
                    //    }
                    //    else
                    //        pnlGA.Visible = true;
                    //}
                    //else {
                    //    DateTime str = Convert.ToDateTime(txtTglBerangkatGA.Text);
                    //    var query = (from claim in data.trClaims.AsEnumerable()
                    //                 join spd in data.trSPDs.AsEnumerable()
                    //                     on claim.noSPD equals spd.noSPD
                    //                 join k in data.msKaryawans
                    //                     on spd.nrp equals k.nrp
                    //                 where //(claim.status.Split('-')[0] == "16" || claim.status.Split('-')[0] == "25")
                    //                    claim.isApprovedAtasan == true
                    //                    && claim.isApprovedGA == null
                    //                  && spd.tglBerangkat==str
                    //                 select new
                    //                 {
                    //                     noSPD = claim.noSPD,
                    //                     nrp = spd.nrp,
                    //                     namaLengkap = spd.namaLengkap,
                    //                     cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                    //                     tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                    //                     tglKembali = spd.tglKembali.ToShortDateString(),
                    //                     uangMakan = claim.biayaMakan,
                    //                     uangSaku = claim.uangSaku,
                    //                     Tiket = claim.tiket,
                    //                     Hotel = claim.hotel,
                    //                     BBM = claim.BBM,
                    //                     Tol = claim.tol,
                    //                     Taxi = claim.taxi,
                    //                     AirportTax = claim.airportTax,
                    //                     laundry = claim.laundry,
                    //                     Parkir = claim.parkir,
                    //                     Lain = claim.biayaLainLain,
                    //                     Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                    //                     status = claim.status.Split('-')[1]
                    //                 });
                    //    gvViewClaimGA.DataSource = query.ToList();
                    //    gvViewClaimGA.DataBind();
                    //    if (!query.Any())
                    //    {
                    //        pnlGA.Visible = false;
                    //    }
                    //    else
                    //        pnlGA.Visible = true;
                    //}
                    #endregion
                }
                else
                    pnlGA.Visible = false;

                data.Dispose();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);

            }
        }

        protected void btnFindFinance_Click(object sender, EventArgs e)
        {
            try
            {
                dsSPDDataContext data = new dsSPDDataContext();
                msKaryawan financeAsal = (from k in data.msKaryawans
                                          join u in data.msUsers on k.nrp equals u.nrp
                                          where u.roleId == 19 && u.nrp == lblNRP.Text
                                          select k
                                     ).FirstOrDefault();
                if (financeAsal != null)
                {
                    #region CR : ian
                    #region changed
                    //if (financeAsal.coCd == "0001" && financeAsal.kodePA == "0001" && financeAsal.kodePSubArea == "0001")
                    //{
                    //    #region financeHO
                    //    if (txtTglBerangkatFinance.Text == "" || txtTglBerangkatFinance == null)
                    //    {
                    //        var query = from claim in data.trClaims.AsEnumerable()
                    //                    join spd in data.trSPDs.AsEnumerable()
                    //                        on claim.noSPD equals spd.noSPD
                    //                    join k in data.msKaryawans
                    //                        on spd.nrp equals k.nrp
                    //                    where (claim.status.Split('-')[0] == "17" && ((k.coCd == financeAsal.coCd) && (k.kodePA == financeAsal.kodePA) && (k.kodePSubArea == financeAsal.kodePSubArea)))
                    //                    || (claim.status.Split('-')[0] == "17" && (k.coCd == financeAsal.coCd) && (k.kodePA == "1510" || k.kodePA == "1520" || k.kodePA == "1000"))
                    //                    select new
                    //                    {
                    //                        noSPD = claim.noSPD,
                    //                        nrp = spd.nrp,
                    //                        namaLengkap = spd.namaLengkap,
                    //                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                    //                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                    //                        tglKembali = spd.tglKembali.ToShortDateString(),
                    //                        uangMakan = claim.biayaMakan,
                    //                        uangSaku = claim.uangSaku,
                    //                        Tiket = claim.tiket,
                    //                        Hotel = claim.hotel,
                    //                        BBM = claim.BBM,
                    //                        Tol = claim.tol,
                    //                        Taxi = claim.taxi,
                    //                        AirportTax = claim.airportTax,
                    //                        laundry = claim.laundry,
                    //                        Parkir = claim.parkir,
                    //                        Lain = claim.biayaLainLain,
                    //                        Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.komunikasi + claim.biayaLainLain,
                    //                        status = claim.status.Split('-')[1]
                    //                    };
                    //        gvViewClaimFinance.DataSource = query.ToList();
                    //        gvViewClaimFinance.DataBind();
                    //        if (!query.Any())
                    //        {
                    //            pnlFinance.Visible = false;

                    //        }
                    //        else
                    //            pnlFinance.Visible = true;
                    //        data.Dispose();
                    //    }
                    //    else
                    //    {
                    //        DateTime str = Convert.ToDateTime(txtTglBerangkatFinance.Text);
                    //        var query = from claim in data.trClaims.AsEnumerable()
                    //                    join spd in data.trSPDs.AsEnumerable()
                    //                        on claim.noSPD equals spd.noSPD
                    //                    join k in data.msKaryawans
                    //                        on spd.nrp equals k.nrp
                    //                    where (claim.status.Split('-')[0] == "17" && ((k.coCd == financeAsal.coCd) && (k.kodePA == financeAsal.kodePA) && (k.kodePSubArea == financeAsal.kodePSubArea))
                    //                    || (claim.status.Split('-')[0] == "17" && (k.coCd == financeAsal.coCd) && (k.kodePA == "1510" || k.kodePA == "1520")))
                    //                    && spd.tglBerangkat == str
                    //                    select new
                    //                    {
                    //                        noSPD = claim.noSPD,
                    //                        nrp = spd.nrp,
                    //                        namaLengkap = spd.namaLengkap,
                    //                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                    //                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                    //                        tglKembali = spd.tglKembali.ToShortDateString(),
                    //                        uangMakan = claim.biayaMakan,
                    //                        uangSaku = claim.uangSaku,
                    //                        Tiket = claim.tiket,
                    //                        Hotel = claim.hotel,
                    //                        BBM = claim.BBM,
                    //                        Tol = claim.tol,
                    //                        Taxi = claim.taxi,
                    //                        AirportTax = claim.airportTax,
                    //                        laundry = claim.laundry,
                    //                        Parkir = claim.parkir,
                    //                        Lain = claim.biayaLainLain,
                    //                        Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.komunikasi + claim.biayaLainLain,
                    //                        status = claim.status.Split('-')[1]
                    //                    };
                    //        gvViewClaimFinance.DataSource = query.ToList();
                    //        gvViewClaimFinance.DataBind();
                    //        if (!query.Any())
                    //        {
                    //            pnlFinance.Visible = false;

                    //        }
                    //        else
                    //            pnlFinance.Visible = true;
                    //        data.Dispose();
                    //    }
                    //    #endregion financeHO
                    //}
                    //else
                    //{
                    //    #region financenonHO
                    //    if (txtTglBerangkatFinance.Text == "" || txtTglBerangkatFinance == null)
                    //    {
                    //        var query = from claim in data.trClaims.AsEnumerable()
                    //                    join spd in data.trSPDs.AsEnumerable()
                    //                        on claim.noSPD equals spd.noSPD
                    //                    join k in data.msKaryawans
                    //                        on spd.nrp equals k.nrp
                    //                    where claim.status.Split('-')[0] == "17"
                    //                    && ((k.coCd == financeAsal.coCd) && (k.kodePA == financeAsal.kodePA)
                    //                    && (k.kodePSubArea == financeAsal.kodePSubArea))
                    //                    select new
                    //                    {
                    //                        noSPD = claim.noSPD,
                    //                        nrp = spd.nrp,
                    //                        namaLengkap = spd.namaLengkap,
                    //                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                    //                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                    //                        tglKembali = spd.tglKembali.ToShortDateString(),
                    //                        uangMakan = claim.biayaMakan,
                    //                        uangSaku = claim.uangSaku,
                    //                        Tiket = claim.tiket,
                    //                        Hotel = claim.hotel,
                    //                        BBM = claim.BBM,
                    //                        Tol = claim.tol,
                    //                        Taxi = claim.taxi,
                    //                        AirportTax = claim.airportTax,
                    //                        laundry = claim.laundry,
                    //                        Parkir = claim.parkir,
                    //                        Lain = claim.biayaLainLain,
                    //                        Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.komunikasi + claim.biayaLainLain,
                    //                        status = claim.status.Split('-')[1]
                    //                    };
                    //        gvViewClaimFinance.DataSource = query.ToList();
                    //        gvViewClaimFinance.DataBind();
                    //        if (!query.Any())
                    //        {
                    //            pnlFinance.Visible = false;

                    //        }
                    //        else
                    //            pnlFinance.Visible = true;
                    //        data.Dispose();
                    //    }
                    //    else
                    //    {
                    //        DateTime str = Convert.ToDateTime(txtTglBerangkatFinance.Text);
                    //        var query = from claim in data.trClaims.AsEnumerable()
                    //                    join spd in data.trSPDs.AsEnumerable()
                    //                        on claim.noSPD equals spd.noSPD
                    //                    join k in data.msKaryawans
                    //                        on spd.nrp equals k.nrp
                    //                    where claim.status.Split('-')[0] == "17"
                    //                    && ((k.coCd == financeAsal.coCd) && (k.kodePA == financeAsal.kodePA)
                    //                    && (k.kodePSubArea == financeAsal.kodePSubArea))
                    //                    && spd.tglBerangkat == str
                    //                    select new
                    //                    {
                    //                        noSPD = claim.noSPD,
                    //                        nrp = spd.nrp,
                    //                        namaLengkap = spd.namaLengkap,
                    //                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                    //                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                    //                        tglKembali = spd.tglKembali.ToShortDateString(),
                    //                        uangMakan = claim.biayaMakan,
                    //                        uangSaku = claim.uangSaku,
                    //                        Tiket = claim.tiket,
                    //                        Hotel = claim.hotel,
                    //                        BBM = claim.BBM,
                    //                        Tol = claim.tol,
                    //                        Taxi = claim.taxi,
                    //                        AirportTax = claim.airportTax,
                    //                        laundry = claim.laundry,
                    //                        Parkir = claim.parkir,
                    //                        Lain = claim.biayaLainLain,
                    //                        Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.komunikasi + claim.biayaLainLain,
                    //                        status = claim.status.Split('-')[1]
                    //                    };
                    //        gvViewClaimFinance.DataSource = query.ToList();
                    //        gvViewClaimFinance.DataBind();
                    //        if (!query.Any())
                    //        {
                    //            pnlFinance.Visible = false;

                    //        }
                    //        else
                    //            pnlFinance.Visible = true;
                    //        data.Dispose();
                    //    }
                    //    #endregion financenonHO
                    //}
                    #endregion changed
                    if (txtTglBerangkatFinance.Text == "" || txtTglBerangkatFinance == null)
                    {
                        var query = from claim in data.trClaims //.AsEnumerable()
                                    join spd in data.trSPDs //.AsEnumerable()
                                        on claim.noSPD equals spd.noSPD
                                    join k in data.msKaryawans
                                        on spd.nrp equals k.nrp
                                    where k.coCd == financeAsal.coCd
                                        && claim.isApprovedGA == true
                                        && claim.isApprovedFinance == null
                                        && claim.isCancel == null
                                    select new
                                    {
                                        noSPD = claim.noSPD,
                                        nrp = spd.nrp,
                                        namaLengkap = spd.namaLengkap,
                                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                        tglKembali = spd.tglKembali.ToShortDateString(),
                                        uangMakan = claim.biayaMakan,
                                        uangSaku = claim.uangSaku,
                                        Tiket = claim.tiket,
                                        Hotel = claim.hotel,
                                        BBM = claim.BBM,
                                        Tol = claim.tol,
                                        Taxi = claim.taxi,
                                        AirportTax = claim.airportTax,
                                        laundry = claim.laundry,
                                        Parkir = claim.parkir,
                                        Lain = claim.biayaLainLain,
                                        Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.komunikasi + claim.biayaLainLain,
                                        status = claim.status //.Split('-')[1]
                                    };
                        gvViewClaimFinance.DataSource = query.ToList();
                        gvViewClaimFinance.DataBind();

                        if (!query.Any())
                            pnlFinance.Visible = false;
                        else
                            pnlFinance.Visible = true;

                        data.Dispose();
                    }
                    else
                    {
                        DateTime str = Convert.ToDateTime(txtTglBerangkatFinance.Text);
                        var query = from claim in data.trClaims //.AsEnumerable()
                                    join spd in data.trSPDs //.AsEnumerable()
                                        on claim.noSPD equals spd.noSPD
                                    join k in data.msKaryawans
                                        on spd.nrp equals k.nrp
                                    where k.coCd == financeAsal.coCd
                                        && claim.isApprovedGA == true
                                        && claim.isApprovedFinance == null
                                        && claim.isCancel == null
                                        && spd.tglBerangkat == str
                                    select new
                                    {
                                        noSPD = claim.noSPD,
                                        nrp = spd.nrp,
                                        namaLengkap = spd.namaLengkap,
                                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                        tglKembali = spd.tglKembali.ToShortDateString(),
                                        uangMakan = claim.biayaMakan,
                                        uangSaku = claim.uangSaku,
                                        Tiket = claim.tiket,
                                        Hotel = claim.hotel,
                                        BBM = claim.BBM,
                                        Tol = claim.tol,
                                        Taxi = claim.taxi,
                                        AirportTax = claim.airportTax,
                                        laundry = claim.laundry,
                                        Parkir = claim.parkir,
                                        Lain = claim.biayaLainLain,
                                        Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.komunikasi + claim.biayaLainLain,
                                        status = claim.status //.Split('-')[1]
                                    };
                        gvViewClaimFinance.DataSource = query.ToList();
                        gvViewClaimFinance.DataBind();

                        if (!query.Any())
                            pnlFinance.Visible = false;
                        else
                            pnlFinance.Visible = true;

                        data.Dispose();
                    }
                    #endregion CR : ian
                }
                else
                    pnlFinance.Visible = false;
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
        }

        protected void btnFindPribadi_Click(object sender, EventArgs e)
        {
            if (Session["sekretaris"] != null && (bool)Session["sekretaris"])
            {
                FindPibadiSekretaris();
            }
            else
            {
                try
                {
                    dsSPDDataContext data = new dsSPDDataContext();
                    if (txtTglBerangkatPribadi.Text == "" || txtTglBerangkatPribadi == null)
                    {
                        // gvViewClaimPribadi2.DataSourceID = string.Empty;
                        //  gvViewClaimPribadi2.DataSourceID = null;

                        //  gvViewClaimPribadi2.DataSource = string.Empty;
                        // gvViewClaimPribadi2.DataSource = null;

                        var query = (from claim in data.trClaims //.AsEnumerable()
                                     join spd in data.trSPDs //.AsEnumerable()
                                         on claim.noSPD equals spd.noSPD
                                     where spd.nrp.Trim() == karyawan.nrp
                                     select new
                                     {
                                         noSPD = claim.noSPD,
                                         nrp = spd.nrp,
                                         namaLengkap = spd.namaLengkap,
                                         cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                         tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                         tglKembali = spd.tglKembali.ToShortDateString(),
                                         uangMakan = claim.biayaMakan,
                                         uangSaku = claim.uangSaku,
                                         Tiket = claim.tiket,
                                         Hotel = claim.hotel,
                                         BBM = claim.BBM,
                                         Tol = claim.tol,
                                         Taxi = claim.taxi,
                                         AirportTax = claim.airportTax,
                                         laundry = claim.laundry,
                                         Parkir = claim.parkir,
                                         Lain = claim.biayaLainLain,
                                         Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                         status = claim.status //.Split('-')[1]
                                     }).OrderByDescending(spd => spd.noSPD);

                        gvViewClaimPribadi2.DataSource = query.ToList();
                        gvViewClaimPribadi2.DataBind();
                        if (!query.Any())
                        {
                            pnlSPDPerorangan.Visible = false;

                        }
                        else
                            pnlSPDPerorangan.Visible = true;
                        data.Dispose();
                    }
                    else
                    {
                        DateTime str = Convert.ToDateTime(txtTglBerangkatPribadi.Text);

                        var query = (from claim in data.trClaims //.AsEnumerable()
                                     join spd in data.trSPDs //.AsEnumerable()
                                         on claim.noSPD equals spd.noSPD
                                     where spd.nrp.Trim() == karyawan.nrp
                                        && spd.tglBerangkat == str
                                     select new
                                     {
                                         noSPD = claim.noSPD,
                                         nrp = spd.nrp,
                                         namaLengkap = spd.namaLengkap,
                                         cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                         tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                         tglKembali = spd.tglKembali.ToShortDateString(),
                                         uangMakan = claim.biayaMakan,
                                         uangSaku = claim.uangSaku,
                                         Tiket = claim.tiket,
                                         Hotel = claim.hotel,
                                         BBM = claim.BBM,
                                         Tol = claim.tol,
                                         Taxi = claim.taxi,
                                         AirportTax = claim.airportTax,
                                         laundry = claim.laundry,
                                         Parkir = claim.parkir,
                                         Lain = claim.biayaLainLain,
                                         Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                         status = claim.status
                                     }).OrderByDescending(spd => spd.noSPD);
                        gvViewClaimPribadi2.DataSource = query.ToList();
                        gvViewClaimPribadi2.DataBind();
                        if (!query.Any())
                        {
                            pnlSPDPerorangan.Visible = false;

                        }
                        else
                            pnlSPDPerorangan.Visible = true;
                        data.Dispose();
                    }


                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        protected void btnSimpan_Click(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmClaimApproval.aspx");
            }
            else
            {
                string strLoginID = (string)Session["IDLogin"];
                classSpd oSPD = new classSpd();
                msKaryawan karyawan = new msKaryawan();
                karyawan = oSPD.getKaryawan(strLoginID);
                int status = int.Parse(lblStatus.Text.Split('-')[0]);
                dsSPDDataContext data = new dsSPDDataContext();
                if (status == 8 || status == 9)
                {
                    trRevisi revisi = new trRevisi();
                    revisi.dibuatOleh = karyawan.nrp;
                    revisi.dibuatTanggal = DateTime.Now;
                    revisi.keteranganRevisi = txtRevisi.Text;
                    revisi.noSPD = lblNoSPD.Text;
                    revisi.nrpRevisi = karyawan.nrp;
                    revisi.status = status;
                    data.trRevisis.InsertOnSubmit(revisi);
                }
                if (status == 12 || status == 13)
                {
                    trTolak tolak = new trTolak();
                    tolak.dibuatOleh = karyawan.nrp;
                    tolak.dibuatTanggal = DateTime.Now;
                    tolak.keteranganTolak = txtRevisi.Text;
                    tolak.noSPD = lblNoSPD.Text;
                    tolak.nrpTolak = karyawan.nrp;
                    tolak.status = status;
                    data.trTolaks.InsertOnSubmit(tolak);
                }
                data.SubmitChanges();
                pnlClaimPerorangan.Visible = false;
            }
        }

        protected void btnListFind_Click(object sender, EventArgs e)
        {
            if (Session["sekretaris"] != null && (bool)Session["sekretaris"])
            {
                listfindSekretaris();
            }
            else
            {
                dsSPDDataContext data = new dsSPDDataContext();
                try
                {
                    if (txtTglSpd.Text == "" || txtTglSpd.Text == null)
                    {
                        var query = (from spd in data.trSPDs
                                     join claim in data.trClaims
                                        on spd.noSPD equals claim.noSPD into aps
                                        from y1 in aps.DefaultIfEmpty()
                                    where spd.nrp.Trim() == karyawan.nrp.Trim()
                                        && spd.isApproved == true
                                        && y1 == null
                                    select new
                                    {
                                        noSPD = spd.noSPD,
                                        nrp = spd.nrp,
                                        namaLengkap = spd.namaLengkap,
                                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                        keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                        tglKembali = spd.tglKembali.ToShortDateString(),
                                        status = spd.status //.Split('-')[1]
                                    }).ToList();
                        grvList.DataSource = query;
                        grvList.DataBind();

                        if (!query.Any())
                            pnlClaimPerorangan.Visible = false;
                        else
                            pnlClaimPerorangan.Visible = true;

                        data.Dispose();
                    }
                    else
                    {
                        DateTime str = Convert.ToDateTime(txtTglSpd.Text);
                        var query = (from spd in data.trSPDs
                                     join claim in data.trClaims
                                        on spd.noSPD equals claim.noSPD into aps
                                     from y1 in aps.DefaultIfEmpty()
                                    where spd.nrp == karyawan.nrp
                                        && spd.isApproved == true
                                        && y1 == null
                                        && spd.tglBerangkat == str
                                    select new
                                    {
                                        noSPD = spd.noSPD,
                                        nrp = spd.nrp,
                                        namaLengkap = spd.namaLengkap,
                                        cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                        keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                        tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                        tglKembali = spd.tglKembali.ToShortDateString(),
                                        status = spd.status //.Split('-')[1]
                                    }).ToList();
                        grvList.DataSource = query;
                        grvList.DataBind();

                        if (!query.Any())
                            pnlClaimPerorangan.Visible = false;
                        else
                            pnlClaimPerorangan.Visible = true;

                        data.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);

                }
            }
        }

        protected void lbListRevisi_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            dsSPDDataContext data = new dsSPDDataContext();
            trClaim claimQ= (from c in data.trClaims
                       where c.noSPD == strNoSpd
                       select c).SingleOrDefault();
            if (claimQ ==null)
            {
                Session["editable"] = true;
            }
            else
                Session["editable"] = false;
            DetailClick(sender);

        }

        protected void lbDetailFKasir_Click(object sender, EventArgs e)
        {
            Session["editable"] = false;
            DetailClick(sender);
        }

        protected void lbCloseKasir_Click(object sender, EventArgs e)
        {
            string status = "20-Claim Close";
            akanUbahStatus(sender, status);
            btnFindKasir_Click(null, null);
            historyApproval(sender, status);
        }

        protected void gvViewClaim_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbSetuju = (LinkButton)e.Row.FindControl("lbSetuju");
                LinkButton lbTolak = (LinkButton)e.Row.FindControl("lbTolak");
                //LinkButton lbRevisi = (LinkButton)e.Row.FindControl("lbRevisi");
                Literal ltrlStatus = (Literal)e.Row.FindControl("ltrlStatus");

                if (ltrlStatus.Text.Split('-')[0].ToString() == "20")
                {
                    lbSetuju.Visible = false;
                    lbTolak.Visible = false;
                    //lbRevisi.Visible = false;
                }
            }
        }

        protected void gvViewClaimGA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbSetuju = (LinkButton)e.Row.FindControl("lbSetujuGA");
                LinkButton lbTolak = (LinkButton)e.Row.FindControl("lbTolakGA");
                //LinkButton lbRevisi = (LinkButton)e.Row.FindControl("lbRevisiGA");
                Literal ltrlStatus = (Literal)e.Row.FindControl("ltrlStatus");

                if (ltrlStatus.Text.Split('-')[0].ToString() == "20")
                {
                    lbSetuju.Visible = false;
                    lbTolak.Visible = false;
                    //lbRevisi.Visible = false;
                }
            }
        }

        protected void gvViewClaimFinance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbSetujuFinance = (LinkButton)e.Row.FindControl("lbSetujuFinance");
                Literal ltrlStatus = (Literal)e.Row.FindControl("ltrlStatus");

                if (ltrlStatus.Text.Split('-')[0].ToString() == "20")
                {
                    lbSetujuFinance.Visible = false;
                }
            }
        }

        protected void gvViewClaimPribadi_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnFindKasir_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            msKaryawan kasirAsal = (from k in data.msKaryawans
                                    join u in data.msUsers on k.nrp equals u.nrp
                                    where u.roleId == 19 && u.nrp == lblNRP.Text
                                    select k
                                ).SingleOrDefault();
            if (kasirAsal != null)
            {
                if (TextBox1.Text == "" || TextBox1.Text == null)
                {
                    var query = from claim in data.trClaims //.AsEnumerable()
                                join spd in data.trSPDs //.AsEnumerable()
                                    on claim.noSPD equals spd.noSPD
                                join k in data.msKaryawans
                                    on spd.nrp equals k.nrp
                                where
                                    //claim.status.Split('-')[0] == "26" &&
                                    ((k.coCd == kasirAsal.coCd || k.coCd == "1000") && (k.kodePA == kasirAsal.kodePA || k.kodePA == "1000")
                                    && (k.kodePSubArea == kasirAsal.kodePSubArea || k.kodePSubArea == "1000"))
                                select new
                                {
                                    noSPD = claim.noSPD,
                                    nrp = spd.nrp,
                                    namaLengkap = spd.namaLengkap,
                                    cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                    tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                    tglKembali = spd.tglKembali.ToShortDateString(),
                                    uangMakan = claim.biayaMakan,
                                    uangSaku = claim.uangSaku,
                                    Tiket = claim.tiket,
                                    Hotel = claim.hotel,
                                    BBM = claim.BBM,
                                    Tol = claim.tol,
                                    Taxi = claim.taxi,
                                    AirportTax = claim.airportTax,
                                    laundry = claim.laundry,
                                    Parkir = claim.parkir,
                                    Lain = claim.biayaLainLain,
                                    Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                    status = claim.status //.Split('-')[1]
                                };
                    gvViewKasir.DataSource = query.ToList();
                    gvViewKasir.DataBind();
                    // gvViewKasir.AllowPaging=true;
                    if (!query.Any())
                    {
                        pnlKasir.Visible = false;
                    }
                    else
                        pnlKasir.Visible = true;
                }
                else {
                    DateTime str = Convert.ToDateTime(TextBox1.Text);
                    var query = from claim in data.trClaims //.AsEnumerable()
                                join spd in data.trSPDs //.AsEnumerable()
                                    on claim.noSPD equals spd.noSPD
                                join k in data.msKaryawans
                                    on spd.nrp equals k.nrp
                                where
                                    //claim.status.Split('-')[0] == "26" &&
                                    ((k.coCd == kasirAsal.coCd || k.coCd == "1000") && (k.kodePA == kasirAsal.kodePA || k.kodePA == "1000")
                                    && (k.kodePSubArea == kasirAsal.kodePSubArea || k.kodePSubArea == "1000"))
                                    && spd.tglBerangkat == str
                                select new
                                {
                                    noSPD = claim.noSPD,
                                    nrp = spd.nrp,
                                    namaLengkap = spd.namaLengkap,
                                    cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                    tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                    tglKembali = spd.tglKembali.ToShortDateString(),
                                    uangMakan = claim.biayaMakan,
                                    uangSaku = claim.uangSaku,
                                    Tiket = claim.tiket,
                                    Hotel = claim.hotel,
                                    BBM = claim.BBM,
                                    Tol = claim.tol,
                                    Taxi = claim.taxi,
                                    AirportTax = claim.airportTax,
                                    laundry = claim.laundry,
                                    Parkir = claim.parkir,
                                    Lain = claim.biayaLainLain,
                                    Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                    status = claim.status //.Split('-')[1]
                                };
                    gvViewKasir.DataSource = query.ToList();
                    gvViewKasir.DataBind();
                    // gvViewKasir.AllowPaging=true;
                    if (!query.Any())
                    {
                        pnlKasir.Visible = false;
                    }
                    else
                        pnlKasir.Visible = true;
                }
            }
            else
                pnlKasir.Visible = false;

            data.Dispose();
        }

        protected void gvViewClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvViewClaim.PageIndex = e.NewPageIndex;
            btnFind_Click(null,null);
        }

        protected void gvViewClaimGA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvViewClaimGA.PageIndex = e.NewPageIndex;
            btnFindGA_Click(null, null);

        }

        protected void gvViewClaimFinance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvViewClaimFinance.PageIndex = e.NewPageIndex;
            btnFindFinance_Click(null, null);
        }

        protected void gvViewKasir_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvViewKasir.PageIndex = e.NewPageIndex;
            btnFindKasir_Click(null, null);

        }

        protected void grvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvList.PageIndex = e.NewPageIndex;
            btnListFind_Click(null, null);

        }

        protected void gvViewClaimPribadi2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvViewClaimPribadi2.PageIndex = e.NewPageIndex;
            btnFindPribadi_Click(null, null);
        }

        private void FindPibadiSekretaris()
        {
            try
            {
                dsSPDDataContext data = new dsSPDDataContext();
                if (txtTglBerangkatPribadi.Text == "" || txtTglBerangkatPribadi == null)
                {
                    var query = (from claim in data.trClaims //.AsEnumerable()
                                 join spd in data.trSPDs //.AsEnumerable()
                                     on claim.noSPD equals spd.noSPD
                                 let u = data.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                                 where (spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp.Trim()))
                                 select new
                                 {
                                     noSPD = claim.noSPD,
                                     nrp = spd.nrp,
                                     namaLengkap = spd.namaLengkap,
                                     cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                     tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                     tglKembali = spd.tglKembali.ToShortDateString(),
                                     uangMakan = claim.biayaMakan,
                                     uangSaku = claim.uangSaku,
                                     Tiket = claim.tiket,
                                     Hotel = claim.hotel,
                                     BBM = claim.BBM,
                                     Tol = claim.tol,
                                     Taxi = claim.taxi,
                                     AirportTax = claim.airportTax,
                                     laundry = claim.laundry,
                                     Parkir = claim.parkir,
                                     Lain = claim.biayaLainLain,
                                     Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                     status = claim.status //.Split('-')[1]
                                 }).OrderByDescending(spd => spd.noSPD);


                    gvViewClaimPribadi2.DataSource = query.ToList();
                    gvViewClaimPribadi2.DataBind();
                    if (!query.Any())
                    {
                        pnlSPDPerorangan.Visible = false;

                    }
                    else
                        pnlSPDPerorangan.Visible = true;
                    data.Dispose();
                }
                else
                {
                    DateTime str = Convert.ToDateTime(txtTglBerangkatPribadi.Text);

                    var query = (from claim in data.trClaims //.AsEnumerable()
                                 join spd in data.trSPDs //.AsEnumerable()
                                     on claim.noSPD equals spd.noSPD
                                 let u = data.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                                 where (spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp)) && spd.tglBerangkat == str
                                 select new
                                 {
                                     noSPD = claim.noSPD,
                                     nrp = spd.nrp,
                                     namaLengkap = spd.namaLengkap,
                                     cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                     tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                     tglKembali = spd.tglKembali.ToShortDateString(),
                                     uangMakan = claim.biayaMakan,
                                     uangSaku = claim.uangSaku,
                                     Tiket = claim.tiket,
                                     Hotel = claim.hotel,
                                     BBM = claim.BBM,
                                     Tol = claim.tol,
                                     Taxi = claim.taxi,
                                     AirportTax = claim.airportTax,
                                     laundry = claim.laundry,
                                     Parkir = claim.parkir,
                                     Lain = claim.biayaLainLain,
                                     Total = claim.biayaMakan + claim.uangSaku + claim.tiket + claim.hotel + claim.BBM + claim.tol + claim.taxi + claim.airportTax + claim.laundry + claim.parkir + claim.biayaLainLain + claim.komunikasi,
                                     status = claim.status
                                 }).OrderByDescending(spd => spd.noSPD);
                    gvViewClaimPribadi2.DataSource = query.ToList();
                   gvViewClaimPribadi2.DataBind();
                    if (!query.Any())
                    {
                        pnlSPDPerorangan.Visible = false;

                    }
                    else
                        pnlSPDPerorangan.Visible = true;
                    data.Dispose();
                }


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void listfindSekretaris()
        {
            dsSPDDataContext data = new dsSPDDataContext();
            try
            {
                if (txtTglSpd.Text == "" || txtTglSpd.Text == null)
                {
                    var query = from spd in data.trSPDs //.AsEnumerable()
                                join claim in data.trClaims
                                  on spd.noSPD equals claim.noSPD into aps
                                from y1 in aps.DefaultIfEmpty()
                                let u = data.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                                where (spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp.Trim()))
                                    //&& (spd.status.Split('-')[0] == "7")
                                    && spd.isApproved == true
                                    && !data.trClaims.Any(p => p.noSPD == spd.noSPD
                                                          //&& (p.status.Split('-')[0] != "14" || p.status.Split('-')[0] != "15")
                                                          && (p.isApprovedAtasan != false || p.isApprovedGA != false || p.isApprovedFinance != false)
                                                         )
                                    && y1 == null
                                select new
                                {
                                    noSPD = spd.noSPD,
                                    nrp = spd.nrp,
                                    namaLengkap = spd.namaLengkap,
                                    cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                    keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                    tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                    tglKembali = spd.tglKembali.ToShortDateString(),
                                    status = spd.status //.Split('-')[1]
                                };
                    grvList.DataSource = query.ToList();
                    grvList.DataBind();

                    if (!query.Any())
                        pnlClaimPerorangan.Visible = false;
                    else
                        pnlClaimPerorangan.Visible = true;

                    data.Dispose();
                }
                else
                {
                    DateTime str = Convert.ToDateTime(txtTglSpd.Text);
                    var query = from spd in data.trSPDs //.AsEnumerable()
                                join claim in data.trClaims
                                  on spd.noSPD equals claim.noSPD into aps
                                from y1 in aps.DefaultIfEmpty()
                                let u = data.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                                where (spd.nrp == karyawan.nrp || u.Contains(spd.nrp))
                                    //&& (spd.status.Split('-')[0] == "7")
                                    && spd.isApproved == true
                                    && !data.trClaims.Any(p => p.noSPD == spd.noSPD
                                                          //&& (p.status.Split('-')[0] != "14" || p.status.Split('-')[0] != "15")
                                                          && (p.isApprovedAtasan != false || p.isApprovedGA != false || p.isApprovedFinance != false)
                                                         )
                                    && y1 == null
                                    && spd.tglBerangkat == str
                                select new
                                {
                                    noSPD = spd.noSPD,
                                    nrp = spd.nrp,
                                    namaLengkap = spd.namaLengkap,
                                    cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                    keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                    tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                    tglKembali = spd.tglKembali.ToShortDateString(),
                                    status = spd.status //.Split('-')[1]
                                };
                    grvList.DataSource = query.ToList();
                    grvList.DataBind();

                    if (!query.Any())
                        pnlClaimPerorangan.Visible = false;
                    else
                        pnlClaimPerorangan.Visible = true;

                    data.Dispose();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);

            }
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedValue != "tglBerangkat")
                txtTglBerangkatGA_CalendarExtender.Enabled = false;
            else txtTglBerangkatGA_CalendarExtender.Enabled = true;
        }

    }
}