using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using eSPD.Core;

namespace eSPD
{
    public partial class frmReportClaimSPD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                pnlView.Visible = false;
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            if (tbNoSPD.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", string.Format("alert('Isi no SPD');"), true);
                return;
            }           

            //this.CrystalReportSource1.Report.FileName = "~/UserControls/Reports/DriverAbsence.rpt";

            //validasi spd punya user login
            string strLoginID = "";
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }
            else
            {
                strLoginID = (string)Session["IDLogin"];
            }
            dsSPDDataContext dsp = new dsSPDDataContext();
            classSpd oSPD = new classSpd();
            msKaryawan karyawan = oSPD.getKaryawan(strLoginID);

            var trSpd = (from k in dsp.trSPDs
                         where k.nrp == karyawan.nrp
                         select k).First();
            if (trSpd == null)
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alertAkses", string.Format("alert('Akses di tolak');"), true);
            else
            {
                this.CrystalReportSource1.ReportDocument.SetParameterValue("noSPDClaim", tbNoSPD.Text);

                LogonReport(this.CrystalReportSource1.ReportDocument);
                pnlView.Visible = true;
            }
        }
        public static void LogonReport(CrystalDecisions.CrystalReports.Engine.ReportDocument reportdocument)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder con = new System.Data.SqlClient.SqlConnectionStringBuilder();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["SPDDevConnectionString"].ConnectionString;

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