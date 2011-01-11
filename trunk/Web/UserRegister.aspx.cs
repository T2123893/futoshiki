/*
 * $Id$
 * 
 * Coursework – Futoshiki.Web
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */



using System;
using System.Diagnostics;
using System.Web.Security;
using System.Web.UI;
using Web.localhost;

namespace Web
{
    public partial class UserRegister : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string u = User.Identity.Name;
            if (string.IsNullOrEmpty(u))
            {
                SpanLogout.Visible = false;
            }
            else
            {
                SpanLogin.Visible = false;
                LblUsername.Text = u;
            }
            //Roles.CreateRole("admin");
            //Roles.CreateRole("user");
        }

        protected void RegisterClick(object sender, EventArgs e)
        {
            bool isOk = false;
            try{
            Membership.CreateUser(Username.Text, Password.Text, Email.Text);


            // use web service to send informaition
            
                InfoSendWebService proxy = new InfoSendWebService();
                string from = "T2123893@my.nut.ac.uk";
                string to = Email.Text;
                string subject = "Futoshik Site Register";
                string body = "Username: " + 
                    Username.Text + "\nPassword: " + Password.Text;
                isOk = proxy.SendMail(from, to, subject, body);
            }
            catch(MembershipCreateUserException mcue)
            {
                Debug.WriteLine(GetType() + " - " + mcue);
                LblMsg.Text = "Sorry, " + mcue.Message;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(GetType() + " - error send mail - " + ex);
            }

            if (isOk)
            {   
                LblMsg.Text = "Thanks for your register. A register comfirm " + 
                    "letter has been sent to you, please check your email.";
            } else
            {
                //Roles.AddUsersToRole(Username.Text, "user");
                FormsAuthentication.RedirectFromLoginPage(Username.Text, false);                
            }

        }

    }
}
