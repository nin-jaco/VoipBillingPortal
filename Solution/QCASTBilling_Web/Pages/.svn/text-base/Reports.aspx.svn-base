
<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Reports.aspx.cs" Inherits="Pages_Reports" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<link rel="stylesheet" type="text/css" href="../Css/jqui/south-street/jquery-ui-1.8.9.custom.css" />
<script type="text/javascript" src= "../Scripts/lib/jquery-1.4.4.min.js"></script>
<script type="text/javascript" src= "../Scripts/lib/jquery-ui-1.8.9.custom.min.js"></script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">


<script type="text/javascript" language="javascript">
    /***

    * Page onload
    ***/

    $(document).ready(function () {
        InitAll();
    });

    function InitAll() {


        $('#MainContent_txtStartDate').datepicker({
            showOn: 'button',
            buttonImage: '../images/calendar.gif',
            buttonImageOnly: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            showWeek: true,
            firstDay: 1,
            dateFormat: 'dd/mm/yy',
            showAnim: 'slideDown',
            changeYear: true,
            yearRange: "-5:+0"
        });

        $('#MainContent_txtEndDate').datepicker({
            showOn: 'button',
            buttonImage: '../images/calendar.gif',
            buttonImageOnly: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            showWeek: true,
            firstDay: 1,
            dateFormat: 'dd/mm/yy',
            showAnim: 'slideDown',
            changeYear: true,
            yearRange: "-5:+0"
        });

    }


    function CheckForClient() {
        var client = document.getElementById(document.forms[0].h_lstClient.value);
        if (client) {
            if (isNaN(client.value)) {
                alert('Please select a client to run a report');
                obj.focus();
                return false;
            }

            // Dont validate dates if its the first two reports
            var report = document.getElementById(document.forms[0].h_lstReport.value);
            if (report.selectedIndex == 0 || report.selectedIndex == 1)
            {
                return true;
            }

            // Check for a date range if it is one of the 3 reports that require a date range
            if (client.selectedIndex != 0 && client.selectedIndex != 1) {
                var startDate = document.getElementById(document.forms[0].h_StartDate.value);
                var endDate = document.getElementById(document.forms[0].h_EndDate.value);

                if (startDate.value == '') {
                    alert('Please enter a start date');
                    startDate.focus();
                    return false;
                }

                if (endDate.value == '') {
                    alert('Please enter an end date');
                    endDate.focus();
                    return false;
                }

                // Now check the start date is not after the end date, now we know there are both start and end dates entered.
                var dt1  = parseInt(startDate.value.substring(0,2),10);
                var mon1 = parseInt(startDate.value.substring(3, 5), 10);
                var yr1 = parseInt(startDate.value.substring(6, 10), 10);
                var dt2 = parseInt(endDate.value.substring(0, 2), 10);
                var mon2 = parseInt(endDate.value.substring(3, 5), 10);
                var yr2 = parseInt(endDate.value.substring(6, 10), 10); 
                var _startDate = new Date(yr1, mon1, dt1); 
                var _endDate = new Date(yr2, mon2, dt2);

                if (_endDate < _startDate)
                {
                    alert('Please enter a start date less than the end date');
                    return false;
                }

            }

            return true;
        }
    }


</script>

            <div align="left">
            <table cellpadding="0" cellspacing="0" style="height:60px; width:910px">
                <tr>
                    <td  colspan="5" style="width:10px; height:5px; background-repeat:no-repeat; background-image:url(../images/top.png)"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td style="width:10px"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="left" valign="top" style="width:4px; background-repeat:repeat-y; background-image:url(../images/left.png)"></td>
                    <td style="width: 910px; height:100px">
                        <table>
                            <tr runat="server" id="trClientRow">
                                <td style="width:10px"></td>
                                <td style="width:100px">
                                    <asp:Label ID="lblClient" runat="server" Text="Client :"></asp:Label>
                                </td>  
                                <td style="width:200px">
                                    <asp:DropDownList ID="lstClient" runat="server" style="width:100%" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="lstClient_SelectedIndexChanged"></asp:DropDownList>
                                </td>  
                                <td style="width:10px"></td>
                                <td style="width:50px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10px"></td>
                                <td style="width:100px">
                                    <asp:Label ID="Label1" runat="server" Text="Select Report :"></asp:Label>
                                </td>  
                                <td style="width:200px">
                                    <asp:DropDownList ID="lstReport" runat="server" style="width:100%" 
                                        onselectedindexchanged="lstReport_SelectedIndexChanged" 
                                        AutoPostBack="True"></asp:DropDownList>
                                </td>
                                <td style="width:10px"></td>
                                <td style="width:10px"></td>
                                <td>
                                    <table runat="server" id="tblInvoices">
                                        <tr>
                                            <td>
                                                <td style="width:120px" id="tdClientHistoryText" runat="server">Client Invoices:</td>
                                                <td style="width:150px" id="tdClientHistoryDropDown" runat="server">
                                                    <asp:DropDownList ID="lstClientHistory" runat="server" style="width:100%"></asp:DropDownList>
                                                </td>
                                            <td>
                                        </tr>
                                    </table>
                                    <table runat="server" id="tblDateRange">
                                        <tr>
                                            <td>
                                        <td>From date:</td>
                                        <td>
                                                    <asp:TextBox class="tbox" ID="txtStartDate" runat="server"
                                                    Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width:8px"></td>
                                        <td>To date:</td>
                                        <td>
                                                    <asp:TextBox class="tbox" ID="txtEndDate" runat="server"
                                                    Width="70px"></asp:TextBox>
                                        </td>
                                            </td>
                                            </tr>
                                    </table>
                                </td>
                                <td style="width:10px"></td>
                                <td style="width:50px"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td align="right">
                                    <asp:ImageButton ID="imgRunReport" ImageUrl="~/Images/go.JPG" runat="server" onclick="imgRunReport_Click" style="height: 17px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" style="background-repeat:repeat-y; width: 4px;  background-image:url(../images/right.png)"></td>
                    <td></td>
                </tr>  
                <tr>
                    <td valign="top" colspan="5" style="height:5px;background-repeat:no-repeat; background-image:url(../images/bottom.png)"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="height:10px;"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>                      
                <tr>
                    <td valign="top" colspan="5">
                    <div  runat="server">
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <rsweb:ReportViewer ID="rptViewer" runat="server" style="width:850px" 
                                        ondrillthrough="rptViewer_Drillthrough" Height="560px">
                                        <LocalReport ReportPath=""></LocalReport>
                                    </rsweb:ReportViewer>                                
                                </td>
                            </tr>
                        </table>
                    </div>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>                      
            </table>
			</div>

</asp:Content>
