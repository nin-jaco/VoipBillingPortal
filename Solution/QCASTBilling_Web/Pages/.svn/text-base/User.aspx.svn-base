<%@ Page Language="C#"  MasterPageFile="~/Site.master"  AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="Pages_User" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
            <div align="left">
            <table>
            <tr style="height: 140px">
                <td style="height: 100px; width:264px">
                    <table cellpadding="0" cellspacing="0" style="height:100%; width:100%">
                        <tr>
                            <td  colspan="3" style="background-image:url(../images/Search_Criteria.jpg); background-repeat:no-repeat; height: 22px"></td>
                            <td></td>
                            <td  style="width:4px"></td>
                        </tr>
                        <tr valign="top">
                            <td align="left" valign="top" style="background-repeat:repeat-y; background-image:url(../images/left.png); width:4px" id="tdSearchLeft" runat="server"></td>
                            <td style="width:260px">
			                <table id="tblView" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD width="15" height="13"></TD>
					<TD align="right" width="45" height="13"></TD>
					<TD width="3" height="13"></TD>
					<TD width="180" height="13"></TD>
					<TD width="25" height="13"></TD>
					<TD width="25" height="13"></TD>
				</TR>
				<TR>
					<td></td>
					<TD align="right"><asp:label id="lblSearch" runat="server" CssClass="textall">Search</asp:label></TD>
					<TD></TD>
					<TD align="right" width="25">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD width="150px"><asp:textbox id="txtSearch" runat="server" Height="18px" CssClass="textall" Width="150px"></asp:textbox></TD>
								<TD align="left"><asp:imagebutton id="btnSearch" runat="server" ImageAlign="Left" 
                                        BorderStyle="None" onclick="btnSearch_Click"></asp:imagebutton></TD>
							</TR>
						</TABLE>
					</TD>
					<TD style="HEIGHT: 30px" align="right" width="25px"></TD>
				</TR>
				<TR>
					<td></td>
					<TD align="right"></TD>
					<TD></TD>
					<TD><asp:radiobutton id="rdExact" runat="server" 
                            CssClass="textall" BorderStyle="None" GroupName="SearchType" Text="Exact"></asp:radiobutton>
                        <asp:radiobutton id="rdContaining" runat="server" CssClass="textall" 
                            BorderStyle="None" GroupName="SearchType" Text="Containing"></asp:radiobutton></TD>
					<TD width="25"></TD>
				</TR>
			</TABLE>                            
                            </td>
                            <td align="right" style="background-repeat:repeat-y; background-image:url(../images/right.png); width: 4px"></td>
                        </tr>
                        <tr valign="top" style="height:4px">
                            <td colspan="3" style="background-image:url(../images/Search_Results_Bottom.jpg); background-repeat:no-repeat ; height: 2px"></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                </td>
                <td style="width:10px"></td>
                <td style="width:620px" valign="top" rowspan="3">
                    <table cellpadding="0" cellspacing="0" style="height:530px; width:100%">
                        <tr style="width:620px">
                            <td  colspan="3" style="background-image:url(../images/main_top.png); background-repeat:no-repeat; height: 22px"></td>
                            <td></td>
                            <td  style="width:4px"></td>
                        </tr>
                        <tr valign="top">
                            <td align="left" valign="top" style="background-repeat:repeat-y; background-image:url(../images/left.png); width:4px" id="td2" runat="server"></td>
                            <td style="width:625px">
                                <table>
                                    <tr>
                                        <td style="width: 10px"></td>
                                        <td style="width: 100px"></td>
                                        <td style="width: 20px"></td>
                                        <td style="width: 100px"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><iewc:toolbar id="toolUser" runat="server" Width="100%" 
                                                BackColor="White" BorderColor="White" AutoPostBack="True"></iewc:toolbar></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>User Id:</td>
                                        <td></td>
                                        <td><asp:TextBox ID="txtUserId" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr style="height:10px">
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>User Name:</td>
                                        <td></td>
                                        <td><asp:TextBox ID="txtUserName" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr style="height:10px">
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>User Profile</td>
                                        <td></td>
                                        <td><asp:DropDownList ID="lstUserProfile" style="width:100%" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:10px">
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>Password</td>
                                        <td></td>
                                        <td><asp:TextBox ID="txtPassword" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                                
                            </td>
                            <td align="right" style="background-repeat:repeat-y; background-image:url(../images/right.png); width: 4px"></td>
                        </tr>
                        <tr valign="top" style="height:4px">
                            <td colspan="3" style="background-image:url(../images/Main_Bottom.png); background-repeat:no-repeat ; height: 4px"></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px">
            <td>
                <table style="height: 20px">
                    <tr>
                        <td></td>
                    </tr>
                </table>
            </td>
            </tr>
            <tr style="height: 360px">
            <td>
                <table cellpadding="0" cellspacing="0" style="height:360px; width:100%">
                    <tr>
                        <td  colspan="3" style="background-image:url(../images/Search_Results_Top.jpg); background-repeat:no-repeat; height: 22px"></td>
                        <td></td>
                        <td  style="width:4px"></td>
                    </tr>
                    <tr valign="top">
                        <td align="left" valign="top" style="background-repeat:repeat-y; background-image:url(../images/left.png); width:4px" id="td1" runat="server"></td>
                        <td style="width:260px">
                        <table width="100%">
				<tr>
					<td><asp:datagrid id="dgNavSub" runat="server" Width="100%" PageSize="20" 
                            AllowPaging="True" ShowHeader="False"
							ShowFooter="True" GridLines="None" AutoGenerateColumns="False" 
                            onitemdatabound="dgNavSub_ItemDataBound">
							<ItemStyle Font-Size="Small" Wrap="False"></ItemStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:LinkButton id="NavSubLink" runat="server"></asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="UserName"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="UserId"></asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="" PrevPageText=""></PagerStyle>
						</asp:datagrid></td>
				</tr>
			</table>
                        <table id="tblnavsub" width="100%">
				            <TR>
					            <TD align="right"><asp:imagebutton id="firstbutton" onclick="PagerButtonClick" runat="server" BorderStyle="None" CommandArgument="0"></asp:imagebutton><asp:imagebutton id="PrevButton" onclick="PagerButtonClick" runat="server" BorderStyle="None" CommandArgument="prev"></asp:imagebutton><asp:imagebutton id="NextButton" onclick="PagerButtonClick" runat="server" BorderStyle="None" CommandArgument="next"></asp:imagebutton><asp:imagebutton id="LastButton" onclick="PagerButtonClick" runat="server" BorderStyle="None" CommandArgument="last"></asp:imagebutton></TD>
				            </TR>
			            </table>
                        </td>
                        <td align="right" style="background-repeat:repeat-y; background-image:url(../images/right.png); width: 4px"></td>
                    </tr>
                    <tr valign="top" style="height:4px">
                        <td colspan="3" style="background-image:url(../images/Search_Results_Bottom.jpg); background-repeat:no-repeat ; height: 2px"></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </td>
            </tr>
            </table>
			</div>


            <script type="text/javascript" language="javascript">
                function newuser() 
                {
                    document.getElementById(document.forms[0].h_txtUserId.value).value = '(new)';
                    document.getElementById(document.forms[0].h_txtUserName.value).value = '';
                    document.getElementById(document.forms[0].h_txtPassword.value).value = '';
                    document.getElementById(document.forms[0].h_txtUserProfile.value).options[0].selected = true;
                    document.getElementById(document.forms[0].h_txtUserName.value).focus();
                    return false;
                }

                function dltuser() {
                    if (document.getElementById(document.forms[0].h_txtUserName.value).value != '') {
                        var retVal = confirm('Remove ' + document.getElementById(document.forms[0].h_txtUserName.value).value + ' from system?')
                        if (retVal == true) {
                            document.forms[0].h_Action.value = 'DELETE';
                            document.forms[0].submit();
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        return false;
                    }
                }

                function saveuser() 
                {
                    if (document.getElementById(document.forms[0].h_txtUserName.value).value == '') 
                    {
                        return false;
                    }
                    else 
                    {
                        document.forms[0].h_Action.value = 'SAVE';
                        document.forms[0].submit();
                    }
                }

            </script>
</asp:Content>
