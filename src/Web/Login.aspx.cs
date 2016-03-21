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
using System.Web.Security;
using System.Web.UI;

namespace Web
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void  BtnLoginClick(object sender, EventArgs e)
        {
            if (Membership.ValidateUser(Username.Text, Password.Text))
            {
                FormsAuthentication.SetAuthCookie(Username.Text, false);
                FormsAuthentication.RedirectFromLoginPage(Username.Text, false);
            }
            else
            {
                LblMsg.Text = "Invalid username and password or the user does not exist!";
            }
        }

    }
}
