<%-- 
$Id$

Coursework – The presentation layer of the Futoshiki puzzle.

This file is the result of my own work. Any contributions to the work by third parties,
other than tutors, are stated clearly below this declaration. Should this statement prove to
be untrue I recognise the right and duty of the Board of Examiners to take appropriate
action in line with the university's regulations on assessment.
--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Game.aspx.cs" Inherits="WebUI.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="Styles/Main.css" rel="stylesheet" type="text/css" />     
    <script src="Scripts/Cell.js" type="text/javascript"></script>   
    <title>Futoshiki Home Page</title>
</head>

<body>
<div id="container" style="margin:0; width:900px;">
    <form id="form1" runat="server">
    
<!--    <div id="header" style="border:solid 1px #008000; height:50px; margin-bottom:5px;"></div>  -->
    
    <div id="mainContent" style="height:600px; margin:5px 0 5px 5px;">
    
        <div id="content" style="border:solid 1px #008000; height:600px; width:680px; float:left;">
            <asp:Table ID="FutoshikiTable" runat="server"/>   
        </div> 
        
        <div id="sideBar" style="border:solid 1px #008000; height:600px;width:205px; float:right; padding-left: 3px;">            
            <input id="ChkCorrectness" type="checkbox" onchange="checkAll()"/>        
            <Label id="LblChecker" for="ChkCorrectness" style="cursor:pointer;">Check the correctness</Label>
        </div>  
                          
    </div>    
    
<!--    <div id="footer" style="border:solid 1px #008000; height:30px;"></div>-->
    
    </form>    
</div>    
</body>

</html>
