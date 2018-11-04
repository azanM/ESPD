using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSPD
{
    public partial class newClaimList : System.Web.UI.Page
    {
        private static msKaryawan karyawan = new msKaryawan();
        private static classSpd oSPD = new classSpd();
        private static string strID = string.Empty;
        private static ClaimApprovalUrl claimApproval = new ClaimApprovalUrl();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }

            strID = (string)Session["IDLogin"];
            karyawan = oSPD.getKaryawan(strID);
            Session["nrpLogin"] = karyawan.nrp;

            if (string.IsNullOrEmpty(karyawan.nrp)) Response.Redirect("frmHome.aspx");

            if (!IsPostBack)
            {
                ClaimGA.Visible = false;
                ClaimFinance.Visible = false;
                switch (userType(karyawan.nrp))
                {
                    case 17: // GA

                        bindGA();
                        ClaimGA.Visible = true;
                        break;
                    case 19:  // finance

                        bindFinance();
                        ClaimFinance.Visible = true;
                        break;

                    case 23: // sekertaris


                        break;
                    default:
                        break;
                }

                BindClaimSPD();
                // BindAtasan();
                BindPersonal();
                BindPersonalExpired();
            }
        }

        #region user type
        private int? userType(string nrp)
        {

            using (var ctx = new dsSPDDataContext())
            {
                var type = ctx.msUsers.FirstOrDefault(o => o.nrp.Equals(nrp));

                if (type != null)
                {
                    return type.roleId;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Create Claim SPD
        protected void gvClaimSPD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string paramField = DropDownList1.Text.Trim() == "--Select--" ? "" : DropDownList1.Text.Trim();
            string a = DropDownList1.SelectedValue;

            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD into aps
                            from y1 in aps.DefaultIfEmpty()
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true
                                && (spd.isCancel == null || spd.isCancel == false)
                                && (y1 == null)
                                && spd.status != "SPD EXPIRED"
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                tglExpired = spd.tglExpired
                            }).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvClaimSPD.DataSource = data;
                    gvClaimSPD.PageIndex = e.NewPageIndex;
                    gvClaimSPD.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvClaimSPD.DataSource = data;
                    gvClaimSPD.PageIndex = e.NewPageIndex;
                    gvClaimSPD.DataBind();
                    Session["dtbl"] = "Nama";
                }

                else if (a == "TglBerangkat")
                {
                    if (txtTglBerangkatAwal.Text != string.Empty && txtTglBerangkatAkhir.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvClaimSPD.DataSource = data;
                        gvClaimSPD.PageIndex = e.NewPageIndex;
                        gvClaimSPD.DataBind();
                        Session["dtbl"] = "tglBerangkat";
                    }
                    else
                    {
                        string paramValue = txtcari.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvClaimSPD.DataSource = data;
                        gvClaimSPD.PageIndex = e.NewPageIndex;
                        gvClaimSPD.DataBind();
                        Session["dtbl"] = "Nama";
                    }
                }
                else
                {
                    gvClaimSPD.DataSource = data;
                    gvClaimSPD.PageIndex = e.NewPageIndex;
                    gvClaimSPD.DataBind();
                    Session["dtbl"] = "Nama";
                }
            }

            BindClaimSPD();
        }

        protected void gvClaimSPD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                Session["noSPD"] = e.CommandArgument.ToString();
                Response.Redirect("newFormClaimInput.aspx");
            }
        }
        protected void gvClaimSPD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ctx = new dsSPDDataContext();
                DateTime date = DateTime.Now;

                string tanggal = (e.Row.Cells[8].Text);
                string nrp = (e.Row.Cells[1].Text);
                {
                    var Nrp = ctx.NRPNotExpireds.ToList().Where(w => w.nrp == nrp);
                    if (Nrp.Count() > 0)
                    {
                        Button btnCreate = (Button)e.Row.FindControl("btnCreate");
                        btnCreate.Enabled = true;
                    }
                    else
                    {
                        if (tanggal != null && tanggal != "&nbsp;")
                        {
                            DateTime sttrDate = Convert.ToDateTime(tanggal);
                            if (date.Date >= sttrDate.Date)
                            {
                                Button btnCreate = (Button)e.Row.FindControl("btnCreate");
                                btnCreate.Enabled = false;
                            }
                        }
                    }
                }


            }

        }
        void BindClaimSPD()
        {
            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD into aps
                            from y1 in aps.DefaultIfEmpty()
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true
                                && (spd.isCancel == null || spd.isCancel == false)
                                && (y1 == null)
                                && spd.status != "SPD EXPIRED"
                            //((spd.tglExpired >= DateTime.Now.AddDays(1)) || spd.status=="SPD FULL APPROVED")
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                tglExpired = spd.tglExpired
                            }).ToList();

                if (!data.Any())
                {
                    ListSPDClaim.Visible = false;
                }
                else
                {
                    ListSPDClaim.Visible = true;
                    gvClaimSPD.DataSource = data;
                    gvClaimSPD.DataBind();
                }
            }
        }
        #endregion

        #region Approval


        #region GA
        protected void gvGA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadBoardingPass")
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                Response.TransmitFile(Server.MapPath("~/Attach/") + e.CommandArgument);
                Response.End();
            }

            if (e.CommandName == "DownloadBoardingPass1")
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                Response.TransmitFile(Server.MapPath("~/Attach/") + e.CommandArgument);
                Response.End();
            }

            if (e.CommandName == "Complete")
            {
                lblMessage.Text = claimApproval.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, "GA");
                lblMessage.Visible = true;
            }

            if (e.CommandName == "Detail")
            {
                string URL = "~/newFormRequestDetailClaim.aspx?encrypt=" + Encrypto.Encrypt(e.CommandArgument.ToString());
                URL = Page.ResolveClientUrl(URL);
                ScriptManager.RegisterStartupScript(this, GetType(), "openDetail", "openDetail('" + URL + "');", true);
            }
            else
            {

                if (e.CommandName == "Approve")
                {
                    lblMessage.Text = claimApproval.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, "GA");
                    lblMessage.Visible = true;
                }

                if (e.CommandName == "Reject")
                {
                    lblMessage.Text = claimApproval.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, "GA");
                    lblMessage.Visible = true;
                }
                lblMessage.Visible = true;
                bindGA();
            }

            if (e.CommandName == "Cancel")
            {
                lblMessage.Text = claimApproval.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, "GA");
                lblMessage.Visible = true;
            }

            if (e.CommandName == "Edit")
            {
                Session["noSPD"] = e.CommandArgument.ToString();
                Response.Redirect("newFormClaimInput.aspx");
            }

        }

        protected void gvGA_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            bindGA();
        }

        protected void gvGA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGA.PageIndex = e.NewPageIndex;
            bindGA();
        }

        protected void ddlParamGA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlParamGA.SelectedValue != "tglBerangkat" && ddlParamGA.SelectedValue != "status")
            {
                ceFilterGA.Enabled = false;
                listClaimStatus.Visible = false;
                txFilterGA.Visible = true;

            }
            else if (ddlParamGA.SelectedValue == "status")
            {
                //ceFilterGA.Enabled = true;
                txFilterGA.Visible = false;
                listClaimStatus.Visible = true;
                SqlCommand cmd = new SqlCommand(" select distinct status from trClaim where status not like '%SPD%' and status <> 'claim expired' ", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
                cmd.Connection.Open();
                SqlDataReader ListStatusClaim;
                ListStatusClaim = cmd.ExecuteReader();
                listClaimStatus.DataSource = ListStatusClaim;
                listClaimStatus.DataValueField = "status";
                listClaimStatus.DataTextField = "status";
                listClaimStatus.DataBind();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

            }
            else
            {
                ceFilterGA.Enabled = true;
                //ceFilterGA.Enabled = false;
                listClaimStatus.Visible = false;
                txFilterGA.Visible = true;
            }
            
        }

        protected void gvGA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button _btnComplete = (Button)e.Row.FindControl("btnComplete");
                _btnComplete.Visible = true;

                var ApprovalGA = (bool?)DataBinder.Eval(e.Row.DataItem, "isApprovedGA");
                var ApprovedAtasan = (bool?)DataBinder.Eval(e.Row.DataItem, "isApprovedAtasan");
                var ApprovalAtasanBool = (ApprovedAtasan == null ? false : ApprovedAtasan);

                var BoardingPass = DataBinder.Eval(e.Row.DataItem, "urlBoardingPass");
                var BoardingPass1 = DataBinder.Eval(e.Row.DataItem, "urlBoardingPass1");



                if (ApprovalGA != null || ApprovedAtasan == null || ApprovalAtasanBool == false)
                {
                    Button btnApprove = (Button)e.Row.FindControl("btnApprove");
                    btnApprove.Visible = false;

                    Button btnReject = (Button)e.Row.FindControl("btnReject");
                    btnReject.Visible = false;
                }

                if (ApprovalGA != null || ApprovedAtasan == null || ApprovalAtasanBool == false)
                {
                    Button btnApprove = (Button)e.Row.FindControl("btnApprove");
                    btnApprove.Visible = false;

                    Button btnReject = (Button)e.Row.FindControl("btnReject");
                    btnReject.Visible = false;
                }

                if (BoardingPass == null)
                {
                    LinkButton btnBoardingPass = (LinkButton)e.Row.FindControl("lnkBoardingPass");
                    btnBoardingPass.Visible = false;
                } if (BoardingPass1 == null)
                {
                    LinkButton btnBoardingPass1 = (LinkButton)e.Row.FindControl("lnkBoardingPass1");
                    btnBoardingPass1.Visible = false;
                }

                string strTerima = e.Row.Cells[9].Text;
                if (strTerima != null && strTerima != "&nbsp;")
                {

                    DateTime dtTerima = DateTime.Parse(strTerima);


                    // add working day
                    DayOfWeek day = dtTerima.DayOfWeek;
                    DateTime tglTerima;
                    DateTime datenow = DateTime.Now;
                    if ((day == DayOfWeek.Saturday))
                    {
                        tglTerima = dtTerima.AddDays(4);
                    }
                    else if (day == DayOfWeek.Sunday)
                    {
                        tglTerima = dtTerima.AddDays(3);
                    }
                    else
                    {
                        tglTerima = dtTerima.AddDays(2);
                    }

                    if (datenow.Date > tglTerima.Date)
                    {
                        Button btnApprove = (Button)e.Row.FindControl("btnApprove");
                        Button btnReject = (Button)e.Row.FindControl("btnReject");
                        Button btnCancel = (Button)e.Row.FindControl("btnCancel");
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");

                        btnApprove.Enabled = false;
                        btnReject.Enabled = false;
                        btnCancel.Enabled = false;
                        btnEdit.Enabled = false;
                    }
                }

                // Add by Yehezkiel (20171003)
                var statusClaim = DataBinder.Eval(e.Row.DataItem, "status");
                string StatusC = Convert.ToString(statusClaim);

                if (StatusC == "Claim Expired")
                {
                    using (var ctx = new dsSPDDataContext())
                    {
                        var dataClaim = ctx.trClaims.FirstOrDefault(o => o.noSPD == e.Row.Cells[0].Text);

                        if (dataClaim.TidakExpired == false || dataClaim.TidakExpired == null)
                        {
                            Button btnApprove = (Button)e.Row.FindControl("btnApprove");
                            btnApprove.Visible = false;

                            Button btnReject = (Button)e.Row.FindControl("btnReject");
                            btnReject.Visible = false;

                            Button btnCancel = (Button)e.Row.FindControl("btnCancel");
                            btnCancel.Visible = false;

                            Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                            btnEdit.Visible = false;

                            Button btnComplete = (Button)e.Row.FindControl("btnComplete");
                            btnComplete.Visible = false;
                        }
                    }
                }
            }
        }

        protected void btnFindGA_Click(object sender, EventArgs e)
        {
            bindGA();
        }

        void bindGA()
        {
            using (var ctx = new dsSPDDataContext())
            {
                string filter = ddlParamGA.Text.Trim() == "--Select--" ? "" : ddlParamGA.SelectedItem.Value;
                string param = String.Empty;
                if (filter == "status")
                {
                    param = listClaimStatus.SelectedValue;
                }
                else
                {
                    param = txFilterGA.Text;
                }
                var data = ctx.sp_GetApprovalClaimSPDOlehGA(filter, param).ToList();
                gvGA.DataSource = data;
                gvGA.DataBind();


            }

        }
        #endregion

        #region finance
        protected void gvFinance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                string URL = "~/newFormRequestDetailClaim.aspx?encrypt=" + Encrypto.Encrypt(e.CommandArgument.ToString());
                URL = Page.ResolveClientUrl(URL);
                ScriptManager.RegisterStartupScript(this, GetType(), "openDetail", "openDetail('" + URL + "');", true);
            }

            if (e.CommandName == "Approve")
            {
                lblMessage.Text = claimApproval.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, "Finance");
                lblMessage.Visible = true;
            }

            if (e.CommandName == "Reject")
            {
                lblMessage.Text = claimApproval.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, "Finance");
                lblMessage.Visible = true;
            }

            if (e.CommandName != "Detail") bindFinance();
        }
        protected void gvFinance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFinance.PageIndex = e.NewPageIndex;
            bindFinance();
        }
        void bindFinance()
        {
            string filterBy = DropDownListApprovalFinance.SelectedValue;

            using (var ctx = new dsSPDDataContext())
            {
                if (Session["IDLogin"].ToString().ToLower() == "dar00004091")
                {
                    var data = (from claim in ctx.trClaims
                                join spd in ctx.trSPDs
                                    on claim.noSPD equals spd.noSPD
                                join k in ctx.msKaryawans
                                    on spd.nrp equals k.nrp
                                where claim.isApprovedGA == true
                                    && claim.isApprovedFinance == null
                                    && claim.isCancel == null
                                    && claim.status != "Claim Expired"
                                select new
                                {
                                    noSPD = claim.noSPD,
                                    nrp = spd.nrp,
                                    namaLengkap = spd.namaLengkap,
                                    cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
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
                                    status = claim.status,
                                    BPHClaim = claim.BPHClaim,
                                    ErrorBPHClaim = claim.errorBPHClaim,
                                    BankKey = k.bankkey,
                                    BankAcount = k.bankacount
                                }).ToList();

                    switch (filterBy)
                    {
                        case "No":
                            data = data
                                .Where(m => m.noSPD.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "Nama":
                            data = data
                                .Where(m => m.namaLengkap.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "BPHClaim":
                            data = data
                                .Where(m => m.BPHClaim.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "ErrorBPHClaim":
                            data = data
                                .Where(m => m.ErrorBPHClaim.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "TglBerangkat":
                            if (txtTglBerangkatAwalApprovalFinance.Text != string.Empty && txtTglBerangkatAkhirApprovalFinance.Text != string.Empty)
                            {
                                var Dateawal = Convert.ToDateTime(txtTglBerangkatAwalApprovalFinance.Text);
                                var Dateakhir = Convert.ToDateTime(txtTglBerangkatAkhirApprovalFinance.Text);

                                data = data
                                    .Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0)
                                    .Where(o => o.tglBerangkat.CompareTo(Dateakhir) <= 0)
                                    .ToList();
                            }
                            else
                            {
                                data = data
                                    .Where(m => m.noSPD.Contains(txtCariApprovalFinance.Text))
                                    .ToList();
                            }

                            break;
                    }

                    gvFinance.DataSource = data;
                    gvFinance.DataBind();
                }
                else
                {
                    var data = (from claim in ctx.trClaims
                                join spd in ctx.trSPDs
                                    on claim.noSPD equals spd.noSPD
                                join k in ctx.msKaryawans
                                    on spd.nrp equals k.nrp
                                where k.coCd == karyawan.coCd
                                    && claim.isApprovedGA == true
                                    && claim.isApprovedFinance == null
                                    && claim.isCancel == null
                                    && claim.status != "Claim Expired"
                                select new
                                {
                                    noSPD = claim.noSPD,
                                    nrp = spd.nrp,
                                    namaLengkap = spd.namaLengkap,
                                    cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
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
                                    status = claim.status,
                                    BPHClaim = claim.BPHClaim,
                                    ErrorBPHClaim = claim.errorBPHClaim,
                                    BankKey = k.bankkey,
                                    BankAcount = k.bankacount
                                }).ToList();

                    switch (filterBy)
                    {
                        case "No":
                            data = data
                                .Where(m => m.noSPD.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "Nama":
                            data = data
                                .Where(m => m.namaLengkap.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "BPHClaim":
                            data = data
                                .Where(m => m.BPHClaim.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "ErrorBPHClaim":
                            data = data
                                .Where(m => m.ErrorBPHClaim.Contains(txtCariApprovalFinance.Text))
                                .ToList();

                            break;
                        case "TglBerangkat":
                            if (txtTglBerangkatAwalApprovalFinance.Text != string.Empty && txtTglBerangkatAkhirApprovalFinance.Text != string.Empty)
                            {
                                var Dateawal = Convert.ToDateTime(txtTglBerangkatAwalApprovalFinance.Text);
                                var Dateakhir = Convert.ToDateTime(txtTglBerangkatAkhirApprovalFinance.Text);

                                data = data
                                    .Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0)
                                    .Where(o => o.tglBerangkat.CompareTo(Dateakhir) <= 0)
                                    .ToList();
                            }
                            else
                            {
                                data = data
                                    .Where(m => m.noSPD.Contains(txtCariApprovalFinance.Text))
                                    .ToList();
                            }

                            break;
                    }

                    gvFinance.DataSource = data;
                    gvFinance.DataBind();
                }
            }
        }

        protected void DropDownListApprovalFinance_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterBy = DropDownListApprovalFinance.SelectedValue;

            switch (filterBy)
            {
                case "No":
                case "Nama":
                case "BPHClaim":
                case "ErrorBPHClaim":
                    txtCariApprovalFinance.Text = "";
                    txtCariApprovalFinance.Visible = true;
                    txtTglBerangkatAwalApprovalFinance.Visible = false;
                    txtTglBerangkatAkhirApprovalFinance.Visible = false;

                    break;
                case "TglBerangkat":
                    txtCariApprovalFinance.Visible = false;
                    txtTglBerangkatAwalApprovalFinance.Visible = true;
                    txtTglBerangkatAkhirApprovalFinance.Visible = true;

                    break;
                default:
                    txtCariApprovalFinance.Text = "";
                    txtCariApprovalFinance.Visible = false;
                    txtTglBerangkatAwalApprovalFinance.Visible = false;
                    txtTglBerangkatAkhirApprovalFinance.Visible = false;

                    break;
            }
        }

        protected void btnCariApprovalFinance_Click(object sender, EventArgs e)
        {
            bindFinance();
        }

        #endregion

        #endregion

        #region claim personal
        protected void btnFindPersonal_Click(object sender, EventArgs e)
        {
            BindPersonal();
        }


        protected void gvPersonal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string paramField = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.Text.Trim();
            string a = DropDownList2.SelectedValue;

            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                statusClaim = claim.status,
                                isApprovedFinance = claim.isApprovedFinance,
                                tglExpired = spd.tglExpired,
                                BPHClaim = claim.BPHClaim,
                                ErrorBPHClaim = claim.errorBPHClaim
                            }).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.PageIndex = e.NewPageIndex;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.PageIndex = e.NewPageIndex;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "Nama";
                }
                else if (a == "BPHClaim")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.BPHClaim.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.PageIndex = e.NewPageIndex;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "BPHClaim";
                }
                else if (a == "ErrorBPHClaim")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.ErrorBPHClaim.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.PageIndex = e.NewPageIndex;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "ErrorBPHClaim";
                }
                else if (a == "TglBerangkat")
                {
                    if (txtTglBerangkatAwal2.Text != string.Empty && txtTglBerangkatAkhir2.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal2.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir2.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvPersonal.DataSource = data;
                        gvPersonal.PageIndex = e.NewPageIndex;
                        gvPersonal.DataBind();
                        Session["dtbl"] = "tglBerangkat";
                    }
                    else
                    {
                        string paramValue = txtcari2.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvPersonal.DataSource = data;
                        gvPersonal.PageIndex = e.NewPageIndex;
                        gvPersonal.DataBind();
                        Session["dtbl"] = "Nama";
                    }
                }
                else
                {

                    string paramValue = txtcari2.Text;
                    gvPersonal.DataSource = data;
                    gvPersonal.PageIndex = e.NewPageIndex;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "Nama";

                }
            }


        }

        protected void gvPersonal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                string URL = "~/newFormRequestDetailClaim.aspx?encrypt=" + Encrypto.Encrypt(e.CommandArgument.ToString());
                URL = Page.ResolveClientUrl(URL);
                ScriptManager.RegisterStartupScript(this, GetType(), "openDetail", "openDetail('" + URL + "');", true);
            }

            if (e.CommandName == "Edit")
            {
                Session["noSPD"] = e.CommandArgument.ToString();
                Response.Redirect("newFormClaimInput.aspx");
            }
        }

        void BindPersonal()
        {
            using (var ctx = new dsSPDDataContext())
            {

                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true

                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                statusClaim = claim.status,
                                isApprovedFinance = claim.isApprovedFinance,
                                tglExpired = spd.tglExpired,
                                BPHClaim = claim.BPHClaim,
                                ErrorBPHClaim = claim.errorBPHClaim
                            }).ToList();

                if (!data.Any())
                {
                    lblMessagePersonal.Visible = true;
                    lblMessagePersonal.Text = "No Data";
                    gvPersonal.Visible = false;
                }
                else
                {
                    lblMessagePersonal.Visible = false;
                    gvPersonal.Visible = true;
                    ClaimPersonal.Visible = true;
                    gvPersonal.DataSource = data;
                    gvPersonal.DataBind();
                }
            }
        }
        #endregion

        private DateTime? convert_date(DateTime? a)
        {
            Convert.ToDateTime(a);
            return a;
        }

        protected void gvPersonal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                var statusClaim = DataBinder.Eval(e.Row.DataItem, "statusClaim");
                var isApprovedFinance = DataBinder.Eval(e.Row.DataItem, "isApprovedFinance");

                bool approved = false;
                if (isApprovedFinance != null)
                {
                    try
                    {
                        approved = (bool)isApprovedFinance;
                    }
                    catch (Exception)
                    {
                        approved = true;
                    }
                }

                if (karyawan.nrp.Equals("99999999") || approved)
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = false;
                }

                if (karyawan.nrp.Equals("99999999") && statusClaim.ToString().ToLower() == "saved")
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;
                }

                DateTime date = DateTime.Now;
                string tanggal = (e.Row.Cells[10].Text);
                if (tanggal != null && tanggal != "&nbsp;")
                {
                    DateTime sttrDate = Convert.ToDateTime(tanggal);
                    if (date.Date >= sttrDate.Date)
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        btnEdit.Enabled = false;
                    }
                }
            }
        }

        // Add by Yehezkiel (20171003)
        protected void gvAtasan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var statusClaim = DataBinder.Eval(e.Row.DataItem, "statusClaim");
                try
                {
                    string Status = Convert.ToString(statusClaim);
                    if (Status == "Claim Expired")
                    {
                        Button btnApprove = (Button)e.Row.FindControl("btnApprove");
                        btnApprove.Visible = false;

                        Button btnReject = (Button)e.Row.FindControl("btnReject");
                        btnReject.Visible = false;
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        // Add by Yehezkiel (20171003)
        protected void gvFinance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var statusClaim = DataBinder.Eval(e.Row.DataItem, "statusClaim");
                try
                {
                    string Status = Convert.ToString(statusClaim);
                    if (Status == "Claim Expired")
                    {
                        Button btnApprove = (Button)e.Row.FindControl("btnApprove");
                        btnApprove.Visible = false;

                        Button btnReject = (Button)e.Row.FindControl("btnReject");
                        btnReject.Visible = false;
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        //add by martha
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = DropDownList1.SelectedValue;


            if (a == "No")
            {
                txtcari.Text = "";
                txtcari.Visible = true;
                txtTglBerangkatAwal.Visible = false;
                txtTglBerangkatAkhir.Visible = false;
            }
            else if (a == "Nama")
            {
                txtcari.Text = "";
                txtcari.Visible = true;
                txtTglBerangkatAwal.Visible = false;
                txtTglBerangkatAkhir.Visible = false;
            }
            else if (a == "TglBerangkat")
            {

                txtcari.Visible = false;
                txtTglBerangkatAwal.Visible = true;
                txtTglBerangkatAkhir.Visible = true;
            }
            else
            {
                txtcari.Text = "";
                txtcari.Visible = false;
                txtTglBerangkatAwal.Visible = false;
                txtTglBerangkatAkhir.Visible = false;
            }
        }

        //add by martha
        protected void btncari_Click(object sender, EventArgs e)
        {
            string paramField = DropDownList1.Text.Trim() == "--Select--" ? "" : DropDownList1.Text.Trim();
            string a = DropDownList1.SelectedValue;

            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD into aps
                            from y1 in aps.DefaultIfEmpty()
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true
                                && (spd.isCancel == null || spd.isCancel == false)
                                && (y1 == null)
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                tglExpired = spd.tglExpired
                            }).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvClaimSPD.DataSource = data;
                    gvClaimSPD.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvClaimSPD.DataSource = data;
                    gvClaimSPD.DataBind();
                    Session["dtbl"] = "Nama";
                }
                else if (a == "TglBerangkat")
                {
                    if (txtTglBerangkatAwal.Text != string.Empty && txtTglBerangkatAkhir.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvClaimSPD.DataSource = data;
                        gvClaimSPD.DataBind();
                        Session["dtbl"] = "tglBerangkat";
                    }
                    else
                    {
                        string paramValue = txtcari.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvClaimSPD.DataSource = data;
                        gvClaimSPD.DataBind();
                        Session["dtbl"] = "Nama";
                    }
                }
            }
        }

        //add by martha
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterBy = DropDownList2.SelectedValue;

            switch (filterBy)
            {
                case "No":
                case "Nama":
                case "BPHClaim":
                case "ErrorBPHClaim":
                    txtcari2.Text = "";
                    txtcari2.Visible = true;
                    txtTglBerangkatAwal2.Visible = false;
                    txtTglBerangkatAkhir2.Visible = false;

                    break;
                case "TglBerangkat":
                    txtcari2.Visible = false;
                    txtTglBerangkatAwal2.Visible = true;
                    txtTglBerangkatAkhir2.Visible = true;

                    break;
                default:
                    txtcari2.Text = "";
                    txtcari2.Visible = false;
                    txtTglBerangkatAwal2.Visible = false;
                    txtTglBerangkatAkhir2.Visible = false;

                    break;
            }
        }

        //add by martha
        protected void btncari2_Click(object sender, EventArgs e)
        {
            string paramField = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.Text.Trim();
            string a = DropDownList2.SelectedValue;

            using (var ctx = new dsSPDDataContext())
            {


                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true

                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                statusClaim = claim.status,
                                isApprovedFinance = claim.isApprovedFinance,
                                tglExpired = spd.tglExpired,
                                BPHClaim = claim.BPHClaim,
                                ErrorBPHClaim = claim.errorBPHClaim
                            }).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "Nama";
                }
                else if (a == "BPHClaim")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.BPHClaim.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "BPHClaim";
                }
                else if (a == "ErrorBPHClaim")
                {
                    string paramValue = txtcari2.Text;
                    data = data.Where(o => o.ErrorBPHClaim.Contains(paramValue)).ToList();
                    gvPersonal.DataSource = data;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "ErrorBPHClaim";
                }
                else if (a == "TglBerangkat")
                {
                    if (txtTglBerangkatAwal2.Text != string.Empty && txtTglBerangkatAkhir2.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal2.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir2.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvPersonal.DataSource = data;
                        gvPersonal.DataBind();
                        Session["dtbl"] = "tglBerangkat";
                    }
                    else
                    {
                        string paramValue = txtcari2.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvPersonal.DataSource = data;
                        gvPersonal.DataBind();
                        Session["dtbl"] = "Nama";
                    }
                }
                else
                {
                    string paramValue = txtcari2.Text;
                    gvPersonal.DataSource = data;
                    gvPersonal.DataBind();
                    Session["dtbl"] = "Nama";
                }
            }
        }


        //-------------------------------
        #region claim SPD Expired
        protected void btnFindExpired_Click(object sender, EventArgs e)
        {
            BindPersonalExpired();
        }


        protected void gvExpired_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            DateTime? fltDate = new DateTime();
            using (var ctx = new dsSPDDataContext())
            {

                //var data = ctx.sp_GetListSPDExpired();
                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD into aps
                            from y1 in aps.DefaultIfEmpty()
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true
                                && (spd.isCancel == null || spd.isCancel == false)
                                && (y1 == null)
                                && spd.status == "SPD EXPIRED"
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                tglExpired = spd.tglExpired
                            }).ToList();
                gvExpired.DataSource = data;
                gvExpired.PageIndex = e.NewPageIndex;
                gvExpired.DataBind();

            }
        }

        protected void gvExpired_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                string URL = "~/newFormRequestDetail.aspx?encrypt=" + Encrypto.Encrypt(e.CommandArgument.ToString());
                URL = Page.ResolveClientUrl(URL);
                ScriptManager.RegisterStartupScript(this, GetType(), "openDetail", "openDetail('" + URL + "');", true);
            }

        }
        void BindPersonalExpired()
        {
            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            join claim in ctx.trClaims
                               on spd.noSPD equals claim.noSPD into aps
                            from y1 in aps.DefaultIfEmpty()
                            where (((bool)Session["sekretaris"]) == true ? spd.nrp.Trim() == karyawan.nrp.Trim() || u.Contains(spd.nrp) : spd.nrp.Trim() == karyawan.nrp.Trim())
                                && spd.isApproved == true
                                && (spd.isCancel == null || spd.isCancel == false)
                                && (y1 == null)
                                && spd.status == "SPD EXPIRED"
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                cabangTujuan = spd.companyCodeTujuan != "0" || spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                tglBerangkat = spd.tglBerangkat,
                                tglKembali = spd.tglKembali,
                                status = spd.status,
                                tglExpired = spd.tglExpired
                            }).ToList();
                // var data = ctx.sp_GetListSPDExpired().ToList();
                // gvExpired.DataSource = data;
                //gvExpired.DataBind();

                if (!data.Any())
                {
                    lblMessageExpired.Visible = true;
                    lblMessageExpired.Text = "No Data";
                    gvExpired.Visible = false;
                }
                else
                {
                    lblMessageExpired.Visible = false;
                    gvExpired.Visible = true;
                    ClaimExpired.Visible = true;
                    gvExpired.DataSource = data;
                    gvExpired.DataBind();
                }



            }
        }


        #endregion
    }
}
