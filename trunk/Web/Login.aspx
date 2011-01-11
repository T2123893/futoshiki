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


<%@ Page Language="C#" AutoEventWireup="true" Inherits="Web.Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <link href="Styles/DftTheme.css" rel="stylesheet" type="text/css" />
    <title>Futoshiki Site :: User Login</title>
</head>

<body>
    <form id="form1" runat="server">
    
   <div id="page">
   
        <div id="header">
            <p class="site-title">Futoshiki Site</p>
            <div id="login">                                 
                    <ul>
                        <li><a href="UserRegister.aspx">Register</a></li>
                    </ul>                                    
            </div>
            <ul id="menu">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="About.aspx">About</a></li>
            </ul>
        </div>
        
        <div id="main">
            <div id="content">    
    
    
    
<fieldset>
        <legend>Login to Your Account</legend>
        <ol>
        <li>
            <asp:Label ID="LblMsg" runat="server" ForeColor="Red" />
        </li>
            <li class="username">
                <label for="Username">Username:</label>
                <asp:Textbox id="Username" runat="server"/>
                <asp:RequiredFieldValidator ID="UNameRequiredFieldValidator" runat="server" 
                    ErrorMessage="Please enter username" ControlToValidate="Username"
                    CssClass="validation-error"/>
            </li>
            <li class="password">
                <label for="Password">Password:</label>
               <asp:TextBox runat="server" ID="Password" TextMode="Password"/> 
                <asp:RequiredFieldValidator ID="PwdRequiredFieldValidator" runat="server" 
                    ErrorMessage="Please enter password" ControlToValidate="Password"
                    CssClass="validation-error" />
            </li>

        </ol>
        <p>
            <asp:Button runat="server" Text="Login" ID="BtnLogin" OnClick="BtnLoginClick"/>
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
