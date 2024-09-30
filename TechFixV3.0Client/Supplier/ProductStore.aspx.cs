using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.ProductServiceReference; // Link to the ProductServiceReference

namespace TechFixV3._0Client.Supplier
{
    public partial class ProductStore : System.Web.UI.Page
    {
        private ProductServiceSoapClient productService = new ProductServiceSoapClient(); // Instance of ProductService

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Fetch supplier ID and bind the products grid using supplier ID
                int supplierId = GetSupplierIdFromCookies();
                BindProductsGrid(supplierId);
            }
        }

        private void BindProductsGrid(int supplierId)
        {
            // Fetch products for the specific supplier and bind to the GridView
            var products = productService.GetProductsBySupplierId(supplierId); // Ensure this method exists
            StockGridView.DataSource = products;
            StockGridView.DataBind();
        }

        protected void AddItemButton_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (IsValidInput())
            {
                string itemName = ItemNameTextBox.Text;
                int quantity = int.Parse(QuantityTextBox.Text);
                decimal price = decimal.Parse(PriceTextBox.Text);
                decimal discount = decimal.Parse(DiscountTextBox.Text);
                int supplierId = GetSupplierIdFromCookies(); // Get supplier ID from cookies

                // Call the web service to add the new product
                string result = productService.AddProduct(itemName, quantity, price, discount, supplierId);

                // Show success message using JavaScript alert
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{result}');", true);

                // Clear input fields
                ClearInputFields();

                // Refresh the Products grid
                BindProductsGrid(supplierId); // Refresh with supplier ID
            }
            else
            {
                // Show validation error using JavaScript alert
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill in all fields correctly.');", true);
            }
        }

        protected void StockGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the row to edit mode
            StockGridView.EditIndex = e.NewEditIndex;
            BindProductsGrid(GetSupplierIdFromCookies());
        }

        protected void StockGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the product ID from DataKey
            int productId = Convert.ToInt32(StockGridView.DataKeys[e.RowIndex].Value);

            // Get the edited values from the GridView row
            GridViewRow row = StockGridView.Rows[e.RowIndex];
            string itemName = ((TextBox)row.Cells[1].Controls[0]).Text;
            int quantity = int.Parse(((TextBox)row.Cells[2].Controls[0]).Text);
            decimal price = decimal.Parse(((TextBox)row.Cells[3].Controls[0]).Text);
            decimal discount = decimal.Parse(((TextBox)row.Cells[4].Controls[0]).Text);

            try
            {
                // Call the web service to update the product
                string result = productService.UpdateProduct(productId, itemName, quantity, price, discount, GetSupplierIdFromCookies());

                // Show success message using JavaScript alert
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{result}');", true);

                // Exit edit mode
                StockGridView.EditIndex = -1;

                // Rebind the grid to reflect changes
                BindProductsGrid(GetSupplierIdFromCookies());
            }
            catch (Exception ex)
            {
                // Show error message using JavaScript alert
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error updating product: {ex.Message}');", true);
            }
        }

        protected void StockGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Exit edit mode and rebind the grid
            StockGridView.EditIndex = -1;
            BindProductsGrid(GetSupplierIdFromCookies());
        }

        protected void StockGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the product ID from DataKey
            int productId = Convert.ToInt32(StockGridView.DataKeys[e.RowIndex].Value);

            // Call the web service to delete the product
            string result = productService.DeleteProduct(productId);

            // Show success message using JavaScript alert
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{result}');", true);

            // Rebind the grid to reflect changes
            BindProductsGrid(GetSupplierIdFromCookies());
        }

        private bool IsValidInput()
        {
            // Validate input fields before adding a product
            if (string.IsNullOrWhiteSpace(ItemNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(DiscountTextBox.Text))
            {
                return false;
            }

            if (!int.TryParse(QuantityTextBox.Text, out _) ||
                !decimal.TryParse(PriceTextBox.Text, out _) ||
                !decimal.TryParse(DiscountTextBox.Text, out _))
            {
                return false;
            }

            return true;
        }

        private void ClearInputFields()
        {
            ItemNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
            DiscountTextBox.Text = "";
        }

        // Helper method to retrieve SupplierID from cookies
        private int GetSupplierIdFromCookies()
        {
            if (Request.Cookies["UserId"] != null)
            {
                return int.Parse(Request.Cookies["UserId"].Value);
            }

            return 0; // Default value if not found
        }
    }
}
