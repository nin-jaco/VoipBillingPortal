using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using QCAstSolution.Classes;
using QCAstWebApplication.QCAstServiceReference;

namespace QCAstWebApplication
{
    public partial class Site : System.Web.UI.MasterPage
    {
        internal int idUser;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUser"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        HideDivs();
                        idUser = int.Parse(Session["IdUser"].ToString());
                        int idProfile = 0;
                        using (QCAstServiceClient serviceRef = new QCAstServiceClient())
                        {
                            idProfile = serviceRef.GetUserFromId(idUser).IdProfile;

                            switch (idProfile)
                            {
                                case 1:
                                    divUserMenu.Visible = true;
                                    break;
                                case 2:
                                    divDatamanagerMenu.Visible = true;
                                    break;
                                case 3:
                                    divClientManagerMenu.Visible = true;
                                    break;
                                case 4:
                                    divAdminMenu.Visible = true;
                                    break;
                                case 5:
                                    divClientMenu.Visible = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        NotifyWebmasterOfError("QCAstBilling", "Site.Master", ex.ToString());
                        Response.Redirect("ErrorPage.aspx?Error=GeneralError", true);
                    }
                }
            }

        }

        private void HideDivs()
        {
            divAdminMenu.Visible = false;
            divClientManagerMenu.Visible = false;
            divClientMenu.Visible = false;
            divDatamanagerMenu.Visible = false;
            divUserMenu.Visible = false;
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