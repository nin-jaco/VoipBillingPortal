using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using QCASTBilling.BLL;

public partial class Pages_Client : System.Web.UI.Page
{

    private DataSet ds = null;
    private int ResultCount = 0;


    protected void Page_Init(object sender, EventArgs e)
    {
        ClientScript.RegisterHiddenField("h_txtClientId", this.txtClientId.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtClientDesc", this.txtClientDesc.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtRangeFrom", this.txtRangeFrom.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtRangeTo", this.txtRangeTo.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtClientCode", this.txtClientCode.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtAccountingCode", this.txtAccountingCode.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtClientUserName", this.txtClientUserName.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtClientPassword", this.txtClientPassword.ClientID.ToString());
        this.txtClientId.Attributes.Add("onclick", "javascript:document.getElementById(document.forms[0].h_txtClientDesc.value).focus();");
        this.txtClientId.Attributes.Add("onfocus", "javascript:document.getElementById(document.forms[0].h_txtClientDesc.value).focus();");
        this.LoadNavSubGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            User user = new User(ConfigurationManager.AppSettings["connString"].ToString());

            Label lblTitle = (Label) Master.FindControl("lblTitle");
            if (lblTitle != null)
            {
                lblTitle.Text = ConfigurationManager.AppSettings["AppName"].ToString() + "Client Management";
            }
            
            Microsoft.Web.UI.WebControls.ToolbarButton newUser = new Microsoft.Web.UI.WebControls.ToolbarButton();
            newUser.Text = "<span onclick=\"javascript:return newuser();\"><IMG src='../Images/create.jpg'></span>";
            newUser.ID = "NEWUSER";
            newUser.ToolTip = "New User";
            toolUser.Items.Add(newUser);

            Microsoft.Web.UI.WebControls.ToolbarButton dltUser = new Microsoft.Web.UI.WebControls.ToolbarButton();
            dltUser.Text = "<span onclick=\"javascript:return dltuser();\";><IMG src='../Images/delete.jpg'></span>";
            dltUser.ID = "DLTUSER";
            dltUser.ToolTip = "Remove User";
            toolUser.Items.Add(dltUser);

            Microsoft.Web.UI.WebControls.ToolbarButton saveUser = new Microsoft.Web.UI.WebControls.ToolbarButton();
            saveUser.Text = "<span onclick=\"javascript:return saveuser();\";><IMG src='../Images/save.jpg'></span>";
            saveUser.ID = "SAVUSER";
            saveUser.ToolTip = "Save User";
            toolUser.Items.Add(saveUser);

            this.firstbutton.ImageUrl = ConfigurationManager.AppSettings["ImagePath"].ToString() + "first.gif";
            this.LastButton.ImageUrl = ConfigurationManager.AppSettings["ImagePath"].ToString() + "last.gif";
            this.NextButton.ImageUrl = ConfigurationManager.AppSettings["ImagePath"].ToString() + "next.gif";
            this.PrevButton.ImageUrl = ConfigurationManager.AppSettings["ImagePath"].ToString() + "previous.gif";

            this.btnSearch.ImageUrl = ConfigurationManager.AppSettings["ImagePath"].ToString() + "go.jpg";

            //LoadNavSubGrid();
        }
        else
        {
            if (Request["h_Action"] != null)
            {
                if (Request["h_Action"] == "SAVE")
                {
                    if (this.txtClientId.Text == "(new)")
                    {
                        insertClient();
                    }
                    else
                    {
                        saveClient();
                    }
                }
                else if (Request["h_Action"] == "DELETE")
                {
                    deleteClient();
                }

            }
        }

        ClientScript.RegisterHiddenField("h_Action", "");
    }

    public void PagerButtonClick(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = (ImageButton)sender;

        switch (btn.CommandArgument.ToString())
        {
            case "next":
                if (dgNavSub.CurrentPageIndex < (dgNavSub.PageCount - 1))
                {
                    dgNavSub.CurrentPageIndex += 1;
                }
                break;
            case "prev":
                if (dgNavSub.CurrentPageIndex > 0)
                {
                    dgNavSub.CurrentPageIndex -= 1;
                }
                break;
            case "last": ;
                dgNavSub.CurrentPageIndex = (dgNavSub.PageCount - 1);
                break;
            default:
                dgNavSub.CurrentPageIndex = Convert.ToInt32(btn.CommandArgument.ToString());
                break;
        }
        BindSQL();
    }

    private void BindSQL()
    {
        if (ds == null)
        {
            firstbutton.Visible = false;
            PrevButton.Visible = false;
            NextButton.Visible = false;
            LastButton.Visible = false;
        }
        else
        {
            dgNavSub.DataSource = ds;
            try
            {
                dgNavSub.DataBind();
            }
            catch (Exception e)
            {
                dgNavSub.CurrentPageIndex = 0;
                dgNavSub.DataBind();
            }

            if (dgNavSub.CurrentPageIndex != 0)
            {
                Prev_Buttons();
                firstbutton.Visible = true;
                PrevButton.Visible = true;
            }
            else
            {
                firstbutton.Visible = false;
                PrevButton.Visible = false;
            }

            if (dgNavSub.CurrentPageIndex != (dgNavSub.PageCount - 1))
            {
                Next_Buttons();
                NextButton.Visible = true;
                LastButton.Visible = true;
            }
            else
            {
                NextButton.Visible = false;
                LastButton.Visible = false;
            }


        }

    }

