<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="NewApprovalTest.aspx.cs" Inherits="eSPD.NewApprovalTest" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        dl {
            display: block;
        }

        .dl-horizontal dt {
            float: left;
            width: 160px;
            overflow: hidden;
            clear: left;
            text-align: right;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .dl-horizontal dd {
            margin-left: 180px;
        }

        .hideIt {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <dl class="dl-horizontal">
            <dt>Tipe</dt>
            <dd>
                <asp:RadioButton ID="rbLN" runat="server" Text="Luar Negri" Checked="True" GroupName="LNDN" />
                <asp:RadioButton ID="rbDN" runat="server" Text="Dalam Negri" GroupName="LNDN" />
            </dd>
            <dt>Tipe Detail</dt>
            <dd>
                <asp:RadioButton ID="rdbHO" runat="server" Text="Kantor Pusat" Checked="True" GroupName="Asal" />
                <asp:RadioButton ID="rdbCbg" runat="server" Text="Cabang" GroupName="Asal" />
            </dd>
            <dt>Golongan</dt>
            <dd>
                <asp:DropDownList ID="cmbGolongan" runat="server">
                    <asp:ListItem>I</asp:ListItem>
                    <asp:ListItem>II</asp:ListItem>
                    <asp:ListItem>III</asp:ListItem>
                    <asp:ListItem>IV</asp:ListItem>
                    <asp:ListItem>V</asp:ListItem>
                    <asp:ListItem>VI</asp:ListItem>
                    <asp:ListItem>VII</asp:ListItem>
                </asp:DropDownList>
            </dd>
            <dt>Direksi</dt>
            <dd>
                <asp:CheckBox ID="cbDireksi" runat="server" />
            </dd>
            <dt></dt>
            <dd>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
            </dd>
            <dx:ASPxComboBox ID="AtasanApproval" 
                runat="server" 
                DataSourceID="LinqDataSource1" 
                DropDownStyle="DropDown"
                HorizontalAlign="Left" 
                TextFormatString="{0}" 
                IncrementalFilteringMode="Contains"
                TextField="namaLengkap" 
                ValueField="nrp" 
                Width="270px" EnableTheming="True" Theme="Aqua">
            </dx:ASPxComboBox>
                <asp:LinqDataSource 
                    ID="LinqDataSource1" 
                    runat="server" 
                    ContextTypeName="eSPD.dsSPDDataContext" 
                    EntityTypeName="" 
                    OrderBy="namaLengkap" 
                    Select="new (nrp, namaLengkap)" 
                    TableName="v_atasans">
                </asp:LinqDataSource>
        </dl>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdateForm" runat="server">
            <ContentTemplate>
                <div class="updateApproval">

                    <asp:GridView ID="gvApproval" runat="server" AutoGenerateColumns="False">

                        <Columns>
                            <asp:TemplateField HeaderText="No">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("IndexLevel") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("Deskripsi") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Atasan">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("RuleID") %>'></asp:Label>
                                    <asp:TextBox ID="txRuleID" runat="server" Text='<%# Eval("RuleID") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipe" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("Tipe") %>'></asp:Label>
                                    <asp:TextBox ID="txTipe" runat="server" Text='<%# Eval("Tipe") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TipeDetail" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("TipeDetail") %>'></asp:Label>
                                    <asp:TextBox ID="txTipeDetail" runat="server" Text='<%# Eval("TipeDetail") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Golongan" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("Golongan") %>'></asp:Label>
                                    <asp:TextBox ID="txGolongan" runat="server" Text='<%# Eval("Golongan") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Posisi" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("Posisi") %>'></asp:Label>
                                    <asp:TextBox ID="txPosisi" runat="server" Text='<%# Eval("Posisi") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        $(function () {
                            var FormatSelection = function (bond) {
                                console.log(bond)
                                return bond.Text
                            }

                            var FormatResult = function (bond) {
                                return '<div class="select2-user-result">' + bond.Text + '</div>'
                            }

                            var InitSelection = function (elem, cb) {
                                return elem
                            }

                            $('.ddlAtasan').select2({
                                placeholder: "Search for a repository",
                                minimumInputLength: 1,
                                multiple: false,
                                cache: true,
                                allowclear: true,
                                quietMillis: 100,
                                id: function (bond) { return bond.Value; },
                                ajax: {
                                    url: '<%= ResolveUrl("~/newapprovaltest.aspx/GetAtasan") %>',
                                    type: 'POST',
                                    params: {
                                        contentType: 'application/json; charset=utf-8'
                                    },
                                    dataType: 'json',
                                    cache: true,
                                    data: function (bond, page) {
                                        return JSON.stringify({
                                            searchText: bond,
                                            additionalFilter: $("#<%=cbDireksi.ClientID%>").is(':checked')
                                        });
                                    },
                                    results: function (bond, page) {
                                        return {
                                            results: bond.d,
                                            more: (bond.d && bond.d.length == 10 ? true : false)
                                        }
                                    }
                                },

                                formatResult: FormatResult,
                                formatSelection: FormatSelection,
                                initSelection: InitSelection,
                                escapeMarkup: function (m) { return m; }
                            });
                        });
                    });



                </script>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
