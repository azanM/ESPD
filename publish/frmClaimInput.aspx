<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmClaimInput - fileupload.aspx.cs" Inherits="eSPD.frmClaimInput" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2 {
            width: 75px;
        }

        .loader{
            display:none;
            position:fixed;
            width:100%;
            height:100%;
            top:0px;
            left:0px;
            right:0px;
            bottom:0px;
            background-color:transparent;
            opacity:0.3;
        }
        .laundry {
            display:none !important;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: auto;" align="left">
        <tr>
            <td>Claim SPD
            </td>
            <td width="20">&nbsp;
            </td>
            <td width="10">&nbsp;
            </td>
            <td class="style2">&nbsp;
            </td>
        </tr>
        <tr>
            <td>No SPD
            </td>
            <td>
                <asp:TextBox ID="txtNoSPD" runat="server" AutoPostBack="True" OnTextChanged="txtNoSPD_TextChanged"
                    Enabled="False"></asp:TextBox>
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">&nbsp;
            </td>
        </tr>
        <tr>
            <td>Nama Lengkap
            </td>
            <td>
                <asp:TextBox ID="txtNamaLengkap" runat="server" Enabled="False"></asp:TextBox>
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">&nbsp;
            </td>
        </tr>
        <tr>
            <td>Tanggal Berangkat
            </td>
            <td class="style3">
                <asp:TextBox ID="txtTglBerangkat" runat="server" Enabled="False"></asp:TextBox>
                <asp:CalendarExtender ID="txtTglBerangkat_CalendarExtender" runat="server" TargetControlID="txtTglBerangkat">
                </asp:CalendarExtender>
                <asp:DropDownList ID="ddlJamBerangkat0" runat="server" Enabled="false">
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                </asp:DropDownList>
                :<asp:DropDownList ID="ddlMenitBerangkat0" runat="server" Enabled="false">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>31</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>33</asp:ListItem>
                    <asp:ListItem>34</asp:ListItem>
                    <asp:ListItem>35</asp:ListItem>
                    <asp:ListItem>36</asp:ListItem>
                    <asp:ListItem>37</asp:ListItem>
                    <asp:ListItem>38</asp:ListItem>
                    <asp:ListItem>39</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>41</asp:ListItem>
                    <asp:ListItem>42</asp:ListItem>
                    <asp:ListItem>43</asp:ListItem>
                    <asp:ListItem>44</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                    <asp:ListItem>46</asp:ListItem>
                    <asp:ListItem>47</asp:ListItem>
                    <asp:ListItem>48</asp:ListItem>
                    <asp:ListItem>49</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>51</asp:ListItem>
                    <asp:ListItem>52</asp:ListItem>
                    <asp:ListItem>53</asp:ListItem>
                    <asp:ListItem>54</asp:ListItem>
                    <asp:ListItem>55</asp:ListItem>
                    <asp:ListItem>56</asp:ListItem>
                    <asp:ListItem>57</asp:ListItem>
                    <asp:ListItem>58</asp:ListItem>
                    <asp:ListItem>59</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Tanggal Kembali
            </td>
            <td>
                <asp:TextBox ID="txtTglKembali" runat="server" Enabled="False"></asp:TextBox>
                <asp:CalendarExtender ID="txtTglKembali_CalendarExtender" runat="server" TargetControlID="txtTglKembali">
                </asp:CalendarExtender>
                <asp:DropDownList ID="ddlJamKembali" runat="server" Enabled="false">
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                </asp:DropDownList>
                :<asp:DropDownList ID="ddlMenitKembali" runat="server" Enabled="false">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>31</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>33</asp:ListItem>
                    <asp:ListItem>34</asp:ListItem>
                    <asp:ListItem>35</asp:ListItem>
                    <asp:ListItem>36</asp:ListItem>
                    <asp:ListItem>37</asp:ListItem>
                    <asp:ListItem>38</asp:ListItem>
                    <asp:ListItem>39</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>41</asp:ListItem>
                    <asp:ListItem>42</asp:ListItem>
                    <asp:ListItem>43</asp:ListItem>
                    <asp:ListItem>44</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                    <asp:ListItem>46</asp:ListItem>
                    <asp:ListItem>47</asp:ListItem>
                    <asp:ListItem>48</asp:ListItem>
                    <asp:ListItem>49</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>51</asp:ListItem>
                    <asp:ListItem>52</asp:ListItem>
                    <asp:ListItem>53</asp:ListItem>
                    <asp:ListItem>54</asp:ListItem>
                    <asp:ListItem>55</asp:ListItem>
                    <asp:ListItem>56</asp:ListItem>
                    <asp:ListItem>57</asp:ListItem>
                    <asp:ListItem>58</asp:ListItem>
                    <asp:ListItem>59</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Keperluan
            </td>
            <td>
                <asp:TextBox ID="txtKeperluan" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LKurs" runat="server" Text="Kurs" Visible="false"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="kurs" runat="server" Visible="false" AutoPostBack="true" OnTextChanged="kurs_TextChanged"
                    Width="75px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="kurs">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td>Jumlah Hari SPD
            </td>
            <td>
                <asp:Label ID="lblJumlahhari" runat="server" Text="Label"></asp:Label>
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:Label ID="ldlr" runat="server" Text="Sub Total ($)" Visible="false"></asp:Label>
            </td>
            <td>Sub Total
            </td>
        </tr>
        <tr>
            <td>Biaya Makan
            </td>
            <td>
                <asp:TextBox ID="txtMakan" runat="server" Enabled="False"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtMakan_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtMakan">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtSTMakanDLR" runat="server" AutoPostBack="True" OnTextChanged="txtSTMakan_TextChanged"
                    Enabled="False" Visible="false" Width="75px">0</asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSTMakan" runat="server" AutoPostBack="True" OnTextChanged="txtSTMakan_TextChanged"
                    Enabled="False">0</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Uang Saku
            </td>
            <td>
                <asp:TextBox ID="txtUangSaku" runat="server" Enabled="False"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtUangSaku_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtUangSaku">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtSTUangSkDLR" runat="server" AutoPostBack="True" OnTextChanged="txtSTUangSk_TextChanged"
                    Enabled="False" Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtSTUangSkDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:TextBox ID="txtSTUangSk" runat="server" AutoPostBack="True" OnTextChanged="txtSTUangSk_TextChanged"
                    Enabled="False">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtSTUangSk_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtSTUangSk">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td>Tiket
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtTiketDLR" runat="server" AutoPostBack="True" OnTextChanged="txtTiket_TextChanged"
                    Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtTiketDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:TextBox ID="txtTiket" runat="server" AutoPostBack="True" OnTextChanged="txtTiket_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtTiket_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtTiket">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkTiket" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="tiketUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="tiketUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>Penginapan
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtHotelDLR" runat="server" AutoPostBack="True" OnTextChanged="txtHotel_TextChanged"
                    Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtHotelDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:TextBox ID="txtHotel" runat="server" AutoPostBack="True" OnTextChanged="txtHotel_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtHotel_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtHotel">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkPenginapan" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="penginapanUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="penginapanUploadComplete" />
            </td>
        </tr>
        <tr><%-- style="display:none !important;">--%>
            <td>Laundry
            </td>
            <td align="right"> <label class="laundry">Maksimum Laundry :</label> 
            </td>
            <td>
                <asp:TextBox ID="txtJmlHariLaundri" runat="server" Enabled="False" Height="20px" CssClass="laundry"
                    Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtJmlHariLaundri_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtJmlHariLaundri">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" Width="80px">
                    <ContentTemplate>
                        <asp:TextBox ID="txtLaundryDLR" runat="server" AutoPostBack="True" OnTextChanged="txtLaundry_TextChanged"
                            Width="75px" Visible="false">0</asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                            ValidChars="1234567890" FilterType="Custom" TargetControlID="txtLaundryDLR">
                        </asp:FilteredTextBoxExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtLaundry" runat="server" AutoPostBack="True" OnTextChanged="txtLaundry_TextChanged"
                            Style="height: 22px">0</asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtLaundry_FilteredTextBoxExtender" runat="server"
                            Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtLaundry">
                        </asp:FilteredTextBoxExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkLaundry" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="laundryUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="laundryUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>Komunikasi
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtKomunikasiDLR" runat="server" OnTextChanged="txtKomunikasi_TextChanged"
                    AutoPostBack="True" Width="75px" Visible="false">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtKomunikasiDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtKomunikasi" runat="server" OnTextChanged="txtKomunikasi_TextChanged"
                    AutoPostBack="True">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtKomunikasi_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtKomunikasi">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkKomunikasi" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="komunikasiUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="komunikasiUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>Air Port Tax
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtAirPortTaxDLR" runat="server" AutoPostBack="True" OnTextChanged="txtAirPortTax_TextChanged"
                    Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtAirPortTaxDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtAirPortTax" runat="server" AutoPostBack="True" OnTextChanged="txtAirPortTax_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtAirPortTax_FilteredTextBoxExtender" runat="server"
                    Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtAirPortTax">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkAirportTax" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="airporttaxUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="airporttaxUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>BBM
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtBBMDLR" runat="server" AutoPostBack="True" OnTextChanged="txtBBM_TextChanged"
                    Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtBBMDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtBBM" runat="server" AutoPostBack="True" OnTextChanged="txtBBM_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtBBM_FilteredTextBoxExtender" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtBBM">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkBBM" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="bbmUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="bbmUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>Tol
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtTolDLR" runat="server" AutoPostBack="True" OnTextChanged="txtTol_TextChanged"
                    Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtTolDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtTol" runat="server" AutoPostBack="True" OnTextChanged="txtTol_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtTol_FilteredTextBoxExtender" runat="server" Enabled="True"
                    ValidChars="1234567890" FilterType="Custom" TargetControlID="txtTol">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkTol" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="tolUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="tolUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>Taxi
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtTaxiDLR" runat="server" AutoPostBack="True" OnTextChanged="txtTaxi_TextChanged"
                    Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" ValidChars="1234567890"
                    FilterType="Custom" Enabled="True" TargetControlID="txtTaxiDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtTaxi" runat="server" AutoPostBack="True" OnTextChanged="txtTaxi_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtTaxi_FilteredTextBoxExtender" runat="server"
                    ValidChars="1234567890" FilterType="Custom" Enabled="True" TargetControlID="txtTaxi">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkTaxi" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="taxiUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="taxiUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>Parkir
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td class="style2">
                <asp:TextBox ID="txtParkirDLR" runat="server" AutoPostBack="True" OnTextChanged="txtParkir_TextChanged"
                    Visible="false" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" ValidChars="1234567890"
                    FilterType="Custom" Enabled="True" TargetControlID="txtParkirDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtParkir" runat="server" AutoPostBack="True" OnTextChanged="txtParkir_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtParkir_FilteredTextBoxExtender" runat="server"
                    ValidChars="1234567890" FilterType="Custom" Enabled="True" TargetControlID="txtParkir">
                </asp:FilteredTextBoxExtender>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkParkir" />
            </td>
            <td>
                <ajax:AsyncFileUpload ID="parkirUpload"
                    OnClientUploadComplete="uploadComplete"
                    OnClientUploadError="uploadError"
                    CompleteBackColor="White"
                    Width="300px"
                    runat="server"
                    UploaderStyle="Traditional"
                    UploadingBackColor="#CCFFFF"
                    ThrobberID="Loader"
                    OnUploadedComplete="parkirUploadComplete" />
            </td>
        </tr>
        <tr>
            <td>Lain-lain
            </td>
            <td>
                <asp:TextBox ID="txtLainlainDetail" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>Jumlah lain - lain
            </td>
            <td class="style2">
                <asp:TextBox ID="txtLainlainDLR" runat="server" AutoPostBack="True" OnTextChanged="txtSTLainlain_TextChanged"
                    Visible="False" Widt="75px" Width="75px">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" ValidChars="1234567890"
                    FilterType="Custom" Enabled="True" TargetControlID="txtLainlainDLR">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtLainlain" runat="server" AutoPostBack="True" OnTextChanged="txtSTLainlain_TextChanged">0</asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtLainlain_FilteredTextBoxExtender" runat="server"
                    ValidChars="1234567890" FilterType="Custom" Enabled="True" TargetControlID="txtLainlain">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td>TOTAL
            </td>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSTMakan" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtSTUangSk" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTiket" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtHotel" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtBBM" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTol" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTaxi" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAirPortTax" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtParkir" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLainlain" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtKomunikasi" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:TextBox ID="txtTotalDLR" runat="server" Enabled="False" Visible="false" Width="75px">0</asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSTMakan" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtSTUangSk" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTiket" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtHotel" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtBBM" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTol" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTaxi" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAirPortTax" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtParkir" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLainlain" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtKomunikasi" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:TextBox ID="txtTotal" runat="server" Enabled="False">0</asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="FLDTujuan" runat="server" />
            </td>
            <td>&nbsp;
            </td>
            <td>Uang Muka
            </td>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSTMakan" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtSTUangSk" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTiket" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtHotel" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtBBM" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTol" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTaxi" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAirPortTax" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtParkir" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLainlain" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtKomunikasi" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:TextBox ID="txtUangMukaDLR" runat="server" Enabled="False" Width="75px" Visible="false">0</asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSTMakan" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtSTUangSk" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTiket" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtHotel" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtBBM" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTol" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTaxi" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAirPortTax" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtParkir" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLainlain" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtKomunikasi" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:TextBox ID="txtUangMuka" runat="server" Enabled="False">0</asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>Claim dikirimkan ke GA</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblAtasan" runat="server" Visible="False"></asp:Label>
                <asp:HiddenField ID="FLDgamode" runat="server" />
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" Enabled="False" OnClick="btnSubmit_Click" />
                &nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                <asp:Label ID="lblKet" runat="server"></asp:Label>
            </td>
            <td>Penyelesaian
            </td>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSTMakan" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtSTUangSk" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTiket" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtHotel" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtBBM" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTol" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTaxi" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAirPortTax" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtParkir" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLainlain" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtKomunikasi" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:TextBox ID="txtPenyelesaianDLR" Visible="false" Width="75px" runat="server"
                            Enabled="False">0</asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSTMakan" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtSTUangSk" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTiket" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtHotel" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtBBM" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTol" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtTaxi" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAirPortTax" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLaundry" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtParkir" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtLainlain" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtKomunikasi" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:TextBox ID="txtPenyelesaian" runat="server" Enabled="False">0</asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <div id="Loader" runat="server" class="loader">
        
                <asp:Image ID="imgLoad" runat="server" ImageUrl="~/Img/al_loading.gif" Height="28px" />
    </div>

    <script type="text/javascript">
        $(function () {
            $(':file').hide();
        });

        // This function will execute after file uploaded successfully
        function uploadComplete(sender, args) {
            alert("Upload success");
        }
        // This function will execute if file upload fails
        function uploadError(sender, args) {
            alert("Upload failed");
        }
    </script>
</asp:Content>
