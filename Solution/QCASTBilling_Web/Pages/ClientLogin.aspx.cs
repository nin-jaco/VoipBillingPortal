using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using QCASTBilling.BLL;

public partial class Account_ClientLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnLogin.ImageUrl = "~\\Images\\go.jpg";
        this.lblUserName.Text = "User name";
        this.lblPassword.Text = "Password";

        if (!Page.IsPostBack)
        {
            Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());
            this.lstClients.DataSource = client.getClients();
            this.lstClients.DataTextField = "ClientDesc";
            this.lstClients.DataValueField = "ClientId";
            this.lstClients.DataBind();
            this.lstClients.Items.Insert(0, "-- Please Select --");
            this.lstClients.SelectedIndex = 0;
        }

        ClientScript.RegisterHiddenField("h_lstClient", this.lstClients.ClientID.ToString());
        this.btnLogin.Attributes.Add("onclick", "javascript:return CheckForClient();");
    }

    protected void btnLogin_Click(object sender, ImageClickEventArgs e)
    {
        Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());
        client.getClient(this.lstClients.SelectedValue.ToString());

        if (client.ClientUserName == null)
        {
            // Invalid user name
            ClientScript.RegisterStartupScript(this.GetType(), "su", "javascript:alert('Invalid client user name.');", true);
        }
        else
        {
            if (client.ClientPassword != this.txtPassword.Text.Trim())
            {
                // Invalid user password
                ClientScript.RegisterStartupScript(this.GetType(), "su", "javascript:alert('Invalid client password.');", true);
            }
            else
            {
                Session["loggedin"] = System.Convert.ToString(true);
                Session["profilename"] = "Client";
                Session["ClientId"] = client.ClientID.ToString();
                Session["ClientDesc"] = client.ClientDesc.ToString();
                Session["logintype"] = "client";
                Response.Redirect("~\\Pages\\About.aspx", true);
            }
        }
    }
}


