<%@ Page Title="Product Store" Language="C#" MasterPageFile="~/Supplier/SupplierMaster.master" AutoEventWireup="true" CodeBehind="ProductStore.aspx.cs" Inherits="TechFixV3._0Client.Supplier.ProductStore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome to Your Product Store</h2>

    <!-- Add New Item Section -->
    <div class="add-item-section">
        <h3>Add New Item</h3>
        <div class="form-group">
            <asp:Label ID="ItemNameLabel" runat="server" Text="Item Name:" AssociatedControlID="ItemNameTextBox" />
            <asp:TextBox ID="ItemNameTextBox" runat="server" CssClass="input-field" />
        </div>
        <div class="form-group">
            <asp:Label ID="QuantityLabel" runat="server" Text="Quantity:" AssociatedControlID="QuantityTextBox" />
            <asp:TextBox ID="QuantityTextBox" runat="server" CssClass="input-field" />
        </div>
        <div class="form-group">
            <asp:Label ID="PriceLabel" runat="server" Text="Price per Unit:" AssociatedControlID="PriceTextBox" />
            <asp:TextBox ID="PriceTextBox" runat="server" CssClass="input-field" />
        </div>
        <div class="form-group">
            <asp:Label ID="DiscountLabel" runat="server" Text="Discount (%):" AssociatedControlID="DiscountTextBox" />
            <asp:TextBox ID="DiscountTextBox" runat="server" CssClass="input-field" />
        </div>
        <div class="form-group">
            <asp:Button ID="AddItemButton" runat="server" Text="Add Item" CssClass="submit-button" OnClick="AddItemButton_Click" />
        </div>
    </div>

    <!-- Display Stock Section with Inline Editing -->
    <div class="stock-details-section">
        <h3>Stock Details</h3>
        <asp:GridView ID="StockGridView" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="ProductId" OnRowEditing="StockGridView_RowEditing" OnRowCancelingEdit="StockGridView_RowCancelingEdit" OnRowUpdating="StockGridView_RowUpdating" OnRowDeleting="StockGridView_RowDeleting">
            <Columns>
                <asp:BoundField DataField="ProductId" HeaderText="Product ID" ReadOnly="True" />
                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                <asp:BoundField DataField="Quantity" HeaderText="Available Quantity" />
                <asp:BoundField DataField="Price" HeaderText="Price per Unit" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Discount" HeaderText="Discount (%)" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>