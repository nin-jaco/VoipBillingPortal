<%@ Page Title="Welcome to QC Ast Billing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QCAstWebApplication.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.jcarousel.pack.js" ></script>
    <script type="text/javascript" src="Scripts/jquery-func.js"></script>
    <style type="text/css">
        .shell { width:920px; margin:0 auto; }
        #slider { height:290px; position:relative; }
        #slider-holder { height:200px; position:relative; overflow:hidden; top:50px; width:856px; left:0px;}
        #slider-holder ul{ height:200px; position:relative; overflow:hidden; width:856px; list-style-type: none;}
        #slider-holder .jcarousel-clip{ height:200px; position:relative; overflow:hidden; width:856px; }
        #slider-holder ul li{ height:200px; position:relative; overflow:hidden; float:left; width:856px; }
        #slider .slide-image{ width:316px; float:left; }
        #slider .slide-info{ width:500px; float:left; padding-left:20px; text-align:left }
        #slider .slide-info p{ padding-top:10px;}

        #slider-nav { font-size:0; line-height:0;}
        #slider-nav a{ width:47px; height:44px; position:absolute; top:110px; text-indent: -4000px;}
        #slider-nav a.prev{ background:url(Images/prev.png); left:-10px; }
        #slider-nav a.next{ background:url(Images/next.png); right:-10px; }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <table cellpadding="0" cellspacing="0" style="width:920px">
        <tr>
            <td align="center">
                <div id="header">
	                <div class="shell">		
		                <div id="slider">
			                <div id="slider-holder">
				                <ul>
				                    <li>
				    	                <div class="cl">&nbsp;</div>
				    	                <div class="slide-image">
				    		                <img src="Images/slide-image.png" alt="" />
				    	                </div>
				    	                <div class="slide-info">
				    		                <h1>QC Solutions<br /><span style="color:silver">Ast Billing</span></h1>
				    		                <p>Web interface and billing engine developed and owned by QC Solutions (Pty) Ltd, 2012.</p>
				    	                </div>
				    	                <div class="cl">&nbsp;</div>
				                    </li>
				                    <li>
				    	                <div class="cl">&nbsp;</div>
				    	                <div class="slide-image">
				    		                <img src="Images/slide-image2.png" alt="" />
				    	                </div>
				    	                <div class="slide-info">
                                            <h1>QC Solutions<br /><span style="color:silver">Ast Billing</span></h1>
				    		                <p>Web interface and billing engine developed and owned by QC Solutions (Pty) Ltd, 2012.</p>
				    	                </div>
				    	                <div class="cl">&nbsp;</div>
				                    </li>
				                    <li>
				    	                <div class="cl">&nbsp;</div>
				    	                <div class="slide-image">
				    		                <img src="Images/slide-image3.png" alt="" />
				    	                </div>
				    	                <div class="slide-info">
				    		                <h1>QC Solutions<br /><span style="color:silver">Ast Billing</span></h1>
				    		                <p>Web interface and billing engine developed and owned by QC Solutions (Pty) Ltd, 2012.</p>
				    	                </div>
				    	                <div class="cl">&nbsp;</div>
				                    </li>
				                </ul>
			                </div>
			                <div id="slider-nav">
				                <a href="#" class="prev">Previous</a>
				                <a href="#" class="next">Next</a>
			                </div>
		                </div>
		
	                </div>
                </div>

            </td>
        </tr>
        <tr>
            <td class="homeSections">&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" style="text-align:left">
                    <tr>
                        <td valign="top" style="width:200px; padding-left:20px">
                            <p>Web interface and billing engine developed and owned by QC Solutions (Pty) Ltd, 2012</p>
                            <p>&nbsp;</p>
                            <p style="text-align:right"><a href="http://www.qcsolutions.co.za" target="_blank">
                                <img src="Images/btnReadMore.png" style="border:none" /></a></p>
                        </td>
                        <td style="width:15px">&nbsp;</td>
                        <td valign="top" style="width:218px">
                            <ul>
                                <li><a href="#">One</a></li>
                                <li><a href="#">Two</a></li>
                                <li><a href="#">Three</a></li>
                            </ul>
                        </td>
                        <td style="width:15px">&nbsp;</td>
                        <td valign="top" style="width:180px">
                            <ul style="margin-left:-20px">
                                <li style="text-align:left"><img src="Images/LaserNet.jpg" /></li>
                                <li style="text-align:left"><img src="Images/iVolve.jpg" /></li>
                                <li style="text-align:left"><img src="Images/QCSolutions.jpg" /></li>
                            </ul>
                        </td>
                        <td style="width:15px">&nbsp;</td>
                        <td valign="top" style="width:239px">
                            <p>QC Solutions (Pty) Ltd is a privately owned company established in 2001 in South Africa. We consist of developers, graphics designer experts and hardware specialists, boasting with a wide variety of languages, packages and platform knowledge delivering scalable robust solutions to a wide variety of clients in South Africa.</p>
                            <p style="text-align:right"><a href="http://www.qcsolutions.co.za" target="_blank">
                                <img src="Images/btnReadMore.png" style="border:none" /></a></p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
