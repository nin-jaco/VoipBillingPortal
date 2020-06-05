#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using QCASTBilling.BLL;



public partial class SiteMaster : System.Web.UI.MasterPage
{

    public string Title { get; set; } 

    protected void Page_Load(object sender, EventArgs e)
    {
        // The following is to emulate someone logged-in so as to avoid going through the login process whilst developing in VS.
        #if(DEBUG)
        //Session["UserId"] = "1";
        //Session["loggedin"] = System.Convert.ToString(true);
        //Session["logintype"] = "user";
        //Session["profilename"] = "Administrator";
        //if (Session["profilename"] == "Client")
        //{
        //    Session["ClientId"] = "1";
        //}
        #endif

        if (Session["loggedin"] == null)
        {
            Response.Redirect("~\\Pages\\UserLogin.aspx", true);
        }
        else
        {
            SetMenus();
        }

        Page.ClientScript.RegisterHiddenField("h_logintype", Session["logintype"].ToString());
    }

    protected void SetMenus()
    {
        string profileName = "";

        if (Session["profilename"].ToString() == "Client")
        {
            profileName = Session["profilename"].ToString();
        }
        else
        {
            User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
            user.getUser(System.Convert.ToInt32(Session["UserId"].ToString()));
            profileName = user.ProfileName;
        }


        switch (profileName)
        {
            case "User":
                this.NavigationMenu.Items.Add(new MenuItem("Billing", "Billing", "", "~\\Pages\\Billing.aspx"));
                break;

            case "Administrator":
                this.NavigationMenu.Items.Add(new MenuItem("User Management", "User", "", "~\\Pages\\User.aspx"));
                this.NavigationMenu.Items.Add(new MenuItem("Client Management", "Client", "", "~\\Pages\\Client.aspx"));
                this.NavigationMenu.Items.Add(new MenuItem("Billing", "Billing", "", "~\\Pages\\Billing.aspx"));
                this.NavigationMenu.Items.Add(new MenuItem("Reports", "Report", "", "~\\Pages\\Reports.aspx"));
                break;

            case "ClientManager":
                this.NavigationMenu.Items.Add(new MenuItem("Client Management", "Client", "", "~\\Pages\\Client.aspx"));
                break;

            case "DataManager":
                this.NavigationMenu.Items.Add(new MenuItem("User Management", "User", "", "~\\Pages\\User.aspx"));
                break;

            case "Client":
                this.NavigationMenu.Items.Add(new MenuItem("Reports", "Report", "", "~\\Pages\\Reports.aspx"));
                break;

        }

    }

}
