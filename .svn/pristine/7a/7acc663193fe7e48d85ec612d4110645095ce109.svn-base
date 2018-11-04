using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.Linq;

namespace eSPD.Core
{
    public class classSpd
    {
        public string strLokasiAwal { get; set; }
        public string strNoSpd { get; set; }
        public int intNrp { get; set; }
        public string strNamaLengkap { get; set; }
        public int intGolongan { get; set; }
        public string strJabatan { get; set; }
        public string strDepartemen { get; set; }
        public string strDivisi { get; set; }
        public string strTmpTujuan { get; set; }
        public string strKeperluan { get; set; }
        public string strTiket { get; set; }
        public DateTime dtmTglBrangkat { get; set; }
        public string strJamBerangkat { get; set; }

        public msKaryawan getKaryawan(string LoginID)
        {
            dsSPDDataContext data = new dsSPDDataContext();
            string mail = string.Empty;
            //IMultipleResults result = data.stpGetKaryawanEmail(LoginID);
            stpGetKaryawanEmailResult hasil = (stpGetKaryawanEmailResult)data.stpGetKaryawanEmail(LoginID).SingleOrDefault();
            msKaryawan karyawanQ = new msKaryawan();
            if (LoginID.ToLower() == "spd")
            {
                karyawanQ.nrp = "99999999";
                karyawanQ.golongan = "III";
                karyawanQ.coCd = "1";
                karyawanQ.kodePSubArea = "1";
                karyawanQ.kodePA = "1";

            }
            else if (LoginID.ToLower() == "is07")
            {
                karyawanQ = (from q in data.msKaryawans
                             where q.nrp == "98989898"
                             select q).FirstOrDefault();
            }
            else
            {
                karyawanQ = (from q in data.msKaryawans
                             where q.email == hasil.EmailID
                             select q).FirstOrDefault();
            }
            //var dts = result.GetResult<DateTime>();

            return karyawanQ;
        }

        internal void sendMail(trSPD spd, string To, msKaryawan kar)
        {
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            StringBuilder emailMessage = new StringBuilder();
            StringBuilder emailMessageFrom = new StringBuilder();
            StringBuilder Content = new StringBuilder();
            dsSPDDataContext data = new dsSPDDataContext();
            var Atasan = (from k in data.msKaryawans
                                 where k.nrp == spd.nrpAtasan
                                 select k);
            //nrp ilang
            msKaryawan Tujuan = new msKaryawan();
            Tujuan = (from k in data.msKaryawans
                      where k.nrp == spd.nrpApprovalTujuan
                      select k).SingleOrDefault();
            msKaryawan Pengirim = new msKaryawan();
            Pengirim = (from k in data.msKaryawans
                        where k.nrp == spd.nrp
                        select k).FirstOrDefault();
            if (kar.nrp=="99999999")
            {
                Pengirim=kar;
                //Pengirim.nrp = kar.nrp;
                //Pengirim.job = kar.job;
                //Pengirim.golongan = kar.golongan;
                Pengirim.email = spd.email;
            }

            //hardcode untuk TRAC HO yang melakukan SPD, GAnya masi ke yulia
            if (Pengirim.kodePA == "1000")
            {
                Pengirim.kodePA = "1";
            }
            if (Pengirim.kodePSubArea == "1000")
            {
                Pengirim.kodePSubArea = "1";
            }
            var gaAsal = (from k in data.msKaryawans
                          join u in data.msUsers on k.nrp equals u.nrp
                          where u.roleId == 17 && k.coCd == Pengirim.coCd && k.kodePA == Pengirim.kodePA && k.kodePSubArea == Pengirim.kodePSubArea
                          select k);
            //msKaryawan atasanGaAsal = (from k in data.msKaryawans
            //                           where k.nrp == gaAsal.nrpAtasan
            //                           select k).SingleOrDefault();
            var KasirAsal = (from k in data.msKaryawans
                                 join u in data.msUsers on k.nrp equals u.nrp
                                 where u.roleId == 20 && k.coCd == Pengirim.coCd && k.kodePA == Pengirim.kodePA && k.kodePSubArea == Pengirim.kodePSubArea
                                 select k);
            var FinanceAsal = (from k in data.msKaryawans
                               join u in data.msUsers on k.nrp equals u.nrp
                               where u.roleId == 19 && k.coCd == Pengirim.coCd && k.kodePA == Pengirim.kodePA && k.kodePSubArea == Pengirim.kodePSubArea
                               select k);

            trClaim claim = new trClaim();
            claim = (from c in data.trClaims
                     where c.noSPD == spd.noSPD
                     select c).FirstOrDefault();


            if (To == "Extend")
            {
                email.Subject = "[Extend SPD] " + spd.noSPD + "-" + spd.namaLengkap;
                string mailAddress = string.Empty;
                string NamaLengkap = string.Empty;
                foreach (msKaryawan item in Atasan)
                {
                    mailAddress += item.email.Trim() + ",";
                    NamaLengkap += item.namaLengkap + "/";
                }
                mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                email.To.Add(mailAddress);
                email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                emailMessage.Append(string.Format("Kepada Yth, "));
                emailMessage.Append("<br />");
                emailMessage.Append(string.Format("Bapak/Ibu {0}", NamaLengkap));
                emailMessage.Append("<br />");
                emailMessage.Append("<br />");
                emailMessage.Append(string.Format("Bapak/Ibu {0} telah melakukan <b>Extend SPD</b> dengan rincian sebagai berikut : ", spd.namaLengkap));
                emailMessage.Append("<br />");
                Content = ContentBodymail(Content, spd, null, kar);
                emailMessage.Append(Content);
                emailMessage.Append("Terima kasih.");
                emailMessage.Append("<br />");
                emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                //send extend mail here
            }
            else {
                if (spd.status.Split('-')[0] == "2")
                {
                    email.Subject = "[Approval SPD Atasan Langsung] " + spd.noSPD + "-" + spd.namaLengkap;
                    string mailAddress = string.Empty;
                    string NamaLengkap = string.Empty;

                    //foreach (var item in gaAsal)
                    //{
                    //    mailAddress += item.email.Trim() + ",";
                    //    NamaLengkap += item.namaLengkap + "/";
                    //}

                    foreach (msKaryawan item in Atasan)
                    {
                        mailAddress += item.email.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                    NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    email.To.Add(mailAddress);
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", NamaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0} telah mengirim <b>Request Approval SPD</b> dengan rincian sebagai berikut : ", spd.namaLengkap));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, null, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Apabila Bapak/ Ibu berada di kantor bisa menggunakan link dibawah ini untuk melakukan approve/reject Request Claim SPD tersebut. Setelah klik approval, masukkan username dan password windows. Approval dinyatakan berhasil jika terdapat notifikasi approval berhasil pada browser. ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));

                }

