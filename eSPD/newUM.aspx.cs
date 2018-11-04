using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;




namespace eSPD
{
    public partial class newUM : System.Web.UI.Page
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
                finance.Visible = false;
                switch (userType(karyawan.nrp))
                {
                    case 17: // GA

                        BindGA();
                        ClaimGA.Visible = true;
                        break;

                    case 19: // GA


                        BindFinance();
                        finance.Visible = true;

                        break;

                    default:
                        break;
                }

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



        protected void gvFinance_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Approve")
            {
                using (var ctx = new dsSPDDataContext())
                {
                    var dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == e.CommandArgument.ToString());
                    dataSpd.tglPenyelesaian = DateTime.Now;
                    dataSpd.statusUM = "approve";

                    ctx.SubmitChanges();

                }
            }

            BindFinance();
        }

        protected void gvFinance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFinance.PageIndex = e.NewPageIndex;
            BindFinance();
        }

        void BindFinance()
        {
            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            where spd.uangMuka != "" && spd.uangMuka != "0" && spd.uangMuka != null
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                uangMuka = spd.uangMuka,
                                tglPenyelesaian = spd.tglPenyelesaian,
                                status = spd.statusUM
                            }).ToList();

                if (!data.Any())
                {
                    finance.Visible = false;
                }
                else
                {
                    finance.Visible = true;
                    gvFinance.DataSource = data;
                    gvFinance.DataBind();
                }
            }
        }





        private DateTime? convert_date(DateTime? a)
        {
            Convert.ToDateTime(a);
            return a;
        }

        protected void gvFinance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var statusUM = DataBinder.Eval(e.Row.DataItem, "status");
                if(statusUM != null)
                {
                    Button lbApprove = (Button)e.Row.FindControl("lbApprove");

                    if (statusUM.ToString().ToLower() != "approve")
                    {
                        lbApprove.Visible = true;
                    }
                    else
                    {
                        lbApprove.Visible = false;
                    }
                }
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = DropDownList1.SelectedValue;


            if (a == "No")
            {
                txtcari.Text = "";
                txtcari.Visible = true;
                txtTglBerangkatAwal.Visible = false;
                txtTglBerangkatAkhir.Visible = false;
                ddlStatusFinance.Visible = false;
            }
            else if (a == "Nama")
            {
                txtcari.Text = "";
                txtcari.Visible = true;
                txtTglBerangkatAwal.Visible = false;
                txtTglBerangkatAkhir.Visible = false;
                ddlStatusFinance.Visible = false;
            }
            else if (a == "TglPenyelesaian")
            {

                txtcari.Visible = false;
                txtTglBerangkatAwal.Visible = true;
                txtTglBerangkatAkhir.Visible = true;
                ddlStatusFinance.Visible = false;
            }
            else if (a == "Status")
            {

                txtcari.Text = "";
                txtcari.Visible = false;
                txtTglBerangkatAwal.Visible = false;
                txtTglBerangkatAkhir.Visible = false;
                ddlStatusFinance.Visible = true;
            }
        }

        protected void btncari_Click(object sender, EventArgs e)
        {
            string paramField = DropDownList1.Text.Trim() == "--Select--" ? "" : DropDownList1.Text.Trim();
            string a = DropDownList1.SelectedValue;

            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            where spd.uangMuka != "" && spd.uangMuka != "0" && spd.uangMuka != null
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                uangMuka = spd.uangMuka,
                                tglPenyelesaian = spd.tglPenyelesaian,
                                status = spd.statusUM
                            }).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari.Text;
                    var datas = (from c in ctx.trSPDs
                                 where
                                    SqlMethods.Like(c.noSPD, "%" + paramValue + "%") &&
                                  c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                 select new
                                 {
                                     noSPD = c.noSPD,
                                     nrp = c.nrp,
                                     namaLengkap = c.namaLengkap,
                                     uangMuka = c.uangMuka,
                                     tglPenyelesaian = c.tglPenyelesaian,
                                     status = c.statusUM
                                 }).ToList();
                    gvFinance.DataSource = datas;
                    gvFinance.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari.Text;
                    var datas = (from c in ctx.trSPDs
                                where
                                    SqlMethods.Like(c.namaLengkap, "%" + paramValue + "%") &&
                                     c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                 select new
                                 {
                                     noSPD = c.noSPD,
                                     nrp = c.nrp,
                                     namaLengkap = c.namaLengkap,
                                     uangMuka = c.uangMuka,
                                     tglPenyelesaian = c.tglPenyelesaian,
                                     status = c.statusUM
                                 }).ToList();
                    gvFinance.DataSource = datas;
                    gvFinance.DataBind();
                    Session["dtbl"] = "Nama";
                }

                else if (a == "TglPenyelesaian")
                {
                    if (txtTglBerangkatAwal.Text != string.Empty && txtTglBerangkatAkhir.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglPenyelesaian >= Convert.ToDateTime(txtTglBerangkatAwal.Text) && o.tglPenyelesaian <= Convert.ToDateTime(txtTglBerangkatAkhir.Text)).ToList();
                        gvFinance.DataSource = data;
                        gvFinance.DataBind();
                        Session["dtbl"] = "TglPenyelesaian";
                    }

                }
                else if (a == "Status")
                {
                    string paramValue = ddlStatusFinance.SelectedValue;
                    data = data.Where(o => o.status==paramValue).ToList();
                    gvFinance.DataSource = data;
                    gvFinance.DataBind();
                    Session["dtbl"] = "Status";
                }
            }
        }




        //===============================================

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = DropDownList2.SelectedValue;


            if (a == "No")
            {
                txtcari2.Text = "";
                txtcari2.Visible = true;
                txtTglBerangkatAwal2.Visible = false;
                txtTglBerangkatAkhir2.Visible = false;
                ddlStatusGA.Visible = false;
            }
            else if (a == "Nama")
            {
                txtcari2.Text = "";
                txtcari2.Visible = true;
                txtTglBerangkatAwal2.Visible = false;
                txtTglBerangkatAkhir2.Visible = false;
                ddlStatusGA.Visible = false;
            }
            else if (a == "TglPenyelesaian")
            {

                txtcari2.Visible = false;
                txtTglBerangkatAwal2.Visible = true;
                txtTglBerangkatAkhir2.Visible = true;
                ddlStatusGA.Visible = false;
            }
            else if (a == "Status")
            {

                txtcari2.Visible = false;
                txtTglBerangkatAwal2.Visible = false;
                txtTglBerangkatAkhir2.Visible = false;
                ddlStatusGA.Visible = true;
            }
        }

        protected void btncari2_Click(object sender, EventArgs e)
        {
            string paramField2 = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.Text.Trim();
            string a2 = DropDownList2.SelectedValue;

            using (var ctx = new dsSPDDataContext())
            {
                var data2 = (from spd in ctx.trSPDs
                             where spd.uangMuka != "" && spd.uangMuka != "0" && spd.uangMuka != null
                             select new
                             {
                                 noSPD = spd.noSPD,
                                 nrp = spd.nrp,
                                 namaLengkap = spd.namaLengkap,
                                 uangMuka = spd.uangMuka,
                                 tglPenyelesaian = spd.tglPenyelesaian,
                                 status = spd.statusUM
                             }).ToList();

                if (a2 == "No")
                {
                    string paramValue2 = txtcari2.Text;
                    var datas2 = (from c in ctx.trSPDs
                                  where
                                    SqlMethods.Like(c.noSPD, "%" + paramValue2 + "%") &&
                                 c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                  select new
                                  {
                                      noSPD = c.noSPD,
                                      nrp = c.nrp,
                                      namaLengkap = c.namaLengkap,
                                      uangMuka = c.uangMuka,
                                      tglPenyelesaian = c.tglPenyelesaian,
                                      status = c.statusUM
                                  }).ToList();
                    gvGA.DataSource = datas2;
                    gvGA.DataBind();
                    Session["dtbl2"] = "No";
                }
                else if (a2 == "Nama")
                {
                    string paramValue2 = txtcari2.Text;
                    var datas2 = (from c in ctx.trSPDs
                                    where
                                        SqlMethods.Like(c.namaLengkap, "%" + paramValue2 + "%") &&
                                        c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                  select new
                                  {
                                      noSPD = c.noSPD,
                                      nrp = c.nrp,
                                      namaLengkap = c.namaLengkap,
                                      uangMuka = c.uangMuka,
                                      tglPenyelesaian = c.tglPenyelesaian,
                                      status = c.statusUM
                                  }).ToList();
                    gvGA.DataSource = datas2;
                    gvGA.DataBind();
                    Session["dtbl2"] = "Nama";
                }

                else if (a2 == "TglPenyelesaian")
                {
                    if (txtTglBerangkatAwal2.Text != string.Empty && txtTglBerangkatAkhir2.Text != string.Empty)
                    {
                        string paramValueawal2 = txtTglBerangkatAwal2.Text;
                        var Dateawal2 = Convert.ToDateTime(paramValueawal2);
                        string paramValueakhir2 = txtTglBerangkatAkhir2.Text;
                        var Dateakhir2 = Convert.ToDateTime(paramValueakhir2);
                        data2 = data2.Where(o => o.tglPenyelesaian >= Convert.ToDateTime(txtTglBerangkatAwal2.Text) && o.tglPenyelesaian <= Convert.ToDateTime(txtTglBerangkatAkhir2.Text)).ToList();
                        gvGA.DataSource = data2;
                        gvGA.DataBind();
                        Session["dtbl2"] = "TglPenyelesaian";
                    }
                    else
                    {
                        string paramValue2 = txtcari2.Text;
                        data2 = data2.Where(o => o.namaLengkap.Contains(paramValue2)).ToList();
                        gvGA.DataSource = data2;
                        gvGA.DataBind();
                        Session["dtbl2"] = "Nama";
                    }
                }
                else if (a2 == "Status")
                {

                    string paramValue2 = ddlStatusGA.SelectedItem.Text;
                    data2 = data2.Where(o => o.status == paramValue2).ToList();
                    gvGA.DataSource = data2;
                    gvGA.DataBind();
                    Session["dtbl2"] = "Status";
                }
                else
                {
                    gvGA.DataSource = data2;
                    gvGA.DataBind();
                }
            }
        }
        protected void gvGA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGA.PageIndex = e.NewPageIndex;
            BindGA();
        }
        void BindGA()
        {
            using (var ctx = new dsSPDDataContext())
            {
                var data = (from spd in ctx.trSPDs
                            where spd.uangMuka != "" && spd.uangMuka != "0" && spd.uangMuka != null
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                uangMuka = spd.uangMuka,
                                tglPenyelesaian = spd.tglPenyelesaian,
                                status = spd.statusUM
                            }).ToList();

                if (!data.Any())
                {
                    ClaimGA.Visible = false;
                }
                else
                {
                    ClaimGA.Visible = true;
                    gvGA.DataSource = data;
                    gvGA.DataBind();
                }
            }

        }
        protected void btnListFind_Click(object sender, EventArgs e)
        {
            BindGA();
        }
        protected void btnExportGA_Click(object sender, EventArgs e)
        {

            using (var ctx = new dsSPDDataContext())
            {
                gvGA.AllowPaging = false;


                string paramField2 = DropDownList2.Text.Trim() == "--Select--" ? "" : DropDownList2.Text.Trim();
                string a2 = DropDownList2.SelectedValue;

                var data2 = (from spd in ctx.trSPDs
                             where spd.uangMuka != "" && spd.uangMuka != "0" && spd.uangMuka != null
                             select new
                             {
                                 noSPD = spd.noSPD,
                                 nrp = spd.nrp,
                                 namaLengkap = spd.namaLengkap,
                                 uangMuka = spd.uangMuka,
                                 tglPenyelesaian = spd.tglPenyelesaian,
                                 status = spd.statusUM
                             }).ToList();

                if (a2 == "No")
                {
                    string paramValue2 = txtcari2.Text;
                    var datas2 = (from c in ctx.trSPDs
                                  where
                                    SqlMethods.Like(c.noSPD, "%" + paramValue2 + "%") &&
                                  c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                  select new
                                  {
                                      noSPD = c.noSPD,
                                      nrp = c.nrp,
                                      namaLengkap = c.namaLengkap,
                                      uangMuka = c.uangMuka,
                                      tglPenyelesaian = c.tglPenyelesaian,
                                      status = c.statusUM
                                  }).ToList();
                    gvGA.DataSource = datas2;
                    gvGA.DataBind();
                    Session["dtbl2"] = "No";
                }
                else if (a2 == "Nama")
                {
                    string paramValue2 = txtcari2.Text;
                    var datas2 = (from c in ctx.trSPDs
                                  where
                                    SqlMethods.Like(c.namaLengkap, "%" + paramValue2 + "%") &&
                                  c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                  select new
                                  {
                                      noSPD = c.noSPD,
                                      nrp = c.nrp,
                                      namaLengkap = c.namaLengkap,
                                      uangMuka = c.uangMuka,
                                      tglPenyelesaian = c.tglPenyelesaian,
                                      status = c.statusUM
                                  }).ToList();
                    gvGA.DataSource = datas2;
                    gvGA.DataBind();
                    Session["dtbl2"] = "Nama";
                }

                else if (a2 == "TglPenyelesaian")
                {
                    if (txtTglBerangkatAwal2.Text != string.Empty && txtTglBerangkatAkhir2.Text != string.Empty)
                    {
                        string paramValueawal2 = txtTglBerangkatAwal2.Text;
                        var Dateawal2 = Convert.ToDateTime(paramValueawal2);
                        string paramValueakhir2 = txtTglBerangkatAkhir2.Text;
                        var Dateakhir2 = Convert.ToDateTime(paramValueakhir2);
                        data2 = data2.Where(o => o.tglPenyelesaian >= Convert.ToDateTime(txtTglBerangkatAwal2.Text) && o.tglPenyelesaian <= Convert.ToDateTime(txtTglBerangkatAkhir2.Text)).ToList();
                        gvGA.DataSource = data2;
                        gvGA.DataBind();
                        Session["dtbl2"] = "TglPenyelesaian";
                    }
                    else
                    {
                        string paramValue2 = txtcari2.Text;
                        data2 = data2.Where(o => o.namaLengkap.Contains(paramValue2)).ToList();
                        gvGA.DataSource = data2;
                        gvGA.DataBind();
                        Session["dtbl2"] = "Nama";
                    }
                }
                else if (a2 == "Status")
                {

                    string paramValue2 = ddlStatusGA.SelectedValue;
                    data2 = data2.Where(o => o.status == paramValue2).ToList();
                    gvGA.DataSource = data2;
                    gvGA.DataBind();
                    Session["dtbl2"] = "Status";
                }
                else
                {
                    gvGA.DataSource = data2;
                    gvGA.DataBind();
                }

            }

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=UangMuka.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

            gvGA.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }



        protected void btnExportFinance_Click(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                gvFinance.AllowPaging = false;

                string paramField = DropDownList1.Text.Trim() == "--Select--" ? "" : DropDownList1.Text.Trim();
                string a = DropDownList1.SelectedValue;

                var data = (from spd in ctx.trSPDs
                            where spd.uangMuka != "" && spd.uangMuka != "0" && spd.uangMuka != null
                            select new
                            {
                                noSPD = spd.noSPD,
                                nrp = spd.nrp,
                                namaLengkap = spd.namaLengkap,
                                uangMuka = spd.uangMuka,
                                tglPenyelesaian = spd.tglPenyelesaian,
                                status = spd.statusUM
                            }).ToList();

                if (a == "No")
                {
                    string paramValue = txtcari.Text;
                    var datas = (from c in ctx.trSPDs
                                  where
                                    SqlMethods.Like(c.noSPD, "%" + paramValue + "%") &&
                                     c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                 select new
                                 {
                                     noSPD = c.noSPD,
                                     nrp = c.nrp,
                                     namaLengkap = c.namaLengkap,
                                     uangMuka = c.uangMuka,
                                     tglPenyelesaian = c.tglPenyelesaian,
                                     status = c.statusUM
                                 }).ToList();
                    gvFinance.DataSource = datas;
                    gvFinance.DataBind();
                    Session["dtbl"] = "No";
                }
                else if (a == "Nama")
                {
                    string paramValue = txtcari.Text;
                    var datas = (from c in ctx.trSPDs
                                 where
                                    SqlMethods.Like(c.namaLengkap, "%" + paramValue + "%") &&
                                   c.uangMuka != "" && c.uangMuka != "0" && c.uangMuka != null
                                 select new
                                 {
                                     noSPD = c.noSPD,
                                     nrp = c.nrp,
                                     namaLengkap = c.namaLengkap,
                                     uangMuka = c.uangMuka,
                                     tglPenyelesaian = c.tglPenyelesaian,
                                     status = c.statusUM
                                 }).ToList();
                    gvFinance.DataSource = datas;
                    gvFinance.DataBind();
                    Session["dtbl"] = "Nama";
                }

                else if (a == "TglPenyelesaian")
                {
                    if (txtTglBerangkatAwal.Text != string.Empty && txtTglBerangkatAkhir.Text != string.Empty)
                    {
                        string paramValueawal = txtTglBerangkatAwal.Text;
                        var Dateawal = Convert.ToDateTime(paramValueawal);
                        string paramValueakhir = txtTglBerangkatAkhir.Text;
                        var Dateakhir = Convert.ToDateTime(paramValueakhir);
                        data = data.Where(o => o.tglPenyelesaian >= Convert.ToDateTime(txtTglBerangkatAwal.Text) && o.tglPenyelesaian <= Convert.ToDateTime(txtTglBerangkatAkhir.Text)).ToList();
                        gvFinance.DataSource = data;
                        gvFinance.DataBind();
                        Session["dtbl"] = "TglPenyelesaian";
                    }
                    else
                    {
                        string paramValue = txtcari.Text;
                        data = data.Where(o => o.namaLengkap.Contains(paramValue)).ToList();
                        gvFinance.DataSource = data;
                        gvFinance.DataBind();
                        Session["dtbl"] = "Nama";
                    }
                }
                else if (a == "Status")
                {

                    string paramValue = ddlStatusFinance.SelectedValue;
                    data = data.Where(o => o.status == paramValue).ToList();
                    gvFinance.DataSource = data;
                    gvFinance.DataBind();
                    Session["dtbl2"] = "Status";
                }
                else
                {
                    gvFinance.DataSource = data;
                    gvFinance.DataBind();
                }

            }

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=UangMuka.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

            gvFinance.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            BindFinance();
            finance.Visible = true;
            DropDownList1.SelectedValue = "--Select--";
            Session["dtbl"] = null;
            txtcari.Text = string.Empty;
            txtcari.Visible = false;
            txtTglBerangkatAwal.Visible = false;
            txtTglBerangkatAkhir.Visible = false;
        }

        protected void btnResetGA_Click(object sender, EventArgs e)
        {
            BindGA();
            finance.Visible = false;
            gvGA.Visible = true;
            DropDownList2.SelectedValue = "--Select--";
            Session["dtbl2"] = null;
            txtcari2.Text = string.Empty;
            txtcari2.Visible = false;
            txtTglBerangkatAwal2.Visible = false;
            txtTglBerangkatAkhir2.Visible = false;
        }
    }
}