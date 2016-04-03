<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdersGridForApprovers.ascx.cs" Inherits="AOrder.OrdersGridForApprovers" %>

<div id="ordersapproverslist">
    <samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, EditColumnHint %>" />
    <asp:GridView ID="OrderApproversGridView" runat="server" AutoGenerateColumns="False" DataSourceID="OrdersDataSource" AllowPaging="True" DataKeyNames="Id">
        <Columns>
            <asp:TemplateField HeaderText="Date">
                <ItemTemplate><%# string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0:MM/dd/yyyy}", Eval("Date"))  %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Customer">
                <ItemTemplate><%# Eval("Customer.Name") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate><%# Eval("Customer.Phone") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Order details">
                <ItemTemplate><%# Eval("Details")%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Amount">
                <ItemTemplate><%# string.Format(System.Globalization.CultureInfo.CurrentUICulture, "${0}", Math.Round((decimal)Eval("Amount"), 2)) %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status" SortExpression="Status"  ItemStyle-HorizontalAlign="Center">
                <ItemTemplate><%# Enum.GetName(typeof(OrderStatus), Eval("Status")) %></ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="StatusDropDownList" DataSourceID="OrderStatusDataSource" SelectedValue='<%# Bind("Status") %>' />
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
            There are no orders registered.
        </EmptyDataTemplate>
    </asp:GridView>
</div>

<asp:ObjectDataSource ID="OrdersDataSource" runat="server" 
        DataObjectTypeName="AOrder.Data.Order" SelectMethod="GetAllOrders" 
        TypeName="AOrder.Data.OrderRepository" UpdateMethod="UpdateStatus" />

<asp:ObjectDataSource ID="OrderStatusDataSource" runat="server" 
        DataObjectTypeName="System.String" SelectMethod="GetAllOrderStatus" 
        TypeName="AOrder.Data.OrderRepository" />