    private void Prev_Buttons()
    {
        string PrevSet = "";

        if ((dgNavSub.CurrentPageIndex + 1) != 1 && ResultCount != -1)
        {
            PrevSet = dgNavSub.PageSize.ToString();

            if ((dgNavSub.CurrentPageIndex + 1) == dgNavSub.PageCount) { }
        }
    }

    private void Next_Buttons()
    {
        string NextSet = "";

        if (dgNavSub.CurrentPageIndex + 1 < dgNavSub.PageCount)
        {
            NextSet = dgNavSub.PageSize.ToString();
        }

        if (dgNavSub.CurrentPageIndex + 1 == dgNavSub.PageCount - 1)
        {
            int EndCount = ResultCount - (dgNavSub.PageSize * (dgNavSub.CurrentPageIndex + 1));
        }

    }

    public void LoadNavSubGrid()
    {
        // get data here
        Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());
        ds = client.getClients();

        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "jss", "<script>" + Environment.NewLine +
                                                  "window.alert('not found');" + Environment.NewLine +
                                                  "</script>" + Environment.NewLine);
            }
            else
            {
                BindSQL();
            }
        }
    }

    protected void dgNavSub_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        System.Web.UI.WebControls.LinkButton NavSubLink = (LinkButton)e.Item.Cells[0].FindControl("NavSubLink");

        if (NavSubLink != null)
        {
            // Add javascript to direct each item to the correct page
            NavSubLink.Text = e.Item.Cells[1].Text;
            NavSubLink.CommandArgument = e.Item.Cells[2].Text;
            //NavSubLink.Attributes.Add("onclick", "javascript:GetUser('" + e.Item.Cells[2].Text + "');");
            NavSubLink.Command += new CommandEventHandler(NavSubLink_Command);
        }
    }

    protected void NavSubLink_Command(object sender, CommandEventArgs args)
    {
        Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());
        client.getClient(args.CommandArgument.ToString());

        this.txtClientId.Text = client.ClientID.ToString();
        this.txtClientDesc.Text = client.ClientDesc;
        this.txtRangeFrom.Text = System.Convert.ToString(client.RangeFrom);
        this.txtRangeTo.Text = System.Convert.ToString(client.RangeTo);
        this.txtClientCode.Text = client.ClientCode;
        this.txtAccountingCode.Text = client.AccountingCode;
        this.txtClientUserName.Text = client.ClientUserName;
        this.txtClientPassword.Text = client.ClientPassword;
    }

    protected void saveClient()
    {
        Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());
        client.ClientID = System.Convert.ToInt32(this.txtClientId.Text);
        client.ClientDesc = this.txtClientDesc.Text;
        client.RangeFrom = System.Convert.ToInt32(this.txtRangeFrom.Text);
        client.RangeTo = System.Convert.ToInt32(this.txtRangeTo.Text);
        client.ClientCode = this.txtClientCode.Text;
        client.AccountingCode = this.txtAccountingCode.Text;
        client.ClientUserName = this.txtClientUserName.Text;
        client.ClientPassword = this.txtClientPassword.Text;
        client.ProfileId = 5;
        if (client.saveClient() == true)
        {
            this.LoadNavSubGrid();
        }
    }

    protected void insertClient()
    {
        Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());
        client.ClientDesc = this.txtClientDesc.Text;
        client.RangeFrom = System.Convert.ToInt32(this.txtRangeFrom.Text);
        client.RangeTo = System.Convert.ToInt32(this.txtRangeTo.Text);
        client.ClientCode = this.txtClientCode.Text;
        client.AccountingCode = this.txtAccountingCode.Text;
        client.ClientUserName = this.txtClientUserName.Text;
        client.ClientPassword = this.txtClientPassword.Text;
        this.txtClientId.Text = System.Convert.ToString(client.insertClient());
        client.ProfileId = 5;
        if (this.txtClientId.Text != "0")
        {
            this.LoadNavSubGrid();
        }
    }

    protected void deleteClient()
    {
        Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());
        client.ClientID = System.Convert.ToInt32(this.txtClientId.Text);
        if (client.deleteClient() == true)
        {
            this.txtClientId.Text = "";
            this.txtClientDesc.Text = "";
            this.txtRangeFrom.Text = "";
            this.txtRangeTo.Text = "";
            this.txtClientCode.Text = "";
            this.txtAccountingCode.Text = "";
            this.txtClientUserName.Text = "";
            this.txtClientPassword.Text = "";
            this.LoadNavSubGrid();
        }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Client client = new Client(ConfigurationManager.AppSettings["connString"].ToString());

        if (this.rdContaining.Checked == true)
        {
            client.searchType = QCASTBilling.BLL.Client.clientSearchType.containing;
        }
        else if (this.rdExact.Checked == true)
        {
            client.searchType = QCASTBilling.BLL.Client.clientSearchType.exact;
        }
        else
        {
            client.searchType = QCASTBilling.BLL.Client.clientSearchType.containing;
            this.rdContaining.Checked = true;
        }
        ds = client.searchClients(this.txtSearch.Text.Trim());
        this.BindSQL();
    }



}