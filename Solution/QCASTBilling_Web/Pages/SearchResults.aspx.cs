using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class SearchResults : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Master.FindControl("lblTitle");
            if (lblTitle != null)
            {
                lblTitle.Text = ConfigurationManager.AppSettings["AppName"].ToString() + "Default";
            }
        }

    }
}
