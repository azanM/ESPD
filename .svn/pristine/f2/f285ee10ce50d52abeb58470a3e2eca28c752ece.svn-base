using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSPD
{
    public partial class newFormRequestDetail : System.Web.UI.Page
    {
        private static msKaryawan karyawan = new msKaryawan();
        private static classSpd oSPD = new classSpd();
        private static string strID = string.Empty;
        List<string> errorMessageHidden = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }

            string noSPD = Encrypto.Decrypt(Request.QueryString["encrypt"]);
            Id.Value = noSPD;

            strID = (string)Session["IDLogin"];
            karyawan = oSPD.getKaryawan(strID);
            Session["nrpLogin"] = karyawan.nrp;

            if (string.IsNullOrEmpty(karyawan.nrp)) Response.Redirect("frmHome.aspx");

            using (var ctx = new dsSPDDataContext())
            {
                var spd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == Id.Value);


                // jika sekertaris          

                if ((spd.dibuatOleh == karyawan.nrp && spd.isSubmit != true) || (isSec(karyawan.nrp, spd.nrp) && spd.isSubmit != true))
                {
                    btnSubmit.Visible = true;
                }
            }

        }

        private bool isSec(string p, string x)
        {
            bool returner = false;
            using (var ctx = new dsSPDDataContext())
            {
                try
                {
                    var datadirect = ctx.msUsers.FirstOrDefault(o => o.nrp == x).roleId;
                    var data = ctx.msUsers.FirstOrDefault(o => o.nrp == p).roleId;
                    if (data == 23 && (datadirect == 13 || datadirect == 14)) returner = true;
                }
                catch (Exception)
                {
                    returner = false;
                }
            }

            return returner;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                var spd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == Id.Value);
                var firstApproval = ctx.ApprovalStatus.FirstOrDefault(o => o.NoSPD == Id.Value && o.IndexLevel.Value == 1);

                if (spd == null)
                {
                    errorMessageHidden.Add("SPD tidak ditemukan");
                }

                if (firstApproval == null)
                {
                    errorMessageHidden.Add("Approval pertama tidak ditemukan");
                }

                if (errorMessageHidden.Count() == 0)
                {
                    spd.diubahOleh = karyawan.nrp;
                    spd.diubahTanggal = DateTime.Now;
                    spd.isSubmitDate = DateTime.Now;
                    spd.nrpAtasan = firstApproval.NrpApproval;
                    spd.status = "Menunggu approval " + firstApproval.ApprovalRule.Deskripsi;
                    spd.isSubmit = true;

                    try
                    {
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        errorMessageHidden.Add("Error Submit SPD, gangguan teknis");
                        errorMessageHidden.Add(ex.Message);
                    }
                    finally
                    {
                        pnlSuccess.Visible = true;
                        pnlError.Visible = false;
                        lblSuccess.Text = "SPD Berhasil di Submit, No SPD : " + spd.noSPD + ", status spd saat ini adalah " + spd.status;
                        btnSubmit.Enabled = false;

                        // send approval atasan
                        EmailCore.ApprovalSPD(firstApproval.NrpApproval, Id.Value, firstApproval.IndexLevel.Value.ToString(), spd);
                    }
                }

                if (errorMessageHidden.Count() > 0)
                {
                    errorMessage.DataSource = errorMessageHidden;
                    errorMessage.DataBind();

                    pnlError.Visible = true;
                    pnlSuccess.Visible = false;
                }
            }
        }

    }
}