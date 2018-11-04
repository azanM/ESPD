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
    public partial class frmFinanceApp : System.Web.UI.Page
    {
        private string strStatusSPD = string.Empty;
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
                //Session["noSPD"] = string.Empty;
                //Session["editable"] = false;
                AD ad = new AD();
                //ArrayList al = ad.Groups("trac\\"+LoginID,true);
                ArrayList o = ad.Groups();

                string strLoginID = string.Empty;
                if (Session["IDLogin"] != null)
                {
                    strLoginID = (string)Session["IDLogin"];
                }
                else
                {
                    strLoginID = SetLabelWelcome();
                }

                if (strLoginID.ToLower() == "wawan010193")
                {
                    //strLoginID = "spd";
                    // strLoginID = "Putu005001";

                    // strLoginID = "arum00003359";
                    //!!!!!! debug Only";
                }
                Session["IDLogin"] = strLoginID;
                classSpd oSPD = new classSpd();
                karyawan = oSPD.getKaryawan(strLoginID);              
                btnListFind_Click(null, null);
               
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

        protected void btnListFind_Click(object sender, EventArgs e)
        {
            bindFindList();
        }

        void bindFindList()
        {
            dsSPDDataContext data = new dsSPDDataContext();
            try
            {
                if (txtTglSpd.Text == "" || txtTglSpd == null)
                {
                    var query = (from spd in data.trSPDs
                                 join claim in data.trClaims on spd.noSPD equals claim.noSPD
                                 where spd.nrp == karyawan.nrp
                                    && claim.isApprovedFinance == true
                                 select new
                                 {
                                     noSPD = spd.noSPD,
                                     nrp = spd.nrp,
                                     namaLengkap = spd.namaLengkap,
                                     cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                     keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                     tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                     tglKembali = spd.tglKembali.ToShortDateString(),
                                     status = claim.status //spd.status
                                 }).OrderByDescending(spd => spd.noSPD).ToList();
                    grvList.DataSource = query;
                    grvList.DataBind();
                    if (!query.Any())
                    {
                        Panel5.Visible = false;

                    }
                    else
                        Panel5.Visible = true;
                    data.Dispose();
                }
                else
                {
                    DateTime str = Convert.ToDateTime(txtTglSpd.Text);
                    var query = (from spd in data.trSPDs
                                 join claim in data.trClaims on spd.noSPD equals claim.noSPD
                                 where spd.nrp == karyawan.nrp
                                    && claim.isApprovedFinance == true
                                    && spd.tglBerangkat == str
                                 select new
                                 {
                                     noSPD = spd.noSPD,
                                     nrp = spd.nrp,
                                     namaLengkap = spd.namaLengkap,
                                     cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                                     keperluan = spd.idKeperluan != null ? spd.msKeperluan.keperluan : spd.keperluanLain,
                                     tglBerangkat = spd.tglBerangkat.ToShortDateString(),
                                     tglKembali = spd.tglKembali.ToShortDateString(),
                                     status = claim.status //spd.status
                                 }).OrderByDescending(spd => spd.noSPD).ToList();
                    grvList.DataSource = query;
                    grvList.DataBind();
                    if (!query.Any())
                    {
                        Panel5.Visible = false;

                    }
                    else
                        Panel5.Visible = true;
                    data.Dispose();
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);

            }
        }

        protected void lbListRevisi_Click(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }
            else
            {
                LinkButton link = (LinkButton)sender;
                GridViewRow gv = (GridViewRow)(link.NamingContainer);
                string strNoSpd = gv.Cells[0].Text;
                string status = gv.Cells[7].Text; //.Split('-')[0];
                Session["noSPD"] = strNoSpd;
                if (status.ToLower() == "save" || status == "SPD Perlu Revisi (Tujuan)" || status == "SPD Perlu Revisi (Atasan)")
                    Session["editable"] = true;
                else
                    Session["editable"] = false;
                Response.Redirect("frmRequestInput.aspx");
            }
        }
    }
}