<%-- 
$Id$

Coursework – The presentation layer of the Futoshiki puzzle.

This file is the result of my own work. Any contributions to the work by third parties,
other than tutors, are stated clearly below this declaration. Should this statement prove to
be untrue I recognise the right and duty of the Board of Examiners to take appropriate
action in line with the university's regulations on assessment.
--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebUI.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
    <title>Futoshiki Home Page</title>
    <link href="Styles/Main.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="FutoshikiTable" runat="server"/>
    </div>
    </form>
</body>

</html>
