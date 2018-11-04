using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSPD.Core;

namespace eSPD
{
    /// <summary>
    /// Summary description for approvalSPD
    /// </summary>
    public class approvalSPD : IHttpHandler
    {

        public void ProcessRequest(HttpContext ctx)
        {
            string noSPD = ctx.Request["nospd"];
            string stat = ctx.Request["conf"];
            string subject = ctx.Request["subject"];
            string type = ctx.Request["type"];

            if (type.ToLower().Trim() == "spd")
                ubahStatusSPD(noSPD, stat, subject, ctx);
            else
                ubahStatusClaim(noSPD, stat, subject, ctx);
        }

        private void ubahStatusSPD(string strNoSpd, string stat, string subject, HttpContext ctx)
        {
            string role = string.Empty,Approver=string.Empty;
            bool sending=false;
            dsSPDDataContext data = new dsSPDDataContext();
            trSPD oSpd = (from spd in data.trSPDs
                          where spd.noSPD == strNoSpd
                          select spd).FirstOrDefault();
            msKaryawan kary = new msKaryawan();
            if (oSpd.nrp != "99999999")
            {
                kary = (from kar in data.msKaryawans
                        where kar.nrp == oSpd.nrp
                        select kar).FirstOrDefault();
            }
                //Ditambahkan oleh martha
            else if (oSpd.nrp == "2950")
            {
                kary.EMail = "YUDAS.TADEUS@SERA.ASTRA.CO.ID";
                kary.nrp = oSpd.nrp;
                kary.namaLengkap = oSpd.namaLengkap;
                kary.golongan = "III";
                kary.Job = oSpd.jabatan;
                kary.posisi = oSpd.jabatan;
                kary.coCd = "1";
                kary.kodePSubArea = "1";
                kary.kodePA = "1";
            }
            else
            {
                kary.EMail = oSpd.email;
                kary.nrp = oSpd.nrp;
                kary.namaLengkap = oSpd.namaLengkap;
                kary.golongan = "III";
                kary.Job = oSpd.jabatan;
                kary.posisi = oSpd.jabatan;
                kary.coCd = "1";
                kary.kodePSubArea = "1";
                kary.kodePA = "1";

            }
            if (subject.ToLower().Trim() == "atasan") 
            {
                if (stat.ToLower().Trim() == "approve")
                {
                    if (oSpd.status.Split('-')[0] == "2")
                    {
                        oSpd.status = "3-Atasan Approve";
                        ctx.Response.Write("Berhasil di approve");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("SPD Sudah Diapprove sebelumnya");
                        sending = false;
                    }                                       
                }
                else
                {
                    if (oSpd.status.Split('-')[0] == "2")
                    {
                        oSpd.status = "12-SPD Tolak (Atasan)";
                        ctx.Response.Write("Berhasil di reject");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("SPD Sudah di tolak sebelumnya");
                        sending = false;
                    }
                }
                role = "1";
                Approver = oSpd.nrpAtasan;
            }
            else if (subject.ToLower().Trim() == "tujuan")
            {
                if (oSpd.tglBerangkat.Date > DateTime.Now.Date)
                {
                    ctx.Response.Write("SPD belum bisa approve/reject karena tanggal berangkat harus sesudah atau sama dengan hari ini");
                    sending = false;
                    return;
                }
                if (stat.ToLower().Trim() == "approve")
                {
                    if (oSpd.status.Split('-')[0] == "3")
                    {
                        oSpd.status = "7-Tempat Tujuan Approve";
                        ctx.Response.Write("Berhasil di approve");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("SPD Sudah Diapprove sebelumnya");
                        sending = false;
                    }

                }
                else
                {
                    if (oSpd.status.Split('-')[0] == "3")
                    {
                        oSpd.status = "13-SPD Tolak (Tujuan)";
                        ctx.Response.Write("Berhasil di reject");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("SPD sudah ditolak sebelumnya");
                        sending = false;
                    }
                }
                role = "2";
                Approver = oSpd.nrpApprovalTujuan;
            }

            if (sending) {
                trApprovalHistory ah = new trApprovalHistory();
                ah.noSPD = strNoSpd;
                ah.idRole = role;
                ah.statusApproval = oSpd.status;
                ah.nrpApprover = Approver;
                ah.approvalDatetime = DateTime.Now;
                data.trApprovalHistories.InsertOnSubmit(ah);
                data.SubmitChanges();
           
               classSpd oClassSPD = new classSpd();    

                switch (oSpd.status.Split('-')[0])
                {
                    case "2":
                        oClassSPD.sendMail(oSpd, "Atasan", kary);
                        break;
                    case "3":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        oClassSPD.sendMail(oSpd, "Tujuan", kary);
                        oClassSPD.sendMail(oSpd, "GA", kary);
                        break;
                    case "6":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        oClassSPD.sendMail(oSpd, "Tujuan", kary);
                        oClassSPD.sendMail(oSpd, "GA", kary);
                        break;
                    case "7":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "8":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "9":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "12":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "13":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                }

            }

            
            data.Dispose();
        }
        private void ubahStatusClaim(string strNoSpd, string stat, string subject, HttpContext ctx)
        {
            string role = string.Empty, Approver = string.Empty;
            dsSPDDataContext data = new dsSPDDataContext();
            bool sending = false;
            trSPD oSpd = (from spd in data.trSPDs
                          where spd.noSPD == strNoSpd
                          select spd).First();
            trClaim oClaim = (from spd in data.trClaims
                          where spd.noSPD == strNoSpd
                          select spd).First();
            msKaryawan kary = new msKaryawan();
            if (oSpd.nrp != "99999999")
            {
                kary = (from kar in data.msKaryawans
                        where kar.nrp == oSpd.nrp
                        select kar).FirstOrDefault();
            }
            else
            {
                kary.EMail = oSpd.email;
                kary.nrp = oSpd.nrp;
                kary.namaLengkap = oSpd.namaLengkap;
                kary.golongan = "III";
                kary.Job = oSpd.jabatan;
                kary.posisi = oSpd.jabatan;
                kary.coCd = "1";
                kary.kodePSubArea = "1";
                kary.kodePA = "1";

            }
            if (subject.ToLower().Trim() == "atasan")
            {
                if (stat.ToLower().Trim() == "approve")
                {
                    if (oSpd.status.Split('-')[0] == "11")
                    {
                        oClaim.status = "16-Claim Approve (Atasan)";
                        oSpd.status = "16-Claim Approve (Atasan)";
                        ctx.Response.Write("Berhasil di approve");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("Claim sudah berhasil Di approve sebelumnya");
                        sending = false;
                    }
                   
                }
                else
                {
                    if (oSpd.status.Split('-')[0] == "11")
                    {
                        oClaim.status = "14-Claim Tolak (Atasan)";
                        oSpd.status = "14-Claim Tolak (Atasan)";
                        ctx.Response.Write("Berhasil di reject");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("Claim sudah berhasil ditolak sebelumnya");
                        sending = false;
                    }
                }
                role = "1";
                Approver = oSpd.nrpAtasan;
            }
            else if (subject.ToLower().Trim() == "ga")
            {
                if (stat.ToLower().Trim() == "approve")
                {
                    if (oSpd.status.Split('-')[0] == "16")
                    {
                        oClaim.status = "17-Claim Approve (GA)";
                        oSpd.status = "17-Claim Approve (GA)";
                        ctx.Response.Write("Berhasil di approve");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("Claim sudah berhasil di-approve sebelumnya");
                        sending = false;
                    }

                }
                else
                {
                    if (oSpd.status.Split('-')[0] == "16")
                    {
                        oClaim.status = "15-Claim Tolak (GA)";
                        oSpd.status = "15-Claim Tolak (GA)";
                        ctx.Response.Write("Berhasil di reject");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("Claim sudah berhasil di-reject sebelumnya");
                        sending = false;
                    }
                }
                role = "17";
                Approver = oSpd.nrpAtasan;
            }
            else if (subject.ToLower().Trim() == "finance")
            {
                if (stat.ToLower().Trim() == "approve")
                {
                    if (oSpd.status.Split('-')[0] == "17")
                    {
                        oClaim.status = "26-Finance Approve";
                        oSpd.status = "26-Finance Approve";
                        ctx.Response.Write("Berhasil di approve");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("Claim sudah berhasil di-Approve sebelumnya");
                        sending = false;
                    }

                }
                else
                {
                    if (oSpd.status.Split('-')[0] == "17")
                    {
                        oClaim.status = "27-Finance Tolak";
                        oSpd.status = "27-Finance Tolak";
                        ctx.Response.Write("Berhasil di reject");
                        sending = true;
                    }
                    else {
                        ctx.Response.Write("Claim sudah berhasil di-reject sebelumnya");
                        sending = false;
                    }
                }
                role = "19";
                Approver = oSpd.nrpAtasan;
            }
            else if (subject.ToLower().Trim() == "kasir")
            {
                if (oSpd.status.Split('-')[0] == "26")
                {
                    oClaim.status = "20-Claim Close";
                    oSpd.status = "20-Claim Close";
                    ctx.Response.Write("Berhasil di Close");
                    sending = true;
                }
                else {
                    ctx.Response.Write("Claim sudah berhasil di-Close sebelumnya");
                    sending = false;
                }
                role = "20";
                Approver = oSpd.nrpAtasan;
            }
            //data.SubmitChanges();
        
            if (sending) {
                classSpd oClassSPD = new classSpd();
                trApprovalHistory ah = new trApprovalHistory();
                ah.noSPD = strNoSpd;
                ah.idRole = role;
                ah.statusApproval = oSpd.status;
                ah.nrpApprover = Approver;
                ah.approvalDatetime = DateTime.Now;
                data.trApprovalHistories.InsertOnSubmit(ah);
                data.SubmitChanges();


                switch (oSpd.status.Split('-')[0])
                {
                    case "2":
                        oClassSPD.sendMail(oSpd, "Atasan", kary);
                        break;
                    case "3":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        oClassSPD.sendMail(oSpd, "GA", kary);
                        oClassSPD.sendMail(oSpd, "Tujuan", kary);
                        break;
                    case "6":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        oClassSPD.sendMail(oSpd, "GA", kary);
                        oClassSPD.sendMail(oSpd, "Tujuan", kary);
                        break;
                    case "8":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "9":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "12":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "13":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "14":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "16":
                        oClassSPD.sendMail(oSpd, "GA", kary);
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "15":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "17":
                        oClassSPD.sendMail(oSpd, "finance", kary);
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "27":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "26":
                        //oClassSPD.sendMail(oSpd, "kasir", kary);
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                    case "20":
                        oClassSPD.sendMail(oSpd, "Pembuat", kary);
                        break;
                }

            }
            
            data.Dispose();
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