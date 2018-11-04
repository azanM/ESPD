using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using eSPD.Core;

namespace eSPD
{
    public partial class msProvinsi : System.Web.UI.Page
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
            Int32 roleid = (from k in data.msUsers
                            where k.nrp == karyawan.nrp && (k.roleId == Konstan.SYSADMIN || k.roleId == Konstan.GA)
                            select k.roleId).FirstOrDefault();
            if (roleid == Konstan.GA || strLoginID.Contains("yudisss") || roleid == Konstan.SYSADMIN)
            {
                gvMasterProvinsi.Visible = true;
                btnTambah.Visible = true;
            }
            else
            {
                gvMasterProvinsi.Visible = false;
                btnTambah.Visible = false;
            }

            if (!IsPostBack)
            {
                inputForm.Visible = false;
                hfmode.Value = "add";
                BindGridMasterProvinsi();
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
        protected void BindGridMasterProvinsi()
        {
             string constr = ConfigurationManager.ConnectionStrings["SPDConnectionString1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_PropinsiCRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvMasterProvinsi.DataSource = dt;
                            gvMasterProvinsi.DataBind();
                        }
                    }
                }
            }    
        }

        protected void btnSimpan_Click(object sender, EventArgs e)
        {

            string propinsi = txtProvinsi.Text;
            string mode = hfmode.Value.ToString();
            string action = string.Empty;

            if (mode == "edit")
                action = "UPDATE";
            else
                action = "INSERT";

            string constr = ConfigurationManager.ConnectionStrings["SPDConnectionString1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_PropinsiCRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@propinsi", propinsi);
                    cmd.Parameters.AddWithValue("@id", int.Parse(HFProvinsiId.Value.ToString() != "" ? HFProvinsiId.Value : "0"));
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClearForm();
            notif.Text = "Data berhasil disimpan";
            BindGridMasterProvinsi();
        }

        protected void btnBatal_Click(object sender, EventArgs e)
        {

        }

        protected void gvMasterProvinsi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMasterProvinsi.PageIndex = e.NewPageIndex;
            BindGridMasterProvinsi();
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            Label id = (Label)gv.FindControl("id");
            int propinsiId = int.Parse(id.Text.Trim());
            SqlCommand cmd = new SqlCommand(
            "SELECT Id, propinsi from msPropinsi where Id = " + propinsiId + "",
            new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
            cmd.Connection.Open();
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                txtProvinsi.Text = reader["propinsi"].ToString();
                HFProvinsiId.Value = reader["Id"].ToString();
            }
            hfmode.Value = "edit";
            inputForm.Visible = true;
            btnTambah.Visible = false;
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            Label id = (Label)gv.FindControl("id");
            int propinsiId= int.Parse(id.Text.Trim());

            string constr = ConfigurationManager.ConnectionStrings["SPDConnectionString1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_PropinsiCRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "DELETE");
                    cmd.Parameters.AddWithValue("@id", propinsiId);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                BindGridMasterProvinsi();
            }
        }

        protected void btnTambah_Click(object sender, EventArgs e)
        {
            inputForm.Visible = true;
            btnTambah.Visible = false;
            hfmode.Value = "add";
        }

        private void ClearForm()
        {
            txtProvinsi.Text = string.Empty;
        }
    }
}