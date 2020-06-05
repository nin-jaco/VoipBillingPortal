
<%@ Page Title="Billing" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Billing.aspx.cs" Inherits="Pages_Billing" %>

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

    function CheckBeforeRun()
    {
        var clientHistory = document.getElementById(document.forms[0].h_lstClientHistory.value);
        if (clientHistory)
        {
            if (clientHistory.selectedIndex == 0)
            {
                var endDate = document.getElementById(document.forms[0].h_EndDate.value);
                if(endDate)
                {
                    if(endDate.value == '')
                    {
                        alert('Please enter an end date for the report to run');
                        return false;
                    }
                }
            }
        }
        return true;
    }





    $(document).ready(function () {
        InitAll();
    });

    function InitAll() {

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
                    <td style="width: 910px; height:60px">
                        <table>
                            <tr>
                                <td style="width:10px"></td>
                                <td style="width:40px">
                                    <asp:Label ID="lblClient" runat="server" Text="Client :"></asp:Label>
                                </td>  
                                <td style="width:150px">
                                    <asp:DropDownList ID="lstClient" runat="server" style="width:100%" 
                                        AutoPostBack="True" onselectedindexchanged="lstClient_SelectedIndexChanged"></asp:DropDownList>
                                </td>  
                                <td style="width:10px"></td>
                                <td style="width:90px">Client Invoices:</td>
                                <td style="width:150px" id="tdClientHistory" runat="server">
                                    <asp:DropDownList ID="lstClientHistory" runat="server" style="width:100%"></asp:DropDownList>
                                </td>
                                <td style="width:10px"></td>
                                <td style="width:60px">Last Date</td>
                                <td>
                                    <asp:TextBox ID="txtEndDate" runat="server" style="width:80px"></asp:TextBox>
                                </td>
                                <td style="width:10px"></td>
                                <td style="width:50px">
                                    <asp:ImageButton ID="imgRunReport" ImageUrl="~/Images/go.JPG" runat="server" 
                                        onclick="imgRunReport_Click" style="height: 17px" />
                                </td>
                                <td style="width:80px">
                                    <asp:Button ID="btnCreateInvoice" runat="server" Text="Create Invoice" 
                                        onclick="btnCreateInvoice_Click" />
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
                    <div id="divReport" runat="server">
                        <table style="width:100%">
                            <tr>
                                <td>
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
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>                      
            </table>
			</div>

            <script type="text/javascript" language="javascript">
                function CheckForClient()
                {
                    var obj = document.getElementById(document.forms[0].h_lstClient.value);
                    if(obj)
                    {
                        if(isNaN(obj.value))
                        {
                            alert('Please select a client in order to print an invoice');
                            obj.focus();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            </script>
</asp:Content>
