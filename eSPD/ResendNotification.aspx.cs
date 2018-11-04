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
    public partial class ResendNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDLogin"] == null)
            {
                Response.Redirect("frmHome.aspx");
            }
        }

        protected void btnProses_Click(object sender, EventArgs e)
        {
            #region old
            //classSpd oSpd = new classSpd();
            //using (dsSPDDataContext data = new dsSPDDataContext())
            //{
            //    if (txtNoSPD.Text != string.Empty)
            //    {
            //        var qSpd = (from spd in data.trSPDs
            //                    where spd.noSPD == txtNoSPD.Text
            //                    select spd).FirstOrDefault();
            //        if (qSpd != null)
            //        {
            //            var qKary = (from kar in data.msKaryawans
            //                         where kar.nrp == qSpd.nrp
            //                         select kar).FirstOrDefault();
            //            string[] status = qSpd.status.Split('-');
            //            switch (status[0])
            //            {
            //                case "2":
            //                    oSpd.sendMail(qSpd, "Atasan", qKary);
            //                    break;
            //                case "3":
            //                    oSpd.sendMail(qSpd, "GA", qKary);
            //                    oSpd.sendMail(qSpd, "Tujuan", qKary);
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "6":
            //                    oSpd.sendMail(qSpd, "GA", qKary);
            //                    oSpd.sendMail(qSpd, "Tujuan", qKary);
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "7":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "8":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "9":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "12":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "13":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "11":
            //                    oSpd.sendMail(qSpd, "Atasan", qKary);
            //                    break;
            //                case "14":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "15":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "16":
            //                    oSpd.sendMail(qSpd, "GA", qKary);
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "17":
            //                    oSpd.sendMail(qSpd, "finance", qKary);
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "18":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "19":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "20":
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;
            //                case "26":
            //                    //oSpd.sendMail(qSpd, "kasir", qKary);
            //                    oSpd.sendMail(qSpd, "Pembuat", qKary);
            //                    break;

            //                default:
            //                    break;
            //            }
            //        }

            //    }
            //}
            #endregion

            Process(txtNoSPD.Text);
        }
        void Process(string noSPD)
        {
            //using (var ctx = new dsSPDDataContext())
            //{
            //    var dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == noSPD);
            //    var approval = ctx.ApprovalStatus.Where(o => o.NoSPD == noSPD).OrderBy(o => o.IndexLevel);

            //    if (dataSpd != null && approval != null)
            //    {
            //        switch (dataSpd.isApproved)
            //        {
            //            case null:
            //                if (approval.Where(o => o.Status == null).Count() == 0)
            //                {
            //                    // proses approval tujuan

            //                    EmailCore.sendEmail(
            //                                    dataSpd.nrpApprovalTujuan,
            //                                    "Approval " + dataSpd.noSPD,
            //                                    "ApprovalSPDAtasanTujuanTemplate.txt",
            //                                    dataSpd,
            //                                    string.Empty,
            //                                    string.Empty,
            //                                    string.Empty,
            //                                    "ApprovalUrl"
            //                                    );

            //                }
            //                else
            //                {
            //                    // proses next approval
            //                    var currentApproval = approval.FirstOrDefault(o => o.Status == null);

            //                    EmailCore.sendEmail(
            //                                         currentApproval.NrpApproval,
            //                                         "Approval " + dataSpd.noSPD,
            //                                         "ApprovalSPDAtasanLangsungTemplate.txt",
            //                                         dataSpd,
            //                                         string.Empty,
            //                                         string.Empty,
            //                                         string.Empty,
            //                                         "ApprovalUrl"
            //                                         );

            //                }

            //                break;

            //            case true:

            //                var dataClaim = ctx.trClaims.FirstOrDefault(o => o.noSPD == noSPD);

            //                if (dataClaim != null)
            //                {
            //                    if (dataClaim.isSubmit == true)
            //                    {
            //                        if (dataClaim.isApprovedAtasan == null && dataClaim.isApprovedFinance == null && dataClaim.isApprovedGA == null)
            //                        {
            //                            //send approval atasan

            //                            EmailCore.sendEmail(
            //                                     dataClaim.nrpAtasan,
            //                                     "Approval claim " + dataClaim.noSPD,
            //                                     "ApprovalClaimAtasanLangsungTemplate.txt",
            //                                     dataSpd,
            //                                     dataClaim.total.ToString(),
            //                                     "Atasan",
            //                                      dataClaim.status,
            //                                     "ClaimApprovalUrl"
            //                                     );
            //                        }

            //                        if (dataClaim.isApprovedAtasan == true && dataClaim.isApprovedFinance == null && dataClaim.isApprovedGA == null)
            //                        {
            //                            //send approval GA

            //                            var role = (from p in ctx.msUsers
            //                                        where p.roleId == 17
            //                                        select p.nrp).ToList();

            //                            var dataGa = ctx.msKaryawans
            //                                .Where(o =>
            //                                    o.coCd == "0001" &&
            //                                    role.Contains(o.nrp));

            //                            foreach (var item in dataGa)
            //                            {
            //                                EmailCore.sendEmail(
            //                                          item.nrp,
            //                                          "Approval claim " + dataClaim.noSPD,
            //                                          "ApprovalClaimGAOrFinanceTemplate.txt",
            //                                          dataSpd,
            //                                          dataClaim.total.ToString(),
            //                                          "ga",
            //                                          dataClaim.status,
            //                                          "ClaimApprovalUrl"
            //                                          );
            //                            }
            //                        }

            //                        if (dataClaim.isApprovedAtasan == true && dataClaim.isApprovedFinance == true && dataClaim.isApprovedGA == null)
            //                        {
            //                            //send approval Finance

            //                            var creator = ctx.msKaryawans.FirstOrDefault(o => o.nrp == dataSpd.nrp);

            //                            var role2 = (from p in ctx.msUsers
            //                                         where p.roleId == 19
            //                                         select p.nrp).ToList();

            //                            var datafinace = ctx.msKaryawans
            //                                .Where(o =>
            //                                    o.coCd == creator.coCd &&
            //                                    role2.Contains(o.nrp));

            //                            foreach (var item in datafinace)
            //                            {
            //                                EmailCore.sendEmail(
            //                                     item.nrp,
            //                                     "Approval claim " + dataClaim.noSPD,
            //                                     "ApprovalClaimGAOrFinanceTemplate.txt",
            //                                     dataSpd,
            //                                     dataClaim.total.ToString(),
            //                                     "finance",
            //                                     dataClaim.status,
            //                                     "ClaimApprovalUrl"
            //                                     );
            //                            }
            //                        }
            //                    }
            //                }

            //                // claim
            //                break;
            //            default:
            //                break;
            //        }


            //    }
            //}
        }
        protected void btnProsesNew_Click(object sender, EventArgs e)
        {
            ProcessNew(txtNoSPD.Text);
        }
        void ProcessNew(string noSPD)
        {
            noSPD = noSPD.Trim();
            using (var ctx = new dsSPDDataContext())
            {
                var dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == noSPD);
                var approval = ctx.ApprovalStatus.Where(o => o.NoSPD == noSPD).OrderBy(o => o.IndexLevel);

                if (dataSpd != null && approval != null)
                {
                    switch (dataSpd.isApproved)
                    {
                        case null:
                            if (approval.Where(o => o.Status == null).Count() == 0)
                            {
                                // proses approval tujuan
                                EmailCore.ApprovalSPD(dataSpd.nrpApprovalTujuan, dataSpd.noSPD, string.Empty, dataSpd);

                            }
                            else
                            {
                                // proses next approval
                                var currentApproval = approval.FirstOrDefault(o => o.Status == null);
                                EmailCore.ApprovalSPD(currentApproval.NrpApproval, dataSpd.noSPD, currentApproval.IndexLevel.ToString(), dataSpd);
                            }

                            break;

                        case true:

                            var dataClaim = ctx.trClaims.FirstOrDefault(o => o.noSPD == noSPD);

                            if (dataClaim != null)
                            {
                                if (dataClaim.isSubmit == true)
                                {
                                    if (dataClaim.isApprovedAtasan == null && dataClaim.isApprovedFinance == null && dataClaim.isApprovedGA == null)
                                    {
                                        //send approval atasan
                                        EmailCore.ApprovalClaim(dataClaim.nrpAtasan, dataClaim.noSPD, dataSpd, "Atasan", dataClaim.status);
                                    }

                                    if (dataClaim.isApprovedAtasan == true && dataClaim.isApprovedFinance == null && dataClaim.isApprovedGA == null)
                                    {
                                        //send approval GA

                                        var role = (from p in ctx.msUsers
                                                    where p.roleId == 17
                                                    select p.nrp).ToList();

                                        var dataGa = ctx.msKaryawans
                                            .Where(o =>
                                                o.coCd == "0001" &&
                                                role.Contains(o.nrp));

                                        foreach (var item in dataGa)
                                        {
                                            EmailCore.ApprovalClaim(item.nrp, dataClaim.noSPD, dataSpd, "GA", dataClaim.status);
                                        }
                                    }

                                    if (dataClaim.isApprovedAtasan == true && dataClaim.isApprovedFinance == null && dataClaim.isApprovedGA == true)
                                    {
                                        //send approval Finance

                                        var creator = ctx.msKaryawans.FirstOrDefault(o => o.nrp == dataSpd.nrp);

                                        var role2 = (from p in ctx.msUsers
                                                     where p.roleId == 19
                                                     select p.nrp).ToList();

                                        var datafinace = ctx.msKaryawans
                                            .Where(o =>
                                                o.coCd == creator.coCd &&
                                                role2.Contains(o.nrp));

                                        foreach (var item in datafinace)
                                        {
                                            EmailCore.ApprovalClaim(item.nrp, dataClaim.noSPD, dataSpd, "finance", dataClaim.status);
                                        }
                                    }
                                }
                            }

                            // claim
                            break;
                        default:
                            break;
                    }


                }
            }
        }
        protected void btnResendAll_Click(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                var query = (from o in ctx.trSPDs
                             where o.isSubmit == true
                             && o.isCancel == null
                             && o.isApproved == null
                             && o.tglBerangkat > DateTime.Now
                             select o.noSPD).Distinct().ToList();

                foreach (var item in query)
                {
                    Process(item);
                    using (StreamWriter w = File.AppendText(@"E:\SPD\logNoSpdResendAll.txt"))
                    {
                        Log(item, w);
                    }
                }
            }
        }
        protected void btnResendAllNew_Click(object sender, EventArgs e)
        {
            using (var ctx = new dsSPDDataContext())
            {
                var query = (from o in ctx.trSPDs
                             where o.isSubmit == true
                             && o.isCancel == null
                             && o.isApproved == null
                             && o.tglBerangkat > DateTime.Now
                             select o.noSPD).Distinct().ToList();

                foreach (var item in query)
                {
                    ProcessNew(item);
                    using (StreamWriter w = File.AppendText(@"E:\SPD\logNoSpdResendAll.txt"))
                    {
                        Log(item, w);
                    }
                }
            }
        }
        public void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}