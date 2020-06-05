using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using QCASTBilling.BLL;

public partial class Account_UserLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnLogin.ImageUrl = "~\\Images\\go.jpg";
        this.lblUserName.Text = "User name";
        this.lblPassword.Text = "Password";
    }



    protected void btnLogin_Click(object sender, ImageClickEventArgs e)
    {
        User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
        user.getUser(this.txtUserName.Text);

        if (user.UserName == null)
        {
            // Invalid user name
            ClientScript.RegisterStartupScript(this.GetType(), "su", "javascript:alert('Invalid user name.');", true);
        }
        else
        {
            if (user.UserPassword != this.txtPassword.Text.Trim())
            {
                // Invalid user password
                ClientScript.RegisterStartupScript(this.GetType(), "su", "javascript:alert('Invalid password.');", true);
            }
            else
            {
                Session["loggedin"] = System.Convert.ToString(true);
                Session["profilename"] = user.ProfileName;
                Session["UserId"] = user.UserID.ToString();
                Session["logintype"] = "user";
                Response.Redirect("~\\Pages\\About.aspx", true);
            }
        }
    }
}


