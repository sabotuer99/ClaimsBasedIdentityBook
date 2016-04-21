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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="AddExpense.aspx.cs" Inherits="AExpense.AddExpense" %>

<asp:Content ID="AddExpenseHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="AddExpenseContent" ContentPlaceholderID="ContentPlaceholder" runat="server">
<form method="post" action="AddExpense.aspx">
    <div id="newexpense">
        <table>
            <tbody>
                <tr>
                    <td>
                        <asp:Label ID="ExpenseDateLabel" AssociatedControlID="ExpenseDate" Text="Date:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="ExpenseDate" runat="server" />
                        &nbsp;<asp:RequiredFieldValidator ID="ExpenseDateRequiredValidator" 
                            runat="server" ErrorMessage="*"
                            ControlToValidate="ExpenseDate" />
                        <asp:CompareValidator ID="ExpenseDateFormatValidator" runat="server" ErrorMessage="Enter a valid date to continue."
                            ControlToValidate="ExpenseDate" Type="Date" Operator="DataTypeCheck" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ExpenseTitleLabel" AssociatedControlID="ExpenseTitle" Text="Title:"
                            runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="ExpenseTitle" runat="server" />
                        &nbsp;<asp:RequiredFieldValidator ID="ExpenseTitleValidator" runat="server" ErrorMessage="*"
                            ControlToValidate="ExpenseTitle" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ExpenseDescriptionLabel" AssociatedControlID="ExpenseDescription"
                            Text="Description:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="ExpenseDescription" runat="server" TextMode="MultiLine" Rows="3"
                            Columns="50" />
                        &nbsp;<asp:RequiredFieldValidator ID="ExpenseDescriptionValidator" 
                            runat="server" ErrorMessage="*"
                            ControlToValidate="ExpenseDescription" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ExpenseAmountLabel" AssociatedControlID="ExpenseAmount" Text="Amount:"
                            runat="server" />
                    </td>
                    <td>
                        $&nbsp;<asp:TextBox ID="ExpenseAmount" runat="server" CssClass="amountText" />
                        &nbsp;<asp:RequiredFieldValidator ID="ExpenseAmountRequiredValidator" 
                            runat="server" ErrorMessage="*"
                            ControlToValidate="ExpenseAmount" />
                        <asp:CompareValidator ID="ExpenseAmountValidator" runat="server" ErrorMessage="Enter a valid amount to continue."
                            ControlToValidate="ExpenseAmount" Type="Currency" 
                            Operator="DataTypeCheck" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ExpenseReimbursementMethodLabel" AssociatedControlID="ExpenseReimbursementMethod"
                            Text="Reimb. Method:" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ExpenseReimbursementMethod" runat="server" />
                        <samples:TooltipInformation ID="TooltipInformation1" runat="server" Text="<%$ Resources:TooltipText, PreferedReimbursementHint %>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ExpenseCostCenterLabel" AssociatedControlID="ExpenseCostCenter" Text="Cost Center:"
                            runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="ExpenseCostCenter" runat="server" CssClass="readonlyField" />
                        <samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, CostCenterHint %>" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="AddExpenseButton" runat="server" Text="Add" OnClick="AddExpenseButtonOnClick" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</asp:Content>
