using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;

namespace eSPD.Core
{
    public class EmailClaimJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (var ctx = new dsSPDDataContext())
            {

                #region Atasan

                var spdClaimAtasan = (from p in ctx.trClaims
                                      join o in ctx.trSPDs on p.noSPD equals o.noSPD
                                      where ((p.isSubmit == true && p.isCancel == null && p.isApprovedAtasan == null && p.isApprovedGA == null && p.isApprovedFinance == null && p.status != "Claim Expired"))
                                      select p).ToList();

                var nospd = (from p in spdClaimAtasan select p.noSPD).Distinct();

                var approval = (from p in ctx.ApprovalStatus
                                where nospd.Contains(p.NoSPD) && p.IndexLevel == 1 && p.Status != null
                                group p by p.NoSPD
                                    into groups
                                    select groups.OrderBy(p => p.IndexLevel).First()).ToList();

                var nrpApproval = (from p in approval
                                   group p by p.NrpApproval
                                       into groups
                                       select groups.OrderBy(p => p.IndexLevel).First().NrpApproval).Distinct().ToList();

                foreach (var item in nrpApproval)
                {
                    var namaAtasan = approval.FirstOrDefault(o => o.NrpApproval == item).Nama;
                    var nrpAtasan = item;

                    var datatoSend = (from p in approval
                                      join o in spdClaimAtasan on p.NoSPD equals o.noSPD
                                      join x in ctx.trSPDs on p.NoSPD equals x.noSPD
                                      where p.NrpApproval == item
                                      select new OutstandingClaim
                                      {
                                          NoSPD = x.noSPD,
                                          Nama = x.namaLengkap,
                                          TglBerangkat = x.tglBerangkat,
                                          TglKembali = x.tglKembali,
                                          Keperluan = x.msKeperluan.keperluan + " - " + x.ketKeperluan + " - " + x.keperluanLain,
                                          JumlahClaim = (Convert.ToInt32(o.biayaMakan + o.uangSaku + o.tiket + o.hotel + o.BBM + o.tol + o.taxi + o.airportTax + o.laundry + o.parkir + o.biayaLainLain + (o.komunikasi != null ? o.komunikasi : 0))).ToString("#,###")
                                      }).ToList();

                    if (datatoSend.Count != 0 || datatoSend != null)
                    {
                        sendToMail(ctx, datatoSend, namaAtasan, nrpAtasan);
                    }
                }

                #endregion

                #region GA

                var spdClaimGa = (from p in ctx.trClaims
                                  join o in ctx.trSPDs on p.noSPD equals o.noSPD
                                  where ((p.isSubmit == true && p.isCancel == null && p.isApprovedAtasan == true && p.isApprovedGA == null && p.isApprovedFinance == null && p.status != "Claim Expired"))
                                  select p).ToList();

                var nospdGa = (from p in spdClaimGa select p.noSPD).Distinct();

                var roleGa = (from p in ctx.msUsers
                              where p.roleId == 17
                              select p.nrp).ToList();

                var dataGa = ctx.msKaryawans
                    .Where(o =>
                        o.coCd == "0001" &&
                        roleGa.Contains(o.nrp));

                foreach (var item in dataGa)
                {
                    var namaGa = item.namaLengkap;
                    var nrpGa = item.nrp;

                    var datatoSend = (from p in spdClaimGa
                                      join x in ctx.trSPDs on p.noSPD equals x.noSPD
                                      select new OutstandingClaim
                                      {
                                          NoSPD = x.noSPD,
                                          Nama = x.namaLengkap,
                                          TglBerangkat = x.tglBerangkat,
                                          TglKembali = x.tglKembali,
                                          Keperluan = x.msKeperluan.keperluan + " - " + x.ketKeperluan + " - " + x.keperluanLain,
                                          JumlahClaim = (Convert.ToInt32(p.biayaMakan + p.uangSaku + p.tiket + p.hotel + p.BBM + p.tol + p.taxi + p.airportTax + p.laundry + p.parkir + p.biayaLainLain + (p.komunikasi != null ? p.komunikasi : 0))).ToString("#,###")
                                      }).ToList();

                    if (datatoSend.Count != 0 || datatoSend != null)
                    {
                        sendToMail(ctx, datatoSend, namaGa, nrpGa);
                    }
                }
                #endregion

                #region Finance

                var spdClaimFinance = (from p in ctx.trClaims
                                       join o in ctx.trSPDs on p.noSPD equals o.noSPD
                                       where ((p.isSubmit == true && p.isCancel == null && p.isApprovedAtasan == true && p.isApprovedGA == true && p.isApprovedFinance == null && p.status != "Claim Expired"))
                                       select p).ToList();

                var nospdFinance = (from p in spdClaimFinance select p.noSPD).Distinct();

                var coCdFinance = (from p in ctx.trSPDs
                                   join k in ctx.msKaryawans on p.nrp equals k.nrp
                                   where nospdFinance.Contains(p.noSPD)
                                   group k by k.coCd
                                       into groups
                                       select groups.OrderBy(k => k.coCd).First().coCd).ToList();

