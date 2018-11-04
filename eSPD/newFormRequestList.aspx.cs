using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace eSPD
{
    public partial class newFormRequestList : System.Web.UI.Page
    {
        private static msKaryawan karyawan = new msKaryawan();
        private static classSpd oSPD = new classSpd();
        private static string strID = string.Empty;
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

            nrp.Value = karyawan.nrp;

            if ((bool)Session["sekretaris"])
            {
                if (!IsPostBack)
                {
                    spdDirector.Visible = true;
                    bindDirect();
                    ddlstatus.Visible = false;
                    txtcari.Visible = false;

                }
            }
            else {
                if (!IsPostBack)
                {
                    bindGvlist();
                }
            }

        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                string URL = "~/newFormRequestDetail.aspx?encrypt=" + Encrypto.Encrypt(e.CommandArgument.ToString());
                URL = Page.ResolveClientUrl(URL);
                ScriptManager.RegisterStartupScript(this, GetType(), "openDetail", "openDetail('" + URL + "');", true);
            }

            if (e.CommandName == "Cancel")
            {
                ApprovalSPDUrl approvalSPDUrl = new ApprovalSPDUrl();
                lblMessage.Text = approvalSPDUrl.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, string.Empty);
                lblMessage.Visible = true;
                bindGvlist();
            }
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var Cancel = DataBinder.Eval(e.Row.DataItem, "isCancel");
                var Approved = DataBinder.Eval(e.Row.DataItem, "isApproved");
                bool isCancel = false;
                bool isApproved = false;
                if (Cancel != null) isCancel = true;
                if (Approved != null) isApproved = true;

                if (karyawan.nrp.Equals("99999999") || isCancel || isApproved)
                {
                    Button btnCancel = (Button)e.Row.FindControl("btnCancel");
                    btnCancel.Visible = false;
                }
            }
        }

        protected void gvDirect_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                string URL = "~/newFormRequestDetail.aspx?encrypt=" + Encrypto.Encrypt(e.CommandArgument.ToString());
                URL = Page.ResolveClientUrl(URL);
                ScriptManager.RegisterStartupScript(this, GetType(), "openDetail", "openDetail('" + URL + "');", true);
            }

            if (e.CommandName == "Cancel")
            {
                ApprovalSPDUrl approvalSPDUrl = new ApprovalSPDUrl();
                lblMessage.Text = approvalSPDUrl.ChangeStatus(e.CommandArgument.ToString(), e.CommandName, karyawan.nrp, string.Empty);
                lblMessage.Visible = true;
                bindDirect();
            }
        }

        protected void gvDirect_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var Cancel = DataBinder.Eval(e.Row.DataItem, "isCancel");
                bool isCancel = false;
                if (Cancel != null) isCancel = true;

                if (karyawan.nrp.Equals("99999999") || isCancel)
                {
                    Button btnCancel = (Button)e.Row.FindControl("btnCancel");
                    btnCancel.Visible = false;
                }
            }
        }
        //add by martha
        void bindDirect()
        {
            using (var ctx = new dsSPDDataContext())
            {
               // DateTime tglBerangkat = (string.IsNullOrEmpty(txtTglBerangkat.Text) ? DateTime.Now : Convert.ToDateTime(txtTglBerangkat.Text));
                var data = (from p in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            where u.Contains(p.nrp)
                            select new
                            {
                                noSPD = p.noSPD,
                                nrp = p.nrp,
                                namaLengkap = p.namaLengkap,
                                tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                keperluan = p.idKeperluan != null ? p.msKeperluan.keperluan : p.keperluanLain,
                                tglBerangkat = p.tglBerangkat,
                                tglKembali = p.tglKembali,
                                isCancel = p.isCancel,
                                status = p.status,
                                tglExpired=p.tglExpired
                            }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();
                gvDirect.DataSource = data;
                gvDirect.DataBind();
                gvDirect.Visible = true;
                Session["dtbl"] = data;
            }

        }

        void bindGvlist()
        {
            using (var ctx = new dsSPDDataContext())
            {
                // DateTime tglBerangkat = (string.IsNullOrEmpty(txtTglBerangkat.Text) ? DateTime.Now : Convert.ToDateTime(txtTglBerangkat.Text));
                var data = (from p in ctx.trSPDs
                            let u = ctx.msKaryawans.Select(x => x.nrp)
                            where u.Contains(p.nrp) && p.nrp.Trim() == karyawan.nrp
                            select new
                            {
                                noSPD = p.noSPD,
                                nrp = p.nrp,
                                namaLengkap = p.namaLengkap,
                                tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                Keperluan = p.ketKeperluan != null ? p.msKeperluan.keperluan : p.ketKeperluan,
                                tglBerangkat = p.tglBerangkat,
                                tglKembali = p.tglKembali,
                                isCancel = p.isCancel,
                                status = p.status,
                                isApproved = p.isApproved,
                                tglExpired = p.tglExpired
                            }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();
                gvList.DataSource = data;
                gvList.DataBind();
                gvList.Visible = true;
                Session["dtbl"] = data;
            }
        }

        protected void gvDirect_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string paramField = DropDownList1.Text.Trim() == "--Select--" ? "" : DropDownList1.Text.Trim();
            string a = DropDownList1.SelectedValue;
            using (var ctx = new dsSPDDataContext())
            {
                var data = (from p in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            where u.Contains(p.nrp)
                            select new
                            {
                                noSPD = p.noSPD,
                                nrp = p.nrp,
                                namaLengkap = p.namaLengkap,
                                tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                keperluan = p.idKeperluan != null ? p.msKeperluan.keperluan : p.keperluanLain,
                                tglBerangkat = p.tglBerangkat,
                                tglKembali = p.tglKembali,
                                isCancel = p.isCancel,
                                status = p.status,
                                tglExpired = p.tglExpired
                            }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();
                if (a == "No")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvDirect.PageIndex = e.NewPageIndex;
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvDirect.PageIndex = e.NewPageIndex;
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
                }
                else if (a == "Status")
                {
                    string paramValue = ddlstatus.SelectedValue;
                    data = data.Where(o => o.status.Contains(paramValue)).ToList();
                    gvDirect.PageIndex = e.NewPageIndex;
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
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
                        gvDirect.PageIndex = e.NewPageIndex;
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();


                    }
                    else
                    {
                        string paramValue = txtcari.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvDirect.PageIndex = e.NewPageIndex;
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();

                    }
                }
                else
                {
                    gvDirect.PageIndex = e.NewPageIndex;
                    bindDirect();
                }

            }
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string paramField = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.Text.Trim();
            string a = DropDownList2.SelectedValue;
            using (var ctx = new dsSPDDataContext())
            {
                var data = (from p in ctx.trSPDs
                            let u = ctx.msKaryawans.Select(x => x.nrp)
                            where u.Contains(p.nrp) && p.nrp.Trim() == karyawan.nrp
                            select new
                            {
                                noSPD = p.noSPD,
                                nrp = p.nrp,
                                namaLengkap = p.namaLengkap,
                                tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                keperluan = p.idKeperluan != null ? p.msKeperluan.keperluan : p.keperluanLain,
                                tglBerangkat = p.tglBerangkat,
                                tglKembali = p.tglKembali,
                                isCancel = p.isCancel,
                                status = p.status,
                                isApproved = p.isApproved,
                                tglExpired = p.tglExpired
                            }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();
                if (a == "No")
                {
                    string paramValue = txtcari1.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvList.PageIndex = e.NewPageIndex;
                    gvList.DataSource = data;
                    gvList.DataBind();
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari1.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvList.PageIndex = e.NewPageIndex;
                    gvList.DataSource = data;
                    gvList.DataBind();
                }
                else if (a == "Status")
                {
                    string paramValue = ddlstatus1.SelectedValue;
                    data = data.Where(o => o.status.Contains(paramValue)).ToList();
                    gvList.PageIndex = e.NewPageIndex;
                    gvList.DataSource = data;
                    gvList.DataBind();
                }
                else if (a == "TglBerangkat")
                {

                    if (txtTglBerangkatAwal1.Text != string.Empty && txtTglBerangkatAkhir1.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal1.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir1.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvList.PageIndex = e.NewPageIndex;
                        gvList.DataSource = data;
                        gvList.DataBind();
                    }
                    else
                    {
                        string paramValue = txtcari1.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvList.PageIndex = e.NewPageIndex;
                        gvList.DataSource = data;
                        gvList.DataBind();

                    }
                }
                else
                {
                    gvList.PageIndex = e.NewPageIndex;
                    bindGvlist();
                }

            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            bindDirect();
        }

        protected void gvDirect_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            bindDirect();
        }

        //add by martha
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string a = DropDownList1.SelectedValue;


                if (a == "No")
                {
                  txtcari.Text = "";
                  txtcari.Visible= true;
                  ddlstatus.Visible = false;
                  txtTglBerangkatAwal.Visible = false;
                  txtTglBerangkatAkhir.Visible = false;
                }
                else if (a == "Nama")
                {
                    txtcari.Text = "";
                    txtcari.Visible = true;
                    ddlstatus.Visible = false;
                    txtTglBerangkatAwal.Visible = false;
                    txtTglBerangkatAkhir.Visible = false;
                }
                else if (a == "Status")
                {
                    txtcari.Visible = false;
                    ddlstatus.Visible = true;
                    txtTglBerangkatAwal.Visible = false;
                    txtTglBerangkatAkhir.Visible = false;
                    SqlCommand cmd = new SqlCommand("Select distinct status from trSPD", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
                    cmd.Connection.Open();

                    SqlDataReader ddlstatusvalue;
                    ddlstatusvalue = cmd.ExecuteReader();

                    ddlstatus.DataSource = ddlstatusvalue;
                    ddlstatus.DataValueField = "status";
                    ddlstatus.DataTextField = "status";
                    ddlstatus.DataBind();

                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                }
                else if (a == "TglBerangkat")
                {
                    txtcari.Visible = false;
                    txtTglBerangkatAwal.Visible = true;
                    txtTglBerangkatAkhir.Visible = true;
                    ddlstatus.Visible = false;
                }
            }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {

            string a = DropDownList2.SelectedValue;


            if (a == "No")
            {
                txtcari1.Text = "";
                txtcari1.Visible = true;
                ddlstatus1.Visible = false;
                txtTglBerangkatAwal1.Visible = false;
                txtTglBerangkatAkhir1.Visible = false;
            }
            else if (a == "Nama")
            {
                txtcari1.Text = "";
                txtcari1.Visible = true;
                ddlstatus1.Visible = false;
                txtTglBerangkatAwal1.Visible = false;
                txtTglBerangkatAkhir1.Visible = false;
            }
            else if (a == "Status")
            {
                txtcari1.Visible = false;
                ddlstatus1.Visible = true;
                txtTglBerangkatAwal1.Visible = false;
                txtTglBerangkatAkhir1.Visible = false;
                SqlCommand cmd = new SqlCommand("Select distinct status from trSPD", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
                cmd.Connection.Open();

                SqlDataReader ddlstatusvalue;
                ddlstatusvalue = cmd.ExecuteReader();

                ddlstatus1.DataSource = ddlstatusvalue;
                ddlstatus1.DataValueField = "status";
                ddlstatus1.DataTextField = "status";
                ddlstatus1.DataBind();

                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }
            else if (a == "TglBerangkat")
            {
                txtcari1.Visible = false;
                txtTglBerangkatAwal1.Visible = true;
                txtTglBerangkatAkhir1.Visible = true;
                ddlstatus1.Visible = false;
            }
        }

        //add by martha
        protected void btncari_Click(object sender, EventArgs e)
        {
            string paramField = DropDownList1.Text.Trim() == "--Select--" ? "" : DropDownList1.Text.Trim();
            string a = DropDownList1.SelectedValue;


            using (var ctx = new dsSPDDataContext())
            {
                var data = (from p in ctx.trSPDs
                            let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                            where u.Contains(p.nrp)
                            select new
                            {
                                noSPD = p.noSPD,
                                nrp = p.nrp,
                                namaLengkap = p.namaLengkap,
                                tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                keperluan = p.idKeperluan != null ? p.msKeperluan.keperluan : p.keperluanLain,
                                tglBerangkat = p.tglBerangkat,
                                tglKembali = p.tglKembali,
                                isCancel = p.isCancel,
                                status = p.status,
                                tglExpired = p.tglExpired
                            }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
                    Session["dtbl"] = "Nama";
                }
                else if (a == "Status")
                {
                    string paramValue = ddlstatus.SelectedValue;
                    data = data.Where(o => o.status.Contains(paramValue)).ToList();
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
                    Session["dtbl"] = "Status";

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
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();


                    }
                    else
                    {
                        string paramValue = txtcari.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();

                    }
                }
            }
        }

        protected void btncari1_Click(object sender, EventArgs e)
        {
            string paramField = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.Text.Trim();
            string a = DropDownList2.SelectedValue;


            using (var ctx = new dsSPDDataContext())
            {
                var data = (from p in ctx.trSPDs
                            let u = ctx.msKaryawans.Select(x => x.nrp)
                            where u.Contains(p.nrp) && p.nrp.Trim() == karyawan.nrp
                            select new
                            {
                                noSPD = p.noSPD,
                                nrp = p.nrp,
                                namaLengkap = p.namaLengkap,
                                tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                Keperluan = p.ketKeperluan != null ? p.msKeperluan.keperluan : p.ketKeperluan,
                                tglBerangkat = p.tglBerangkat,
                                tglKembali = p.tglKembali,
                                isCancel = p.isCancel,
                                status = p.status,
                                isApproved = p.isApproved,
                                tglExpired = p.tglExpired
                            }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari1.Text;
                    data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                    gvList.DataSource = data;
                    gvList.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari1.Text;
                    data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                    gvList.DataSource = data;
                    gvList.DataBind();
                    Session["dtbl"] = "Nama";
                }
                else if (a == "Status")
                {
                    string paramValue = ddlstatus1.SelectedValue;
                    data = data.Where(o => o.status.Contains(paramValue)).ToList();
                    gvList.DataSource = data;
                    gvList.DataBind();
                    Session["dtbl"] = "Status";

                }
                else if (a == "TglBerangkat")
                {
                    if (txtTglBerangkatAwal1.Text != string.Empty && txtTglBerangkatAkhir1.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal1.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir1.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvList.DataSource = data;
                        gvList.DataBind();


                    }
                    else
                    {
                        string paramValue = txtcari1.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvList.DataSource = data;
                        gvList.DataBind();

                    }
                }
            }
        }

        //add by martha

        protected void gvDirect_Sorting(object sender, GridViewSortEventArgs e)
        {

            string sortDir = string.Empty;

            if (Session["Sort"] == null)
            {

                sortDir = "DESC";
            }
            else if (Session["Sort"].ToString() == "ASC")
            {
                sortDir = "DESC";
            }
            else
            {
                sortDir = "ASC";
            }
            Session["Sort"] = sortDir;
            Session["SortCol"] = e.SortExpression;

            //string Ses = Session["dtbl"].ToString();
            string SortCol = Session["SortCol"].ToString();

            string paramField = DropDownList1.Text.Trim() == "--Select--" ? "" : DropDownList1.Text.Trim();
            //string a = DropDownList1.SelectedValue;
            if (paramField == "" && SortCol == "" && sortDir == "")
            {
                using (var ctx = new dsSPDDataContext())
                {
                    var data = (from p in ctx.trSPDs
                                let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                                where u.Contains(p.nrp)
                                select new
                                {
                                    noSPD = p.noSPD,
                                    nrp = p.nrp,
                                    namaLengkap = p.namaLengkap,
                                    tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                    keperluan = p.idKeperluan != null ? p.msKeperluan.keperluan : p.keperluanLain,
                                    tglBerangkat = p.tglBerangkat,
                                    tglKembali = p.tglKembali,
                                    isCancel = p.isCancel,
                                    status = p.status,
                                    tglExpired = p.tglExpired
                                }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
                 }
            }
                else if ((paramField != "" && SortCol != "" && sortDir != ""))
                {
                    using (var ctx = new dsSPDDataContext())
                {
                    var data = (from p in ctx.trSPDs
                                let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                                where u.Contains(p.nrp)
                                select new
                                {
                                    noSPD = p.noSPD,
                                    nrp = p.nrp,
                                    namaLengkap = p.namaLengkap,
                                    tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                    keperluan = p.idKeperluan != null ? p.msKeperluan.keperluan : p.keperluanLain,
                                    tglBerangkat = p.tglBerangkat,
                                    tglKembali = p.tglKembali,
                                    isCancel = p.isCancel,
                                    status = p.status,
                                    tglExpired= p.tglExpired
                                }).OrderBy( o=>SortCol + "" + sortDir).ToList();

                    string Ses = DropDownList1.SelectedValue;
                    if (Ses == "No")
                    {
                        string paramValue = txtcari.Text;
                        data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();
                    }
                    else if (Ses == "Nama")
                    {
                        string paramValue = txtcari.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();

                    }
                    else if (Ses == "Status")
                    {
                        string paramValue = ddlstatus.SelectedValue;
                        data = data.Where(o => o.status.Contains(paramValue)).ToList();
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();


                    }
                    else if (Ses == "TglBerangkat")
                    {
                        string paramValueawal = txtTglBerangkatAwal.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvDirect.DataSource = data;
                        gvDirect.DataBind();

                    }
                    }
                }
            else if ((paramField == "" && SortCol != "" && sortDir != ""))
            {
                using (var ctx = new dsSPDDataContext())
                {
                    var data = (from p in ctx.trSPDs
                                let u = ctx.msUsers.Where(x => x.roleId == 14 || x.roleId == 13).Select(x => x.nrp)
                                where u.Contains(p.nrp)
                                select new
                                {
                                    noSPD = p.noSPD,
                                    nrp = p.nrp,
                                    namaLengkap = p.namaLengkap,
                                    tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                    keperluan = p.idKeperluan != null ? p.msKeperluan.keperluan : p.keperluanLain,
                                    tglBerangkat = p.tglBerangkat,
                                    tglKembali = p.tglKembali,
                                    isCancel = p.isCancel,
                                    status = p.status,
                                    tglExpired=p.tglExpired
                                }).OrderBy(o => SortCol + "" + sortDir).ToList();
                    gvDirect.DataSource = data;
                    gvDirect.DataBind();
                }
            }

             //
            }

        protected void gvList_Sorting(object sender, GridViewSortEventArgs e)
        {

            string sortDir = string.Empty;

            if (Session["Sort"] == null)
            {

                sortDir = "DESC";
            }
            else if (Session["Sort"].ToString() == "ASC")
            {
                sortDir = "DESC";
            }
            else
            {
                sortDir = "ASC";
            }
            Session["Sort"] = sortDir;
            Session["SortCol"] = e.SortExpression;

            //string Ses = Session["dtbl"].ToString();
            string SortCol = Session["SortCol"].ToString();

            string paramField = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.Text.Trim();
            //string a = DropDownList1.SelectedValue;
            if (paramField == "" && SortCol == "" && sortDir == "")
            {
                using (var ctx = new dsSPDDataContext())
                {
                    var data = (from p in ctx.trSPDs
                                let u = ctx.msKaryawans.Select(x => x.nrp)
                                where u.Contains(p.nrp) && p.nrp.Trim() == karyawan.nrp
                                select new
                                {
                                    noSPD = p.noSPD,
                                    nrp = p.nrp,
                                    namaLengkap = p.namaLengkap,
                                    tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                    Keperluan = p.ketKeperluan != null ? p.msKeperluan.keperluan : p.ketKeperluan,
                                    tglBerangkat = p.tglBerangkat,
                                    tglKembali = p.tglKembali,
                                    isCancel = p.isCancel,
                                    status = p.status,
                                    isApproved = p.isApproved,
                                    tglExpired = p.tglExpired
                                }).OrderByDescending(o => o.noSPD).OrderByDescending(o => o.tglBerangkat).ToList();
                    gvList.DataSource = data;
                    gvList.DataBind();
                 }
            }
                else if ((paramField != "" && SortCol != "" && sortDir != ""))
                {
                    using (var ctx = new dsSPDDataContext())
                {
                    var data = (from p in ctx.trSPDs
                                let u = ctx.msKaryawans.Select(x => x.nrp)
                                where u.Contains(p.nrp)
                                select new
                                {
                                    noSPD = p.noSPD,
                                    nrp = p.nrp,
                                    namaLengkap = p.namaLengkap,
                                    tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                    Keperluan = p.ketKeperluan != null ? p.msKeperluan.keperluan : p.ketKeperluan,
                                    tglBerangkat = p.tglBerangkat,
                                    tglKembali = p.tglKembali,
                                    isCancel = p.isCancel,
                                    status = p.status,
                                    tglExpired=p.tglExpired
                                }).OrderBy( o=>SortCol + "" + sortDir).ToList();

                    string Ses = DropDownList2.SelectedValue;
                    if (Ses == "No")
                    {
                        string paramValue = txtcari1.Text;
                        data = data.Where(o => o.noSPD.Contains(paramValue)).ToList();
                        gvList.DataSource = data;
                        gvList.DataBind();
                    }
                    else if (Ses == "Nama")
                    {
                        string paramValue = txtcari1.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvList.DataSource = data;
                        gvList.DataBind();

                    }
                    else if (Ses == "Status")
                    {
                        string paramValue = ddlstatus1.SelectedValue;
                        data = data.Where(o => o.status.Contains(paramValue)).ToList();
                        gvList.DataSource = data;
                        gvList.DataBind();


                    }
                    else if (Ses == "TglBerangkat")
                    {
                        string paramValueawal = txtTglBerangkatAwal1.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir1.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglBerangkat.CompareTo(Dateawal) >= 0 && o.tglBerangkat.CompareTo(Dateakhir) <= 0).ToList();
                        gvList.DataSource = data;
                        gvList.DataBind();

                    }
                    }
                }
            else if ((paramField == "" && SortCol != "" && sortDir != ""))
            {
                using (var ctx = new dsSPDDataContext())
                {
                    var data = (from p in ctx.trSPDs
                                let u = ctx.msKaryawans.Select(x => x.nrp)
                                where u.Contains(p.nrp) && p.nrp.Trim() == karyawan.nrp
                                select new
                                {
                                    noSPD = p.noSPD,
                                    nrp = p.nrp,
                                    namaLengkap = p.namaLengkap,
                                    tujuan = p.companyCodeTujuan != null ? p.companyCodeTujuan + " - " + p.personelAreaTujuan + " - " + p.pSubAreaTujuan : p.tempatTujuanLain,
                                    Keperluan = p.ketKeperluan != null ? p.msKeperluan.keperluan : p.ketKeperluan,
                                    tglBerangkat = p.tglBerangkat,
                                    tglKembali = p.tglKembali,
                                    isCancel = p.isCancel,
                                    status = p.status,
                                    tglExpired = p.tglExpired
                                }).OrderBy(o => SortCol + "" + sortDir).ToList();
                    gvList.DataSource = data;
                    gvList.DataBind();
                }
            }
            }

        protected void gvList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            bindGvlist();
        }
    }
}
