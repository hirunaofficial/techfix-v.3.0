using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.InventoryServiceReference; // Link to the InventoryServiceReference
using TechFixV3._0Client.ProductServiceReference; // Link to the ProductServiceReference
using TechFixV3._0Client.UserServiceReference; // Link to the UserServiceReference

namespace TechFixV3._0Client.Admin
{
    public partial class ManageInventory : System.Web.UI.Page
    {
        private InventoryServiceSoapClient inventoryService = new InventoryServiceSoapClient();
        private ProductServiceSoapClient productService = new ProductServiceSoapClient(); // Create an instance of ProductService
        private UserServiceSoapClient userService = new UserServiceSoapClient(); // Create an instance of UserService

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSupplierGrid();
                BindSupplierProductsGrid();
                BindInventoryGrid();
                BindSupplierDropDown();
            }
        }

        private void BindSupplierGrid()
        {
            var suppliers = userService.GetUsersByRole("Supplier");
            SupplierGridView.DataSource = suppliers;
            SupplierGridView.DataBind();
        }

        private void BindSupplierProductsGrid()
        {
            var products = productService.GetProducts();
            SupplierProductsGridView.DataSource = products;
            SupplierProductsGridView.DataBind();
        }

        private void BindInventoryGrid()
        {
            var inventoryItems = inventoryService.GetInventory();
            InventoryGridView.DataSource = inventoryItems;
            InventoryGridView.DataBind();
        }

        private void BindSupplierDropDown()
        {
            var suppliers = userService.GetUsersByRole("Supplier");

            SupplierDropDown.DataSource = suppliers;
            SupplierDropDown.DataTextField = "Name";
            SupplierDropDown.DataValueField = "Id";
            SupplierDropDown.DataBind();

            SupplierDropDown.Items.Insert(0, new ListItem("Select Supplier", ""));
        }

        protected void SupplierDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindNewItemDropDown(); // Bind the products based on the selected supplier
            BindSupplierProductGrid(); // Update the supplier products grid
        }

        private void BindNewItemDropDown()
        {
            int selectedSupplierId = int.Parse(SupplierDropDown.SelectedValue);
            var products = productService.GetProductsBySupplierId(selectedSupplierId); // Ensure this method exists
            NewItemDropDown.DataSource = products;
            NewItemDropDown.DataTextField = "ItemName";
            NewItemDropDown.DataValueField = "ProductId";
            NewItemDropDown.DataBind();
        }

        private void BindSupplierProductGrid()
        {
            int selectedSupplierId = int.Parse(SupplierDropDown.SelectedValue);
            var products = productService.GetProductsBySupplierId(selectedSupplierId); // Ensure this method exists
            SupplierProductsGridView.DataSource = products;
            SupplierProductsGridView.DataBind();
        }

        protected void NewItemDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedProductId = int.Parse(NewItemDropDown.SelectedValue);
            var product = productService.GetProductById(selectedProductId); // Ensure this method exists

            if (product != null)
            {
                NewPriceTextBox.Text = product.Price.ToString("F2");
                NewDiscountTextBox.Text = product.Discount.ToString("F2");
            }
        }

        protected void AddItemButton_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                string itemName = NewItemDropDown.SelectedItem.Text; // Get the selected item name
                int quantity = int.Parse(NewQuantityTextBox.Text);
                decimal price = decimal.Parse(NewPriceTextBox.Text);
                decimal discount = decimal.Parse(NewDiscountTextBox.Text);
                int supplierId = int.Parse(SupplierDropDown.SelectedValue); // Get selected supplier ID

                string result = inventoryService.AddInventoryItem(itemName, quantity, price, discount, supplierId);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

                BindInventoryGrid();
            }
        }

        protected void InventoryGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            InventoryGridView.EditIndex = e.NewEditIndex;
            BindInventoryGrid();
        }

        protected void InventoryGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            InventoryGridView.EditIndex = -1;
            BindInventoryGrid();
        }

        protected void InventoryGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Retrieve the Item ID from DataKey
            int itemId = (int)InventoryGridView.DataKeys[e.RowIndex].Value;

            // Retrieve the updated values from the GridView
            GridViewRow row = InventoryGridView.Rows[e.RowIndex];
            int quantity = int.Parse(((TextBox)row.FindControl("QuantityTextBox")).Text);

            // Call the web service to update the inventory item
            string result = inventoryService.UpdateInventory(itemId, quantity, 0, 0, 0); // Price and discount are not editable
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

            // Set GridView back to normal mode
            InventoryGridView.EditIndex = -1;
            BindInventoryGrid(); // Refresh the GridView
        }

        protected void InventoryGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Retrieve the Item ID from DataKey
            int itemId = (int)InventoryGridView.DataKeys[e.RowIndex].Value;

            // Call the web service to delete the inventory item
            string result = inventoryService.DeleteInventoryItem(itemId);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

            // Refresh the GridView after deletion
            BindInventoryGrid();
        }

        private bool IsValidInput()
        {
            // Check if all required fields are filled out for adding an item
            if (string.IsNullOrWhiteSpace(NewItemDropDown.SelectedValue) ||
                string.IsNullOrWhiteSpace(NewQuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewPriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewDiscountTextBox.Text) ||
                string.IsNullOrWhiteSpace(SupplierDropDown.SelectedValue))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill in all fields.');", true);
                return false;
            }

            // Validate quantity
            if (!int.TryParse(NewQuantityTextBox.Text, out _))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid quantity format.');", true);
                return false;
            }

            return true;
        }
    }
}
