<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="QCAstWebApplication.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome to QC Solutions Ast Billing</title>
    <link rel="Stylesheet" href="Styles/StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <table style="width:100%">
                    <tr>
                        <td><img src="Images/QCAstLogo.png" alt="QC Solutions Ast Billing" style="margin:20px 40px" /></td>
                        <td><!--<img src="Images/partnerLogos.png" alt="Ast Solutions' Partners" style="float:right; margin-right:40px" />--></td>
                    </tr>
                </table>
            </div>
            <center>
                <table class="loginTable" id="Table2" cellspacing="0" cellpadding="0" align="center">
				    <tr>
					    <td colspan="3" class="loginTop"></td>
				    </tr>
				    <tr>
                        <td class="loginLeftBorder"></td>
						<td height="7px"></td>
                        <td align="right" class="loginRightBorder"></td>
                    </tr>
				    <tr>
					    <td class="loginLeftBorder"></td>
						<td><asp:Label runat="server" ID="lblError" ForeColor="Red" /></td>
                        <td align="right" class="loginRightBorder"></td>
					</tr>
                    <tr>
                        <td class="loginLeftBorder"></td>
						<td><asp:label id="lblUserName" runat="server" CssClass="textall">Username:</asp:label></td>
                        <td align="right" class="loginRightBorder"></td>
                    </tr>
					<tr>
                        <td class="loginLeftBorder"></td>
						<td align="center">
                            <asp:textbox id="txtUserName" tabIndex="2" runat="server" CssClass="textboxStyle"></asp:textbox>
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" runat="server" ErrorMessage="<br />Please enter a Username" ControlToValidate="txtUserName" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td align="right" class="loginRightBorder"></td>
                    </tr>
                    <tr>
                        <td class="loginLeftBorder"></td>
						<td><asp:label id="lblPassword" runat="server" CssClass="textall">Password:</asp:label></td>
                        <td align="right" class="loginRightBorder"></td>
                    </tr>
                    <tr>
                        <td class="loginLeftBorder"></td>
						<td align="center">
                            <asp:textbox id="txtPassword" tabIndex="3" runat="server" CssClass="textboxStyle" TextMode="Password"></asp:textbox>
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" runat="server" ErrorMessage="<br />Please provide a password" ControlToValidate="txtPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td align="right" class="loginRightBorder"></td>
                    </tr>
                    <tr>
                        <td class="loginLeftBorder"></td>
						<td><asp:imagebutton id="btnLogin" tabIndex="4" runat="server" 
                                        CausesValidation="False" BorderStyle="None" onclick="btnLogin_Click" ImageUrl="Images/btnLogin.png" Height="27px" Width="76px"></asp:imagebutton></td>
                        <td align="right" class="loginRightBorder"></td>
                    </tr>
                    <tr>
                        <td class="loginLeftBorder"></td>
						<td><img src="Images/loginLogos.jpg" alt="QC Ast Billing Partners" /></td>
                        <td align="right" class="loginRightBorder"></td>
                    </tr>
				    <tr>
					    <td colspan="3" style="background-image:url('Images/login_bottom.jpg'); height:4px"></td>
				    </tr>
			    </table>
                <table style="color:White; font-size:11px">
                    <tr>
                        <td>&copy; Copyright QC Solutions, All rights reserved, 2012</td>
                    </tr>                    
                </table>
            </center>
        </div>
    </form>
</body>
</html>
