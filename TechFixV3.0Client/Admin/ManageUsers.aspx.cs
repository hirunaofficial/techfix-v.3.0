using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.UserServiceReference; // Ensure the service reference is correct

namespace TechFixV3._0Client.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        private UserServiceSoapClient userService = new UserServiceSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUserGrid();
            }
        }

        private void BindUserGrid()
        {
            // Fetch users and bind to the GridView
            var users = userService.GetUsers();
            UsersGridView.DataSource = users;
            UsersGridView.DataBind();
        }

        protected void AddUserButton_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (IsValidInput())
            {
                string username = UsernameTextBox.Text;
                string password = PasswordTextBox.Text;
                string role = RoleDropDown.SelectedValue; // Role can be Admin or Supplier
                string location = LocationTextBox.Text;
                string contact = ContactTextBox.Text;
                string email = EmailTextBox.Text;

                // Call the web service to add the user
                string result = userService.AddUser(username, password, role, username, location, contact, email);
                // Display alert with the result message
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

                // Refresh the GridView
                BindUserGrid();
            }
        }

        protected void UsersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set GridView to Edit Mode
            UsersGridView.EditIndex = e.NewEditIndex;
            BindUserGrid();
        }

        protected void UsersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel Edit Mode
            UsersGridView.EditIndex = -1;
            BindUserGrid();
        }

        protected void UsersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Retrieve the User ID from DataKey
            int userId = (int)UsersGridView.DataKeys[e.RowIndex].Value;

            // Retrieve the updated values from the GridView
            GridViewRow row = UsersGridView.Rows[e.RowIndex];

            // Correctly cast controls from the row
            string username = ((TextBox)row.FindControl("UsernameTextBox")).Text;
            string password = ((TextBox)row.FindControl("PasswordTextBox")).Text; // Allow password editing
            string role = ((DropDownList)row.FindControl("RoleDropDown")).SelectedValue; // Role must be Admin or Supplier
            string location = ((TextBox)row.FindControl("LocationTextBox")).Text;
            string contact = ((TextBox)row.FindControl("ContactTextBox")).Text;
            string email = ((TextBox)row.FindControl("EmailTextBox")).Text;

            // Validate role
            if (role != "Admin" && role != "Supplier")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid role selected.');", true);
                return;
            }

            // Update the user via the web service
            string result = userService.UpdateUser(userId, username, password, role, username, location, contact, email);
            // Display alert with the result message
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

            // Set GridView back to normal mode
            UsersGridView.EditIndex = -1;
            BindUserGrid(); // Refresh the GridView
        }

        protected void UsersGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Retrieve the User ID from DataKey
            int userId = (int)UsersGridView.DataKeys[e.RowIndex].Value;
            // Delete the user via the web service
            string result = userService.DeleteUser(userId);
            // Display alert with the result message
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

            // Refresh the GridView after deletion
            BindUserGrid();
        }

        private bool IsValidInput()
        {
            // Check if all required fields are filled out
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                string.IsNullOrWhiteSpace(LocationTextBox.Text) ||
                string.IsNullOrWhiteSpace(ContactTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill in all fields.');", true);
                return false;
            }

            // Validate email format
            if (!System.Text.RegularExpressions.Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid email format.');", true);
                return false;
            }

            return true;
        }
    }
}
