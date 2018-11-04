using eSPD.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Net;
using System.Threading;
using System.Globalization;

namespace eSPD
{
    /// <summary>
    /// Summary description for ApprovalSPDUrl
    /// </summary>
    public class ApprovalSPDUrl : IHttpHandler
    {
        string errorMessage = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            string NoSPD = Encrypto.Decrypt(context.Request["sCrypt"]);
            string EmailAppproval = Encrypto.Decrypt(context.Request["eCrypt"]);
            string NrpApproval = Encrypto.Decrypt(context.Request["nCrypt"]);
            string index = context.Request["index"];
            string Action = Encrypto.Decrypt(context.Request["Action"]);

            context.Response.ContentType = "text/plain";
            context.Response.Write(ChangeStatus(NoSPD, Action, NrpApproval, index));
        }

        #region change status
        public string ChangeStatus(string nospd, string action, string nrp, string index)
        {
            using (var ctx = new dsSPDDataContext())
            {
                // get data spd
                var dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == nospd);

                // get approval count buat spd
                var approvalCount = ctx.ApprovalStatus.Where(o => o.NoSPD == nospd).Count();

                if (dataSpd == null) errorMessage += "SPD tidak ditemukan, kemungkinan data lama, jadi spd ini tidak bisa di " + action;
                if (approvalCount == 0) errorMessage += "Approval " + nospd + " tidak ditemukan, kemungkinan data lama, jadi spd ini tidak bisa di " + action;

                // jika tidak ada error message
                if (string.IsNullOrEmpty(errorMessage))
                {
                    switch (action.ToLower())
                    {
                        case "cancel":
                            // jika sudah di cancel return cancel alert
                            if (dataSpd.isCancel == true)
                            {
                                if (dataSpd.isCancelDate == null) dataSpd.isCancelDate = DateTime.Now;
                                return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + "  telah di" + action + " pada : " + dataSpd.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                            }

                            //cancel method
                            CancelSPD(dataSpd, nrp, nospd);

                            break;
                        case "reject":
                            // jika sudah di cancel return cancel alert
                            if (dataSpd.isCancel == true)
                            {
                                if (dataSpd.isCancelDate == null) dataSpd.isCancelDate = DateTime.Now;
                                return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + "  telah dicancel pada : " + dataSpd.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                            }

                            // jika sudah di reject return reject alert
                            if (dataSpd.isApproved == false)
                            {
                                if (dataSpd.isApprovedDate == null) dataSpd.isApprovedDate = DateTime.Now;
                                return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + "  telah di" + action + " pada : " + dataSpd.isApprovedDate.Value.ToString("dd MMMM yyyy HH:mm");
                            }

                            //reject method
                            string status = RejectSPD(dataSpd, nrp, nospd, index);

                            if (!string.IsNullOrEmpty(status))
                            {
                                return status;
                            }

                            break;
                        case "approve":
                            // jika sudah di reject return reject alert
                            if (dataSpd.isApproved != null)
                            {
                                string stringStatus = dataSpd.isApproved.Value == true ? "approve" : "reject";
                                if (dataSpd.isApprovedDate == null) dataSpd.isApprovedDate = DateTime.Now;
                                return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + "  telah " + stringStatus + " pada : " + dataSpd.isApprovedDate.Value.ToString("dd MMMM yyyy HH:mm");
                            }

                            // jika sudah di cancel return cancel alert
                            if (dataSpd.isCancel == true)
                            {
                                if (dataSpd.isCancelDate == null) dataSpd.isCancelDate = DateTime.Now;
                                return "approval tidak bisa dieksekusi, karena No Spd : " + nospd + "  telah dicancel pada : " + dataSpd.isCancelDate.Value.ToString("dd MMMM yyyy HH:mm");
                            }

                            //approve method
                            string status2 = ApproveSPD(dataSpd, nrp, nospd, index);

                            if (!string.IsNullOrEmpty(status2))
                            {
                                return status2;
                            }

                            break;
                        default:
                            break;
                    }

                    return "No Spd : " + nospd + " berhasil di" + action;
                }
                else // jika error return error
                {
                    return errorMessage;
                }
            }
        }
        #endregion

        #region cancel
        private void CancelSPD(trSPD dataSpd, string nrp, string nospd)
        {
            using (var ctx = new dsSPDDataContext())
            {
                dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == nospd);
                dataSpd.status = "SPD Cancel";
                dataSpd.isCancel = true;
                dataSpd.diubahOleh = nrp;
                dataSpd.diubahTanggal = DateTime.Now;
                dataSpd.isCancelDate = DateTime.Now;

                // get data claim
                var dataClaim = ctx.trClaims.FirstOrDefault(o => o.noSPD == nospd);
                if (dataClaim != null)
                {
                    dataClaim.status = "SPD Cancel";
                    dataClaim.isCancel = true;
                    dataClaim.diubahOleh = nrp;
                    dataClaim.diubahTanggal = DateTime.Now;
                    dataClaim.isCancelDate = DateTime.Now;
                }

                ctx.SubmitChanges();

                // get all approval
                var approval = ctx.ApprovalStatus.Where(o => o.NoSPD == nospd).ToList();

                //get all people has been approve
                var approvalHasApproved = approval.Where(o => o.Status != null).Distinct().ToList();

                //send email to all people has been approved
                foreach (var item in approvalHasApproved)
                {
                    EmailCore.InformasiCancelSPD(item.NrpApproval, dataSpd.noSPD, dataSpd);
                }
            }
        }
        #endregion

        #region reject
        private string RejectSPD(trSPD dataSpd, string nrp, string nospd, string index)
        {
            using (var ctx = new dsSPDDataContext())
            {
                dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == nospd);
                string returner = string.Empty;
                // get all approval
                var approval = ctx.ApprovalStatus.Where(o => o.NoSPD == nospd).ToList();

                // get current approval
                int? indexInt = null;
                if (!string.IsNullOrEmpty(index)) indexInt = Convert.ToInt32(index);

                var currentApproval = approval.FirstOrDefault(o => o.NrpApproval == nrp && o.Status == null && o.IndexLevel == indexInt);

                // jika current approval, absolute bukan approval tujuan yang rejek, dikarenakan status masih null
                if (currentApproval != null)
                {
                    dataSpd.status = "SPD ditolak " + currentApproval.ApprovalRule.Deskripsi;
                    dataSpd.isApproved = false;
                    dataSpd.diubahTanggal = DateTime.Now;
                    dataSpd.isApprovedDate = DateTime.Now;

                    currentApproval.Status = false;
                    currentApproval.ModifiedDate = DateTime.Now;
                }

                // jika nrp rejek sama dengan nrp approval tujuan, & current approval tidak ditemukan
                if (nrp == dataSpd.nrpApprovalTujuan && currentApproval == null)
                {
                    dataSpd.status = "SPD Ditolak tujuan";

                    dataSpd.isApproved = false;
                    dataSpd.diubahTanggal = DateTime.Now;
                    dataSpd.isApprovedDate = DateTime.Now;

                    if (dataSpd.status == "Expired")
                    {
                        returner += "SPD Expired, sehingga tidak bisa di Reject.";
                    }

                    else if (DateTime.Now.Date < dataSpd.tglBerangkat.Date)
                    {
                        returner += "SPD hanya bisa direject pada tanggal : " + dataSpd.tglBerangkat.ToString("dd MMMM yyyy")
                                + ", atau lebih.";
                    }
                }

                if (string.IsNullOrEmpty(returner))
                {
                    ctx.SubmitChanges();

                    // send informasi ke pembuat
                    EmailCore.InformasiSPD(dataSpd.dibuatOleh, dataSpd.noSPD, dataSpd);
                }

                return returner;
            }
        }
        #endregion

        #region approve
        private string ApproveSPD(trSPD dataSpd, string nrp, string nospd, string index)
        {
            using (var ctx = new dsSPDDataContext())
            {
                //nrp = "85";
                dataSpd = ctx.trSPDs.FirstOrDefault(o => o.noSPD == nospd);

                if (dataSpd.status.ToLower().Contains("expired") )
                {
                    return "SPD Expired, sehingga tidak bisa di Approved.";
                }

                // get all approval
                var approval = ctx.ApprovalStatus.Where(o => o.NoSPD == nospd).OrderBy(o => o.IndexLevel).ToList();

                // get current approval
                int? indexInt = null;
                if (!string.IsNullOrEmpty(index)) indexInt = Convert.ToInt32(index);

                var currentApproval = approval.FirstOrDefault(o => o.NrpApproval == nrp && o.Status == null && o.IndexLevel == indexInt);

                // approval tujuan, dan pastikan approval semua sudah diapprove
                var countCekApproval = approval.Where(o => o.Status == null || o.Status == false).Count();

                if (nrp == dataSpd.nrpApprovalTujuan && currentApproval == null && string.IsNullOrEmpty(index) && countCekApproval == 0)
                {
                    if (dataSpd.status.ToLower().Contains("expired"))
                    {
                        return "SPD Expired, sehingga tidak bisa di Approved.";
                    }

                    //cuman boleh ngeprove hari berangkat ato udah nyampe,
                    else if (DateTime.Now.Date < dataSpd.tglBerangkat.Date)
                    {
                        return "SPD hanya bisa diapprove pada tanggal : " + dataSpd.tglBerangkat.ToString("dd MMMM yyyy")
                       + ", atau lebih.";
                    }
                    else
                    {
                        dataSpd.status = "SPD Full Approved";

                        dataSpd.isApproved = true;
                        dataSpd.diubahTanggal = DateTime.Now;
                        dataSpd.isApprovedDate = DateTime.Now;

                        ctx.SubmitChanges();
                        // send informasi ke pembuat
                        //EmailCore.InformasiSPD(dataSpd.dibuatOleh, dataSpd.noSPD, dataSpd);//ditutup dulu oleh azan

                        return string.Empty;
                    }
                }

                //probabilitas 1% untuk approval tujuan dua kali approve, saat ini method sudah di cover sama method if action diatas, kecuali anomali data

                // jika current approval, absolute bukan approval tujuan yang approve, dikarenakan status masih null
                if (currentApproval != null && !string.IsNullOrEmpty(index))
                {
                    // get next approval
                    var nextApproval = approval.FirstOrDefault(o => o.IndexLevel == (currentApproval.IndexLevel + 1));

                    //set status menunggu approval selanjutnya
                    if (nextApproval != null)
                    {
                        dataSpd.status = "SPD Menunggu approval " + nextApproval.ApprovalRule.Deskripsi;

                        currentApproval.Status = true;
                        currentApproval.ModifiedDate = DateTime.Now;

                        ctx.SubmitChanges();

                        string indexNext = nextApproval.IndexLevel.Value.ToString();
                        // send email approval Atasan atau tujuan
                        //EmailCore.ApprovalSPD(nextApproval.NrpApproval, dataSpd.noSPD, indexNext, dataSpd); //TO DO Email ditutup dulu oleh azan

                        // send informasi ke pembuat
                        //EmailCore.InformasiSPD(dataSpd.dibuatOleh, dataSpd.noSPD, dataSpd); //TO DO Email ditutup dulu oleh azan

                    }
                    //jika tidak ada next approval lagi
                    else
                    {
                    //cek uang muka, jika lebih dari 0 maka akan generate text
                        //var dc = new dsSPDDataContext();
                        var data = (from a in ctx.trSPDs
                                    where a.noSPD == nospd
                                    select new
                                    {
                                        uangMuka = a.uangMuka
                                    }).FirstOrDefault();

                        Int64 _uangMuka = 0;
                        if(data.uangMuka != string.Empty)
                        {
                            _uangMuka = Convert.ToInt64(data.uangMuka);
                        }

                        if (_uangMuka > 0)
                        {
                            string respMessage = GenerateText(nospd, ctx); //generate text
                            if (respMessage == "success")
                            {
                                currentApproval.Status = true;
                                currentApproval.ModifiedDate = DateTime.Now;

                                ctx.SubmitChanges();
                            }
                            else
                                return respMessage;

                            //EmailCore.InformasiGenerateUangMukaSuccess(dataSpd.dibuatOleh, dataSpd.noSPD, dataSpd);
                        }
                        else
                        {
                            dataSpd.status = "SPD Menunggu approval tujuan";

                            currentApproval.Status = true;
                            currentApproval.ModifiedDate = DateTime.Now;

                            ctx.SubmitChanges();
                        }

                        // send email approval Atasan atau tujuan
                        //EmailCore.ApprovalSPD(dataSpd.nrpApprovalTujuan, dataSpd.noSPD, string.Empty, dataSpd); //TO DO Email tutup by azan

                        // send informasi ke pembuat
                        //EmailCore.InformasiSPD(dataSpd.dibuatOleh, dataSpd.noSPD, dataSpd); //TO DO Email tutup by azan

                        //EmailCore.InformasiSPDExpired(dataSpd.dibuatOleh, dataSpd.noSPD, dataSpd); //TO DO Email tutup by azan

                        //send email ke GA
                        if (dataSpd.tiket.ToLower().Contains("dicarikan"))
                        {
                            //  var role = (from p in ctx.msUsers
                            //        where p.roleId == 17
                            //    select p.nrp).ToList();

                            //   var dataGa = ctx.msKaryawans
                            //     .Where(o =>
                            //         o.coCd == "0001" &&
                            //        role.Contains(o.nrp));

                            // foreach (var item in dataGa)
                            // {

                            //  }

                            // ===== TO DO Email ===== start
                            Helper hp = new Helper();
                            DataSet dsCheckBcc = hp.wsEmail("tiket");
                            if (dsCheckBcc.Tables[0].Rows.Count > 0)
                            {
                                DataRow drCheckBcc = dsCheckBcc.Tables[0].Rows[0];
                                DataView dvCheckBcc = new DataView(dsCheckBcc.Tables[0]);
                                for (int i = 0; i < dvCheckBcc.Count; i++)
                                {
                                    //  bccAddress += drCheckBcc.Table.Rows[i][0].ToString();
                                    // bccAddress += ",";
                                    string nrpGA = drCheckBcc.Table.Rows[i][1].ToString();

                                    //EmailCore.InformasiPencarianTiket(nrpGA, dataSpd.noSPD, dataSpd);//tutup by azan
                                }

                            }
                            // ===== TO DO Email ===== end
                        }
                    }
                    // return ok
                    return string.Empty;
                }
                else
                // jika pernah approve
                {
                    // find has approved
                    var hasApproved = approval.Where(o => o.NrpApproval == nrp && o.Status != null);

                    if (hasApproved.Count() > 0)
                    {
                        var data = hasApproved.FirstOrDefault(o => o.IndexLevel == indexInt);

                        var status = data.Status != null ? data.Status.Value : false;

                        var stringStatus = status == true ? "approve" : "reject";
                        if (data.ModifiedDate == null) data.ModifiedDate = DateTime.Now;
                        return "approval tidak dieksekusi karena No Spd : " + nospd + ", telah " + stringStatus + " sebelumnya pada : " + data.ModifiedDate.Value.ToString("dd MMMM yyyy HH:mm");
                    }
                    else
                    {
                        // probabilitas 1% karena pasti selalu masuk if induk terlebih dahulu, kecuali anomali data
                        return string.Empty;
                    }
                }
            }
        }
        #endregion

        //==================== add agung ============================
        public string GenerateText(string nospd, dsSPDDataContext dc)
        {
            try
            {
                var data = dc.trSPDs.FirstOrDefault(o => o.noSPD == nospd);

                string approvalDay = DateTime.Now.Day.ToString();
                string approvalMonth = DateTime.Now.Month.ToString();
                string approvalYear = DateTime.Now.Year.ToString();

                string _NoSpd = data.noSPD;
                string _Status = "R";
                string _approvaltgl = approvalDay + "." + approvalMonth + "." + approvalYear;
                string _NRP = data.nrp;
                string _NamaLengkap = data.namaLengkap;

                string _CostCenter = string.Empty;
                if (data.costCenter.Split('-').Count() > 0)
                {
                    _CostCenter = data.costCenter.Split('-')[0];
                }
                else
                {
                    _CostCenter = data.costCenter;
                }
                string _UangMakan = "0";
                string _UangSaku = "0";
                string _Tiket = "0";
                string _Hotel = "0";
                string _Laundry = "0";
                string _Komunikasi = "0";
                string _AirPortTax = "0";
                string _BBM = "0";
                string _Tol = "0";
                string _Taxi = "0";
                string _Parkir = "0";
                string _Biayalainlain = "0";
                string _UangMuka = data.uangMuka.ToString();

                string message = "";
                message += _NoSpd.Trim() + "|";
                message += _Status.Trim() + "|";
                message += _approvaltgl.Trim() + "|";
                message += _NRP.Trim() + "|";
                message += _NamaLengkap.Trim() + "|";
                message += _CostCenter.Trim() + "|";
                message += _UangMakan.Trim() + "|";
                message += _UangSaku.Trim() + "|";
                message += _Tiket.Trim() + "|";
                message += _Hotel.Trim() + "|";
                message += _Laundry.Trim() + "|";
                message += _Komunikasi.Trim() + "|";
                message += _AirPortTax.Trim() + "|";
                message += _BBM.Trim() + "|";
                message += _Tol.Trim() + "|";
                message += _Taxi.Trim() + "|";
                message += _Parkir.Trim() + "|";
                message += _Biayalainlain.Trim() + "|";
                message += _UangMuka.Trim();
                //message += data.tglBerangkat.Day + "." + data.tglBerangkat.Month + "." + data.tglBerangkat.Year + "|";
                //message += data.tglKembali.Day + "." + data.tglKembali.Month + "." + data.tglKembali.Year;

                string day = DateTime.Now.Day.ToString();
                string bulan = DateTime.Now.Month.ToString();
                string tahun = DateTime.Now.Year.ToString();
                string jam = DateTime.Now.Hour.ToString();
                string menit = DateTime.Now.Minute.ToString();
                string detik = DateTime.Now.Second.ToString();
                string milSecond = DateTime.Now.Millisecond.ToString();

                string tanggal = day + bulan + tahun + "-" + jam + menit + detik + "-" + milSecond;

                string fileName = "espd-request" + "_" + tanggal + ".txt";

                string _User = Encrypto.Decrypt(ConfigurationManager.AppSettings["Username"]);
                string _Pass = Encrypto.Decrypt(ConfigurationManager.AppSettings["Password"]);
                string _Domain = Encrypto.Decrypt(ConfigurationManager.AppSettings["Domain"]);
                string _PathSource = ConfigurationManager.AppSettings["GenerateText"];
                string _PathGenerateReq = ConfigurationManager.AppSettings["PathGenerateReq"];
                string _BackupReq = ConfigurationManager.AppSettings["BackupReq"];

                string filePath = string.Format("{0}\\{1}", _PathSource, fileName);
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }

                string sourceFile = _PathSource + fileName;
                string backUpFile = _BackupReq + fileName;
                string destinationFile = _PathGenerateReq + fileName;
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

                    for (int i = 0; i <= 2; i++)
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

                            List<string> errorBPHUMs = messageBPH.Split(new char[] { ' ' }).ToList();

                            string bphNo = errorBPHUMs[4].ToString();

                            textList.Add(new ListGenerateTextModel() { NoSPD = noSPD, NamaFile = Path.GetFileName(file), Type = type, Tanggal = fileDate, Message = messageBPH, BPH = bphNo, FilePath = file });

                            #region command gak kepake
                            //if (noSPD == data.noSPD && type == "R")
                            //{
                            //    string errorBPHUM = cols.LastOrDefault();
                            //    string respMessage = string.Empty;

                            //    if (!errorBPHUM.ToLower().Contains("successfully"))
                            //    {
                            //        data.errorBPHUM = errorBPHUM;

                            //        respMessage = errorBPHUM;
                            //    }
                            //    else
                            //    {
                            //        data.status = "SPD Menunggu approval tujuan";

                            //        List<string> errorBPHUMs = errorBPHUM.Split(new char[] { ' ' }).ToList();

                            //        data.errorBPHUM = errorBPHUM;
                            //        data.BPHUM = errorBPHUMs[4].ToString();

                            //        respMessage = "success";
                            //    }

                            //    dc.SubmitChanges();

                            //    fileName = Path.GetFileName(file);
                            //    string archiveSapRespFile = _PathArchiveSapResp + fileName;

                            //    File.Move(file, archiveSapRespFile);

                            //    return respMessage;
                            //}
                            #endregion
                        }

                        var dataListText = textList
                                            .Where(m => m.NoSPD.Contains(data.noSPD))
                                            .Where(m => m.Type.Contains("R"))
                                            .OrderByDescending(m => m.Tanggal)
                                            .Select(m => new
                                            {
                                                m.NamaFile,
                                                m.FilePath,
                                                m.Message,
                                                m.BPH,
                                                m.Tanggal
                                            }).ToList();

                        if (dataListText.Count > 0)
                        {
                            string respMessage = string.Empty;

                            if (!dataListText[0].Message.ToLower().Contains("successfully"))
                            {
                                data.errorBPHUM = dataListText[0].Message;

                                respMessage = dataListText[0].Message;
                            }
                            else
                            {
                                data.status = "SPD Menunggu approval tujuan";

                                data.errorBPHUM = dataListText[0].Message;
                                data.BPHUM = dataListText[0].BPH;

                                respMessage = "success";
                            }

                            dc.SubmitChanges();

                            foreach(var fileMove in dataListText)
                            {
                                string archiveSapRespFile = _PathArchiveSapResp + fileMove.NamaFile;

                                File.Move(fileMove.FilePath, archiveSapRespFile);
                            }

                            return respMessage;
                        }

                        Thread.Sleep(5000);
                    }

                    return "File SAP tidak ditemukan, silahkan mencoba kembali.";
                }
            }
            catch (Exception ex)
            {
                //EmailCore.InformasiGenerateUangMukaFailed(dataSpd.dibuatOleh, dataSpd.noSPD, dataSpd);
                return ex.Message;
            }
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}