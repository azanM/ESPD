using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.Linq;
using System.DirectoryServices;

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
            //dsSPDDataContext data = new dsSPDDataContext();
            using (dsSPDDataContext data = new dsSPDDataContext())
            {
                string mail = string.Empty;
                //IMultipleResults result = data.stpGetKaryawanEmail(LoginID);

                //stpGetKaryawanEmailResult hasil = (stpGetKaryawanEmailResult)data.stpGetKaryawanEmail(LoginID).SingleOrDefault();
                stpGetKaryawanEmailResult hasil = new stpGetKaryawanEmailResult();
                msKaryawan karyawanQ = new msKaryawan();

                #region getemailfromAD

                //string uName = "";
                //string uid = HttpContext.Current.User.Identity.Name.Replace(@"TRAC\", "");
                //DirectorySearcher dirSearcher = new DirectorySearcher();
                //DirectoryEntry entry = new DirectoryEntry(dirSearcher.SearchRoot.Path);
                ////dirSearcher.Filter = "(&(objectClass=user)(objectcategory=person)(mail=" + uid + "*))";
                //dirSearcher.Filter = String.Format("(sAMAccountName={0})", "wawan010193");

                //SearchResult srEmail = dirSearcher.FindOne();

                //string propName = "mail";
                //ResultPropertyValueCollection valColl = srEmail.Properties[propName];
                //string cek = "";
                //foreach (string propNama in srEmail.Properties.PropertyNames)
                //{
                //    ResultPropertyValueCollection valueCollection =
                //    srEmail.Properties[propNama];
                //    foreach (Object propertyValue in valueCollection)
                //    {
                //        cek =Convert.ToString(propertyValue);
                //        if(cek.Contains("1706"))
                //        {
                //            cek = cek + "0";
                //        }
                //        if (cek.Contains("1138"))
                //        {
                //            cek = cek + "0";
                //        }
                //        if (cek.ToUpper().Contains("ENDARMAWAN.WIBISONO@SERA.ASTRA.CO.ID"))
                //        {
                //            cek = cek + "0";
                //        }
                //    }
                //}
                #endregion getemailfromAD


                if (LoginID.ToLower() == "iusr")
                {
                    karyawanQ.nrp = "99999999";
                    karyawanQ.golongan = "III";
                    karyawanQ.coCd = "0001";
                    karyawanQ.kodePSubArea = "0001";
                    karyawanQ.kodePA = "0001";

                }
                else if (LoginID.ToLower() == "spd")
                {
                    karyawanQ.nrp = "99999999";
                    karyawanQ.golongan = "III";
                    karyawanQ.coCd = "0100";
                    karyawanQ.kodePSubArea = "0001";
                    karyawanQ.kodePA = "0001";

                }
                else if (LoginID.ToLower() == "is08")
                {
                    karyawanQ = (from q in data.msKaryawans
                                 where q.nrp == "99999999"
                                 select q).FirstOrDefault();
                }
                else if (LoginID.ToLower() == "hp")
                {
                    karyawanQ.nrp = "99999999";
                    karyawanQ.golongan = "III";
                    karyawanQ.coCd = "0100";
                    karyawanQ.kodePSubArea = "0001";
                    karyawanQ.kodePA = "0001";
                }
                else
                {
                   
                        hasil = (stpGetKaryawanEmailResult)data.stpGetKaryawanEmail(LoginID).SingleOrDefault();
                 

                    //data golongan kosong.makanya diisi manual disini

                    if (hasil != null)
                    {
                        //start change TAS
                        karyawanQ = (from q in data.msKaryawans
                                     where q.EMail.ToUpper() == hasil.EmailID.ToUpper()
                                     select q).FirstOrDefault();
                        #region untuk testing
                        //karyawanQ = (from q in data.msKaryawans
                        //             where q.nrp == "99999999"
                        //             select q).FirstOrDefault();
                        #endregion
                        //end change TAS

                        if (karyawanQ.golongan == string.Empty)
                        {
                            karyawanQ.golongan = "I";

                        }

                        if (karyawanQ.posisi.ToLower().Contains("dept"))
                        {
                            karyawanQ.posisi = "Kadept";

                        }
                        else

                            if (karyawanQ.posisi.ToLower().Contains("Branch Manager"))
                            {
                                karyawanQ.posisi = "BM";

                            }
                            else

                                if (karyawanQ.posisi.ToLower().Contains("div") || karyawanQ.posisi.ToLower().Contains("Manager"))
                                {
                                    karyawanQ.posisi = "Kadiv/OM";

                                }
                                else

                                    if (karyawanQ.posisi.ToLower().Contains("dire"))
                                    {
                                        karyawanQ.posisi = "Direksi";
                                    }
                                    else
                                    {
                                        karyawanQ.posisi = "Karyawan";
                                    }
                    }
                }

                //var dts = result.GetResult<DateTime>();

                return karyawanQ;
            }
        }

        public void sendMail(trSPD spd, string To, msKaryawan kar)
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
            if (kar.nrp == "99999999")
            {
                Pengirim = kar;
                //Pengirim.nrp = kar.nrp;
                //Pengirim.job = kar.job;
                //Pengirim.golongan = kar.golongan;
                Pengirim.EMail = spd.email;
            }
            //kalau sekretaris mbikinin untuk direksi
            var user = (from u in data.msUsers
                        where (u.roleId == Konstan.DIREKTUR || u.roleId == Konstan.PRESIDEN_DIREKTUR)
                        select u.nrp);
            if (user.Contains(Pengirim.nrp))
            {
                Pengirim.EMail = "STEFANIE@SERA.ASTRA.CO.ID";
                if (To == "Pembuat")
                {
                    email.CC.Add("ERNA.STAFITRI@SERA.ASTRA.CO.ID");
                    email.CC.Add("SINTA.KRISNITAWATI@SERA.ASTRA.CO.ID");
                }
            }


            //hardcode untuk TRAC HO yang melakukan SPD, GAnya masi ke yulia
            //if (Pengirim.kodePA == "1000")
            //{
            //    Pengirim.kodePA = "1";
            //}
            //if (Pengirim.kodePSubArea == "1000")
            //{
            //    Pengirim.kodePSubArea = "1";
            //}

            //ditutup dulu. nanti klalu sudah implemen ke cabang baru dibuka lagi
            //var gaAsal = (from k in data.msKaryawans
            //              join u in data.msUsers on k.nrp equals u.nrp
            //              where u.roleId == 17 && k.coCd == Pengirim.coCd && k.kodePA == Pengirim.kodePA && k.kodePSubArea == Pengirim.kodePSubArea
            //              select k);
            //sementara makae ini dulu

            //msKaryawan atasanGaAsal = (from k in data.msKaryawans
            //                           where k.nrp == gaAsal.nrpAtasan
            //                           select k).SingleOrDefault();
            //var KasirAsal = (from k in data.msKaryawans
            //                 join u in data.msUsers on k.nrp equals u.nrp
            //                 //where u.roleId == 20 && k.coCd == Pengirim.coCd && k.kodePA == Pengirim.kodePA && k.kodePSubArea == Pengirim.kodePSubArea
            //                 where u.nrp == "1704" || u.nrp == "2076" || u.nrp == "1706" || u.nrp == "1547" || u.nrp == "201"
            //                 select k);
            //var FinanceAsal = (from k in data.msKaryawans
            //                   join u in data.msUsers on k.nrp equals u.nrp
            //                   //where u.roleId == 19 && k.coCd == Pengirim.coCd && k.kodePA == Pengirim.kodePA && k.kodePSubArea == Pengirim.kodePSubArea
            //                   where u.nrp == "1704" || u.nrp == "2076" || u.nrp == "1706" || u.nrp == "1547" || u.nrp == "201"
            //                   select k);

            //var gaAsal = (object)null;
            //var KasirAsal = (object)null;
            //var FinanceAsal = (object)null;
            var gaAsal = (from k in data.msKaryawans
                          join u in data.msUsers on k.nrp equals u.nrp
                          where u.roleId == 17 && k.coCd == kar.coCd && k.kodePA == kar.kodePA && k.kodePSubArea == kar.kodePA
                          select k);
            var KasirAsal = (from k in data.msKaryawans
                             join u in data.msUsers on k.nrp equals u.nrp
                             where u.roleId == 20 && k.coCd == kar.coCd && k.kodePA == kar.kodePA && k.kodePSubArea == kar.kodePA
                             select k);
            var FinanceAsal = (from k in data.msKaryawans
                               join u in data.msUsers on k.nrp equals u.nrp
                               where u.roleId == 19 && k.coCd == kar.coCd && k.kodePA == kar.kodePA && k.kodePSubArea == kar.kodePA
                               select k);





            if (gaAsal.Count() == 0)
            {
                gaAsal = (from k in data.msKaryawans
                          join u in data.msUsers on k.nrp equals u.nrp
                          where u.roleId == 17 && k.coCd == "0001" && k.kodePA == "0001" && k.kodePSubArea == "0001"
                          select k);

            }
            if (KasirAsal.Count() == 0)
            {
                KasirAsal = (from k in data.msKaryawans
                             join u in data.msUsers on k.nrp equals u.nrp
                             where u.roleId == 19 && k.coCd == "0001" && k.kodePA == "0001" && k.kodePSubArea == "0001"
                             select k);
            }
            if (FinanceAsal.Count() == 0)
            {
                FinanceAsal = (from k in data.msKaryawans
                               join u in data.msUsers on k.nrp equals u.nrp
                               where u.roleId == 19 && k.coCd == "0001" && k.kodePA == "0001" && k.kodePSubArea == "0001"
                               select k);
            }

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
                    mailAddress += item.EMail.Trim() + ",";
                    NamaLengkap += item.namaLengkap + "/";
                }
                if (mailAddress.Length > 0)
                {
                    mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                    NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    email.To.Add(mailAddress);
                }
                email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
            else
            {
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
                        mailAddress += item.EMail.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    if (mailAddress.Length > 0)
                    {
                        mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                        NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                        email.To.Add(mailAddress);
                    }
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    //emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "atasan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));

                }

                else if ((spd.status.Split('-')[0] == "3" || spd.status.Split('-')[0] == "6" || spd.status.Split('-')[0] == "8"
                    || spd.status.Split('-')[0] == "12") && To == "Pembuat")
                {
                    email.Subject = "[Informasi SPD Atasan Langsung] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.EMail.Trim());
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                else if ((spd.status.Split('-')[0] == "7" || spd.status.Split('-')[0] == "9"
                 || spd.status.Split('-')[0] == "13") && To == "Pembuat")
                {
                    email.Subject = "[Informasi Approval SPD Atasan Tujuan] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.EMail.Trim());
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                        mailAddress += item.EMail.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    if (mailAddress.Length > 0)
                    {
                        mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                        NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    }
                    email.To.Add(mailAddress);
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                    email.To.Add(Tujuan.EMail.Trim());
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "tujuan", "spd"));
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2} ", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2} ", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "tujuan", "spd"));
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "tujuan", "spd"));
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
                        mailAddress += item.EMail.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    if (mailAddress.Length > 0)
                    {
                        mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                        NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    }
                    email.Subject = "[Approval Claim Atasan Langsung] " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "atasan", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));
                }
                else if ((spd.status.Split('-')[0] == "14" || spd.status.Split('-')[0] == "16"
                    || spd.status.Split('-')[0] == "18") && To == "Pembuat")
                {
                    email.Subject = "[Informasi Claim Atasan Langsung] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.EMail.Trim());
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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

                else if ((spd.status.Split('-')[0] == "15" || spd.status.Split('-')[0] == "17"
                    || spd.status.Split('-')[0] == "19") && To == "Pembuat")
                {
                    email.Subject = "[Informasi Claim GA] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.EMail.Trim());
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                    email.Subject = "[Informasi Claim Finance] " + spd.noSPD + "-" + spd.namaLengkap + "-" + spd.status.Split('-')[1];
                    email.To.Add(Pengirim.EMail.Trim());
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                        mailAddress += item.EMail.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    if (mailAddress.Length > 0)
                    {
                        mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                        NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    }
                    email.Subject = "[Approval Claim GA] " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "ga", "claim"));
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
                        mailAddress += item.EMail.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    if (mailAddress.Length > 0)
                    {
                        mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                        NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    }
                    email.Subject = "[Approval Claim Finance] " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    //emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("<a href='http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("Approve : http://trac39/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("Reject : http://trac39/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Apabila Bapak/ Ibu berada di luar kantor bisa menggunakan link dibawah ini ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}'>Approve</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(" ");
                    emailMessage.Append("|");
                    emailMessage.Append(" ");
                    //emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("<a href='http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}'>Reject</a>", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    emailMessage.Append("<br />");
                    emailMessage.Append("Jika link di atas tidak dapat diklik, copy dan gunakan URL dibawah ini di browser: ");
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("Approve : http://118.97.80.12/SPDService/appSPD.ashx?conf=approve&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append("<br />");
                    //emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
                    emailMessage.Append(string.Format("Reject : http://118.97.80.12/SPDService/appSPD.ashx?conf=reject&nospd={0}&subject={1}&type={2}", spd.noSPD, "finance", "claim"));
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
                        mailAddress += item.EMail.Trim() + ",";
                        NamaLengkap += item.namaLengkap + "/";
                    }
                    if (mailAddress.Length > 0)
                    {
                        mailAddress = mailAddress.Remove(mailAddress.Length - 1);
                        NamaLengkap = NamaLengkap.Remove(NamaLengkap.Length - 1);
                    }
                    email.Subject = "Approval Claim " + spd.noSPD + "-" + spd.namaLengkap;
                    email.To.Add(mailAddress);
                    email.Bcc.Add("yulia.safitri@sera.astra.co.id");
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

            ////yeah, i know... this is bad
            //var addr = new System.Net.Mail.MailAddress("Stefanie@sera.astra.co.id");
            //if (email.To.Contains(addr))
            //{
            //    email.CC.Add("sinta.krisnitawati@sera.astra.co.id");
            //    email.CC.Add("Erna.Stafitri@sera.astra.co.id");
            //}

            email.From = new System.Net.Mail.MailAddress("email.spd@sera.astra.co.id");

            email.Priority = System.Net.Mail.MailPriority.High;
            email.Body = emailMessage.ToString();
            email.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Send(email);
        }

        public void sendMailClaim(trClaim claim, msKaryawan kar, trSPD spd)
        {
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            StringBuilder emailMessage = new StringBuilder();
            StringBuilder emailMessageFrom = new StringBuilder();
            StringBuilder Content = new StringBuilder();
            dsSPDDataContext data = new dsSPDDataContext();

            if (kar.nrp == "99999999")
            {
                //Pengirim = kar;
                ////Pengirim.nrp = kar.nrp;
                ////Pengirim.job = kar.job;
                ////Pengirim.golongan = kar.golongan;
                //Pengirim.email = spd.email;
            }


               string sqlFormattedDate = spd.tglExpired.HasValue
                ? spd.tglExpired.Value.ToString("dd MMMM yyyy")
                : "<not available>"; 
            email.Subject = "Data Claim SPD Belum Lengkap " + claim.noSPD;

            email.To.Add(kar.EMail);

            email.Bcc.Add("espddeveloper@gmail.com");
            emailMessage.Append(string.Format("Kepada Yth, " + kar.namaLengkap));
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("Bapak/Ibu {0} berikut kami informasikan data claim yang belum lengkap : ", kar.namaLengkap));
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            Content = ContentClaimMail(Content, claim, kar);
            emailMessage.Append(Content);

            
       // emailMessage.Append("Jika dalam waktu 5 hari kerja dari email ini dikirim dokumen bukti fisik masih belum diterima oleh GA maka item claim tersebut di atas dinyatakan hangus.");
            emailMessage.Append("Dokumen akan ditunggu maksimal 14 hari setelah tanggal kembali SPD. Tanggal expired claim adalah " + sqlFormattedDate + ". Jika sampai dengan  " + sqlFormattedDate + "dokumen bukti fisik masih belum diterima oleh GA, maka claim tersebut di atas dinyatakan hangus.");
            emailMessage.Append("<br />");
            emailMessage.Append("Terima kasih.");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append(string.Format("<b style='color: red;'>Catatan : E-mail ini dikirim otomatis oleh Sistem Pembuatan SPD.Tidak perlu membalas E-mail ini </b>"));

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

        private StringBuilder ContentClaimMail(StringBuilder emailMessage, trClaim claim, msKaryawan kar)
        {
            //update penambahan 26 september 2018
            if (claim.uangSaku_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti uang saku");
                emailMessage.Append("<br />");
            }

            if (claim.hotelTanpaPenginapan_cek == true)
            {
                emailMessage.Append("- " + "Mohon dibuatkan berita acara dengan approval ADH cabang tujuan");
                emailMessage.Append("<br />");
            }//

            if (claim.tiket_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti Tiket");
                emailMessage.Append("<br />");
            }
            if (claim.hotel_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti Penginapan");
                emailMessage.Append("<br />");
            }

            if (claim.laundry_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti Laundry");
                emailMessage.Append("<br />");
            }
            if (claim.komunikasi_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti Komunikasi");
                emailMessage.Append("<br />");
            }
            if (claim.airportTax_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti AirPort Tax");
                emailMessage.Append("<br />");
            }
            if (claim.BBM_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti BBM");
                emailMessage.Append("<br />");
            }
            if (claim.tol_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti TOL");
                emailMessage.Append("<br />");
            }
            if (claim.taxi_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti Taxi");
                emailMessage.Append("<br />");
            }
            if (claim.parkir_cek == true)
            {
                emailMessage.Append("- " + "Lampiran / bukti Parkir");
                emailMessage.Append("<br />");
            }
            return emailMessage;
        }


    }

}