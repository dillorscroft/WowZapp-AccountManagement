<%@ Page Title="Content Pack Items" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ContentPackItems.aspx.cs" Inherits="ContentUploader.ContentPackItems"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="pnlPackItem" runat="server" Visible="false">
        <fieldset>
            <legend>Edit:
                <asp:Literal ID="titleLegend" runat="server"></asp:Literal></legend>
            <asp:Panel ID="updateNotification" runat="server" Visible="false">
                <p>
                    Successfuly updated!</p>
            </asp:Panel>
            <ul class="insertForm">
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Title:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="editPackTitle" Name="txtContentPackTitle" MaxLength="25"
                                Width="200px"></asp:TextBox>
                            <br />
                            <asp:HiddenField ID="ItemPackID" runat="server" />
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Content Pack Type:&nbsp;</li>
                        <li>
                            <asp:DropDownList runat="server" ID="selectedPackType">
                            </asp:DropDownList>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Icon:&nbsp;</li>
                        <li>
                            <telerik:radbinaryimage id="Image1" runat="server" width="70px" height="70px" resizemode="Fit" />
                            <asp:FileUpload ID="editFileIcon" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Large:&nbsp; </li>
                        <li>
                            <asp:FileUpload ID="editFileDataLarge" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Medium:&nbsp; </li>
                        <li>
                            <asp:FileUpload ID="editFileDataMedium" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Small:&nbsp; </li>
                        <li>
                            <asp:FileUpload ID="editFileDataSmall" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Tiny:&nbsp;</li>
                        <li>
                            <asp:FileUpload ID="editFileDataTiny" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
            </ul>
        </fieldset>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="width: 770px;">
                    <asp:Button ID="btnDeletePack" runat="server" Text="Delete" OnClick="btnDeletePack_Click" OnClientClick="javascript:return confirm('Delete this item?')" />
                </td>
                <td align="right">
                    <asp:Button ID="btnBackFromEdit" runat="server" Text="Back" OnClick="btnBackFromEdit_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="editPackItem" runat="server" Text="Save" OnClick="editPackItem_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlPackList" runat="server">
        <div style="max-height: 550px; overflow: scroll; padding-bottom: 20px; min-height: 200px;">
            <telerik:radlistview id="rptPackItems" allowpaging="false" groupitemcount="5" runat="server"
                enableembeddedskins="false" enableembeddedbasestylesheet="false" groupplaceholderid="ItemsContainer"
                itemplaceholderid="PackItems" onneeddatasource="rptPackItems_NeedDataSource"
                onitemdatabound="rptPackItems_ItemDataBound">
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
                 <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("ContentPackItemID", "~/ContentPackItems.aspx?Item={0}")%>'>

                    <telerik:RadBinaryImage ID="imageContentPackItemIcon" runat="server" DataValue='<%# Eval("ContentPackItemIcon") %>' Style="margin-top:10px; border:0;"
                                            Width="70px" Height="70px" ResizeMode="Fit" Visible='<%# Eval("ContentPackItemIcon") == null ? false : true %>' /><br />
                <%# Eval("ContentItemTitle")%>
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
                    <asp:Button ID="btnBack" OnClick="btnBack_Click" runat="server" Text="Back" Style="margin-top: 20px;" />
                </td>
                <td align="right">
                    <asp:Button ID="btnNewItem" OnClick="btnNewItem_Click" runat="server" Text="New Item"
                        Style="margin-top: 20px;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlNewPackItems" runat="server" Visible="false">
        <fieldset>
            <legend>Insert new item</legend>
            <asp:Panel ID="pnlInsertNotification" runat="server" Visible="false">
            <p>
                    Successfuly updated!</p>
            </asp:Panel>
          
            <ul class="insertForm">
                <li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Title:&nbsp;</li>
                        <li>
                            <asp:TextBox runat="server" ID="txtContentPackTitle" Name="txtContentPackTitle" MaxLength="25"
                                Width="200px"></asp:TextBox></li>
                    </ul>
                </li>
                <%--<li style="width: 600px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Content Pack Type:&nbsp;</li>
                        <li>
                            <asp:DropDownList runat="server" ID="ddlContentPacks">
                            </asp:DropDownList>
                        </li>
                    </ul>
                </li>--%>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Icon:&nbsp;</li>
                        <li>
                            <asp:FileUpload ID="fileIcon" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Large:&nbsp; </li>
                        <li>
                            <asp:FileUpload ID="fileDataLarge" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Medium:&nbsp; </li>
                        <li>
                            <asp:FileUpload ID="fileDataMedium" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Small:&nbsp;</li>
                        <li>
                            <asp:FileUpload ID="fileDataSmall" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
                <li style="width: 700px; float: left;">
                    <ul>
                        <li style="width: 120px; text-align: right;">Data Tiny:&nbsp;</li>
                        <li>
                            <asp:FileUpload ID="fileDataTiny" runat="server"  Width="500px" size="50"></asp:FileUpload>
                        </li>
                    </ul>
                </li>
            </ul>
        </fieldset>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="width: 770px;">
                </td>
                <td align="right">
                    <asp:Button ID="btnBackInsert" runat="server" Text="Back" OnClick="btnBackInsert_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="insertPackItem" runat="server" Text="Save" OnClick="insertPackItem_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
