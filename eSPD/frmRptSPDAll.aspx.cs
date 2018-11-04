using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Microsoft.Reporting.WebForms;
using eSPD.Core;

namespace eSPD
{
    public partial class frmRptSPDAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            classSpd cspd = new classSpd();


            string strLoginID = string.Empty;
            classSpd oSPD = new classSpd();
            if (Session["IDLogin"] != null)
            {
                strLoginID = (string)Session["IDLogin"];
            }
            else
            {
                strLoginID = SetLabelWelcome();
            }
            msKaryawan karyawan = oSPD.getKaryawan(strLoginID);
            dsSPDDataContext data = new dsSPDDataContext();
            var roleid = (from k in data.msUsers
                          where k.nrp == karyawan.nrp
                          select k.roleId).ToList();
           
            if (roleid.Contains(Konstan.GA) || strLoginID.Contains("yudisss") || roleid.Contains(Konstan.SYSADMIN))
            {
                pnlFilter.Visible = true;
            }
            else
            {
                pnlFilter.Visible = false;
                pnlView.Visible = false;
            }
            if (!Page.IsPostBack)
            {
                dsSPDDataContext dsp = new dsSPDDataContext();
                //var query = (from k in dsp.msKaryawans where k.statusKaryawan.Trim() == "Active" select k);
                var query = (from k in dsp.msKaryawans orderby k.namaLengkap select k);

                checkBoxes1.DataTextField = "namaLengkap";
                checkBoxes1.DataValueField = "namaLengkap";
                checkBoxes1.DataSource = query.ToList();
                checkBoxes1.DataBind();
                checkBoxes1.Items.Add(new ListItem("Test", "Test"));
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            IList<ReportParameter> parameters = new List<ReportParameter>();

            for (int i = 0; i < checkBoxes1.Texts.SelectBoxCaption.Split(',').Count(); i++)
            {
                parameters.Add(new ReportParameter("NamaLengkap", checkBoxes1.Texts.SelectBoxCaption.Split(',')[i].Trim()));
            }
            //parameters.Add(new ReportParameter("NamaLengkap", "klk"));
            //parameters.Add(new ReportParameter("NamaLengkap", "IKA DHAMAYANTI"));
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportViewer1.LocalReport.Refresh();
        }

        protected void LinqClaim_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

        }

        protected void checkBoxes_SelcetedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder strText = new StringBuilder();
            StringBuilder strValue = new StringBuilder();
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    strText.Append(item.Text + ", ");
                    strValue.Append(item.Value + ", ");
                }
            }
            if (strText.Length > 2)
            {
                strText.Remove(strText.Length - 2, 2);
                strValue.Remove(strValue.Length - 2, 2);
                checkBoxes1.Texts.SelectBoxCaption = strText.ToString();
                //lbldaybox.Text = strValue.ToString();
            }
            else
            {
                checkBoxes1.Texts.SelectBoxCaption = "-Pilih Nama-";
            }
        }
    }
}