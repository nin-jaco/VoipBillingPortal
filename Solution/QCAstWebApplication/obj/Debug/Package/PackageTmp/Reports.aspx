<%@ Page Title="Run Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="QCAstWebApplication.Reports" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/highcharts.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <table cellpadding="0" cellspacing="0" style="text-align:left">
        <tr>
            <td>
                <h2 style="margin-left:15px">Reports</h2>
                <p class="radialShadow"></p>
                 <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblError" ForeColor="Red" />
                <asp:Label runat="server" ID="lblSuccess" ForeColor="Turquoise" />
            </td>
0        </tr>
        <tr>
            <td class="reportForm">
                <table cellpadding="0" cellspacing="0" align="center" style="margin-top:40px">
                    <tr>
                        <td>
                            Report:
                        </td>
                        <td><asp:DropDownList ID="ddlReport" runat="server" Width="150" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged" AutoPostBack="True" CssClass="ddlStyle" DataTextField="ReportName" DataValueField="IdReport"></asp:DropDownList></td>
                        <td><asp:ImageButton ID="iRunReport" ImageUrl="~/Images/btnGo.png" runat="server" onclick="iRunReport_Click" /></td>
                    </tr>
                    <tr id="trClientRow" runat="server">
                        <td colspan="3"> 
                            <table cellpadding="5" cellspacing="0">
                                <tr>
                                    <td>Client:</td>
                                    <td><asp:DropDownList ID="ddlClient" runat="server" Width="150" AutoPostBack="True" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged" DataTextField="ClientName" DataValueField="IdClient" CssClass="ddlStyle"></asp:DropDownList></td>
                                    <td>Invoices:</td>
                                    <td><asp:DropDownList ID="lstClientHistory" runat="server" width="150" CssClass="ddlStyle" DataTextField="InvoiceDate" DataValueField="IdInvoice"></asp:DropDownList></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trDateRow" runat="server">
                        <td colspan="3">
                            <table cellpadding="5" cellspacing="0">
                                <tr>
                                    <td>From Date:</td>
                                    <td>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" Format="yyyy-MM-dd" PopupButtonID="imgCalendar">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="textboxDate" ReadOnly="true"></asp:TextBox>
                                        <asp:Image runat="server" ID="imgCalendar" ImageUrl="~/Images/calendar.gif" />
                                    </td>
                                    <td>To Date:</td>
                                    <td>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" Format="yyyy-MM-dd" PopupButtonID="imgCalendarTwo">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="textboxDate" ReadOnly="true"></asp:TextBox>
                                        <asp:Image runat="server" ID="imgCalendarTwo" ImageUrl="~/Images/calendar.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="lblReportName" /></td>
        </tr>
        <tr>
            <td>
                <div id="graphContainer" style="width: 100%; height: 400px" runat="server">
                    <asp:Literal ID="grapSum" runat="server"></asp:Literal>
                </div>
                <div id="Div1"  runat="server">
                    <table style="width:100%" align="center">
                        <tr>
                            <td align="center">
                                <rsweb:ReportViewer ID="rptViewer" runat="server" style="width:850px" 
                                    ondrillthrough="rptViewer_Drillthrough" Height="560px">
                                    <LocalReport ReportPath=""></LocalReport>
                                </rsweb:ReportViewer>                                
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
