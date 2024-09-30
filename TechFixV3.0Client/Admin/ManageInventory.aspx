<%@ Page Title="Manage Inventory" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="ManageInventory.aspx.cs" Inherits="TechFixV3._0Client.Admin.ManageInventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Inventory</h2>

    <!-- Display Supplier List Table Section -->
    <div class="supplier-section">
        <h3>Suppliers</h3>
        <asp:GridView ID="SupplierGridView" runat="server" AutoGenerateColumns="False" CssClass="table">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Supplier ID" />
                <asp:TemplateField HeaderText="Supplier Name">
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Contact Number">
                    <ItemTemplate>
                        <%# Eval("ContactNumber") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate>
                        <%# Eval("Email") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <!-- Display Selected Supplier Products Table Section -->
    <div class="supplier-products-section">
        <h3>Supplier Products</h3>
        <asp:GridView ID="SupplierProductsGridView" runat="server" AutoGenerateColumns="False" CssClass="table">
            <Columns>
                <asp:BoundField DataField="ProductId" HeaderText="Product ID" />
                <asp:BoundField DataField="SupplierId" HeaderText="Supplier ID" />
                <asp:TemplateField HeaderText="Product Name">
                    <ItemTemplate>
                        <%# Eval("ItemName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <%# Eval("Quantity") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <%# Eval("Price") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <%# Eval("Discount") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <!-- Display Inventory Table Section -->
    <div class="inventory-section">
        <h3>Inventory</h3>
        <asp:GridView ID="InventoryGridView" runat="server" AutoGenerateColumns="False" CssClass="table" 
            OnRowEditing="InventoryGridView_RowEditing" 
            OnRowDeleting="InventoryGridView_RowDeleting" 
            OnRowCancelingEdit="InventoryGridView_RowCancelingEdit"
            OnRowUpdating="InventoryGridView_RowUpdating"
            DataKeyNames="ItemId">
            <Columns>
                <asp:BoundField DataField="ItemId" HeaderText="Item ID" />
                <asp:TemplateField HeaderText="Item Name">
                    <ItemTemplate>
                        <%# Eval("ItemName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <%# Eval("Quantity") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="QuantityTextBox" runat="server" Text='<%# Bind("Quantity") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <%# Eval("Price") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="PriceTextBox" runat="server" Text='<%# Bind("Price") %>' ReadOnly="true" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <%# Eval("Discount") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="DiscountTextBox" runat="server" Text='<%# Bind("Discount") %>' ReadOnly="true" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Supplier ID">
                    <ItemTemplate>
                        <%# Eval("SupplierId") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Supplier and Product Selection Section -->
    <div class="selection-section">
        <h3>Add New Inventory Item</h3>
        
        <div class="form-group">
            <asp:Label ID="SupplierLabel" runat="server" Text="Select Supplier:" AssociatedControlID="SupplierDropDown" />
            <asp:DropDownList ID="SupplierDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SupplierDropDown_SelectedIndexChanged">
                <asp:ListItem Text="Select Supplier" Value="" /> 
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label ID="ProductLabel" runat="server" Text="Select Product:" AssociatedControlID="NewItemDropDown" />
            <asp:DropDownList ID="NewItemDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="NewItemDropDown_SelectedIndexChanged">
                <asp:ListItem Text="Select Product" Value="" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label ID="NewQuantityLabel" runat="server" Text="Quantity:" AssociatedControlID="NewQuantityTextBox" />
            <asp:TextBox ID="NewQuantityTextBox" runat="server" CssClass="input-field" />
        </div>

        <div class="form-group">
            <asp:Label ID="NewPriceLabel" runat="server" Text="Price:" AssociatedControlID="NewPriceTextBox" />
            <asp:TextBox ID="NewPriceTextBox" runat="server" CssClass="input-field" ReadOnly="true" />
        </div>

        <div class="form-group">
            <asp:Label ID="NewDiscountLabel" runat="server" Text="Discount:" AssociatedControlID="NewDiscountTextBox" />
            <asp:TextBox ID="NewDiscountTextBox" runat="server" CssClass="input-field" ReadOnly="true" />
        </div>

        <asp:Button ID="AddItemButton" runat="server" Text="Add Item" CssClass="submit-button" OnClick="AddItemButton_Click" />
    </div>
</asp:Content>
