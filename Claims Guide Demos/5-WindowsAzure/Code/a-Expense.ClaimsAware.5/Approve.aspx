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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Approve.aspx.cs" Inherits="AExpense.Approve" %>

<asp:Content ID="ApproveHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="ApproveContent" ContentPlaceholderID="ContentPlaceholder" runat="server">

 <div id="expenselist">
    <asp:GridView ID="MyExpensesGridView" runat="server" AutoGenerateColumns="False" DataSourceID="ExpensesDataSource" AllowPaging="True" DataKeyNames="Id">
        <Columns>        
            <asp:TemplateField HeaderText="User Name">
                <ItemTemplate><%# ((User)Eval("User")).UserName %></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" />
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" ReadOnly="True" />
            <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" />            
            <asp:TemplateField HeaderText="Amount">
                <ItemTemplate><%# string.Format(System.Globalization.CultureInfo.CurrentUICulture, "${0}", Math.Round((decimal)Eval("Amount"), 2)) %></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ReadOnly="True" />
            <asp:BoundField DataField="ReimbursementMethod" HeaderText="Reimbursement Method" ReadOnly="True" />
            <asp:TemplateField HeaderText="Approved" SortExpression="Approved"  ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# (Boolean.Parse(Eval("Approved").ToString())) ? "Yes" : "No"%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox runat="server" ID="Approved" Checked='<%# Bind("Approved") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
            <asp:CommandField ButtonType="Image" ShowEditButton="True" 
                CancelImageUrl="~/Styling/Images/cancel.gif" CancelText="Cancel"
                EditImageUrl="~/Styling/Images/edit.png" HeaderText="Edit" 
                UpdateImageUrl="~/Styling/Images/update.png" UpdateText="Update" 
                ItemStyle-HorizontalAlign="Center" />
        </Columns>
        <HeaderStyle BackColor="#e6e6e6" />
        <AlternatingRowStyle BackColor="#e6f0ff" />
        <EmptyDataTemplate>
            There are no expenses registered.
        </EmptyDataTemplate>
    </asp:GridView>
 </div>
    
    <asp:ObjectDataSource ID="ExpensesDataSource" runat="server" 
        DataObjectTypeName="AExpense.Data.Expense" SelectMethod="GetAllExpenses" 
        TypeName="AExpense.Data.ExpenseRepository" UpdateMethod="UpdateApproved">
    </asp:ObjectDataSource>

</asp:Content>
