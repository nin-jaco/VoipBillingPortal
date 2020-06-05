<%@ Page Title="Oops we encountered an error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="QCAstWebApplication.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <table cellpadding="0" cellspacing="0" style="text-align:left; width:920px">
        <tr>
            <td>
                <h2 style="margin-left:15px" runat="server" id="hErrorTitle">
                </h2>
                <p class="radialShadow"></p>
                <p style="margin-left:15px" id="pErrorDescription" runat="server"></p>
                <p class="warningText" style="margin-left:15px">
                    The webmaster has been notified of this error and should be attending to it momentarily.</p>
            </td>
        </tr>
    </table>
</asp:Content>
