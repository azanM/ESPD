<%@ Master Language="C#" AutoEventWireup="true" CodeBehind="MasterPage.master.cs" Inherits="eSPD.MasterPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="App_Themes/Main/Site.css" rel="stylesheet" type="text/css" />
      <link href="style/style.css" rel="stylesheet" type="text/css" /> 
        <link href="style/select2.css" rel="stylesheet" type="text/css" /> 
    
    <script type="text/javascript" src="Script/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Script/select2.min.js"></script>
    <asp:ContentPlaceHolder ID="head" runat="server">
    </asp:ContentPlaceHolder>
    <style type="text/css">
        .style1
        {
            float: left;
            padding-left: 8px;
            padding-right: 0px;
            padding-top: 4px;
            padding-bottom: 4px;
        }
    </style>
</head>
<body  >
  <form id="form1" runat="server"  >
    <div class="page" >
      <table width="1050" align="center" cellpadding="0" cellspacing="0" >
        <tr>
        <td colspan="3" width="100%" >
          <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
              <td align="left" style="margin-left:20px;" class="HeaderLeft">
              <table>
              <tr>
              <td style="margin-left:20px;" >
              <asp:Image runat="server" ID="IMGlOGO" ImageUrl="~/Style/Banner-ESPD.jpg" 
                      Height="121px" Width="1038px" />
              </td>
              </tr>
              </table>              
                  </td>
              <td class="HeaderRepeater" align="right">
                  <%--<div id="clockDisplay" style="font-size:small; font-family:Verdana;" class="clockStyle"></div>--%><%--<span id="clockDisplay" style="font-size:small; font-family:Verdana;" class="clockStyle" ></span>--%>
              </td>
              <td class="HeaderRight"></td>
            </tr>
          </table>
        </td>
        </tr>
        <tr>
        <td colspan="3">
        <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
        <td style="background-color:#414141;" >&nbsp;</td>       
        </tr>
        </table>
        </td>
        </tr>
        <tr>                   
          <td align="left" >
          <table width="100%">
            <tr> 
            <td>
                <asp:Menu ID="mnuMain" runat="server" CssClass="menu" 
                    DynamicEnableDefaultPopOutImage="False" 
                    DynamicPopOutImageUrl="~/App_Themes/Theme/Images/img_rmenuarrow.gif" 
                    EnableViewState="false" IncludeStyleBlock="false" Orientation="Horizontal" 
                    StaticDisplayLevels="2" StaticEnableDefaultPopOutImage="False">
                    <Items>
                        <asp:menuitem NavigateUrl="~/frmHome.aspx" Text="Home" Value="Home">
                            <asp:menuitem Text="Master" Value="mstr">
                               <%-- <asp:MenuItem NavigateUrl="~/frmMasterKaryawan.aspx" Text="Karyawan" 
                                    Value="Karyawan"></asp:MenuItem>--%>
                                <asp:MenuItem NavigateUrl="~/msCostCenter.aspx" Text="Cost Center" 
                                    Value="CostCenter"></asp:MenuItem>
                                     <asp:MenuItem NavigateUrl="~/msUser.aspx" Text="User" 
                                    Value="User"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/frmMsRole.aspx" Text="Role" 
                                    Value="Role"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/frmMsAngkutan.aspx" Text="Angkutan" 
                                    Value="Angkutan"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/frmMsKeperluan.aspx" Text="Keperluan" 
                                    Value="Keperluan"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/frmMsPlafon.aspx" Text="Plafon" 
                                    Value="Plafon"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/frmMsPlafonGolongan.aspx" Text="Golongan Plafon" 
                                    Value="Plafon"></asp:MenuItem>
                            </asp:menuitem>
                            <asp:menuitem Text="Transaction" Value="tscn">
                                <asp:menuitem NavigateUrl="~/frmRequestInput.aspx" Text="Create SPD" 
                                    Value="msCreate"></asp:menuitem>
                                <asp:MenuItem NavigateUrl="frmClaimApproval.aspx" Text="Claim SPD" 
                                    Value="msClaim"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/frmFinanceApp.aspx" Text="Finance Approved" 
                                    Value="Finance Approved"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/frmSPDFinder.aspx" Text="Search All" 
                                    Value="Search All"></asp:MenuItem>
                            </asp:menuitem>
                             <asp:menuitem Text="Reports" Value="rpt">
                                <asp:menuitem NavigateUrl="~/frmReportClaimSPD.aspx" Text="Report Claim SPD" 
                                    Value="ReportClaim"></asp:menuitem>   
                                <asp:menuitem NavigateUrl="~/frmRptSPD.aspx" Text="Report SPD" 
                                    Value="ReportSPD"></asp:menuitem>
                                <asp:menuitem NavigateUrl="~/frmRptClaimAll.aspx" Text="Report Claim All" 
                                    Value="ReportClaimAll"></asp:menuitem>      
                                    <asp:menuitem NavigateUrl="~/frmRptSPDAll.aspx" Text="Report SPD All" 
                                    Value="ReportSPDAll"></asp:menuitem>                             
                            </asp:menuitem>                             
                          <%--<asp:MenuItem Text="Laporan" Value="lpr">
                               <asp:menuitem NavigateUrl="~/frmReportSPD.aspx" Text="Report SPD" 
                                    Value="msCreate"></asp:menuitem>
                                <asp:MenuItem NavigateUrl="frmReportClaim.aspx" Text="Report Claim SPD" 
                                    Value="msClaim"></asp:MenuItem>
                                    <asp:menuitem NavigateUrl="~/frmReportCrystalSPD.aspx" Text="Report SPD CR" 
                                    Value="msCreate"></asp:menuitem>
                                <asp:MenuItem NavigateUrl="~/frmReportCrystalClaim.aspx" Text="Report Claim SPD CR" 
                                    Value="msClaim"></asp:MenuItem>
                            </asp:MenuItem>--%>
                            <asp:MenuItem Text="Setting" Value="st">
                                <asp:MenuItem Text="User Maintenance" NavigateUrl="~/frmAssignRole.aspx" Value="UM"></asp:MenuItem>
                                <asp:MenuItem Text="Page Access Security" Value="GPM"></asp:MenuItem>
                                <asp:MenuItem Text="Data Access Security" Value="DAS"></asp:MenuItem>
                            </asp:MenuItem>
                            <asp:menuitem Text="Help" Value="hlp">
                                <asp:menuitem NavigateUrl="~/frmUserManual.aspx" Text="User Manual" 
                                    Value="usrManual"></asp:menuitem>                                                                
                            </asp:menuitem>                          
                        </asp:menuitem>
                    </Items>
                    <StaticMenuStyle CssClass="staticMenu" HorizontalPadding="0px" 
                        VerticalPadding="0px" />
                    <StaticMenuItemStyle CssClass="staticMenuItem" ItemSpacing="1px" />
                    <StaticHoverStyle CssClass="statichover" />
                    <DynamicMenuStyle BackColor="#CCCCCC" CssClass="dynamicMenu" 
                        HorizontalPadding="1px" />
                    <DynamicMenuItemStyle BackColor="#dbe8f4" CssClass="dynamicMenuItem" 
                        ItemSpacing="1px" />
                    <DynamicHoverStyle BackColor="#89AFDC" BorderStyle="None" ForeColor="White" />
                    <StaticSelectedStyle CssClass="staticMenuItem" ItemSpacing="1px" />
                </asp:Menu>
                </td>                                               
              <td >
                 <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release" >
                 </asp:ScriptManager>
              </td>
             <td>
                <asp:LoginView ID="LoginView1" runat="server" >
                  <LoggedInTemplate>
                      <asp:LoginName ID="LoginName1" runat="server" FormatString="Welcome, {0}"/>
                      <asp:Label runat="server" ID="lblLogin" CssClass="logouttheme" ForeColor="#3333FF" />
                  </LoggedInTemplate>
                  <AnonymousTemplate><span class="logouttheme" style="color:White; width:200px;">You are not logged in</span>
                  
                  </AnonymousTemplate>
                </asp:LoginView>         
                <asp:LinkButton runat="server" ToolTip="Log Out" Text="Keluar" Visible="true" 
                      ID="lblLogout"></asp:LinkButton>                    
                </td>                            
            </tr>
          </table>                                
          </td>
        </tr>
        <tr>
          <td colspan="3" style="background-color:White; height:500px; padding:10px; vertical-align:top;" >
            <table width="100%" cellpadding="0" cellspacing="0" >
              <tr>                
                <td width="80%" style="vertical-align:top;"  >
                  <asp:ContentPlaceHolder id="ContentPlaceHolder1" runat="server">        
                  </asp:ContentPlaceHolder>
                </td>
              </tr>
            </table>
          </td>          
        </tr>
        <tr>
        <td colspan="3" width="100%" >
          <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
              <td class="Footer"></td>
              <td class="FooterRepeater" align="right"></td>
              <td class="FooterRight"></td>
            </tr>
          </table>
        </td>          
        </tr>
      </table>      
    </div>
  </form>
</body>
</html>
