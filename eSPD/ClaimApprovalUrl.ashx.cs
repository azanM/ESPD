using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace eSPD
{
    /// <summary>
    /// Summary description for ClaimApprovalUrl
    /// </summary>
    public class ClaimApprovalUrl : IHttpHandler
    {
        private classSpd cs = new classSpd();
        string errorMessage = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            string NoSPD = Encrypto.Decrypt(context.Request["sCrypt"]);
            string EmailAppproval = Encrypto.Decrypt(context.Request["eCrypt"]);
            string NrpApproval = Encrypto.Decrypt(context.Request["nCrypt"]);
            string Action = Encrypto.Decrypt(context.Request["Action"]);
            string ClaimApprove = context.Request["ClaimApprove"];

            context.Response.ContentType = "text/plain";
            context.Response.Write(ChangeStatus(NoSPD, Action, NrpApproval, ClaimApprove));
        }

        public class ListGenerateTextModel
        {
            public string NoSPD { get; set; }
            public string NamaFile { get; set; }
            public string Type { get; set; }
            public DateTime Tanggal { get; set; }
            public string Message { get; set; }
            public string BPH { get; set; }
            public string FilePath { get; set; }

        }

        public string ChangeStatus(string nospd, string action, string nrp, string claimapprove)
        {
            using (var ctx = new dsSPDDataContext())
            {
                try
                {
                    // get data spd & claim
                    var dataClaim = ctx.trClaims.FirstOrDefault(o => o.noSPD == nospd);
                    var dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == nospd);

                    if (dataClaim != null && dataSpd != null)
                    {
                        // get user creator nrp

                        // approve
                        if (action.ToLower().Contains("approve"))
                        {
                            if (dataClaim.status.ToLower().Contains("expired"))
                            {
                                return "Claim Expired, sehingga tidak bisa di Approved.";
                            }

                            switch (claimapprove.ToLower())
                            {
                                case "atasan":
                                    // jika sudah pernah diapproved/reject return reject alert
                                    if (dataClaim.isApprovedAtasan != null)
                                    {
                                        string stringStatus = dataClaim.isApprovedAtasan.Value == true ? "approve" : "reject";
                                        if (dataClaim.isApprovedAtasanDate == null) dataClaim.isApprovedAtasanDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah " + stringStatus + " pada : " + dataClaim.isApprovedAtasanDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    // jika sudah di cancel return cancel alert
                                    if (dataClaim.isCancel == true)
                                    {
                                        if (dataClaim.isCancelDate == null) dataClaim.isCancelDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah dicancel pada : " + dataClaim.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }

                                    dataClaim.isApprovedAtasan = true;
                                    dataClaim.isApprovedAtasanDate = DateTime.Now;
                                    dataClaim.ApprovedAtasanBy = nrp;
                                    dataClaim.status = "Menunggu approval GA";

                                    //    var role = (from p in ctx.msUsers
                                    //        where p.roleId == 17
                                    //   select p.nrp).ToList();

                                    //   var dataGa = ctx.msKaryawans
                                    //     .Where(o =>
                                    //       o.coCd == "0001" &&
                                    //      role.Contains(o.nrp));
                                    // foreach (var item in dataGa)
                                    // {
                                    //     EmailCore.ApprovalClaim(item.nrp, dataClaim.noSPD, dataSpd, "ga", dataClaim.status);
                                    // }

                                    Helper hp = new Helper();
                                    DataSet dsCheckBcc = hp.wsEmail("claim");
                                    if (dsCheckBcc.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow drCheckBcc = dsCheckBcc.Tables[0].Rows[0];
                                        DataView dvCheckBcc = new DataView(dsCheckBcc.Tables[0]);
                                        for (int i = 0; i < dvCheckBcc.Count; i++)
                                        {
                                            //  bccAddress += drCheckBcc.Table.Rows[i][0].ToString();
                                            // bccAddress += ",";
                                            string nrpGA = drCheckBcc.Table.Rows[i][1].ToString();

                                            EmailCore.ApprovalClaim(nrpGA, dataClaim.noSPD, dataSpd, "ga", dataClaim.status);
                                        }

                                    }



                                    EmailCore.InformasiClaim(dataSpd.dibuatOleh, dataClaim.noSPD, dataSpd, dataClaim.status);

                                    break;
                                case "ga":
                                    // jika sudah pernah diapproved/reject return reject alert
                                    if (dataClaim.isApprovedGA != null)
                                    {
                                        string stringStatus = dataClaim.isApprovedGA.Value == true ? "approve" : "reject";
                                        if (dataClaim.isApprovedGADate == null) dataClaim.isApprovedGADate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah " + stringStatus + " pada : " + dataClaim.isApprovedGADate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    // jika sudah di cancel return cancel alert
                                    if (dataClaim.isCancel == true)
                                    {
                                        if (dataClaim.isCancelDate == null) dataClaim.isCancelDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah dicancel pada : " + dataClaim.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }

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

                                        //EmailCore.ApprovalClaim(item.nrp, dataClaim.noSPD, dataSpd, "finance", dataClaim.status);
                                    }

                                    // Tambahan Aldo Generate txt
                                    var dc = new dsSPDDataContext();
                                    var query = (from a in dc.trClaims
                                                 join b in dc.trSPDs on a.noSPD equals b.noSPD
                                                 where a.noSPD == nospd
                                                 select new
                                                 {
                                                     NoSpd = a.noSPD,
                                                     Status = "S",
                                                     approvaltgl = DateTime.Now.Date,
                                                     NRP = b.nrp,
                                                     NamaLengkap = b.namaLengkap,
                                                     CostCenter = b.costCenter,
                                                     UangMakan = a.biayaMakan,
                                                     UangSaku = a.uangSaku,
                                                     Tiket = a.tiket,
                                                     Hotel = a.hotel,
                                                     Laundry = a.laundry,
                                                     Komunikasi = a.komunikasi,
                                                     AirPortTax = a.airportTax,
                                                     BBM = a.BBM,
                                                     Tol = a.tol,
                                                     Taxi = a.taxi,
                                                     Parkir = a.parkir,
                                                     Biayalainlain = a.biayaLainLain,
                                                     UangMuka = b.uangMuka,
                                                     TglBerangkat = b.tglBerangkat,
                                                     TglKembali = b.tglKembali
                                                 }).FirstOrDefault();

                                    string approvalDay = DateTime.Now.Day.ToString();
                                    string approvalMonth = DateTime.Now.Month.ToString();
                                    string approvalYear = DateTime.Now.Year.ToString();
                                    string _approvaltgl = approvalDay + "." + approvalMonth + "." + approvalYear;

                                    string _CostCenter = string.Empty;
                                    string message = "";
                                    message += query.NoSpd + "|";
                                    message += query.Status + "|";
                                    message += _approvaltgl.Trim() + "|";
                                    message += query.NRP + "|";
                                    message += query.NamaLengkap + "|";

                                    if (query.CostCenter.Split('-').Count() > 0)
                                    {
                                        _CostCenter = query.CostCenter.Split('-')[0];
                                    }
                                    else
                                    {
                                        _CostCenter = query.CostCenter;
                                    }

                                    message += _CostCenter + "|";
                                    message += query.UangMakan + "|";
                                    message += query.UangSaku + "|";
                                    message += query.Tiket + "|";
                                    message += query.Hotel + "|";
                                    message += query.Laundry + "|";
                                    message += query.Komunikasi + "|";
                                    message += query.AirPortTax + "|";
                                    message += query.BBM + "|";
                                    message += query.Tol + "|";
                                    message += query.Taxi + "|";
                                    message += query.Parkir + "|";
                                    message += query.Biayalainlain + "|";
                                    message += query.UangMuka + "|";
                                    //message += query.TglBerangkat.Day + "." + query.TglBerangkat.Month + "." + query.TglBerangkat.Year + "|";
                                    //message += query.TglKembali.Day + "." + query.TglKembali.Month + "." + query.TglKembali.Year;

                                    message += query.TglBerangkat.Day + "." + query.TglBerangkat.Month + "." + query.TglBerangkat.Year + "|"; //update 12 oktober 2018
                                    message += query.TglKembali.Day + "." + query.TglKembali.Month + "." + query.TglKembali.Year; //update 12 oktober 2018

                                    string tahun = DateTime.Now.Year.ToString();
                                    string bulan = DateTime.Now.Month.ToString();
                                    string tanggal = DateTime.Now.Day.ToString();
                                    string jam = DateTime.Now.Hour.ToString();
                                    string menit = DateTime.Now.Minute.ToString();
                                    string detik = DateTime.Now.Second.ToString();
                                    string milisec = DateTime.Now.Millisecond.ToString();

                                    string datetime = tahun + bulan + tanggal + "-" + jam + menit + detik + "-" + milisec;
                                    string filename = "Settlement_" + datetime + ".txt";

                                    //string _User = Encrypto.Decrypt(ConfigurationManager.AppSettings["Username"]);
                                    //string _Pass = Encrypto.Decrypt(ConfigurationManager.AppSettings["Password"]);
                                    //string _Domain = Encrypto.Decrypt(ConfigurationManager.AppSettings["Domain"]);

                                    string _User = ConfigurationManager.AppSettings["Username"];
                                    string _Pass = ConfigurationManager.AppSettings["Password"];
                                    string _Domain = ConfigurationManager.AppSettings["Domain"];

                                    string _PathSource = ConfigurationManager.AppSettings["GenerateText"];
                                    string _PathGenerateResp = ConfigurationManager.AppSettings["PathGenerateResp"];
                                    string _BackupSett = ConfigurationManager.AppSettings["BackupSett"];

                                    string filePath = string.Format("{0}\\{1}", _PathSource, filename);
                                    using (StreamWriter writer = new StreamWriter(filePath, true))
                                    {
                                        writer.WriteLine(message);
                                        writer.Close();
                                    }

                                    string backUpFile = _BackupSett + filename;
                                    string sourceFile = _PathSource + filename;
                                    string destinationFile = _PathGenerateResp + filename;                                   
                                    bool ExistsFile = false;

                                    using (new Impersonator(_User, _Domain, _Pass))
                                    {
                                        File.Copy(sourceFile, destinationFile);
                                        if (File.Exists(destinationFile))
                                        {
                                            ExistsFile = true;
                                        }
                                    }

                                    if (ExistsFile == true)
                                    {
                                        File.Move(sourceFile, backUpFile);
                                        ExistsFile = false;
                                    }

                                    using (new Impersonator(_User, _Domain, _Pass))
                                    {
                                        string _PathSapResp = ConfigurationManager.AppSettings["PathSapResp"];
                                        string _PathArchiveSapResp = ConfigurationManager.AppSettings["PathArchiveSapResp"];
                                        List<ListGenerateTextModel> textList = new List<ListGenerateTextModel>();

                                        bool isFileFound = false;
                                        for (int i = 0; i <= 4; i++)
                                        {
                                            foreach (string file in Directory.EnumerateFiles(_PathSapResp, "*.txt"))
                                            {
                                                string contents = File.ReadAllText(file);
                                                List<string> cols = contents.Split(new char[] { '|' }).ToList();
                                                List<string> fileNameList = file.Split(new char[] { '\\' }).ToList();
                                                List<string> textName = fileNameList.LastOrDefault().Split(new char[] { '_' }).ToList();

                                                string tgl = textName.LastOrDefault().Remove(textName.LastOrDefault().Length - 4, 4);

                                                var fileDate = DateTime.ParseExact(tgl, "yyyyMMdd-HHmmss-fff", CultureInfo.InvariantCulture);
                                                string noSPD = cols[0];
                                                string type = cols[1];
                                                string messageBPH = cols.LastOrDefault();
                                                List<string> errorBPHClaims = messageBPH.Split(new char[] { ' ' }).ToList();

                                                string bphNo = errorBPHClaims[4].ToString();

                                                textList.Add(new ListGenerateTextModel() { NoSPD = noSPD, NamaFile = Path.GetFileName(file), Type = type, Tanggal = fileDate, Message = messageBPH, BPH = bphNo, FilePath = file });
                                                

                                            }

                                            var data = textList
                                                    .Where(m => m.NoSPD.Contains(dataClaim.noSPD))
                                                    .Where(m => m.Type.Contains("S"))
                                                    .OrderByDescending(m => m.Tanggal)
                                                    .Select(m => new
                                                    {
                                                        m.NamaFile,
                                                        m.FilePath,
                                                        m.Message,
                                                        m.BPH,
                                                        m.Tanggal
                                                    }).ToList();

                                            if(data.Count > 0)
                                            {
                                                isFileFound = true;
                                                string archiveSapRespFile;

                                                if (!data[0].Message.ToLower().Contains("successfully"))
                                                {
                                                    dataClaim.errorBPHClaim = data[0].Message;
                                                    ctx.SubmitChanges();

                                                    foreach(var fileMoveFailed in data)
                                                    {
                                                        archiveSapRespFile = _PathArchiveSapResp + fileMoveFailed.NamaFile;

                                                        File.Move(fileMoveFailed.FilePath, archiveSapRespFile);
                                                    }

                                                    return "Claim " + nospd + " gagal di " + action + " karena " + data[0].Message;
                                                }

                                                dataClaim.isApprovedGA = true;
                                                dataClaim.isApprovedGADate = DateTime.Now;
                                                dataClaim.ApprovedGABy = nrp;
                                                dataClaim.status = "Menunggu approval Finance";

                                                dataClaim.errorBPHClaim = data[0].Message;
                                                dataClaim.BPHClaim = data[0].BPH;

                                                foreach(var fileMoveSuccess in data)
                                                {
                                                    archiveSapRespFile = _PathArchiveSapResp + fileMoveSuccess.NamaFile;

                                                    File.Move(fileMoveSuccess.FilePath, archiveSapRespFile);
                                                }

                                                break;
                                            }

                                            if (isFileFound)
                                                break;

                                            Thread.Sleep(5000);
                                        }

                                        if (!isFileFound)
                                            return "Claim " + nospd + " gagal di " + action + " karena file SAP tidak ditemukan, silahkan mencoba kembali.";
                                    }

                                    EmailCore.InformasiClaim(dataSpd.dibuatOleh, dataClaim.noSPD, dataSpd, dataClaim.status);

                                    break;
                                case "finance":
                                    // jika sudah pernah diapproved/reject return reject alert
                                    if (dataClaim.isApprovedFinance != null)
                                    {
                                        string stringStatus = dataClaim.isApprovedFinance.Value == true ? "approve" : "reject";
                                        if (dataClaim.isApprovedFinanceDate == null) dataClaim.isApprovedFinanceDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah " + stringStatus + " pada : " + dataClaim.isApprovedFinanceDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    // jika sudah di cancel return cancel alert
                                    if (dataClaim.isCancel == true)
                                    {
                                        if (dataClaim.isCancelDate == null) dataClaim.isCancelDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah dicancel pada : " + dataClaim.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }

                                    dataClaim.isApprovedFinance = true;
                                    dataClaim.isApprovedFinanceDate = DateTime.Now;
                                    dataClaim.ApprovedFinanceBy = nrp;
                                    dataClaim.status = "Finance Approved";

                                    EmailCore.InformasiClaim(dataSpd.dibuatOleh, dataClaim.noSPD, dataSpd, dataClaim.status);

                                    break;
                                default:
                                    break;
                            }

                        }
                        else if (action.ToLower().Contains("reject")) //reject
                        {
                            switch (claimapprove.ToLower())
                            {
                                case "atasan":
                                    // jika sudah pernah diapproved/reject return reject alert
                                    if (dataClaim.isApprovedAtasan != null)
                                    {
                                        string stringStatus = dataClaim.isApprovedAtasan.Value == true ? "approve" : "reject";
                                        if (dataClaim.isApprovedAtasanDate == null) dataClaim.isApprovedAtasanDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah " + stringStatus + " pada : " + dataClaim.isApprovedAtasanDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    // jika sudah di cancel return cancel alert
                                    if (dataClaim.isCancel == true)
                                    {
                                        if (dataClaim.isCancelDate == null) dataClaim.isCancelDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah dicancel pada : " + dataClaim.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    dataClaim.ApprovedAtasanBy = nrp;
                                    dataClaim.isApprovedAtasan = false;
                                    dataClaim.isApprovedAtasanDate = DateTime.Now;

                                    break;
                                case "ga":
                                    // jika sudah pernah diapproved/reject return reject alert
                                    if (dataClaim.isApprovedGA != null)
                                    {
                                        string stringStatus = dataClaim.isApprovedGA.Value == true ? "approve" : "reject";
                                        if (dataClaim.isApprovedGADate == null) dataClaim.isApprovedGADate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah " + stringStatus + " pada : " + dataClaim.isApprovedGADate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    // jika sudah di cancel return cancel alert
                                    if (dataClaim.isCancel == true)
                                    {
                                        if (dataClaim.isCancelDate == null) dataClaim.isCancelDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah dicancel pada : " + dataClaim.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    dataClaim.ApprovedGABy = nrp;
                                    dataClaim.isApprovedGA = false;
                                    dataClaim.isApprovedGADate = DateTime.Now;

                                    break;
                                case "finance":
                                    // jika sudah pernah diapproved/reject return reject alert
                                    if (dataClaim.isApprovedFinance != null)
                                    {
                                        string stringStatus = dataClaim.isApprovedFinance.Value == true ? "approve" : "reject";
                                        if (dataClaim.isApprovedFinanceDate == null) dataClaim.isApprovedFinanceDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah " + stringStatus + " pada : " + dataClaim.isApprovedFinanceDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    // jika sudah di cancel return cancel alert
                                    if (dataClaim.isCancel == true)
                                    {
                                        if (dataClaim.isCancelDate == null) dataClaim.isCancelDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah dicancel pada : " + dataClaim.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    dataClaim.ApprovedFinanceBy = nrp;
                                    dataClaim.isApprovedFinance = false;
                                    dataClaim.isApprovedFinanceDate = DateTime.Now;

                                    break;
                                default:
                                    break;
                            }

                            dataClaim.status = "Rejected by " + claimapprove;

                            EmailCore.InformasiClaim(dataSpd.dibuatOleh, dataClaim.noSPD, dataSpd, dataClaim.status);

                        }
                        else if (action.ToLower().Contains("cancel")) //cancel
                        {
                            switch (claimapprove.ToLower())
                            {
                                case "ga":
                                    // jika sudah di cancel return cancel alert
                                    if (dataClaim.isCancel == true)
                                    {
                                        if (dataClaim.isCancelDate == null) dataClaim.isCancelDate = DateTime.Now;
                                        return "approval claim tidak bisa dieksekusi, karena claim No Spd : " + nospd + "  telah dicancel pada : " + dataClaim.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                                    }
                                    dataClaim.isCancel = true;
                                    dataClaim.isCancelDate = DateTime.Now;
                                    dataClaim.status = "Cancel by " + claimapprove;

                                    EmailCore.InformasiClaim(dataSpd.dibuatOleh, dataClaim.noSPD, dataSpd, dataClaim.status);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if(action.ToLower().Contains("complete"))
                        {
                            switch (claimapprove.ToLower())
                            {
                                case "ga":
                                    dataClaim.TidakExpired = true;
                                    ctx.SubmitChanges();
                                    return nospd + " Expired berhasil diperpanjang";
                                default:
                                    break;
                            }
                        }

                        ctx.SubmitChanges();
                        return "Claim " + nospd + " berhasil di " + action + " status saat ini berubah menjadi " + dataClaim.status;
                    }
                    else
                    {
                        setErrorLog("Data claim atau spd tidak ditemukan", nospd);
                        return "Claim " + nospd + " gagal di" + action + ", data tidak ditemukan";
                    }
                }
                catch (Exception e)
                {
                    setErrorLog(e, nospd);

                    return nospd + " _ " + e.Message;
                }
            }
        }

        void setErrorLog(object ex, string nospd)
        {
            try
            {
                string path = @"E:\SPD\ErrorMail\ErrorApproveClaim_" + nospd + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt";
                if (!File.Exists(path))
                {
                    (new FileInfo(path)).Directory.Create();
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(ex.ToString() + Environment.NewLine);
                        tw.Close();
                    }
                }
                else
                {
                    using (StreamWriter tw = File.AppendText(path))
                    {
                        tw.WriteLine(ex.ToString() + Environment.NewLine);
                        tw.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
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