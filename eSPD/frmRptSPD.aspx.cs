using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace eSPD
{
    public partial class frmRptSPD : System.Web.UI.Page
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
            this.CrystalReportSource1.ReportDocument.SetParameterValue("noSPD", tbNoSPD.Text);

            LogonReport(this.CrystalReportSource1.ReportDocument);
            pnlView.Visible = true;
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