                else if ((spd.status.Split('-')[0] == "3" || spd.status.Split('-')[0] == "6" ||  spd.status.Split('-')[0] == "8"
                    || spd.status.Split('-')[0] == "12" ) && To == "Pembuat")
                {
                    email.Subject = "[SPD Atasan Langsung] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.email.Trim());
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", Pengirim.namaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} adalah <b>{1}</b>.Berikut ini rincian SPD tersebut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, null, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Terima kasih.");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if (( spd.status.Split('-')[0] == "7" ||spd.status.Split('-')[0] == "9"
                 || spd.status.Split('-')[0] == "13") && To == "Pembuat")
                {
                    email.Subject = "[Approval SPD Atasan Tujuan] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.email.Trim());
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", Pengirim.namaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} adalah <b>{1}</b>.Berikut ini rincian SPD tersebut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, null, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Terima kasih.");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if ((spd.status.Split('-')[0] == "3" || spd.status.Split('-')[0] == "6") && To == "GA")
                {
                    email.Subject = "[Pencarian Tiket] " + spd.noSPD + "-" + spd.namaLengkap;
                    string mailAddress = string.Empty;
                    string NamaLengkap = string.Empty;
                    foreach (var item in gaAsal)
                    {
                        mailAddress += item.email.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                    NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    email.To.Add(mailAddress);
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", NamaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} <b>Sudah Disetujui atasan</b>. Mohon bantuannya untuk mencarikan tiket dengan rincian sebagai berikut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, null, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Terima kasih.");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));

                }
                else if ((spd.status.Split('-')[0] == "3" || spd.status.Split('-')[0] == "6") && To == "Tujuan")
                {
                    email.Subject = "[Approval SPD Atasan Tujuan] " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(Tujuan.email.Trim());
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", Tujuan.namaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Bapak/Ibu {0}, telah mengirim <b>Request Approval SPD Atasan Tempat Tujuan</b> dengan rincian sebagai berikut : ", spd.namaLengkap));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, null, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Apabila Bapak/ Ibu berada di kantor bisa menggunakan link dibawah ini untuk melakukan approve/reject Request Claim SPD tersebut. Setelah klik approval, masukkan username dan password windows. Approval dinyatakan berhasil jika terdapat notifikasi approval berhasil pada browser. ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("*Catatan : Mohon lakukan approval apabila karyawan yang melakukan SPD sudah tiba di tempat tujuan");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>**Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if (spd.status.Split('-')[0] == "11")
                {
                    string mailAddress = string.Empty;
                    string NamaLengkap = string.Empty;
                    foreach (var item in Atasan)
                    {
                        mailAddress += item.email.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                    NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    email.Subject = "[Approval Claim Atasan Langsung] " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", NamaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Bapak/Ibu {0} telah mengirim <b>Request Approval Claim SPD</b> dengan rincian sebagai berikut : ", spd.namaLengkap));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, claim, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Apabila Bapak/ Ibu berada di kantor bisa menggunakan link dibawah ini untuk melakukan approve/reject Request Claim SPD tersebut. Setelah klik approval, masukkan username dan password windows. Approval dinyatakan berhasil jika terdapat notifikasi approval berhasil pada browser. ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if ((spd.status.Split('-')[0] == "14" || spd.status.Split('-')[0] == "16" 
                    || spd.status.Split('-')[0] == "18" ) && To == "Pembuat")
                {
                    email.Subject = "[Claim Atasan Langsung] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.email.Trim());
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", Pengirim.namaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Status Request Claim SPD Bapak/Ibu {0} adalah <b>{1}</b>.Berikut ini rincian Claim SPD tersebut:  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, claim, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Terima kasih.");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }

                else if ((spd.status.Split('-')[0] == "15" ||  spd.status.Split('-')[0] == "17"
                    || spd.status.Split('-')[0] == "19") && To == "Pembuat")
                {
                    email.Subject = "[Claim GA] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.email.Trim());
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", Pengirim.namaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Status Request Claim SPD Bapak/Ibu {0} adalah <b>{1}</b>.Berikut ini rincian Claim SPD tersebut:  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, claim, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Terima kasih.");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if ((spd.status.Split('-')[0] == "20" || spd.status.Split('-')[0] == "26" || spd.status.Split('-')[0] == "27") && To == "Pembuat")
                {
                    email.Subject = "[Claim Finance] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.email.Trim());
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", Pengirim.namaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Status Request Claim SPD Bapak/Ibu {0} adalah <b>{1}</b>.Berikut ini rincian Claim SPD tersebut:  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, claim, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Terima kasih.");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if ((spd.status.Split('-')[0] == "16") && To == "GA")
                {
                    string mailAddress = string.Empty;
                    string NamaLengkap = string.Empty;
                    foreach (var item in gaAsal)
                    {
                        mailAddress += item.email.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                    NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    email.Subject = "[Approval Claim GA] " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", NamaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} <b>Sudah Di-Approve atasan</b> dengan rincian sebagai berikut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, claim, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Apabila Bapak/ Ibu berada di kantor bisa menggunakan link dibawah ini untuk melakukan approve/reject Request Claim SPD tersebut. Setelah klik approval, masukkan username dan password windows. Approval dinyatakan berhasil jika terdapat notifikasi approval berhasil pada browser. ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if ((spd.status.Split('-')[0] == "17") && To == "finance")
                {

                    string mailAddress = string.Empty;
                    string NamaLengkap = string.Empty;
                    foreach (var item in FinanceAsal)
                    {
                        mailAddress += item.email.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                    NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    email.Subject = "[Approval Claim Finance] " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", NamaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} <b>Sudah Di-Approve GA</b> dengan rincian sebagai berikut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, claim, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Apabila Bapak/ Ibu berada di kantor bisa menggunakan link dibawah ini untuk melakukan approve/reject Request Claim SPD tersebut. Setelah klik approval, masukkan username dan password windows. Approval dinyatakan berhasil jika terdapat notifikasi approval berhasil pada browser. ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://trac54/spd_is07/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://trac54/spd_is07/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPD/approvalSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPD/approvalSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if ((spd.status.Split('-')[0] == "26") && To == "kasir")
                {

                    string mailAddress = string.Empty;
                    string NamaLengkap = string.Empty;
                    foreach (var item in KasirAsal)
                    {
                        mailAddress += item.email.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                    NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    email.Subject = "Approval Claim " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("MilisKowawaCIST@sera.astra.co.id");
                    emailMessage.Append(string.Format("Kepada Yth, "));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Bapak/Ibu {0}", NamaLengkap));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");

                    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} <b>Sudah Di-Approve Finance</b> dengan rincian sebagai berikut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
                    emailMessage.Append("<br />");
                    Content = ContentBodymail(Content, spd, claim, kar);
                    emailMessage.Append(Content);
                    emailMessage.Append("Terima kasih.");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
            }

           
           
            #region Unused
            //else if ((spd.status.Split('-')[0] == "17") && To == "GA")
            //{
            //    email.Subject = "Informasi Claim";
            //    //next to do
            //    //email.To.Add(Atasan.email);
            //    emailMessage.Append(string.Format("Kepada Yth, "));
            //    emailMessage.Append("<br />");
            //    emailMessage.Append(string.Format("Bapak/Ibu {0}", Atasan.namaLengkap));
            //    emailMessage.Append("<br />");
            //    emailMessage.Append("<br />");


            //    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} Sudah Di-Approve atasan dengan rincian sebagai berikut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
            //    emailMessage.Append("<br />");
            //    Content = ContentBodymail(Content, spd, null);
            //    emailMessage.Append(Content);
            //    emailMessage.Append("Terima kasih.");
            //    emailMessage.Append("<br />");
            //    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
            //}
            //else if ((spd.status.Split('-')[0] == "20") && To == "GA")
            //{
            //    email.Subject = "Informasi Claim";
            //    //next to do
            //    //email.To.Add(Atasan.email);
            //    emailMessage.Append(string.Format("Kepada Yth, "));
            //    emailMessage.Append("<br />");
            //    emailMessage.Append(string.Format("Bapak/Ibu {0}", Atasan.namaLengkap));
            //    emailMessage.Append("<br />");
            //    emailMessage.Append("<br />");


            //    emailMessage.Append(string.Format("Status Request SPD Bapak/Ibu {0} Sudah Di-Approve atasan dengan rincian sebagai berikut :  ", spd.namaLengkap, spd.status.Split('-')[1]));
            //    emailMessage.Append("<br />");
            //    Content = ContentBodymail(Content, spd, null);
            //    emailMessage.Append(Content);
            //    emailMessage.Append("Terima kasih.");
            //    emailMessage.Append("<br />");
            //    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
            //} 
            #endregion


            email.From = new System.Net.Mail.MailAddress("email.spd@sera.astra.co.id");

            email.Priority = System.Net.Mail.MailPriority.High;
            email.Body = emailMessage.ToString();
            email.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Send(email);
        }


        private StringBuilder ContentBodymail(StringBuilder emailMessage, trSPD spd, trClaim claim, msKaryawan kar)
        {
            //System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            //StringBuilder Content = new StringBuilder();
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("<b>Detail Karyawan </b>"));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("No. SPD : {0}", spd.noSPD));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("NRP : {0}", spd.nrp));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Nama Lengkap : {0}", spd.namaLengkap));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("No Handphone : {0}", spd.NoHP != null ? spd.NoHP : "-"));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Jabatan : {0}", kar.posisi));

            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Organisasi Unit : {0}", kar.organisasiUnit != null ? kar.organisasiUnit : "-"));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Company Code : {0}", kar.companyCode != null ? kar.companyCode : "-"));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Personel Area : {0}", kar.personelArea != null ? kar.personelArea : "-"));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Personel Sub Area : {0}", kar.pSubArea != null ? kar.pSubArea : "-"));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Cost Center Pembebanan : {0}", spd.costCenter != null ? spd.costCenter : "-"));
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("<b>Detail SPD </b>"));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Tempat Tujuan : {0}", spd.tempatTujuanLain != null ? spd.tempatTujuanLain : spd.companyCodeTujuan + " - " + spd.personelAreaTujuan + " - " + spd.pSubAreaTujuan));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Keperluan : {0}", spd.keperluanLain != null ? spd.keperluanLain : spd.msKeperluan.keperluan));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Tanggal Berangkat : {0} ", String.Format("{0:MM/dd/yyyy}", spd.tglBerangkat)));
            emailMessage.Append(string.Format("Jam Berangkat : {0} ", spd.jamBerangkat));
            emailMessage.Append(string.Format("Menit Berangkat : {0} ", spd.menitBerangkat));
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Tanggal Kembali : {0} ", String.Format("{0:MM/dd/yyyy}", spd.tglKembali)));
            emailMessage.Append(string.Format("Jam Kembali : {0} ", spd.jamKembali));
            emailMessage.Append(string.Format("Menit Kembali : {0} ", spd.menitKembali));
            if (claim != null)
            {
                emailMessage.Append("<br />");
                emailMessage.Append(string.Format("<b>Total Claim : {0} </b>", claim.total));
            }                            
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");

            return emailMessage;

        }


    }

}