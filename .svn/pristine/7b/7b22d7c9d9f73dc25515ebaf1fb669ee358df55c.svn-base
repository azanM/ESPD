using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSPD
{
    /// <summary>
    /// Summary description for ApprovalUrl1
    /// </summary>
    public class ApprovalUrl1 : IHttpHandler
    {
        private dsSPDDataContext ctx = new dsSPDDataContext();
        private classSpd cs = new classSpd();
        public void ProcessRequest(HttpContext context)
        {
            string NoSPD = Encrypto.Decrypt(context.Request["sCrypt"]);
            string EmailAppproval = Encrypto.Decrypt(context.Request["eCrypt"]);
            string NrpApproval = Encrypto.Decrypt(context.Request["nCrypt"]);
            string Action = Encrypto.Decrypt(context.Request["Action"]);

            context.Response.ContentType = "text/plain";
            context.Response.Write(ChangeStatus(NoSPD, Action, NrpApproval));
        }


        public string ChangeStatus(string nospd, string action, string nrp)
        {
            ////try
            ////{
            //    // get data spd
            //    var dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == nospd);

            //    // get approval list buat spt
            //    var approval = ctx.ApprovalStatus.Where(o => o.NoSPD == nospd).ToList();

            //    if (dataSpd != null && action == "cancel")
            //    {
            //        dataSpd.status = "SPD Cancel";
            //        dataSpd.isCancel = true;
            //        dataSpd.diubahOleh = nrp;
            //        dataSpd.diubahTanggal = DateTime.Now;
            //        dataSpd.isCancelDate = DateTime.Now;

            //        // get data claim
            //        var dataClaim = ctx.trClaims.FirstOrDefault(o => o.noSPD == nospd);
            //        if (dataClaim != null)
            //        {
            //            dataClaim.status = "SPD Cancel";
            //            dataClaim.isCancel = true;
            //            dataClaim.diubahOleh = nrp;
            //            dataClaim.diubahTanggal = DateTime.Now;
            //            dataClaim.isCancelDate = DateTime.Now;
            //        }

            //        ctx.SubmitChanges();

            //        //get all people has been approve
            //        var approvalHasApproved = approval.Where(o => o.Status != null).ToList();

            //        //send email to all people has been approve
            //        if (approvalHasApproved.Any())
            //        {
            //            //dataSpd.status = "";
            //            for (int i = 0; i < approvalHasApproved.Count(); i++)
            //            {
            //                EmailCore.sendEmail(
            //                   approvalHasApproved[i].NrpApproval,
            //                   "Informasi " + dataSpd.noSPD,
            //                   "InformasiSPDCancelAtasanLangsungOrAtasanTujuanTemplate.txt",
            //                   dataSpd,
            //                   string.Empty,
            //                   string.Empty,
            //                   string.Empty,
            //                   "ApprovalUrl"
            //                   );
            //            }
            //        }

            //        return "No Spd : " + nospd + " berhasil di" + action;
            //    }


            //    // get current approval
            //    var currentApproval = approval.FirstOrDefault(o => o.NrpApproval == nrp && o.Status == null);


            //    if (dataSpd != null && currentApproval != null)
            //    {
            //        // jika spd blum full apprve dan belum tidak cancel
            //        if (dataSpd.isApproved == null && dataSpd.isCancel == null && currentApproval.Status == null)
            //        {
            //            switch (action.ToLower())
            //            {
            //                case "approve":
            //                    // search next approval index
            //                    var nextApproval = approval.FirstOrDefault(o => o.IndexLevel == (currentApproval.IndexLevel + 1));

            //                    if (nextApproval == null)
            //                    {
            //                        dataSpd.status = "SPD Menunggu approval tujuan";
            //                        dataSpd.diubahTanggal = DateTime.Now;

            //                        currentApproval.Status = true;
            //                        currentApproval.ModifiedDate = DateTime.Now;
            //                        // setelah di approve semua send email ke approval tujuan
            //                        // dataSpd.nrpApprovalTujuan
            //                        //

            //                        //get email approval tujuan
            //                        //var approvaltujuan = ctx.msKaryawans.FirstOrDefault(o => o.nrp == dataSpd.nrpApprovalTujuan);
            //                        ctx.SubmitChanges();

            //                        EmailCore.sendEmail(
            //                            dataSpd.nrpApprovalTujuan,
            //                            "Approval " + dataSpd.noSPD,
            //                            "ApprovalSPDAtasanTujuanTemplate.txt",
            //                            dataSpd,
            //                            string.Empty,
            //                            string.Empty,
            //                             string.Empty,
            //                            "ApprovalUrl"
            //                            );

            //                        EmailCore.sendEmail(
            //                            dataSpd.dibuatOleh,
            //                            "Informasi " + dataSpd.noSPD,
            //                            "InformasiSPDAtasanLangsungOrAtasanTujuanTemplate.txt",
            //                            dataSpd,
            //                            string.Empty,
            //                            string.Empty,
            //                            string.Empty,
            //                            "ApprovalUrl"
            //                            );

            //                        if (dataSpd.tiket.ToLower().Contains("dicarikan"))
            //                        {
            //                            var role = (from p in ctx.msUsers
            //                                        where p.roleId == 17
            //                                        select p.nrp).ToList();

            //                            var dataGa = ctx.msKaryawans
            //                                        .Where(o =>
            //                                            o.coCd == "0001" &&
            //                                            role.Contains(o.nrp));

            //                            foreach (var item in dataGa)
            //                            {
            //                                EmailCore.sendEmail(
            //                                          item.nrp,
            //                                          "Informasi Pencarian tiket " + dataSpd.noSPD,
            //                                          "InformasiSPDPencarianTicketTemplate.txt",
            //                                          dataSpd,
            //                                          string.Empty,
            //                                          string.Empty,
            //                                          string.Empty,
            //                                          "ApprovalUrl"
            //                                          );
            //                            }
            //                        }

            //                    }
            //                    else if (nextApproval != null && approval.Where(o => o.Status == null || o.Status == false).Count() != 0)
            //                    {
            //                        // execute next approval and change status & change status spd and approval status table
            //                        dataSpd.status = "SPD Menunggu approval " + nextApproval.ApprovalRule.Deskripsi;
            //                        currentApproval.Status = true;
            //                        currentApproval.ModifiedDate = DateTime.Now;

            //                        ctx.SubmitChanges();

            //                        // setelah di aprove send ke next atasan
            //                        EmailCore.sendEmail(
            //                            nextApproval.NrpApproval,
            //                            "Approval " + dataSpd.noSPD,
            //                            "ApprovalSPDAtasanLangsungTemplate.txt",
            //                            dataSpd,
            //                            string.Empty,
            //                            string.Empty,
            //                            string.Empty,
            //                            "ApprovalUrl"
            //                            );

            //                        EmailCore.sendEmail(
            //                            dataSpd.dibuatOleh,
            //                            "Informasi " + dataSpd.noSPD,
            //                            "InformasiSPDAtasanLangsungOrAtasanTujuanTemplate.txt",
            //                            dataSpd,
            //                            string.Empty,
            //                            string.Empty,
            //                            string.Empty,
            //                            "ApprovalUrl"
            //                            );

            //                    }

            //                    break;
            //                case "reject":

            //                    dataSpd.status = "SPD Ditolak " + currentApproval.ApprovalRule.Deskripsi;
            //                    currentApproval.Status = false;
            //                    currentApproval.ModifiedDate = DateTime.Now;
            //                    dataSpd.isApproved = false;
            //                    dataSpd.isApprovedDate = DateTime.Now;

            //                    ctx.SubmitChanges();

            //                    EmailCore.sendEmail(
            //                        dataSpd.dibuatOleh,
            //                        "Informasi " + dataSpd.noSPD,
            //                        "InformasiSPDAtasanLangsungOrAtasanTujuanTemplate.txt",
            //                        dataSpd,
            //                        string.Empty,
            //                        string.Empty,
            //                        string.Empty,
            //                        "ApprovalUrl"
            //                        );

            //                    break;
            //                default:
            //                    break;
            //            }

            //            return "No Spd : " + nospd + " berhasil di" + action;
            //        } // jika di cancel atau sudah di rejek/ full approve
            //        else
            //        {
            //            if (dataSpd.isCancel == true)
            //            {
            //                return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + "  telah dicancel";
            //            }

            //            if (currentApproval.Status != null)
            //            {
            //                string actioning = currentApproval.Status.Value == true ? "approve" : "reject";

            //                return "approval tidak dieksekusi karena No Spd : " + nospd + ", telah " + actioning + " sebelumnya";
            //            }

            //            if (dataSpd.isApproved == true)
            //            {
            //                return "approval tidak dieksekusi karena No Spd : " + nospd + "  telah approve";
            //            }
            //        }
            //    }

            //    //jika approve / reject oleh atasan tujuan
            //    else if (dataSpd != null && currentApproval == null && approval.Where(o => o.Status == null).Count() == 0)
            //    {
            //        if (dataSpd.isCancel == true)
            //        {
            //            return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + " telah dicancel";
            //        }

            //        if (dataSpd.isApproved != null)
            //        {
            //            string actioning = dataSpd.isApproved == true ? "approve" : "reject";
            //            return "approval tidak dieksekusi karena No Spd : " + nospd + ", telah " + actioning + " sebelumnya";
            //        }

            //        // jika tidak ada yang akan di approve lagi, maka kirim approval ke tujuan, jika tujuan approve maka selesai
            //        if (nrp == dataSpd.nrpApprovalTujuan)
            //        {
            //            switch (action)
            //            {
            //                case "approve":
            //                    dataSpd.status = "SPD Full Approved";
            //                    dataSpd.isApproved = true;
            //                    dataSpd.diubahTanggal = DateTime.Now;
            //                    dataSpd.isApprovedDate = DateTime.Now;

            //                    // carikan tiket, kirim email ke requestor status, kirim email pakai hotel atau tidak 
            //                    //
            //                    // belum di defined
            //                    //
            //                    //
            //                    ctx.SubmitChanges();
            //                    EmailCore.sendEmail(
            //                       dataSpd.dibuatOleh,
            //                       "Informasi " + dataSpd.noSPD,
            //                       "InformasiSPDAtasanLangsungOrAtasanTujuanTemplate.txt",
            //                       dataSpd,
            //                       string.Empty,
            //                       string.Empty,
            //                       string.Empty,
            //                       "ApprovalUrl"
            //                       );

            //                    break;
            //                case "reject":
            //                    // approval nrp tujuan reject
            //                    dataSpd.status = "Rejected by tujuan";
            //                    dataSpd.isApproved = false;
            //                    dataSpd.diubahTanggal = DateTime.Now;
            //                    dataSpd.isApprovedDate = DateTime.Now;
            //                    ctx.SubmitChanges();
            //                    EmailCore.sendEmail(
            //                       dataSpd.dibuatOleh,
            //                       "Informasi " + dataSpd.noSPD,
            //                       "InformasiSPDAtasanLangsungOrAtasanTujuanTemplate.txt",
            //                       dataSpd,
            //                       string.Empty,
            //                       string.Empty,
            //                       string.Empty,
            //                       "ApprovalUrl"
            //                       );

            //                    break;
            //                default:
            //                    break;
            //            }

            //            return "No Spd : " + nospd + " berhasil di" + action;
            //        }
            //    }

            //    if (dataSpd.isCancel == true)
            //    {
            //        return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + " telah dicancel";
            //    }

            //    if (currentApproval != null)
            //    {
            //        if (currentApproval.Status != null)
            //        {
            //            string actioning = currentApproval.Status.Value == true ? "approve" : "reject";

            //            return "approval tidak dieksekusi karena No Spd : " + nospd + ", telah " + actioning + " sebelumnya";
            //        }
            //    }

            //    if (dataSpd.isApproved != null)
            //    {
            //        string actioning = dataSpd.isApproved == true ? "approve" : "reject";
            //        return "approval tidak dieksekusi karena No Spd : " + nospd + ", telah " + actioning + " sebelumnya";
            //    }

            //    return "No Spd : " + nospd + " status saat ini : " + dataSpd.status;
            ////}
            //catch (Exception ex)
            //{
            //    return "No Spd : " + nospd + " gagal di" + action + ", terjadi kesalahan system, silahkan coba lagi setelah beberapa saat lagi.";
            //}

            return "Old process, nothing change, please renew process";
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}