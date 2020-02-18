using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookStoreCrud
{
    public partial class Form1 : Form
    {
        string connectionString = @"Server=localhost;Database=book_store;Uid=root;Pwd=root;";

        int bookID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            GridFill();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mySqlCon = new MySqlConnection(connectionString))
            {
                mySqlCon.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("BookAddOrEdit", mySqlCon);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_BookID", bookID);
                mySqlCommand.Parameters.AddWithValue("_BookName", txtBookName.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("_Author", txtAuthor.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("_Description", txtDescription.Text.Trim());
                mySqlCommand.ExecuteNonQuery();
                MessageBox.Show("Submitted Successfully");
                Clear();
                GridFill();
           
            }

        }

        void GridFill()
        {
            using (MySqlConnection mySqlCon = new MySqlConnection(connectionString))
            {
                mySqlCon.Open();

                MySqlDataAdapter sqlData = new MySqlDataAdapter("BookViewAll", mySqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dataTableBook = new DataTable();
                sqlData.Fill(dataTableBook);
                dgvBook.DataSource = dataTableBook;
                dgvBook.Columns[0].Visible = false;
            }
        }


        void Clear()
        {
            txtBookName.Text = txtAuthor.Text = txtDescription.Text = txtSearch.Text = "";
            bookID = 0;
            btnSave.Text = "save";
            btnDelete.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            using(MySqlConnection mySqlConn = new MySqlConnection(connectionString))
            {
                mySqlConn.Open();
                MySqlCommand sqlCommand = new MySqlCommand("BookDeleteById", mySqlConn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_BookID", bookID);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Delete Successfully");
                Clear();
                GridFill();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void dgvBook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvBook_DoubleClick(object sender, EventArgs e)
        {
            if(dgvBook.CurrentRow.Index != -1)
            {
                txtBookName.Text = dgvBook.CurrentRow.Cells[1].Value.ToString();
                txtAuthor.Text = dgvBook.CurrentRow.Cells[2].Value.ToString();
                txtDescription.Text = dgvBook.CurrentRow.Cells[3].Value.ToString();
                bookID = Convert.ToInt32(dgvBook.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "Update";
                btnDelete.Enabled = Enabled;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mySqlConn = new MySqlConnection(connectionString))
            {
                mySqlConn.Open();

                MySqlDataAdapter sqlData = new MySqlDataAdapter("BookSearchByValue", mySqlConn);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("_SearchValue", txtSearch.Text);
                DataTable dataTableBook = new DataTable();
                sqlData.Fill(dataTableBook);
                dgvBook.DataSource = dataTableBook;
                dgvBook.Columns[0].Visible = false;

            }
        }
    }
}
