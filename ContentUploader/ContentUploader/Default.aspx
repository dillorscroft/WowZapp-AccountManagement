<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ContentUploader._Default" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="pnlPackItem" runat="server" Visible="false" Style="position: relative;">
        <fieldset>
            <legend>Edit:
                <asp:Literal ID="titleLegend" runat="server"></asp:Literal></legend>
            <asp:Panel ID="updateNotification" runat="server" Visible="false">
                <p>
                    Successfuly updated!</p>
            </asp:Panel>
            <ul class="insertForm">
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Title:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="editPackTitle" Name="txtContentPackTitle" MaxLength="25"
                                Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                ControlToValidate="txtContentPackTitle" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="ItemPackID" runat="server" />
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Description:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="editPackDescription" TextMode="MultiLine" Rows="3"
                                Columns="10" Name="txtContentPackTitle" MaxLength="25" Width="300px"></asp:TextBox>
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Content Pack Type:&nbsp;</li>
                        <li>
                            <asp:DropDownList runat="server" ID="selectedPackType">
                            </asp:DropDownList>
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Price:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="editContentPackPrice" Name="txtContentPackTitle"
                                MaxLength="25" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                                ControlToValidate="txtContentPackPrice" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                        </li>
                        <li style="width: 120px; text-align: right;">Sale Price:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="editContentPackSalePrice" Name="txtContentPackTitle"
                                MaxLength="25" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"
                                ControlToValidate="txtContentPackSalePrice" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Sale End Date:&nbsp;</li>
                        <li>
                            <telerik:raddatepicker id="editSaleEndDate" runat="server" zindex="30001" dateinput-dateformat="MM/dd/yyyy"
                                mindate="1/1/1800" maxdate="1/1/4900" />
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Available Date:&nbsp;</li>
                        <li>
                            <telerik:raddatepicker id="editAvailableDate" runat="server" zindex="30001" dateinput-dateformat="MM/dd/yyyy"
                                mindate="1/1/1800" maxdate="1/1/4900" />
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">End Date:&nbsp;</li>
                        <li>
                            <telerik:raddatepicker id="editEndDate" runat="server" zindex="30001" dateinput-dateformat="MM/dd/yyyy"
                                mindate="1/1/1800" maxdate="1/1/4900" />
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">
                            <asp:CheckBox ID="editIsFree" runat="server" />&nbsp;</li>
                        <li>Content Pack Is Free: </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">
                            <telerik:radbinaryimage id="EditimageContentPackIcon" runat="server" width="70px"
                                height="70px" resizemode="Fit" />
                            &nbsp;</li>
                        <li>&nbsp;
                            <asp:FileUpload ID="editFileIcon" runat="server" Width="400px" size="40"></asp:FileUpload>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                ControlToValidate="filePackIcon" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                </li>
            </ul>
            <div class="contentPackAD">
                Content Pack Ad:<br />
                <telerik:radbinaryimage id="EditimageContentPackAD" runat="server" style="max-width: 250px; max-height:300px;
                    margin: 10px 0;" resizemode="Fit" />
                <asp:FileUpload ID="editPackAd" runat="server" Width="300px" size="30"></asp:FileUpload>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                    ControlToValidate="filePackAd" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
            </div>
        </fieldset>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <asp:Button ID="btnDeletePack" runat="server" Text="Delete" OnClick="btnDeletePack_Click" OnClientClick="javascript:return confirm('Delete this item and all child items?')" />
                </td>
                <td style="width: 700px;">
                    <asp:Button ID="btnBackFromEdit" runat="server" Text="Back" OnClick="btnBackFromEdit_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnViewItems" runat="server" Text="View Items" OnClick="btnViewItems_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="editPackItem" runat="server" Text="Save" OnClick="editPackItem_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlPackList" runat="server">
        <div style="width: 100%; border-bottom: 1px solid #eee;">
            Content Type:&nbsp;
            <asp:DropDownList runat="server" ID="contentType" OnSelectedIndexChanged="contentType_SelectedIndexChanged"
                AutoPostBack="true">
            </asp:DropDownList>
            <br />
            <br />
        </div>
        <br />
        <div style="max-height: 550px; overflow: scroll; padding-bottom: 20px; min-height: 200px;">
            <telerik:radlistview id="rptPacks" allowpaging="false" groupitemcount="5" runat="server"
                enableembeddedskins="false" enableembeddedbasestylesheet="false" groupplaceholderid="ItemsContainer"
                itemplaceholderid="PackItems" onneeddatasource="rptPacks_NeedDataSource">
            <LayoutTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <asp:PlaceHolder ID="ItemsContainer" runat="server" />
                </table>
            </LayoutTemplate>
            <GroupTemplate>
            <tr> 
                <asp:PlaceHolder ID="PackItems" runat="server" />
            </tr>
            </GroupTemplate>
            <ItemTemplate>
                 <td align="center">
                 <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("ContentPackID", "~/Default.aspx?Pack={0}")%>'>

                    <telerik:RadBinaryImage ID="imageContentPackIcon" runat="server" DataValue='<%# Eval("ContentPackIcon") %>' Style="margin-top:10px; border:0;"
                                            Width="70px" Height="70px" ResizeMode="Fit" Visible='<%# Eval("ContentPackIcon") == null ? false : true %>' /><br />
                 <%# Eval("ContentPackTitle")%>
                 </asp:HyperLink>
                </td>  
            </ItemTemplate>
        </telerik:radlistview>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="width: 770px;">
                </td>
                <td align="right">
                    <asp:Button ID="btnNewCategory" runat="server" Text="New Category" OnClick="btnNewCategory_Click"
                        Style="margin-top: 20px;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlNewItem" runat="server" Visible="false" Style="position: relative;">
        <fieldset>
            <legend>New Content Pack</legend>
            <asp:Panel ID="pnlNewRecord" runat="server" Visible="false">
                <p>
                    Successfuly updated!</p>
            </asp:Panel>
            <ul class="insertForm">
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Title:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="txtContentPackTitle" Name="txtContentPackTitle" MaxLength="25"
                                Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtContentPackTitle" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Description:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="txtPackDescription" TextMode="MultiLine" Rows="3"
                                Columns="10" Name="txtContentPackTitle" MaxLength="25" Width="300px"></asp:TextBox>
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Content Pack Type:&nbsp;</li>
                        <li>
                            <asp:DropDownList runat="server" ID="ContentPackType">
                            </asp:DropDownList>
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Price:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="txtContentPackPrice" Name="txtContentPackTitle" MaxLength="25"
                                Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                ControlToValidate="txtContentPackPrice" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                        </li>
                        <li style="width: 120px; text-align: right;">Sale Price:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="txtContentPackSalePrice" Name="txtContentPackTitle"
                                MaxLength="25" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                                ControlToValidate="txtContentPackSalePrice" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Sale End Date:&nbsp;</li>
                        <li>
                            <telerik:raddatepicker id="SaleEndDate" runat="server" zindex="30001" dateinput-dateformat="MM/dd/yyyy"
                                mindate="1/1/1800" maxdate="1/1/4900" />
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Available Date:&nbsp;</li>
                        <li>
                            <telerik:raddatepicker id="AvailableDate" runat="server" zindex="30001" dateinput-dateformat="MM/dd/yyyy"
                                mindate="1/1/1800" maxdate="1/1/4900" />
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">End Date:&nbsp;</li>
                        <li>
                            <telerik:raddatepicker id="EndDate" runat="server" zindex="30001" dateinput-dateformat="MM/dd/yyyy"
                                mindate="1/1/1800" maxdate="1/1/4900" />
                        </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">
                            <asp:CheckBox ID="IsFree" runat="server" />&nbsp;</li>
                        <li>Content Pack Is Free: </li>
                    </ul>
                </li>
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Select an Icon:&nbsp;</li>
                        <li>
                            <asp:FileUpload ID="filePackIcon" runat="server" Width="400px" size="40"></asp:FileUpload>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                                ControlToValidate="filePackIcon" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                </li>
            </ul>
            <div class="contentPackAD">
                Content Pack Ad:<br />
                <img src="Styles/noPhoto.jpg" alt="" style="width: 250px; height: 300px; margin: 10px 0;" />
                <asp:FileUpload ID="filePackAd" runat="server" Width="300px" size="30"></asp:FileUpload>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                    ControlToValidate="filePackAd" ValidationGroup="InsertPack"></asp:RequiredFieldValidator>
            </div>
        </fieldset>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="width: 770px;">
                </td>
                <td align="right">
                    <asp:Button ID="btnBack" OnClick="btnBack_Click" runat="server" Text="Back" />
                </td>
                <td align="right">
                    <asp:Button ID="insertPackItem" runat="server" Text="Save" OnClick="insertPackItem_Click"
                        ValidationGroup="InsertPack" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
