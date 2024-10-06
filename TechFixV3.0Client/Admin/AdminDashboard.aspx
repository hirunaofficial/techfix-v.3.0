<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="TechFixV3._0Client.Admin.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="dashboard-title">Welcome to the Admin Dashboard</h2>
    <p class="dashboard-intro">This is the main admin dashboard where you can manage users, inventory, and orders. Use the sections below to navigate to the specific management pages.</p>

    <div class="admin-sections">
        <div class="section">
            <h3>Manage Users</h3>
            <p>View and manage all users in the system, including adding, editing, and removing users as needed.</p>
            <a href="ManageUsers.aspx" class="btn">Go to Manage Users</a>
        </div>

        <div class="section">
            <h3>Manage Inventory</h3>
            <p>Update and manage the system’s inventory, track stock levels, and add new items to the system.</p>
            <a href="ManageInventory.aspx" class="btn">Go to Manage Inventory</a>
        </div>

        <div class="section">
            <h3>Manage Orders</h3>
            <p>Track and manage all incoming orders, including order status, shipping, and customer notifications.</p>
            <a href="ManageOrders.aspx" class="btn">Go to Manage Orders</a>
        </div>
    </div>
</asp:Content>