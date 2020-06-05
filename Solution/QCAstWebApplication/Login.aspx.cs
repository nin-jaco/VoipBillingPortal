using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QCAstSolution.Classes;
using System.Web.Security;
using QCAstWebApplication.QCAstServiceReference;

namespace QCAstWebApplication
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            string result = "";
            string idUser = "";
            lblError.Text = "";

            try
            {
                using (QCAstServiceClient serviceRef = new QCAstServiceClient())
                {
                    List<KeyValuePair<string, string>> authenticationResult = serviceRef.AuthenticateUser(txtUserName.Text, txtPassword.Text);
                    result = authenticationResult.Where(p => p.Key == "Result").FirstOrDefault().Value;
                    if (result != "Success")
                    {
                        lblError.Text = result;
                    }
                    else
                    {
                        idUser = authenticationResult.Where(p => p.Key == "IdUser").FirstOrDefault().Value;
                        Session.Add("IdUser", idUser);
                        FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, false);
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyWebmasterOfError("QCAstBilling", "Login", ex.ToString());
                lblError.Text = "Website encountered an error.";
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