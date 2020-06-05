using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using QCASTBilling.BLL;

public partial class Pages_User : System.Web.UI.Page
{
    private DataSet ds = null;
    private int ResultCount = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        ClientScript.RegisterHiddenField("h_txtUserId", this.txtUserId.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtUserName", this.txtUserName.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtPassword", this.txtPassword.ClientID.ToString());
        ClientScript.RegisterHiddenField("h_txtUserProfile", this.lstUserProfile.ClientID.ToString());
        this.txtUserId.Attributes.Add("onclick", "javascript:document.getElementById(document.forms[0].h_txtUserName.value).focus();");
        this.txtUserId.Attributes.Add("onfocus", "javascript:document.getElementById(document.forms[0].h_txtUserName.value).focus();");
        this.LoadNavSubGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Master.FindControl("lblTitle");
            if (lblTitle != null)
            {
                lblTitle.Text = ConfigurationManager.AppSettings["AppName"].ToString() + "User Management";
            }

            User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
            this.lstUserProfile.DataSource = user.getUserProfiles();
            this.lstUserProfile.DataTextField = "ProfileName";
            this.lstUserProfile.DataValueField = "ProfileId";
            this.lstUserProfile.DataBind();

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
            if(Request["h_Action"] != null)
            {
                if (Request["h_Action"] == "SAVE")
                {
                    if (this.txtUserId.Text == "(new)")
                    {
                        insertUser();
                    }
                    else
                    {
                        saveUser();
                    }
                }
                else if (Request["h_Action"] == "DELETE")
                {
                    deleteUser();
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
        User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
        ds = user.getUsers();

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
        User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
        user.getUser(System.Convert.ToInt32(args.CommandArgument.ToString()));

        this.txtUserId.Text = user.UserID.ToString();
        this.txtUserName.Text = user.UserName;
        this.lstUserProfile.Text = user.UserProfileID.ToString();
        this.txtPassword.Text = user.UserPassword;
    }

    protected void saveUser()
    {
        User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
        user.UserID = System.Convert.ToInt32(this.txtUserId.Text);
        user.UserName = this.txtUserName.Text;
        user.UserPassword = this.txtPassword.Text;
        user.UserProfileID = System.Convert.ToInt32(this.lstUserProfile.SelectedValue);
        if (user.saveUser() == true)
        {
            this.LoadNavSubGrid();
        }
    }

    protected void insertUser()
    {
        User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
        user.UserName = this.txtUserName.Text;
        user.UserPassword = this.txtPassword.Text;
        user.UserProfileID = System.Convert.ToInt32(this.lstUserProfile.SelectedValue);
        this.txtUserId.Text = System.Convert.ToString(user.insertUser());
        if(this.txtUserId.Text != "0")
        {
            this.LoadNavSubGrid();
        }
    }

    protected void deleteUser()
    {
        User user = new User(ConfigurationManager.AppSettings["connString"].ToString());
        user.UserID = System.Convert.ToInt32(this.txtUserId.Text);
        if (user.deleteUser() == true)
        {
            this.txtUserId.Text = "";
            this.txtUserName.Text = "";
            this.txtPassword.Text = "";

            this.lstUserProfile.ClearSelection();
            this.lstUserProfile.Items[0].Selected = true;
            this.LoadNavSubGrid();
        }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        User user = new User(ConfigurationManager.AppSettings["connString"].ToString());

        if (this.rdContaining.Checked == true)
        {
            user.searchType = QCASTBilling.BLL.User.userSearchType.containing;
        }
        else if (this.rdExact.Checked == true)
        {
            user.searchType = QCASTBilling.BLL.User.userSearchType.exact;
        }
        else
        {
            user.searchType = QCASTBilling.BLL.User.userSearchType.containing;
            this.rdContaining.Checked = true;
        }
        ds = user.searchUsers(this.txtSearch.Text.Trim());
        this.BindSQL();
    }
}