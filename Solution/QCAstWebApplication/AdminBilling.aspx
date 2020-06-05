<%@ Page Title="Billing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminBilling.aspx.cs" Inherits="QCAstWebApplication.AdminBilling" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <table cellpadding="0" cellspacing="0" style="width:920px; text-align:left">
        <tr>
            <td>
                <h2 style="margin-left:15px">Billing</h2>
                <p class="radialShadow"></p>
                <asp:Label runat="server" ForeColor="Red" ID="lblError" />
                <asp:Label runat="server" ForeColor="Turquoise" ID="lblSuccess" />
            </td>
        </tr>
        <tr>
            <td class="billingForm" align="center">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                <table align="center" style="margin-left:20px; margin-top:25px">
                    <tr>
                        <td>
                            <asp:Label ID="lblClient" runat="server" Text="Client:"></asp:Label>
                        </td>  
                        <td>
                            <asp:DropDownList ID="ddlClients" runat="server" CssClass="ddlStyle" AutoPostBack="True" onselectedindexchanged="DdlClientsSelectedIndexChanged" DataTextField="ClientName" DataValueField="IdClient"></asp:DropDownList>
                        </td>  
                        <td>Client Invoices:</td>
                        <td id="tdClientHistory" runat="server">
                            <asp:DropDownList ID="ddlClientInvoiceHistory" runat="server" CssClass="ddlStyle" DataTextField="InvoiceDate" DataValueField="IdInvoice"></asp:DropDownList>
                        </td>
                        <td>Last Date:</td>
                        <td>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="textboxDate"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtEndDate" Format="yyyy-MM-dd" PopupButtonID="imgCalendar" >
                            </ajaxToolkit:CalendarExtender>
                            <asp:Image runat="server" ID="imgCalendar" ImageUrl="~/Images/calendar.gif" Margin="0px" />
                        </td>
                        <td style="text-align:left">
                            <asp:ImageButton ID="ibGo" ImageUrl="~/Images/btnGo.png" runat="server" 
                                onclick="IbGoClick" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>  
                        <td>
                            &nbsp;</td>  
                        <td>&nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td colspan="2">
                            <asp:ImageButton ID="btnCreateInvoice" ImageUrl="~/Images/btnCreateInvoice.png" runat="server" onclick="BtnCreateInvoiceClick" />
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divReport" runat="server">
                    <table style="width:100%" align="center">
                        <tr>
                            <td align="center">
                                <rsweb:ReportViewer ID="rptViewer" runat="server" style="width:850px" 
                                    Height="560px">
                                    <LocalReport ReportPath="">
                                    </LocalReport>
                                </rsweb:ReportViewer>                                
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
