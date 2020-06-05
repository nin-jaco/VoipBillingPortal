<%@ Page Title="Client Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientManagement.aspx.cs" Inherits="QCAstWebApplication.ClientManagement" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <table cellpadding="0" cellspacing="0" style="text-align:left; width:920px">
        <tr>
            <td>
                <h2 style="margin-left:15px">Client Management</h2>
                <p class="radialShadow"></p>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <ajaxToolkit:ConfirmButtonExtender ID="cbeConfirmDelete" runat="server" ConfirmText="Are you sure you want to delete this Client?" TargetControlID="ibDeleteClient" >
                </ajaxToolkit:ConfirmButtonExtender>
                <ajaxToolkit:ConfirmButtonExtender ID="cbeConfirmEdit" runat="server" ConfirmText="Are you sure you want to edit this Client?" TargetControlID="ibEditClient">
                </ajaxToolkit:ConfirmButtonExtender>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" style="width:920px">
                    <tr>
                        <td style="width:268px;" valign="top">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                        <td class="searchClient" style="height:186px">
                            <table align="center" class="blockOne">
                                <tr>
                                    <td colspan="2" style="color:White; font-style:italic">Search for a Client:</td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label runat="server" ID="lblSearchError" ForeColor="Red" /></td>
                                </tr>
                                <tr>
                                    <td><asp:textbox id="tbSearch" runat="server" CssClass="textboxStyle" Width="150px"></asp:textbox></td>
                                    <td><asp:imagebutton id="btnSearch" runat="server" ImageAlign="Left" 
                                        BorderStyle="None" onclick="btnSearch_Click" ImageUrl="~/Images/btnGoWhite.png"></asp:imagebutton></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:RadioButtonList runat="server" ID="rblSearchFormat" RepeatDirection="Horizontal" ForeColor="White">
                                            <asp:ListItem Enabled="true" Selected="True" Text="All" Value="All"></asp:ListItem>
                                            <asp:ListItem Enabled="true" Text="Containing" Value="Containing"></asp:ListItem>
                                            <asp:ListItem Enabled="true" Text="Exact" Value="Exact"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                                <tr>
                        <td class="selectClient" valign="top">
                            <table align="center" class="blockTwo">
                                <tr>
                                    <td style="font-style:italic">Select the Client to edit:</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lbSearchedClients" runat="server" Width="200px" ShowHeader="False" 
                                        BackColor="Transparent" ForeColor="Gray"
							                GridLines="None" AutoGenerateColumns="False" onselectedindexchanged="lbSearchedClients_SelectedIndexChanged" 
                                            BorderStyle="solid" BorderWidth="0"
                                            SelectionMode="Single" AutoPostBack="True" DataTextField="ClientName" 
                                            DataValueField="IdClient" Height="180px">
						                </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                            </table>
                        </td>
                        <td style="width:652px" valign="top" class="amendClient">
                            <table align="left" style="margin-left:35px; margin-top:70px" cellpadding="5" cellspacing="0">
                                <tr>
                                    <td colspan="2" style="font-style:italic">
                                        Amend client details, delete or add new client here:</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblAmendError" ForeColor="Red" />
                                        <asp:Label runat="server" ID="lblSuccess" ForeColor="Turquoise" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Client Id:</td>
                                    <td><asp:TextBox ID="tbClientId" runat="server" CssClass="textboxStyle" Enabled="false"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><span class="requiredField">*</span>Client Name:</td>
                                    <td><asp:TextBox ID="tbClientDesc" runat="server" CssClass="textboxStyle"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Range From:</td>
                                    <td><asp:TextBox ID="tbRangeFrom" runat="server" CssClass="textboxStyle"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="tbRangeFrom" FilterType="Numbers" >
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Range To:</td>
                                    <td><asp:TextBox ID="tbRangeTo" runat="server" CssClass="textboxStyle"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="tbRangeTo" FilterType="Numbers" >
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="requiredField">*</span>Client Code:</td>
                                    <td><asp:TextBox ID="tbClientCode" runat="server" CssClass="textboxStyle"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><span class="requiredField">*</span>Accounting Code:</td>
                                    <td><asp:TextBox ID="tbAccountingCode" runat="server" CssClass="textboxStyle"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Cellular Number:</td>
                                    <td><asp:TextBox ID="tbCellular" runat="server" CssClass="textboxStyleSmall" MaxLength="10"></asp:TextBox><span class="smallText">(Format: 0115552323)</span>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbCellular" FilterType="Numbers" >
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Email Address:</td>
                                    <td><asp:TextBox ID="tbEmail" runat="server" CssClass="textboxStyle"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbEmail" FilterType="Custom, Numbers, LowercaseLetters" ValidChars="@." >
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preferred Method of Communication:</td>
                                    <td><asp:DropDownList runat="server" ID="ddlPrefContactMethod" DataTextField="Description" DataValueField="IdMethod" CssClass="ddlStyle" /></td>
                                </tr>
                                <tr>
                                    <td>Maximum Duration:</td>
                                    <td><asp:TextBox ID="tbMaxDuration" runat="server" CssClass="textboxStyleSmall"></asp:TextBox><span class="smallText">(Hours)</span>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbMaxDuration" FilterType="Numbers" >
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Maximum Price:</td>
                                    <td><asp:TextBox ID="tbMaxPrice" runat="server" CssClass="textboxStyleSmall"></asp:TextBox><span class="smallText">(Rands)</span>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbMaxPrice" FilterType="Numbers" >
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Maximum International Duration:</td>
                                    <td><asp:TextBox ID="tbMaxIntDuration" runat="server" CssClass="textboxStyleSmall"></asp:TextBox><span class="smallText">(Hours)</span>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="tbMaxIntDuration" FilterType="Numbers" >
                                        </ajaxToolkit:FilteredTextBoxExtender></td>
                                </tr>
                                <tr>
                                    <td>Maximum International Price:</td>
                                    <td><asp:TextBox ID="tbMaxIntPrice" runat="server" CssClass="textboxStyleSmall"></asp:TextBox><span class="smallText">(Rands)</span>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="tbMaxIntPrice" FilterType="Numbers" >
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr id="trButtons" runat="server">
                                    <td colspan="2" style="text-align:right">
                                        <asp:ImageButton runat="server" ImageUrl="~/Images/btnSubmit.png" 
                                            ID="ibBtnSubmit" onclick="ibBtnSubmit_Click" />
                                        <asp:ImageButton runat="server" ImageUrl="~/Images/btnCancel.png" ID="ibCancel" 
                                            onclick="ibCancel_Click" />
                                    </td>
                                </tr>
                                <tr id="trIcons" runat="server">
                                    <td colspan="2" style="text-align:right">
                <asp:ImageButton ID="ibAddClient" runat="server" 
                                            ImageUrl="Images/iconAddClient.png" ToolTip="Add Client" 
                                            onclick="ibAddClient_Click" />
                                        <asp:ImageButton ID="ibDeleteClient" runat="server" 
                                            ImageUrl="Images/iconDeleteClient.png" ToolTip="Delete Client" 
                                            onclick="ibDeleteClient_Click" />
                                        <asp:ImageButton ID="ibEditClient" runat="server" 
                                            ImageUrl="Images/iconEditClient.png" ToolTip="Edit Client" 
                                            onclick="ibEditClient_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
