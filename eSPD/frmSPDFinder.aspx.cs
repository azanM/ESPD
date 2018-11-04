using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using eSPD.Core;

namespace eSPD
{
    public partial class frmSPDFinder : System.Web.UI.Page
    {
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
            //UID = "anton009190"
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
        msKaryawan karyawan = new msKaryawan();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                string strLoginID = string.Empty;
                bool ga = false;

                if (Session["IDLogin"] != null)
                {
                    strLoginID = (string)Session["IDLogin"];
                }
                else
                {
                    strLoginID = SetLabelWelcome();
                }

                ga = cek_ga(strLoginID);

                if (cek_ga(strLoginID) == false && cek_su(strLoginID) == false)
                {
                    Response.Redirect("frmHome.aspx");
                }

                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(strLoginID);


                if (ddlTipe.SelectedValue == "spd")
                {
                    PanelSPD.Visible = false;
                    PanelClaim.Visible = false;
                }
                else
                {
                    PanelSPD.Visible = false;
                    PanelClaim.Visible = false;
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

        protected void LinqDataSource1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            finder();
        }


        protected void grSPD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grSPD.PageIndex = e.NewPageIndex;
            btnFind_Click(null, null);
        }

        protected void grClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grClaim.PageIndex = e.NewPageIndex;
            btnFind_Click(null, null);
        }


        void finder()
        {
            dsSPDDataContext data = new dsSPDDataContext();

            bool SPD=true;
            string filter = string.Empty;
            
            try
            {
               // filter = getFilter();

                if (ddlTipe.SelectedValue == "spd")
                {
                    SPD = true;
                }
                else
                {
                    SPD = false;
                }

               
                    if (SPD)
                    {
                        var query = from spd in data.trSPDs.AsEnumerable()
                                    join k in data.msKaryawans
                                    on spd.nrp equals k.nrp
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

                        if (txtNama.Text != null && txtNama.Text != "") {
                            query = query.Where(namaLengkap => namaLengkap.ToString().ToLower().Contains(txtNama.Text.Trim().ToLower()));
                         }

                        if (txtTglBerangkat.Text != null && txtTglBerangkat.Text != "") {
                            String str = Convert.ToDateTime(txtTglBerangkat.Text).ToShortDateString().ToString().Trim();
                            query = query.Where(tglBerangkat => tglBerangkat.ToString().Contains(str));

                        }

                        if (ddlStatus.SelectedValue.ToLower() != "all") {
                            String val =ddlStatus.SelectedValue.ToString().Trim();
                            query = query.Where(status => status.ToString().Contains(val));
                        }
                            
                       

                        grSPD.DataSource = query.ToList();
                        grSPD.DataBind();
                        if (!query.Any())
                        {
                            PanelSPD.Visible = false;

                        }
                        else
                            PanelSPD.Visible = true;
                            PanelClaim.Visible = false;
                        data.Dispose();

                    }
                    else
                    {

                        var query = (from claim in data.trClaims.AsEnumerable()
                                     join spd in data.trSPDs.AsEnumerable()
                                         on claim.noSPD equals spd.noSPD
                                     join k in data.msKaryawans
                                         on spd.nrp equals k.nrp
                                     
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
                                     });

                        if (txtNama.Text != null && txtNama.Text != "")
                        {
                            query = query.Where(namaLengkap => namaLengkap.ToString().ToLower().Contains(txtNama.Text.Trim().ToLower()));
                        }

                        if (txtTglBerangkat.Text != null && txtTglBerangkat.Text != "")
                        {
                            String str = Convert.ToDateTime(txtTglBerangkat.Text).ToShortDateString().ToString();
                            query = query.Where(tglBerangkat => tglBerangkat.ToString().Contains(str));

                        }

                        if (ddlStatus.SelectedValue.ToLower() != "all")
                        {
                           query = query.Where(status => status.ToString() == ddlStatus.SelectedValue.ToString());
                        }

                        grClaim.DataSource = query.ToList();
                        grClaim.DataBind();
                        if (!query.Any())
                        {
                            PanelClaim.Visible = false;
                        }
                        else
                            PanelClaim.Visible = true;
                            PanelSPD.Visible = false;
                        data.Dispose();
                    }
                

                

               
            }


            catch (Exception ex)
            {
                Response.Write(ex.Message);

            }
        }

        protected void ddlStatus_DataBound(object sender, EventArgs e)
        {
            ddlStatus.Items.Insert(0, new ListItem("All", "all"));  
        }

        protected void DetailGA_Click(object sender, EventArgs e)
        {
           if(cek_su(Session["IDLogin"].ToString())){
                Session["editable"] =false;
           }

           if (cek_ga(Session["IDLogin"].ToString())) {
               Session["editable"] = true;
               Session["gamode"] = true;
               Session["Role"] = "GA";
           }

           if (Session["IDLogin"].ToString().ToLower() == "wawan010193")
           {
               Session["editable"] = true;
               Session["gamode"] = true;
               Session["Role"] = "GA";
           }         
          
            DetailClick(sender);
        }

        protected void DetailSPD_Click(object sender, EventArgs e)
        {
            if (cek_su(Session["IDLogin"].ToString()))
            {
                Session["editable"] = false;
            }

            if (cek_ga(Session["IDLogin"].ToString()))
            {
                Session["editable"] = true;
                Session["gamode"] = true;
                Session["Role"] = "GA";
            }

                if (Session["IDLogin"].ToString().ToLower() == "wawan010193")
                {
                    Session["editable"] = true;
                    Session["gamode"] = true;
                    Session["Role"] = "GA";
                }
            DetailClicks(sender);
        }

        protected void lbListSPDCancel_Click(object sender, EventArgs e)
        {
            if (cek_ga(Session["IDLogin"].ToString()))
            {
                //cr : 2015-1-30 ian
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

                    ApprovalUrl1 approvalUrl = new ApprovalUrl1();

                    lblStat.Text = approvalUrl.ChangeStatus(noSPD, action, nrpApproval);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

                #region changed
                //string status = "29-SPD Cancel";
                //cek_spd(sender, status);
                //historyApproval(sender, status);
                #endregion

                btnFind_Click(null, null);
            }
        }

        protected void lbListClaimCancel_Click(object sender, EventArgs e)
        {
            if (cek_ga(Session["IDLogin"].ToString()))
            {
                //cr : 2015-01-30 ian
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
                //string status = "29-SPD Cancel";
                //cek_claim(sender, status);
                //historyApproval(sender, status);
                #endregion

                btnFind_Click(null, null);
            }
        }

        private void DetailClick(object sender)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            Session["noSPD"] = strNoSpd;
            Response.Redirect("frmClaimInput.aspx");
        }

        private void DetailClicks(object sender)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            Session["noSPD"] = strNoSpd;
            Response.Redirect("frmRequestInput.aspx");
        }

        private bool cek_ga(string LoginID)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            classSpd oSPD = new classSpd();
            msKaryawan karyawan = new msKaryawan(); ;
            string t_nrp = oSPD.getKaryawan(LoginID).nrp;

            var user = (from u in data.msUsers
                        join k in data.msKaryawans on u.nrp equals k.nrp
                        where u.roleId == 17 && k.nrp == t_nrp
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

        private bool cek_su(string LoginID)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            classSpd oSPD = new classSpd();
            msKaryawan karyawan = new msKaryawan(); ;
            string t_nrp = oSPD.getKaryawan(LoginID).nrp;

            var user = (from u in data.msUsers
                        join k in data.msKaryawans on u.nrp equals k.nrp
                        where u.roleId == 24 && k.nrp == t_nrp
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

        void cek_claim(object sender, string status)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            dsSPDDataContext data = new dsSPDDataContext();
            trClaim claim = new trClaim();
            var cekClaim = (from c in data.trClaims
                            where c.noSPD == strNoSpd
                            select c).FirstOrDefault();
            if (cekClaim == null)
            {

            }
            else
            {
                cekClaim.status = status;
                data.SubmitChanges();
            }


        }

        void cek_spd(object sender, string status) {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            dsSPDDataContext data = new dsSPDDataContext();
            trClaim claim = new trClaim();
            var cekSPD = (from spd in data.trSPDs
                            where spd.noSPD == strNoSpd
                            select spd).First();
            if (cekSPD == null)
            {

            }
            else
            {
                cekSPD.status = status;
                data.SubmitChanges();
            }
        }

        private void historyApproval(object sender, string status)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
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
                string strNRP = strLoginID;
                string strStatus = status;
                //var role = (from msUser in data.msUsers.AsEnumerable()
                //            where msUser.nrp == karyawan.nrp
                //            select msUser).First();
                //var role = (from msRole in data.msRoles.AsEnumerable()
                //            where msRole.namaRole == karyawan.posisi
                //            select msRole).First();

                //string strRole = Convert.ToString(role.id);
                //string strRole = Convert.ToString(role.roleId);
                string strRole = "";
                var role = (from msRole in data.msRoles.AsEnumerable()
                            where msRole.namaRole == karyawan.posisi
                            select msRole).First();

                if (role != null) { strRole = Convert.ToString(role.id); }
                else
                {
                    var user = (from msUser in data.msUsers.AsEnumerable()
                                where msUser.nrp == karyawan.nrp
                                select msUser).First();
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

        #region unsused
       
        //string getFilter() {
        //    bool nama = false;
        //    bool tanggal = false;
        //    //bool status =false;
        //    string valFilter=string.Empty;

        //    if ((txtNama.Text == null || txtNama.Text == "") && (txtTglBerangkat.Text == null || txtTglBerangkat.Text == "") && ddlStatus.SelectedValue.ToLower() == "all")
        //    {
        //        return string.Empty;
        //    }
        //    else {
        //        if (txtNama.Text != null && txtNama.Text != "")
        //        {
        //            valFilter += "spd.namaLengkap.Contains(\""+txtNama.Text+"\")";
        //            nama = true;
        //        }

        //        if (nama) {
        //            valFilter += "&&";
        //        }

        //        if (txtTglBerangkat.Text != null || txtTglBerangkat.Text != "") {
        //            valFilter += "spd.tglBerangkat==\"" + Convert.ToDateTime(txtTglBerangkat)+"\"";
        //            tanggal = true;
        //        }

        //        if (tanggal) {
        //            valFilter += "&&";
        //        }

        //        if (ddlStatus.SelectedValue.ToLower() != "all") {
        //            valFilter += "claim.status.Split('-')[0] == \"" + ddlStatus.SelectedValue.ToLower() + "\"";
        //        }

        //        return valFilter;

        //    }

        //}
        #endregion
    }
}