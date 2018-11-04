using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eSPD.Core;
namespace eSPD
{
    public partial class frmReportSPD : System.Web.UI.Page
    {
        //msKaryawan karyawan = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            DateTime? tglBerangkat = txtTglBerangkat.Text == "" ? null : convert(Convert.ToDateTime(txtTglBerangkat.Text));
            var query = from spd in data.trSPDs.AsEnumerable()
                            join keperluan in data.msKeperluans.AsEnumerable()
                                on spd.idKeperluan equals keperluan.id
                        where spd.noSPD == txtNoSPD.Text || spd.namaLengkap==txtNamaLengkap.Text || spd.tglBerangkat == tglBerangkat
                        select new
                        {
                            noSPD = spd.noSPD,
                            nrp = spd.nrp,
                            namaLengkap = spd.namaLengkap,
                            cabangTujuan = spd.companyCodeTujuan != null ? spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan : spd.tempatTujuanLain,
                            keperluan= keperluan.keperluan,
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
            link.Attributes.Add("onclick", "popReport('" + strNoSpd + "')");
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
            Response.Redirect("frmClaimInput.aspx");
        }
        private DateTime? convert(DateTime? date)
        {
            Convert.ToDateTime(date);

            return date;
        }

        protected void gvViewSPD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbViewSPD = (LinkButton)e.Row.FindControl("lbViewSPD");
                GridViewRow gv = (GridViewRow)(lbViewSPD.NamingContainer);
                string strStatus = gv.Cells[7].Text;

               // Label lblRole = (Label)e.Row.FindControl("lblRole");

               // strStatus.Split(
                if (strStatus == "Menunggu Approval Atasan" || strStatus == "Save")
                {
                    lbViewSPD.Visible = false;
                }
                else
                {
                    lbViewSPD.Visible = true;
                }
            }

        }

    }
}