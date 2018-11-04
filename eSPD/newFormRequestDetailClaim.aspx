<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newFormRequestDetailClaim.aspx.cs" Inherits="eSPD.newFormRequestDetailClaim" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body {
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            color: #555;
        }

        .bs-callout {
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            -moz-border-right-colors: none;
            -moz-border-top-colors: none;
            border-color: #eee;
            border-image: none;
            border-radius: 3px;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            margin: 20px 0;
            padding: 20px;
        }

        .bs-callout-danger {
            border-left-color: #ce4844;
        }

        .bs-callout-info {
            border-left-color: #1b809e;
        }

        .bs-callout h4 {
            margin-bottom: 5px;
            margin-top: 0;
            border-bottom: 1px solid #f5f5f5;
            padding-bottom: 5px;
            margin-bottom: 10px;
        }


        .bs-callout-danger h4 {
            color: #ce4844;
        }

        .bs-callout-info h4 {
            color: #1b809e;
        }

        dl {
            margin-bottom: 20px;
            margin-top: 0;
        }

        .dl-horizontal dt {
            clear: left;
            float: left;
            overflow: hidden;
            text-align: right;
            text-overflow: ellipsis;
            white-space: nowrap;
            width: 170px;
        }

        dt {
            font-weight: 700;
        }

        dd, dt {
            line-height: 1.42857;
        }

        .dl-horizontal dd {
            margin-left: 180px;
        }

        dd {
            display: block;
        }

        table.gridtable {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #DDD;
            border-collapse: collapse;
        }

            table.gridtable th {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #DDD;
                background-color: #f5f5f5;
            }

            table.gridtable td {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #DDD;
                background-color: #ffffff;
            }

            table.gridtable tr.Sub {
                font-weight: bold;
            }

                table.gridtable tr.Sub td {
                    background-color: #f5f5f5;
                }

            table.gridtable td:last-child {
                text-align: right;
            }

            table.gridtable td:first-child {
                min-width: 300px;
            }

        .panel {
            background-color: #fff;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.05);
            margin-bottom: 20px;
        }

        .panel-default > .panel-heading {
            background-color: #f5f5f5;
            border-color: #ddd;
            color: #333;
        }

        .panel-heading {
            border-bottom: 1px solid transparent;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
            padding: 10px 15px;
        }

        .panel-body {
            padding: 15px;
        }

        .panel-footer {
            background-color: #f5f5f5;
            border-bottom-left-radius: 3px;
            border-bottom-right-radius: 3px;
            border-top: 1px solid #ddd;
            padding: 10px 15px;
        }

        h3 {
            border-bottom: 1px solid #e3e3e3;
            padding-bottom: 5px;
        }

            h3 span {
                float: right;
            }
    </style>

    <script type="text/javascript">
        function printDocument() {
            var printContents = document.getElementById('printable').innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h3>Data Claim
            <span>
                <button type="button" onclick="printDocument()">Print</button>
            </span>
        </h3>
        <div class="container" id="printable">
            <div class="panel panel-default">
                <div class="panel-heading">Detail SPD</div>
                <div class="panel-body">
                    <asp:LinqDataSource ID="LinqDataClaim" runat="server" ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" TableName="v_claim_spds" Where="noSPD == @noSPD">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="Id" Name="noSPD" PropertyName="Value" Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                    <asp:LinqDataSource ID="LinqDataSource1" runat="server"></asp:LinqDataSource>
                    <asp:FormView ID="fvSPD" runat="server" DataKeyNames="noSPD" DataSourceID="LinqDataClaim">
                        <ItemTemplate>
                            <div class="bs-callout bs-callout-info">
                                <h4>Informasi Karyawan
                                </h4>
                                <dl class="dl-horizontal">
                                    <dt>SPD:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="noSPDLabel" runat="server" Text='<%# Eval("noSPD") %>' /></dd>
                                    <dt>Asal:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="asalLabel" runat="server" Text='<%# Bind("asal") %>' /></dd>
                                    <dt>Nama:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="namaLengkapLabel" runat="server" Text='<%# Bind("namaLengkap") %>' /></dd>
                                    <dt>Golongan:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="idGolonganLabel" runat="server" Text='<%# Bind("idGolongan") %>' /></dd>
                                    <dt>Jabatan:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="jabatanLabel" runat="server" Text='<%# Bind("jabatan") %>' /></dd>
                                    <dt>Email:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="emailLabel" runat="server" Text='<%# Bind("email") %>' /></dd>
                                    <dt>No HP:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="NoHPLabel" runat="server" Text='<%# Bind("NoHP") %>' /></dd>
                                    <dt>Posisi:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="posisiLabel" runat="server" Text='<%# Bind("posisi") %>' /></dd>
                                </dl>
                            </div>

                            <div class="bs-callout bs-callout-info">
                                <h4>Keperluan & Tujuan
                                </h4>
                                <dl class="dl-horizontal">

                                    <dt>Tujuan:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="TujuanLabel" runat="server" Text='<%# Bind("Tujuan") %>' /></dd>
                                    <dt>WilayahTujuan:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="WilayahTujuanLabel" runat="server" Text='<%# Bind("WilayahTujuan") %>' /></dd>
                                    <dt>Company Tujuan:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="companyCodeTujuanLabel" runat="server" Text='<%# Bind("companyCodeTujuan") %>' /></dd>
                                    <dt>Area:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="personelAreaTujuanLabel" runat="server" Text='<%# Bind("personelAreaTujuan") %>' /></dd>
                                    <dt>Sub Area:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="pSubAreaTujuanLabel" runat="server" Text='<%# Bind("pSubAreaTujuan") %>' /></dd>
                                    <dt>Tempat Tujuan Lain:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="tempatTujuanLainLabel" runat="server" Text='<%# Bind("tempatTujuanLain") %>' /></dd>
                                    <dt>ket Keperluan:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="ketKeperluanLabel" runat="server" Text='<%# Bind("ketKeperluan") %>' /></dd>
                                    <dt>Ket Lain:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="keperluanLainLabel" runat="server" Text='<%# Bind("keperluanLain") %>' /></dd>
                                </dl>
                            </div>

                            <div class="bs-callout bs-callout-info">
                                <h4>Informasi Keberangkatan & akomodasi
                                </h4>
                                <dl class="dl-horizontal">
                                    <dt>Tiket:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="tiketLabel" runat="server" Text='<%# Bind("tiket") %>' /></dd>
                                    <dt>Berangkat:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="tglBerangkatLabel" runat="server" Text='<%# Bind("tglBerangkat", "{0:dd MMMM yyyy}") %>' />&nbsp;
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("jamBerangkat") %>' />&nbsp; 
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("menitBerangkat") %>' />
                                    </dd>
                                    <dt>Kembali:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="tglKembaliLabel" runat="server" Text='<%# Bind("tglKembali", "{0:dd MMMM yyyy}") %>' />&nbsp;
                                        <asp:Label ID="jamKembaliLabel" runat="server" Text='<%# Bind("jamKembali") %>' />&nbsp;
                                         <asp:Label ID="menitKembaliLabel" runat="server" Text='<%# Bind("menitKembali") %>' />
                                    </dd>
                                    <dt>Cost Center:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="costCenterLabel" runat="server" Text='<%# Bind("costCenter") %>' /></dd>
                                    <dt>keterangan:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="keteranganLabel" runat="server" Text='<%# Bind("keterangan") %>' /></dd>
                                </dl>
                            </div>

                            <div class="bs-callout bs-callout-info">
                                <h4>Informasi Lain SPD</h4>
                                <dl class="dl-horizontal">
                                    <dt>Status SPD:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="statusLabel" runat="server" Text='<%# Bind("status") %>' /></dd>
                                    <dt>Dibuat Tanggal:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="dibuatTanggalLabel" runat="server" Text='<%# Bind("dibuatTanggal", "{0:dd MMMM yyyy}") %>' /></dd>
                                </dl>
                            </div>

                            <div class="bs-callout bs-callout-info">
                                <h4>Informasi Claim
                                </h4>

                                <dl class="dl-horizontal">
                                    <dt>Jumlah hari:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("jmlHariSpd") %>' />
                                    </dd>
                                    <dt>Keterangan Lain-lain:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="ketLainLainLabel" runat="server" Text='<%# Bind("ketLainLain") %>' /></dd>
                                </dl>
                                <br />
                                <table class="gridtable" style="width:100%;">
                                    <thead>
                                        <tr>
                                            <th>Keterangan</th>
                                            <th>Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Biaya makan</td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("biayaMakan", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Uang saku</td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("uangSaku", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr class="Sub">
                                            <td>Sub total </td>
                                            <td>
                                                <%#(Convert.ToInt32(Eval("biayaMakan")) + Convert.ToInt32(Eval("uangSaku"))).ToString("#,##0")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tiket</td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("tiketClaim", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Penginapan</td>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("hotel", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Laundry</td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("laundry", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Komunikasi</td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("komunikasi", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Airport Tax</td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text='<%# Bind("airportTax", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>BBM</td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("BBM", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Tol</td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("Tol", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Taksi</td>
                                            <td>
                                                <asp:Label ID="Label13" runat="server" Text='<%# Bind("taxi", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Parkir</td>
                                            <td>
                                                <asp:Label ID="Label14" runat="server" Text='<%# Bind("parkir", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Biaya lain - lain</td>
                                            <td>
                                                <asp:Label ID="Label15" runat="server" Text='<%# Bind("biayaLainLain", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr class="Sub">
                                            <td>Sub total </td>
                                            <td>
                                                <%# (
                                                    Convert.ToInt32(Eval("tiketClaim")) + 
                                                    Convert.ToInt32(Eval("hotel")) + 
                                                    Convert.ToInt32(Eval("laundry")) + 
                                                    Convert.ToInt32(Eval("komunikasi")) + 
                                                    Convert.ToInt32(Eval("airportTax")) + 
                                                    Convert.ToInt32(Eval("BBM")) + 
                                                    Convert.ToInt32(Eval("Tol")) + 
                                                    Convert.ToInt32(Eval("taxi")) + 
                                                    Convert.ToInt32(Eval("parkir")) +
                                                    Convert.ToInt32(Eval("biayaLainLain"))
                                                ).ToString("#,##0")%>
                                            </td>
                                        </tr>
                                        <tr class="Sub">
                                            <td>Grand Total</td>
                                            <td>
                                                <asp:Label ID="Label16" runat="server" Text='<%# Bind("total", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr>
                                            <td>Uang muka</td>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" Text='<%# Bind("uangMuka", "{0:#,##0}") %>' /></td>
                                        </tr>
                                        <tr class="Sub">
                                            <td>Penyelesaian</td>
                                            <td>
                                                <asp:Label ID="Label18" runat="server" Text='<%# Bind("penyelesaian", "{0:#,##0}") %>' /></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div class="bs-callout bs-callout-info">
                                <h4>Informasi Lain Claim</h4>
                                <dl class="dl-horizontal">
                                    <dt>Status Claim:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="statusClaimLabel" runat="server" Text='<%# Bind("statusClaim") %>' /></dd>
                                    <dt>Dibuat Tanggal:</dt>
                                    <dd>&nbsp;
                                        <asp:Label ID="dibuatTanggalClaimLabel" runat="server" Text='<%# Bind("dibuatTanggalClaim", "{0:dd MMMM yyyy}") %>' /></dd>
                                </dl>
                            </div>
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:HiddenField ID="Id" runat="server" />

                    <%--<h4>Approval
                        </h4>
                        <asp:HiddenField ID="Id" runat="server" />
                        <asp:GridView ID="gvList" runat="server" CssClass="gridtable" AutoGenerateColumns="False" DataSourceID="LinqData">
                            <Columns>
                                <asp:BoundField DataField="IndexLevel" HeaderText="No" ReadOnly="True" SortExpression="IndexLevel" />
                                <asp:BoundField DataField="Nama" HeaderText="Nama" ReadOnly="True" SortExpression="Nama" />
                                <asp:CheckBoxField DataField="Status" HeaderText="Status" ReadOnly="True" SortExpression="Status" />
                                <asp:BoundField DataField="ModifiedDate" HeaderText="ModifiedDate" ReadOnly="True" SortExpression="ModifiedDate" DataFormatString="{0:dd MMMM yyyy}" HtmlEncode="false" />
                            </Columns>
                        </asp:GridView>
                        <asp:LinqDataSource ID="LinqData" runat="server" ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" OrderBy="IndexLevel" Select="new (IndexLevel, Nama, Status, ModifiedDate)" TableName="ApprovalStatus" Where="NoSPD == @NoSPD">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="Id" Name="NoSPD" PropertyName="Value" Type="String" />
                            </WhereParameters>
                        </asp:LinqDataSource>--%>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
