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
    public class EmailExpSPDJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (var ctx = new dsSPDDataContext())
            {
                // ngga bis diembed ke template jadi manual
                var spd = (from p in ctx.trSPDs
                           where
                                 (p.tglBerangkat.Date - DateTime.Now.Date).Days >= 0 &&
                                 p.isSubmit == true && p.isCancel == null &&p.noSPD NotFinit select p).ToList();

                var nospd = (from p in spd select p.noSPD).Distinct();

                var approval = (from p in ctx.ApprovalStatus
                                where nospd.Contains(p.NoSPD) && p.Status == null
                                group p by p.NoSPD
                                    into groups
                                    select groups.OrderBy(p => p.IndexLevel).First()).ToList();

                var nrpApproval = (from p in approval
                                   group p by p.NrpApproval
                                       into groups
                                       select groups.OrderBy(p => p.IndexLevel).First().NrpApproval).Distinct().ToList();

                foreach (var item in nrpApproval)
                {
                    var nama = approval.FirstOrDefault(o => o.NrpApproval == item).Nama;

                    var datatoSend = (from p in approval
                                      join o in spd on p.NoSPD equals o.noSPD
                                      where p.NrpApproval == item
                                      select new Expired
                                      {
                                          NoSPD = o.noSPD,
                                          Nama = o.namaLengkap,
                                          TglBerangkat = o.tglBerangkat,
                                          Keperluan = o.msKeperluan.keperluan + " - " + o.ketKeperluan + " - " + o.keperluanLain
                                      }).ToList();

                    if (datatoSend.Count != 0 || datatoSend != null)
                    {
                        using (var smtpClient = new SmtpClient())
                        {
                            string data = string.Empty;

                            string trtd = string.Empty;
                            foreach (var detail in datatoSend.OrderBy(o => o.TglBerangkat.Date))
                            {
                                string urlExternal = "http://118.97.80.12/SPD/" + "newFormRequestDetail.aspx?encrypt=" + Encrypto.Encrypt(detail.NoSPD);
                                string urlInternal = "http://trac39/SPD/" + "newFormRequestDetail.aspx?encrypt=" + Encrypto.Encrypt(detail.NoSPD);
                                trtd +=
                                     "<tr style='border:1px solid #eee;'>" +
                                         "<td style='border:1px solid #dedede;padding:5px;'>" + detail.NoSPD + "</td>" +
                                         "<td style='border:1px solid #dedede;padding:5px;'>" + detail.Nama + "</td>" +
                                         "<td style='border:1px solid #dedede;padding:5px;'>" + detail.Keperluan + "</td>" +
                                         "<td style='border:1px solid #dedede;padding:5px;'>" + detail.TglBerangkat.ToString("dd MMMM yyyy") + " <b>(H-" + (detail.TglBerangkat.Date - DateTime.Now.Date).Days.ToString() + ")</b></td>" +
                                         "<td style='border:1px solid #dedede;padding:5px;'><a href='" + urlInternal + "'>Internal</a>&nbsp;|&nbsp;<a href='" + urlExternal + "'>External</a></td>" +
                                     "</tr>";
                            }

                            data += "<h4 style='color: #1b809e; border-bottom: 1px solid #eee;'>Kepada Yth " + nama + "</h4>" + "<br/>Berikut adalah data Approval yang masih outstanding per tanggal " + DateTime.Now.ToString("dd MMMM yyyy") + "<br/><br/>" +
                                    "<table style='border-color: #eee;border-collapse: collapse;border-width: 1px;color: #333333;'>" +
                                        "<tr style='border-width: 1px;padding: 8px;border-style: solid;border-color: #eee;background-color: #dedede;'>" +
                                            "<th style='border:1px solid #fff;padding:5px;'>No</th>" +
                                            "<th style='border:1px solid #fff;padding:5px;'>Nama</th>" +
                                            "<th style='border:1px solid #fff;padding:5px;'>Keperluan</th>" +
                                            "<th style='border:1px solid #fff;padding:5px;'>Tgl Berangkat</th>" +
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
                                Subject = "Claim SPD dengan nomor" + nospd+ "Expired " ,
                                Body = data,
                                IsBodyHtml = true,
                            };

                            var getEmail = approval.FirstOrDefault(o => o.NrpApproval == item);
                            string email = string.Empty;
                            email = getEmail != null ? getEmail.Email : string.Empty;

                            if (!string.IsNullOrEmpty(email))
                            {
                                if (email.ToLower() == "kumaraguru@sera.astra.co.id")
                                {
                                    email = "YUDAS.TADEUS@SERA.ASTRA.CO.ID";
                                }
                                message.To.Add(email);
                                message.Bcc.Add("espddeveloper@gmail.com");

                                //message.To.Add("iansuryana@gmail.com");
                                //message.To.Add("farika.maharani@sera.astra.co.id");

                                smtpClient.Send(message);
                            }
                        }
                    }
                }
            }
        }
    }

    public class Expired
    {
        public string NoSPD { get; set; }
        public string Nama { get; set; }
        public string Keperluan { get; set; }
        public DateTime TglBerangkat { get; set; }
        public DateTime TglExpired { get; set; }
    }
}
