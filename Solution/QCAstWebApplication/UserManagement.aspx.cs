using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using QCAstSolution.Classes;
using QCAstWebApplication.QCAstServiceReference;

namespace QCAstWebApplication
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    using (QCAstServiceClient serviceClient = new QCAstServiceClient())
                    {
                        int idUser = int.Parse(Session["IdUser"].ToString());
                        User user = serviceClient.GetUserFromId(idUser);

                        if (user.IdProfile != 4 & user.IdProfile != 2)
                        {
                            Response.Redirect("ErrorPage.aspx?Error=NotAuthorised", false);
                        }
                        else
                        {
                            ddlProfiles.DataSource = serviceClient.GetAllProfiles();
                            ddlProfiles.DataBind();
                            ddlClient.DataSource = serviceClient.GetAllClients("All", "");
                            ddlClient.DataBind();
                            lbSearchedUsers.DataSource = serviceClient.GetAllUsers("All", "");
                            lbSearchedUsers.DataBind();
                            ddlProfiles.Items.Insert(0, new ListItem("Please select..", "0"));
                            ddlClient.Items.Insert(0, new ListItem("Please select..", ""));
                            trButtons.Visible = false;
                            ibDeleteUser.Enabled = false;
                            ibEditUser.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "User Management", ex.ToString());
                    Response.Redirect("ErrorPage.aspx?Error=GeneralError", true);
                }
            }
        }

        protected void ibNewUser_Click(object sender, ImageClickEventArgs e)
        {
            ClearTextAndListBoxes();
            tbUserId.Text = "(new)";
            tbSearch.Enabled = false;
            hfFormAction.Value = "New";
            trIcons.Visible = false;
            trButtons.Visible = true;
        }

        protected void ibCancel_Click(object sender, ImageClickEventArgs e)
        {
            ClearTextAndListBoxes();
        }

        protected void ClearTextAndListBoxes()
        {
            tbUserId.Text = "";
            tbUsername.Text = "";
            tbPassword.Text = "";
            ddlProfiles.SelectedIndex = 0;
            ddlClient.SelectedIndex = 0;
            tbSearch.Text = "";
            lbSearchedUsers.Items.Clear();
            trButtons.Visible = false;
            trIcons.Visible = true;
            hfFormAction.Value = "";
            lblSearchError.Text = "";
            lblAmendError.Text = "";
            lblSuccess.Text = "";
            tbSearch.Enabled = true;
            ibDeleteUser.Enabled = false;
            ibEditUser.Enabled = false;
        }

        protected void ibBtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            lblAmendError.Text = "";
            bool usernameTaken = false;
            try
            {
                if (tbUsername.Text == "" | tbPassword.Text == "" | ddlProfiles.SelectedIndex == 0)
                {
                    lblAmendError.Text = "Please provide values for the Username, Password and Profile fields";
                }
                else 
                {
                    if (ddlProfiles.SelectedIndex != 4 & ddlClient.SelectedIndex == 0)
                    {
                        lblAmendError.Text = "User not Admin. Please provide values for the Client field";
                    }
                    else
                    {
                        using (QCAstServiceClient client = new QCAstServiceClient())
                        {
                            usernameTaken = client.CheckIfUserExists(tbUsername.Text.Trim());
                        }
                        if (usernameTaken)
                        {
                            lblAmendError.Text = "Username taken, please enter new username";
                        }
                        else
                        {
                            SaveUser();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Ast Billing", "Usermanagement", ex.ToString());
                lblAmendError.Text = "The website encountered a problem. The webmaster has been notified and will be attending to the problem.";
            }
        }

        private void SaveUser()
        {
            bool success = false;
            string username = tbUsername.Text.Trim();
            try
            {
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    success = client.SaveUser(ddlProfiles.SelectedIndex, username, tbPassword.Text.Trim(),
                                              ddlClient.SelectedValue != "" ? int.Parse(ddlClient.SelectedValue) : (int?)null);
                }
                if (success)
                {
                    ClearTextAndListBoxes();
                    lblSuccess.Text = string.Format("User '{0}' has been successfully added.", username);
                }
                else
                {
                    lblAmendError.Text = string.Format("The website encountered and error saving User '{0}'",
                                                       tbUsername.Text);
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Ast Billing", "Usermanagement", ex.ToString());
                lblAmendError.Text = "The website encountered a problem. The webmaster has been notified and will be attending to the problem.";
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string searchTerm = tbSearch.Text;
            ClearTextAndListBoxes();

            if (searchTerm == "" & rblSearchFormat.SelectedValue != "All")
            {
                lblSearchError.Text = "Please enter a search term";
            }
            else
            {
                try
                {
                    using (QCAstServiceClient client = new QCAstServiceClient())
                    {
                        lblSearchError.Text = "";
                        List<User> users = client.GetAllUsers(rblSearchFormat.SelectedValue, searchTerm);

                        if (users == null)
                        {
                            lblSearchError.Text = "No results found";
                        }
                        else
                        {
                            lbSearchedUsers.DataSource = users;
                            lbSearchedUsers.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotifyWebmasterOfError("Billing", "User Management Search", ex.ToString());
                    lblSearchError.Text = "Error experienced";
                }
            }
        }

        protected void lbSearchedUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(((ListBox) sender).SelectedValue);
            try
            {
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    User user = client.GetUserFromId(id);
                    tbUserId.Text = user.IdUser.ToString();
                    tbUsername.Text = user.Username;
                    tbPassword.Text = user.Password;
                    ddlProfiles.SelectedIndex = user.IdProfile;
                    if (user.IdClient != null)
                    {
                        ddlClient.SelectedIndex = ddlClient.Items.IndexOf(ddlClient.Items.FindByValue(user.IdClient.Value.ToString()));
                    }
                    else
                    {
                        ddlClient.SelectedIndex = 0;
                    }
                }
                ibEditUser.Enabled = true;
                ibDeleteUser.Enabled = true;
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "User Management SearchSelectedIndexChanged", ex.ToString()); 
            }
        }

        protected void ibDeleteUser_Click(object sender, ImageClickEventArgs e)
        {
            DeleteUser();
        }

        private void DeleteUser()
        {
            lblAmendError.Text = "";
            bool success = false;
            string username = tbUsername.Text;
            try
            {
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    success = client.DeleteUser(int.Parse(tbUserId.Text));
                }
                if (success)
                {
                    ClearTextAndListBoxes();
                    lblSuccess.Text = string.Format("User {0} has been successfully deleted.", username);
                }
                else
                {
                    lblAmendError.Text = "Cannot delete the user at this time. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "User Management DeleteUser", ex.ToString());
                lblAmendError.Text =
                    "The website encountered an error processing your request. The webmaster has been notified and will be attending to it shortly.";
            }
        }

        protected void ibSaveUser_Click(object sender, ImageClickEventArgs e)
        {
            if (tbUsername.Text == "" | tbPassword.Text == "" | ddlProfiles.SelectedIndex == 0)
            {
                lblAmendError.Text = "Please provide values for the Username, Password and Profile fields";
            }
            else
            {
                if (ddlProfiles.SelectedIndex != 4 & ddlClient.SelectedIndex == 0)
                {
                    lblAmendError.Text = "User not Admin. Please provide values for the Client field";
                }
                else
                {
                    EditUser();
                }
            }
        }

        private void EditUser()
        {
            string username = tbUsername.Text;
            lblAmendError.Text = "";
            bool success = false;
            try
            {
                using (QCAstServiceClient client = new QCAstServiceClient())
                {
                    success = client.EditUser(int.Parse(tbUserId.Text), ddlProfiles.SelectedIndex, username,
                                              tbPassword.Text,
                                              ddlClient.SelectedValue != ""
                                                  ? int.Parse(ddlClient.SelectedValue)
                                                  : (int?) null);
                }
                if (success)
                {
                    ClearTextAndListBoxes();
                    lblSuccess.Text = string.Format("User {0} has been successfully amended", username);
                }
                else
                {
                    lblAmendError.Text = "The user could not be amended at this time";
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("Billing", "User Management EditUser", ex.ToString());
                lblAmendError.Text =
                    "The website encountered a problem updating the User details.  The webmaster has been notified and will be attending to it shortly";
            }
        }

        public void NotifyWebmasterOfError(string website, string page, string error)
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress("jaco@qcsolutions.co.za"));
            message.Subject = "Exception experienced on page";
            message.Body = string.Format("Website {0}, Page {1}, error {2}", website, page, error);
            SmtpClient client = new SmtpClient();
            client.Send(message);
        }
    }
}