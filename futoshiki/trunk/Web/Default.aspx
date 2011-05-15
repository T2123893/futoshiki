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

<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>


<head>
    <link href="Styles/DftTheme.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Futoshiki.css" rel="Stylesheet" type="text/css" />
    <script src="Scripts/Core.js" type="text/javascript"></script>  
    <title>Futoshiki Site</title>
</head>

<body>

<form id="form1" runat="server">   
   
   <div id="page">
   
        <div id="header">
            <p class="site-title">Futoshiki Site</p>
            <div id="login">            
                <span id="SpanLogout" runat="server">                   
                 <p>
                    Welcome  
                    <asp:Label ID="LblUsername" runat="server"/>
                    <asp:LinkButton ID="LinkLogout" runat="server" Text="Logout" 
                        onclick="LinkLogoutClick"/>                                  
                 </p>
                </span>  
                <span id="SpanLogin" runat="server">           
                <ul>
                    <li><a href="UserRegister.aspx">Register</a></li>
                    <li><a href="Login.aspx">Login</a></li>
                </ul> 
                </span>               
            </div>
            <div ID="MsgDiv" runat="server"/>
            <ul id="menu">
                <li><a href="#">Home</a></li>
                <li><a href="About.aspx">About</a></li>
            </ul>            
        </div>
        
        <div id="main">
            <div id="content">
             <div id="tagline">A standard (5*5) game of Futoshiki. Every time will be different. Ready? Go!</div>
                
            <div id="futoshikiDiv">
            
                <div id="futoshikiContent">  
                    <asp:Table ID="FutoshikiTable" runat="server"/>   
                </div> 
                
                <div id="futoshikiSideBar">            
                    <input id="ChkCorrectness" type="checkbox" onclick="checkAll()"/>        
                    <Label id="LblChecker" for="ChkCorrectness">Trace the correctness</Label>
                    <br />                  
                    <input ID="BtnReset" type="button" value="Reset Grid" OnClick="resetGrid()"/>
                    <br />
                    <asp:Button ID="ButChkSln" runat="server" OnClick="BtnChkSlnClick" 
                        Text="Check Solution" OnClientClick="return checkSolution()" />
                    <br />
                     <asp:Button ID="BtnShowSln" runat="server" OnClick="BtnShowSolution" 
                        Text="Show Solution" OnClientClick="return showSolution()" />
                    <br />
                    <asp:Button ID="BtnSave" runat="server" OnClick="BtnSaveClick" 
                        Text="Save" OnClientClick="return setHidData()"/>
                    <br />
                    <asp:DropDownList ID="ScaleList" runat="server" DataSourceID="GameScaleDS" 
                        DataTextField='Text' DataValueField='Value'                         
                        onselectedindexchanged="ScaleListSelectedIndexChanged" 
                        AutoPostBack="True"/>
                    <asp:XmlDataSource ID="GameScaleDS" runat="server" 
                        DataFile="~/App_Data/GameScaleList.xml"></asp:XmlDataSource>
                    <br />                      
                    <asp:HiddenField ID="HidData" runat="server" />
                </div>  
                                  
            </div>                 
                        
            </div>
        </div>
        
        <div id="footer">
            &copy; 2011 - Futoshiki Site
        </div>        
        
    </div>    
  
    </form>

</body>
</html>