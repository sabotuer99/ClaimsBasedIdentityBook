<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrerequisitesControl.ascx.cs" Inherits="Litware.Portal.Controls.PrerequisitesControl" %>

<div id="prerequisitesinfo">
    Prerequisites not met:
    <ul>
        <%
            foreach (var error in this.Errors)
            {%>
        <li><%=error%></li>
        <%
            }%>
    </ul>
    To setup the prerequisites, run the command StartHere.cmd in the samples root folder.
</div>