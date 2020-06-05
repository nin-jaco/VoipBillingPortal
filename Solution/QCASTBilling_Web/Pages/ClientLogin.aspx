<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientLogin.aspx.cs" Inherits="Account_ClientLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        function CheckForClient() {
            var obj = document.getElementById(document.forms[0].h_lstClient.value);
            if (obj) {
                if (isNaN(obj.value)) {
                    alert('Please select a client');
                    obj.focus();
                    return false;
                }
                else {
                    return true;
                }
            }
        }
</script>
</head>
<body>
    <form id="form1" runat="server">

			<TABLE style="LEFT: 516px; POSITION: absolute; TOP: 198px" id="Table2" 
                cellSpacing="0" cellPadding="0" width="300px" bgColor="white" border="0">
				<TR>
					<TD colSpan="5" style="background-image:url(../images/login_top.jpg); height:22px"></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD style="background-repeat:repeat-y; background-image:url(../images/left.png); width:4px"></TD>
					<TD></TD>
					<TD vAlign="middle" align="center">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="200" border="0">
							<TR>
								<TD height="5">Client</TD>
							</TR>
							<tr>
								<TD style="HEIGHT: 7px"><asp:DropDownList ID="lstClients" style="width: 100%" runat="server"></asp:DropDownList></TD>
							</tr>
							<TR>
								<TD><asp:label id="lblUserName" runat="server" CssClass="textall" Height="14px" Width="69px"></asp:label></TD>
							</TR>
                            <tr>
								<TD style="HEIGHT: 7px"></TD>
							</tr>
							<TR>
								<TD><asp:textbox id="txtUserName" tabIndex="2" runat="server" CssClass="boxes" Width="100%"></asp:textbox></TD>
							</TR>
							<TR>
								<TD><asp:label id="lblPassword" runat="server" CssClass="textall" Height="14px" Width="69px"></asp:label></TD>
							</TR>
                            <tr>
								<TD style="HEIGHT: 7px"></TD>
							</tr>
							<TR>
								<TD><asp:textbox id="txtPassword" tabIndex="3" runat="server" CssClass="boxes" Width="100%" TextMode="Password"></asp:textbox></TD>
							</TR>
							<TR>
								<TD height="5"></TD>
							</TR>
							<TR>
								<TD><asp:imagebutton id="btnLogin" tabIndex="4" runat="server" 
                                        CausesValidation="False" BorderStyle="None" onclick="btnLogin_Click"></asp:imagebutton></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
							</TR>
							<TR>
								<TD><asp:hyperlink id="linkForgotPassword" runat="server" CssClass="textall" Font-Size="10pt" Font-Names="Verdana"
										Visible="False"></asp:hyperlink></TD>
							</TR>
						</TABLE>
					</TD>
					<TD></TD>
					<TD align="right" style="background-repeat:repeat-y; background-image:url(../images/right.png); width:4px"></TD>
				</TR>
				<TR>
					<TD colSpan="5" style="background-image:url(../images/login_bottom.gif); height:4px"></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
			</TABLE>
    </form>
</body>
</html>
