using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM2Demo
{
    public partial class FrmUser : Form
    {
        int selectedUserId;
        public FrmUser()
        {
            InitializeComponent();
            string conString = "Data Source=rs\\sqlstudents;Initial Catalog=POSDB;Integrated Security=True";

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {

            if (btnAddUser.Text == "Add")
            {
                enableTextboxes();
                btnClear.Enabled = true;
                btnAddUser.Text = "Save";
            }
            else if (btnAddUser.Text == "Save")
            {

                string firstName = txtFirst.Text;
                string lastName = txtLast.Text;
                string position = cbPosition.Text;

                if (firstName != string.Empty && lastName != string.Empty)
                {
                    string tempUsername = firstName.ToLower() + "." + lastName.ToLower();
                    string tempPassword = position + firstName.Substring(0, 1) + lastName.Substring(0, 1);


                    insertUser(tempUsername, tempPassword, firstName, lastName, position);
                    showUser();
                    resetForm();
                    clearTextboxes();
                }
                else
                {
                    MessageBox.Show("Fields cannot be empty.");
                }

            }
        }

        private bool validateInputs(string input1, string input2)
        {
            if (input1 == null || input2 == null)
            {
                return false;
            }
            else return true;
        }

        private void clearTextboxes()
        {
            txtFirst.Clear();
            txtLast.Clear();
            cbPosition.SelectedIndex = 0;
        }
        private void resetForm()
        {
            btnAddUser.Enabled = true;
            btnShow.Enabled = true;
            btnClear.Enabled = false;
            btnEditUser.Enabled = false;

            txtFirst.Enabled = false;
            txtLast.Enabled = false;
            cbPosition.Enabled = false;
        }
        private void enableTextboxes()
        {
            txtFirst.Enabled = true;
            txtLast.Enabled = true;
            cbPosition.Enabled = true;
        }

        private void disableTextboxes()
        {
            txtFirst.Enabled = false;
            txtLast.Enabled = false;
            cbPosition.Enabled = false;
        }

        private void insertUser(string username, string pass, string firstName, string lastName, string position)
        {


            string conString = "Data Source=rs\\sqlstudents;Initial Catalog=POSDB;Integrated Security=True";
            string insertQuery = "INSERT INTO tblUser (username, password, firstName, lastName, position, firstLogin ) VALUES(@username, @pass, @firstName, @lastName, @position, 1)";

            SqlConnection conn = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@position", position);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 1)
                MessageBox.Show("Insert successful.");
            btnAddUser.Text = "Add";

        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            showUser();
        }

        public void showUser()
        {
            //show the list of users  to datagrid

            string conString = "Data Source=rs\\sqlstudents;Initial Catalog=POSDB;Integrated Security=True";
            string query = "select username, password, firstName, lastName, position from tblUser order by userId";

            SqlConnection conn = new SqlConnection(conString);
            conn.Open();
            SqlDataAdapter sqladapter = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            sqladapter.Fill(dt);

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dt;
            dgUser.DataSource = dt; //lalagyan ng laman si datagrid (Name) dgUser
        }

        public void showUserDetails()
        {
            //show the list of users  to datagrid

            string conString = "Data Source=rs\\sqlstudents;Initial Catalog=POSDB;Integrated Security=True";
            string query = "select firstName, lastName, position from tblUser order by userId";

            SqlConnection conn = new SqlConnection(conString);
            conn.Open();
            SqlDataAdapter sqladapter = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            sqladapter.Fill(dt);

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dt;
            dgUser.DataSource = dt; //lalagyan ng laman si datagrid (Name) dgUser
        }

        private void FrmUser_Load(object sender, EventArgs e)
        {
            resetForm();
            btnShow.Hide();
            showUser();
        }

        private void dgUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnEditUser.Text == "Edit")
            {
                btnAddUser.Enabled = false;
                btnEditUser.Enabled = true;
                enableTextboxes();
                btnEditUser.Text = "Update";

                if (e.RowIndex >= 0)
                {
                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dgUser.Rows[e.RowIndex];
                    //populate the textbox from specific value of the coordinates of column and row.
                    txtFirst.Text = row.Cells[2].Value.ToString();
                    txtLast.Text = row.Cells[3].Value.ToString();
                    string currentPosition = row.Cells[4].Value.ToString();
                    int positionIndex;
                    if (currentPosition == "Admin")
                    {
                        positionIndex = 0;
                    }
                    else
                    {
                        positionIndex = 1;
                    }
                    cbPosition.SelectedIndex = positionIndex;
                }

                selectedUserId = e.RowIndex + 1;
                Console.WriteLine(selectedUserId);

            }



        }

        private void updateUser(string firstName, string lastName, string position)
        {
            string conString = "Data Source=rs\\sqlstudents;Initial Catalog=POSDB;Integrated Security=True";
            string updateQuery = "UPDATE tblUser SET firstName = @firstName, lastName = @lastName, position = @position WHERE userId = @selectedUserId";

            SqlConnection conn = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(updateQuery, conn);

            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@position", position);
            cmd.Parameters.AddWithValue("@selectedUserId", selectedUserId);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected == 1)
                MessageBox.Show("Update successful.");
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (btnEditUser.Text == "Edit")
            {
                btnAddUser.Enabled = false;
                btnEditUser.Enabled = true;
                btnClear.Enabled = true;
                enableTextboxes();
                btnEditUser.Text = "Update";
            }
            else if (btnEditUser.Text == "Update")
            {
                string firstName = txtFirst.Text;
                string lastName = txtLast.Text;
                string position = cbPosition.Text;

                updateUser(firstName, lastName, position);

                showUser();
                resetForm();
                clearTextboxes();
                btnEditUser.Text = "Edit";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearTextboxes();

        }

        private void dgUser_Click(object sender, EventArgs e)
        {

        }
    }
}
