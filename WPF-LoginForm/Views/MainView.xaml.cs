using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Input;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Application = System.Windows.Forms.Application;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB; Database=MVVMLoginDb; Integrated Security=true");

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM [UserClient]", connection);
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            connection.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }
        public void ClearData()
        {
            FirstNametxt.Text = "";
            LastNametxt.Text = "";
            Descriptiontxt.Text = "";
            CheckUptxt.Text = "";
            Pricetxt.Text = "";
            Idtxt.Text = "";
        }

        private void Price_KeyPress(object sender, KeyEventArgs e)
        {
            string[] nums = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-" };
            if (!nums.Contains(e.KeyData.ToString()))
                e.Handled = true;
        }
        public bool IsValid()
        {
            if (FirstNametxt.Text == string.Empty)
            {
                MessageBox.Show("First Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (LastNametxt.Text == string.Empty)
            {
                MessageBox.Show("Last Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Descriptiontxt.Text == string.Empty)
            {
                MessageBox.Show("Description is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (CheckUptxt.Text == String.Empty)
            {
                MessageBox.Show("CheckUp is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Pricetxt.Text == String.Empty)
            {
                int.Parse(Pricetxt.Text);
            }
            return true;
        }
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (IsValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO [UserClient] VALUES (@FirstName, @LastName, @Description, @CheckUp, @Price)", connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@FirstName", FirstNametxt.Text);
                    cmd.Parameters.AddWithValue("@LastName", LastNametxt.Text);
                    cmd.Parameters.AddWithValue("@Description", Descriptiontxt.Text);
                    cmd.Parameters.AddWithValue("@CheckUp", CheckUptxt.Text);
                    cmd.Parameters.AddWithValue("@Price", Pricetxt.Text);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully Created!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public bool idFilled()
        {
            if (Idtxt.Text == String.Empty)
            {
                MessageBox.Show("Id is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (idFilled())
            {
            connection.Open();
            SqlCommand cmd = new SqlCommand("delete from [UserClient] where Id=" + Idtxt.Text + "", connection);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully");
                connection.Close();
                ClearData();
                LoadGrid();
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (idFilled())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("update [UserClient] set FirstName='" + FirstNametxt.Text + "',LastName='" + LastNametxt.Text + "',Description='" + Descriptiontxt.Text + "',CheckUp='" + CheckUptxt.Text + "',Price='" + Pricetxt.Text + "' where Id=" + Idtxt.Text + "", connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Update Successfully");
                    connection.Close();
                    ClearData();
                    LoadGrid();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
