<%--
 * $Id$
 * 
 * Coursework – Futoshiki.Web
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment.  
--%>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="Web.UserRegister" Codebehind="UserRegister.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <link href="Styles/DftTheme.css" rel="stylesheet" type="text/css" />    
    <title>Futoshiki Site :: User Register</title>
</head>

<body>
    <form id="form1" runat="server">
    
   <div id="page">
   
        <div id="header">
            <p class="site-title">Futoshiki Site</p>
            <div id="login">
                    <span id="SpanLogout" runat="server">
                    <p>
                        Welcome  <asp:Label ID="LblUsername" runat="server" Text="Label"/><asp:HyperLink ID="LinkLogout" runat="server"> Logout</asp:HyperLink>
                    </p>                
                    </span>             
                    <span id="SpanLogin" runat="server">
                    <ul>
                        <li><a href="Login.aspx">Login</a></li>
                    </ul>                
                    </span>
            </div>
            <ul id="menu">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="About.aspx">About</a></li>
            </ul>
        </div>
        
        <div id="main">
            <div id="content">    
    <fieldset>
        <legend>Register Form</legend>
        <ol>
        <li>
        <asp:Label ID="LblMsg" runat="server" ForeColor="Red" />       
        </li>
            <li class="email">
                <label for="Email">Email:</label> 
                <asp:Textbox runat="server" id="Email"/>
                <asp:RequiredFieldValidator ID="EmailRequiredFieldValidator" 
                    runat="server" ErrorMessage="Please enter email" 
                    ControlToValidate="Email" CssClass="validation-error"/>
                <asp:RegularExpressionValidator ID="EmailRegularExpressionValidator" 
                    runat="server" 
                    ErrorMessage="Please check the format and enter a correct e-mail address" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    ControlToValidate="Email" CssClass="validation-error"/>                      
            </li>
            
            <li class="username">
                <label for="Username">Username:</label> 
                <asp:TextBox runat="server" id="Username" />
                <asp:RequiredFieldValidator ID="UsernameRequiredFieldValidator" 
                    runat="server" ErrorMessage="Please enter username"
                    CssClass="validation-error" ControlToValidate="Username" />
            </li>
            
            <li class="password">
                <label for="Password">Password:</label> 
                <asp:Textbox runat="server" id="Password" TextMode="password" />
                <asp:RequiredFieldValidator ID="PwdRequiredFieldValidator" 
                    runat="server" ErrorMessage="Please enter password"
                    CssClass="validation-error" ControlToValidate="Password"/>
                <asp:RegularExpressionValidator ID="PwdLenthValidator" 
                    runat="server" ErrorMessage="The minimum password length must be 6" 
                    ControlToValidate="Password" ValidationExpression="\S{6,}"
                    CssClass="validation-error"/>
            </li>
            <li class="comfirmPassword">
                <label for="ConfirmPassword">Confirm Password:</label>
                <asp:Textbox runat="server" id="ConfirmPassword" TextMode="password" />
                <asp:RequiredFieldValidator ID="CfmPwdRequiredFieldValidator" 
                    runat="server" ErrorMessage="Please enter password again"
                    CssClass="validation-error" ControlToValidate="ConfirmPassword"/>
                <asp:CompareValidator ID="PswCompareValidator" runat="server" 
                    ErrorMessage="Passwords must be same" CssClass="validation-error"
                    ControlToCompare="Password" ControlToValidate="ConfirmPassword"/>
            </li>

        </ol>
        <p>
            <asp:Button runat="server" ID="Register" Text="Register" onclick="RegisterClick"/>
        </p>
    </fieldset>
    
</div>
        </div>
        
        <div id="footer">
            &copy; 2011 - Futoshiki Site
        </div>        
        
    </div>        

    </form>
</body>
</html>