                foreach (var itemcoCdFinance in coCdFinance)
                {
                    var roleFinance = (from p in ctx.msUsers
                                       where p.roleId == 19
                                       select p.nrp).ToList();

                    var dataFinance = ctx.msKaryawans
                        .Where(o =>
                            o.coCd == itemcoCdFinance &&
                            roleFinance.Contains(o.nrp));

                    foreach (var item in dataFinance)
                    {
                        var namaFinance = item.namaLengkap;
                        var nrpFinance = item.nrp;

                        var datatoSend = (from p in spdClaimFinance
                                          join x in ctx.trSPDs on p.noSPD equals x.noSPD
                                          join k in ctx.msKaryawans on x.nrp equals k.nrp
                                          where k.coCd == item.coCd
                                          select new OutstandingClaim
                                          {
                                              NoSPD = x.noSPD,
                                              Nama = x.namaLengkap,
                                              TglBerangkat = x.tglBerangkat,
                                              TglKembali = x.tglKembali,
                                              Keperluan = x.msKeperluan.keperluan + " - " + x.ketKeperluan + " - " + x.keperluanLain,
                                              JumlahClaim = (Convert.ToInt32(p.biayaMakan + p.uangSaku + p.tiket + p.hotel + p.BBM + p.tol + p.taxi + p.airportTax + p.laundry + p.parkir + p.biayaLainLain + (p.komunikasi != null ? p.komunikasi : 0))).ToString("#,###")
                                          }).ToList();

                        if (datatoSend.Count != 0 || datatoSend != null)
                        {
                            sendToMail(ctx, datatoSend, namaFinance, nrpFinance);
                        }
                    }
                }
                #endregion

            }
        }

        public void sendToMail(dsSPDDataContext ctx, List<OutstandingClaim> datatoSend, string nama, string nrp)
        {
            using (var smtpClient = new SmtpClient())
            {
                string data = string.Empty;

                string trtd = string.Empty;
                foreach (var detail in datatoSend.OrderBy(o => o.TglBerangkat.Date))
                {
                    string urlExternal = "http://118.97.80.12/SPD/" + "newFormRequestDetailClaim.aspx?encrypt=" + Encrypto.Encrypt(detail.NoSPD);
                    string urlInternal = "http://trac39/SPD/" + "newFormRequestDetailClaim.aspx?encrypt=" + Encrypto.Encrypt(detail.NoSPD);
                    trtd +=
                         "<tr style='border:1px solid #eee;'>" +
                             "<td style='border:1px solid #dedede;padding:5px;'>" + detail.NoSPD + "</td>" +
                             "<td style='border:1px solid #dedede;padding:5px;'>" + detail.Nama + "</td>" +
                             "<td style='border:1px solid #dedede;padding:5px;'>" + detail.Keperluan + "</td>" +
                             "<td style='border:1px solid #dedede;padding:5px;'>" + detail.TglBerangkat.ToString("dd MMMM yyyy") + "</td>" +
                             "<td style='border:1px solid #dedede;padding:5px;'>" + detail.TglKembali.ToString("dd MMMM yyyy") + "</td>" +
                             "<td style='border:1px solid #dedede;padding:5px;'>" + detail.JumlahClaim + "</td>" +
                             "<td style='border:1px solid #dedede;padding:5px;'><a href='" + urlInternal + "'>Internal</a>&nbsp;|&nbsp;<a href='" + urlExternal + "'>External</a></td>" +
                         "</tr>";
                }

                data += "<h4 style='color: #1b809e; border-bottom: 1px solid #eee;'>Kepada Yth " + nama + "</h4>" + "<br/>Berikut adalah data Approval Claim yang masih outstanding per tanggal " + DateTime.Now.ToString("dd MMMM yyyy") + "<br/><br/>" +
                        "<table style='border-color: #eee;border-collapse: collapse;border-width: 1px;color: #333333;'>" +
                            "<tr style='border-width: 1px;padding: 8px;border-style: solid;border-color: #eee;background-color: #dedede;'>" +
                                "<th style='border:1px solid #fff;padding:5px;'>No</th>" +
                                "<th style='border:1px solid #fff;padding:5px;'>Nama</th>" +
                                "<th style='border:1px solid #fff;padding:5px;'>Keperluan</th>" +
                                "<th style='border:1px solid #fff;padding:5px;'>Tgl Berangkat</th>" +
                                "<th style='border:1px solid #fff;padding:5px;'>Tgl Kembali</th>" +
                                "<th style='border:1px solid #fff;padding:5px;'>Total Claim</th>" +
                                "<th style='border:1px solid #fff;padding:5px;'>Link</th>" +
                            "</tr>" +
                            trtd +
                        "</table><br/><br/><br/>" +
                        "Terima kasih. <br/>" +
                        "Catatan : Internal (jika berada di kantor) dan External (jika berada di luar kantor)<br/>" +
                        "<span style='color: #FF0000'><b>E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini</b></span>";

                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                MailMessage message = new MailMessage
                {
                    Sender = new MailAddress(section.From), // on Behave of When From differs
                    From = new MailAddress(section.From),
                    Subject = "Outstanding Approval Claim SPD " + DateTime.Now.Date.ToString("dd MMMM yyyy"),
                    Body = data,
                    IsBodyHtml = true,
                };

                var getEmail = ctx.msKaryawans.FirstOrDefault(o => o.nrp == nrp);
                string email = string.Empty;
                email = getEmail != null ? getEmail.EMail : string.Empty;

                if (!string.IsNullOrEmpty(email))
                {
                    if (email.ToLower() == "kumaraguru@sera.astra.co.id")
                    {
                        message.CC.Add("YUDAS.TADEUS@SERA.ASTRA.CO.ID");
                       // email = "YUDAS.TADEUS@SERA.ASTRA.CO.ID";
                    }
                    message.To.Add(email);
                    message.Bcc.Add("espddeveloper@gmail.com");

                    smtpClient.Send(message);
                }
            }
        }
    }

    public class OutstandingClaim
    {
        public string NoSPD { get; set; }
        public string Nama { get; set; }
        public string Keperluan { get; set; }
        public DateTime TglBerangkat { get; set; }
        public DateTime TglKembali { get; set; }
        public string JumlahClaim { get; set; }
    }
}
