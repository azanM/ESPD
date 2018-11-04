﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eSPD
{
    class EspdEmail
    {
        public string DetailURL { get; set; }
        public string DetailClaimURL { get; set; } 
        public string ApproveUrl { get; set; }
        public string RejectUrl { get; set; }
        public string ApproveUrlExternal { get; set; }
        public string RejectUrlExternal { get; set; }

        //email encrypt
        public string eCrypt { get; set; }

        //nrp encrypt
        public string nCrypt { get; set; }

        //no spd encypt
        public string sCrypt { get; set; }
        public string NoSPD { get; set; }
        public string indexApproval { get; set; }
        public string approverName { get; set; }

        //Informasi/Approval SPD Atasan Langsung
        public string spdRequester { get; set; }
        public string nrpRequester { get; set; }
        public string statusName { get; set; }
        public string noHp { get; set; }
        public string golongan { get; set; }
        public string jabatan { get; set; }
        public string organisasiUnit { get; set; }
        public string companyCode { get; set; }
        public string personalArea { get; set; }
        public string personelSubArea { get; set; }
        public string costCenterPembebanan { get; set; }
        public string tempatTujuan { get; set; }
        public string keperluan { get; set; }
        public string tanggalBerangkat { get; set; }
        public string jamBerangkat { get; set; }
        public string menitBerangkat { get; set; }
        public string tanggalKembali { get; set; }
        public string jamKembali { get; set; }
        public string menitKembali { get; set; }
        public string angkutan { get; set; }
        public string hotel { get; set; }
        public string totalDays { get; set; }
        public string alasan { get; set; }


        public string tglExpired { get; set; }
        public string uangMuka { get; set; }
        public string BPHUM { get; set; }

        

        // new request spd status list
        public string spdStatusList { get; set; }

        public string claimApprove { get; set; }

        // new request claim
        public string claimStatusList { get; set; }

        //detail Claim
        public string CbiayaMakan { get; set; }
        public string CuangSaku { get; set; }
        public string Ctiket { get; set; }
        public string Chotel { get; set; }
        public string CBBM { get; set; }
        public string Ctol { get; set; }
        public string Ctaxi { get; set; }
        public string CairportTax { get; set; }
        public string Claundry { get; set; }
        public string Cparkir { get; set; }
        public string Ckomunikasi { get; set; }
        public string CbiayaLainLain { get; set; }
        public string CtotalClaim { get; set; }
        public string CuangMuka { get; set; }
        public string Cpenyelesaian { get; set; }
        public string CBPHClaim { get; set; }
        
    }
}