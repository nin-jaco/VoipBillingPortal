using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Pages_LogOff : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //FormsAuthentication.SignOut();
        Session["loggedin"] = System.Convert.ToString(false);
        Session["profilename"] = "";
        Session["UserId"] = "";
        Session["logintype"] = "";
        Response.Redirect("UserLogin.aspx");
    }
}