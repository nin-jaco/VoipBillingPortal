<%@ Page Title="QC Ast: User Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="QCAstWebApplication.UserManagement" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <table cellpadding="0" cellspacing="0" style="text-align:left; width:920px">
        <tr>
            <td>
                <h2 style="margin-left:15px">User Management</h2>
                <p class="radialShadow"></p>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <ajaxToolkit:ConfirmButtonExtender ID="cbeConfirmDelete" runat="server" ConfirmText="Are you sure you want to delete this user?" TargetControlID="ibDeleteUser">
                </ajaxToolkit:ConfirmButtonExtender>
                <ajaxToolkit:ConfirmButtonExtender ID="cbeConfirmEdit" runat="server" ConfirmText="Are you sure you want to edit this user?" TargetControlID="ibEditUser">
                </ajaxToolkit:ConfirmButtonExtender>

            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="searchUser">
                            <table align="center" class="blockOne">
                                <tr>
                                    <td colspan="2" style="color:White; font-style:italic">Search for a User:</td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="color:White; font-style:italic">
                                        <asp:Label runat="server" ID="lblSearchError" ForeColor="Red" />
                                        </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:textbox id="tbSearch" runat="server" CssClass="textboxStyle" Width="150px"></asp:textbox></td>
                                    <td>
                                        <asp:imagebutton id="btnSearch" runat="server" ImageAlign="Left" 
                                        BorderStyle="None" onclick="btnSearch_Click" ImageUrl="Images/btnGoWhite.png"></asp:imagebutton>
                                    </td>
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
                        <td rowspan="2" class="amendUser" valign="top">
                            <table align="left" style="margin-left:65px; margin-top:50px" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td colspan="2" style="font-style:italic">
                <asp:HiddenField runat="server" ID="hfFormAction" />
                                        Amend user details, delete or add new user here:</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblAmendError" ForeColor="Red" />
                                        <asp:Label runat="server" ID="lblSuccess" ForeColor="Turquoise" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>UserID:</td>
                                    <td><asp:TextBox ID="tbUserId" runat="server" CssClass="textboxStyle" ReadOnly="true" Enabled="false"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><span class="requiredField">*</span>Username:</td>
                                    <td><asp:TextBox ID="tbUsername" runat="server" CssClass="textboxStyle"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="requiredField">*</span>User Profile:</td>
                                    <td><asp:DropDownList ID="ddlProfiles" Width="250" runat="server" DataTextField="Name" DataValueField="IdProfile" CssClass="ddlStyle" ></asp:DropDownList>
                                        </td>
                                </tr>
                                <tr>
                                    <td>Client:</td>
                                    <td><asp:DropDownList ID="ddlClient" Width="250" runat="server" DataTextField="ClientName" DataValueField="IdClient" CssClass="ddlStyle" ></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><span class="requiredField">*</span>Password:</td>
                                    <td><asp:TextBox ID="tbPassword" runat="server" CssClass="textboxStyle"></asp:TextBox>
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
                <asp:ImageButton ID="ibNewUser" runat="server" 
                                            ImageUrl="Images/iconAddUser.png" ToolTip="Add User" 
                                            onclick="ibNewUser_Click" />
                                        <asp:ImageButton ID="ibDeleteUser" runat="server" 
                                            ImageUrl="Images/iconDeleteUser.png" ToolTip="Delete User" 
                                            onclick="ibDeleteUser_Click" />
                                        <asp:ImageButton ID="ibEditUser" runat="server" 
                                            ImageUrl="Images/iconSaveUser.png" ToolTip="Edit User" 
                                            onclick="ibSaveUser_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="selectUser" valign="top">
                            <table align="center" class="blockTwo">
                                <tr>
                                    <td style="font-style:italic">Select the User to edit:</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lbSearchedUsers" runat="server" Width="200px" ShowHeader="False" 
                                        BackColor="Transparent" ForeColor="Gray"
							                GridLines="None" AutoGenerateColumns="False" onselectedindexchanged="lbSearchedUsers_SelectedIndexChanged" 
                                            BorderStyle="solid" BorderWidth="0"
                                            SelectionMode="Single" AutoPostBack="True" DataTextField="Username" 
                                            DataValueField="IdUser" Height="180px">
						                </asp:ListBox>
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
