using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;

using System.Globalization;
using System.Configuration;
namespace eSPD
{
    public partial class frmReportCrystalClaim : System.Web.UI.Page
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
            pnlView.Visible = false;
                        string strLoginID = SetLabelWelcome();

                        if (strLoginID.ToLower() == "wawan010193")
                        {
                            //!!!! debug Only
                            //strLoginID = "Putu005001";
                            //strLoginID = "rika009692";
                        }
            Session["IDLogin"] = strLoginID;
            classSpd oSPD = new classSpd();
            //untuk non karyawan
            Panel1.Visible = false;
            if (strLoginID.ToUpper() != "SPD")
            {
                karyawan = oSPD.getKaryawan(strLoginID);
                // msUser user = new msUser;
                dsSPDDataContext data = new dsSPDDataContext();
                var user = from r in data.msUsers
                           where r.nrp == karyawan.nrp
                           select r;
                foreach (msUser item in user)
                {
                    if (item.roleId == 17 || item.roleId == 19 || item.roleId == 20)
                    {
                        Panel1.Visible = true;
                    }
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
            DateTime? tglBerangkat = txtTglBerangkat.Text == "" ? null : convert(Convert.ToDateTime(txtTglBerangkat.Text));
            var query = from claim in data.trClaims.AsEnumerable()
                        join
                            spd in data.trSPDs.AsEnumerable()
                            on claim.noSPD equals spd.noSPD
                        join keperluan in data.msKeperluans.AsEnumerable()
                            on spd.idKeperluan equals keperluan.id
                        where spd.noSPD == txtNoSPD.Text || spd.namaLengkap.ToUpper() == txtNamaLengkap.Text.ToUpper() || spd.tglBerangkat == tglBerangkat
                        select new
                        {
                            noSPD = spd.noSPD,
                            nrp = spd.nrp,
                            namaLengkap = spd.namaLengkap,
                            cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                            keperluan = keperluan.keperluan,
                            tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                            tglKembali = spd.tglKembali.ToShortDateString(),
                            status = spd.status.Split('-')[1]
                        };
            gvViewSPD.DataSource = query;
            gvViewSPD.DataBind();
        }
        protected void lbViewSPD_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            param(strNoSpd);
            LogonReport(this.CrystalReportSource1.ReportDocument);
            pnlView.Visible = true;
        }
        protected void lbDetail_Click(object sender, EventArgs e)
        {
            Session["editable"] = false;
            DetailClick(sender);
        }

        private void DetailClick(object sender)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            string strNoSpd = gv.Cells[0].Text;
            Session["noSPD"] = strNoSpd;
            param(strNoSpd);
            LogonReport(this.CrystalReportSource1.ReportDocument);
            pnlView.Visible = true;
        }
        private DateTime? convert(DateTime? date)
        {
            Convert.ToDateTime(date);

            return date;
        }
        private void param(string strNoSpd)
        {
            this.CrystalReportSource1.ReportDocument.SetParameterValue("noSPD", strNoSpd);
        }
        public static void LogonReport(CrystalDecisions.CrystalReports.Engine.ReportDocument reportdocument)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder con = new System.Data.SqlClient.SqlConnectionStringBuilder();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["SPDConnectionString1"].ConnectionString;

            CrystalDecisions.Shared.ConnectionInfo consinfo = new CrystalDecisions.Shared.ConnectionInfo();
            consinfo.ServerName = con.DataSource;
            consinfo.UserID = con.UserID;
            consinfo.DatabaseName = con.InitialCatalog;
            consinfo.Password = con.Password;
            consinfo.Type = CrystalDecisions.Shared.ConnectionInfoType.SQL;

            CrystalDecisions.CrystalReports.Engine.Tables myTables = reportdocument.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table myTable in myTables)
            {
                CrystalDecisions.Shared.TableLogOnInfo myTableLogonInfo = myTable.LogOnInfo;
                myTableLogonInfo.ConnectionInfo = consinfo;
                myTable.ApplyLogOnInfo(myTableLogonInfo);
            }

        }
    }
}