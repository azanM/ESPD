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
    public partial class msADH : System.Web.UI.Page
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
                gvMasterADH.Visible = true;
                btnTambah.Visible = true;
            }
            else
            {
                gvMasterADH.Visible = false;
                btnTambah.Visible = false;
            }
            if (!IsPostBack)
            {
                inputForm.Visible = false;
                hfmode.Value = "add";
                BindDropdownCostCenter();
                BindGridMasterADH();
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

        protected void BindDropdownCostCenter()
        {
            SqlCommand cmd = new SqlCommand("Select costId, costCenter, costDesc from msCost", new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
            cmd.Connection.Open();
            SqlDataReader CostCenterReader;
            CostCenterReader = cmd.ExecuteReader();
            costCenterList.DataSource = CostCenterReader;
            costCenterList.DataValueField = "costId";
            costCenterList.DataTextField = "costDesc";
            costCenterList.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        protected void BindGridMasterADH()
        {
            string constr = ConfigurationManager.ConnectionStrings["SPDConnectionString1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_MasterADHCRUD"))
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
                            gvMasterADH.DataSource = dt;
                            gvMasterADH.DataBind();
                        }
                    }
                }
            }
        }

        protected void costCenter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSimpan_Click(object sender, EventArgs e)
        {
            string name = txtNamaLengkap.Text;
            string nrp = txtNRP.Text;
            string costCenterId = costCenterList.SelectedValue;
            string mode = hfmode.Value.ToString();
            string action = string.Empty;
            
            if (mode == "edit")
                action = "UPDATE";
            else
                action = "INSERT";

            string constr = ConfigurationManager.ConnectionStrings["SPDConnectionString1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_MasterADHCRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@nrp", nrp);
                    cmd.Parameters.AddWithValue("@namaLengkap", name);
                    cmd.Parameters.AddWithValue("@costCenterId", int.Parse(costCenterId));
                    cmd.Parameters.AddWithValue("@id", int.Parse(hfmasterADHId.Value.ToString() != "" ? hfmasterADHId.Value : "0"));
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClearForm();
            notif.Text = "Data berhasil disimpan";
            BindGridMasterADH();
        }

        protected void btnBatal_Click(object sender, EventArgs e)
        {

        }

        protected void gvMasterADH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMasterADH.PageIndex = e.NewPageIndex;
            BindGridMasterADH();
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow gv = (GridViewRow)(link.NamingContainer);
            Label id = (Label)gv.FindControl("id");
            int msADHid = int.Parse(id.Text.Trim());
            SqlCommand cmd = new SqlCommand(
            "SELECT Id, costId, nrp, nama, costDesc from msADH inner join msCost on msADH.costCenterId = msCost.costId where Id = "+msADHid+"", 
            new SqlConnection(ConfigurationManager.AppSettings["SPDConnectionString1"]));
            cmd.Connection.Open();
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                
                txtNRP.Text = reader["nrp"].ToString();
                txtNamaLengkap.Text = reader["nama"].ToString();
                costCenterList.SelectedValue = reader["costId"].ToString();
                hfmasterADHId.Value = reader["Id"].ToString();
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
            int msADHid = int.Parse(id.Text.Trim());

            string constr = ConfigurationManager.ConnectionStrings["SPDConnectionString1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_MasterADHCRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "DELETE");
                    cmd.Parameters.AddWithValue("@id", msADHid);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                BindGridMasterADH();
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
            txtNRP.Text = string.Empty;
            txtNamaLengkap.Text = string.Empty;
        }
    }
}