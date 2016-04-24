<%--
===============================================================================
 Microsoft patterns & practices
 Cliams Identity Guide V2
===============================================================================
 Copyright © Microsoft Corporation.  All rights reserved.
 This code released under the terms of the 
 Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
===============================================================================
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AExpense.Default" %>

<asp:Content ID="DefaultHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="DefaultContent" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <div id="expenselist">
        <asp:GridView ID="MyExpensesGridView" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:c}" />
                <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" SortExpression="CostCenter" />
                <asp:BoundField DataField="ReimbursementMethod" HeaderText="Reimbursement Method"
                    SortExpression="ReimbursementMethod" />
                <asp:TemplateField HeaderText="Approved" SortExpression="Approved"  ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate><%# (Boolean.Parse(Eval("Approved").ToString())) ? "Yes" : "No"%></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#e6e6e6" />
            <AlternatingRowStyle BackColor="#e6f0ff" />
            <EmptyDataTemplate>
                There are no expenses registered for <i>
                    <%= ((User)Session["LoggedUser"]).FullName %></i>.
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
