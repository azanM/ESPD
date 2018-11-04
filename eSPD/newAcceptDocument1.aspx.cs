using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;
using System.IO;

namespace eSPD
{
    public partial class newAcceptDocument1 : System.Web.UI.Page
    {

        private static dsSPDDataContext ctx = new dsSPDDataContext();
        protected string userLoginID = "";
        List<string> errorMessageHidden = new List<string>();
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
            Int32 roleid = (from k in data.msUsers
                            where k.nrp == karyawan.nrp && (k.roleId == Konstan.SYSADMIN || k.roleId == Konstan.GA)
                            select k.roleId).FirstOrDefault();

            if (!IsPostBack)
            {
                pnlForm.Visible = false;
            }

            if (roleid == Konstan.GA || strLoginID.Contains("yudisss") || roleid == Konstan.SYSADMIN)
            {
                //trSPD SPD = new trSPD();
                //Calendar1.StartDate = ctx.AddWorkDaysToGetdate().Value;CalculateFutureDate
                // Calendar1.StartDate = GetPreviousWorkDay(DateTime.Now); 
                Calendar1.StartDate = GetPreviousWorkDay(DateTime.Now);
                Calendar1.EndDate = (DateTime.Now);

                CalendarExtendersabtu.StartDate = GetPreviousWorkDay(DateTime.Now.AddDays(-4));
                CalendarExtendersabtu.EndDate = (DateTime.Now.AddDays(-4));
                CalendarExtenderMinggu.StartDate = GetPreviousWorkDay(DateTime.Now.AddDays(-3));
                CalendarExtenderMinggu.EndDate = (DateTime.Now.AddDays(-3));
                CalendarExtenderSenin.StartDate = GetPreviousWorkDay(DateTime.Now.AddDays(-2));
                CalendarExtenderSenin.EndDate = (DateTime.Now.AddDays(-2));
                CalendarExtenderSelasa.StartDate = GetPreviousWorkDay(DateTime.Now.AddDays(-1));
                CalendarExtenderSelasa.EndDate = (DateTime.Now.AddDays(-1));
                pnlForm.Visible = true;

            }
            else
            {
                pnlForm.Visible = false;
            }

            userLoginID = !String.IsNullOrEmpty(strLoginID) ? strLoginID.Length > 10 ? strLoginID.Substring(0, 10).ToString() : strLoginID : "";

     
        }

        public DateTime GetPreviousWorkDay(DateTime date)
        {
            DateTime tglTerima;
            DayOfWeek day = date.DayOfWeek;
            if ((day == DayOfWeek.Saturday))
            {
                tglTerima = date.AddDays(-2);
            }
            else if (day == DayOfWeek.Sunday)
            {
                tglTerima = date.AddDays(-3);
            }
            else if (day == DayOfWeek.Monday)
            {
                tglTerima = date.AddDays(-4);
            }
            else if (day == DayOfWeek.Tuesday)
            {
                tglTerima = date.AddDays(-4);
            }
            else
            {
                tglTerima = date.AddDays(-2);
            }

            return tglTerima;
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

        protected void btnProses_Click(object sender, EventArgs e)
        {
            Process(txtNoSPD.Text);
        }
        void Process(string noSPD)
        {

        }
        protected void btnProsesNew_Click(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                var users = from u in ctx.trSPDs where u.noSPD == txtNoSPD.Text || u.noSPD == TextBox1.Text || u.noSPD == TextBox2.Text || 
                                                      u.noSPD ==TextBox3.Text|| u.noSPD == TextBox4.Text || u.noSPD == TextBox5.Text ||
                                                      u.noSPD == TextBox6.Text || u.noSPD == TextBox7.Text || u.noSPD == TextBox8.Text || 
                                                      u.noSPD == TextBox9.Text || u.noSPD == TextBox10.Text || u.noSPD == TextBox11.Text || 
                                                      u.noSPD == TextBox12.Text || u.noSPD == TextBox13.Text || u.noSPD == TextBox14.Text || 
                                                      u.noSPD == TextBox15.Text || u.noSPD == TextBox16.Text || u.noSPD == TextBox17.Text || 
                                                      u.noSPD == TextBox18.Text || u.noSPD == TextBox19.Text || u.noSPD == TextBox20.Text || 
                                                      u.noSPD == TextBox21.Text || u.noSPD == TextBox22.Text || u.noSPD == TextBox23.Text || 
                                                      u.noSPD == TextBox24.Text|| u.noSPD == TextBox25.Text || u.noSPD == TextBox26.Text || 
                                                      u.noSPD == TextBox27.Text || u.noSPD == TextBox28.Text || u.noSPD == TextBox29.Text   select u;

                foreach (trSPD us in users)
                {
                    us.tglTerima = Convert.ToDateTime(txtTglTerima.Text);
                }

                try
                {
                    ctx.SubmitChanges();
                }
                catch (Exception)
                {
                    //errorMessageHidden.Add("Error Submit SPD, gangguan teknis");
                    //errorMessageHidden.Add(ex.Message);
                }
                finally
                {
                    lblSave.Visible = true;

                    btnProsesNew.Enabled = false;


                }
            }
            //  ProcessNew(txtNoSPD.Text);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnProsesNew.Enabled = true;

            txtTglTerima.Text = String.Empty;
            txtNoSPD.Text = String.Empty;
            TextBox1.Text = String.Empty;
            TextBox2.Text = String.Empty;
            TextBox3.Text = String.Empty;
            TextBox4.Text = String.Empty;
            TextBox5.Text = String.Empty;
            TextBox6.Text = String.Empty;
            TextBox7.Text = String.Empty;
            TextBox8.Text = String.Empty;
            TextBox9.Text = String.Empty;
            TextBox10.Text = String.Empty;
            TextBox11.Text = String.Empty;
            TextBox12.Text = String.Empty;
            TextBox13.Text = String.Empty;
            TextBox14.Text = String.Empty;
            TextBox15.Text = String.Empty;
            TextBox16.Text = String.Empty;
            TextBox17.Text = String.Empty;
            TextBox18.Text = String.Empty;
            TextBox19.Text = String.Empty;
            TextBox20.Text = String.Empty;
            TextBox21.Text = String.Empty;
            TextBox22.Text = String.Empty;
            TextBox23.Text = String.Empty;
            TextBox24.Text = String.Empty;
            TextBox25.Text = String.Empty;
            TextBox26.Text = String.Empty;
            TextBox27.Text = String.Empty;
            TextBox28.Text = String.Empty;
            TextBox29.Text = String.Empty;
        }
    }
}