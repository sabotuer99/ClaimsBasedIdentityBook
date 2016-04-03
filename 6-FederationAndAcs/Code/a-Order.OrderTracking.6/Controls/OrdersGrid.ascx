<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdersGrid.ascx.cs" Inherits="AOrder.OrdersGrid" %>

<div id="orderslist">
    <asp:GridView ID="OrdersGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
        <Columns>
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yyyy}" />
            <asp:TemplateField HeaderText="Customer">
                <ItemTemplate><%# Eval("Customer.Name") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate><%# Eval("Customer.Phone") %></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Details" HeaderText="Order details" SortExpression="Details" />
            <asp:TemplateField HeaderText="Amount">
                <ItemTemplate><%# string.Format(System.Globalization.CultureInfo.CurrentUICulture, "${0}", Math.Round((decimal)Eval("Amount"), 2)) %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status" SortExpression="Status"  ItemStyle-HorizontalAlign="Center">
                <ItemTemplate><%# Enum.GetName(typeof(OrderStatus), Eval("Status")) %></ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle BackColor="#e6e6e6" />
        <AlternatingRowStyle BackColor="#e6f0ff" />
        <EmptyDataTemplate>
            There are no orders registered.
        </EmptyDataTemplate>
    </asp:GridView>
</div>
