using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Econolodge
{
    /// <summary>
    /// Interaction logic for Staffinfo.xaml
    /// </summary>
    public partial class Staffinfo : Window
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Urshil\Visual Studio\Econolodge\Hoteldb.mdf"";Integrated Security=True;Connect Timeout=30");
        public Staffinfo()
        {
            InitializeComponent();
            DisplayCurrentDate();
        }
        private void DisplayCurrentDate()
        {
            DateTime currentDate = DateTime.Today;
            string formattedDate = currentDate.ToString("MMMM dd, yyyy");
            dateText.Text = formattedDate;
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {

            Con.Open();
            string selectedGender = (Gender.SelectedItem as ComboBoxItem)?.Content.ToString();
            SqlCommand cmd = new SqlCommand("INSERT INTO Staff_tbl VALUES (@StaffId, @StaffName, @StaffPhone, @Gender, @StaffPassword)", Con);
            cmd.Parameters.AddWithValue("@StaffId", staffid.Text);
            cmd.Parameters.AddWithValue("@StaffName", staffname.Text);
            cmd.Parameters.AddWithValue("@StaffPhone", staffphone.Text);
            cmd.Parameters.AddWithValue("@Gender", selectedGender);
            cmd.Parameters.AddWithValue("@StaffPassword", staffpassword.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Staff Successfully Added");
            Con.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string Myquery = "select * from Staff_tbl";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            staffgrid.ItemsSource = dt.AsDataView();
            DataContext = da;
            Con.Close();
        }

        private void StaffEditbtn_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string selectedGender = (Gender.SelectedItem as ComboBoxItem)?.Content.ToString();
            string myquery = "UPDATE Staff_tbl SET StaffName = @StaffName, StaffPhone = @StaffPhone, Gender = @Gender, StaffPassword = @StaffPassword WHERE StaffId = @StaffId";
            SqlCommand cmd = new SqlCommand(myquery, Con);
            cmd.Parameters.AddWithValue("@StaffName", staffname.Text);
            cmd.Parameters.AddWithValue("@StaffPhone", staffphone.Text);
            cmd.Parameters.AddWithValue("@Gender", selectedGender);
            cmd.Parameters.AddWithValue("@StaffPassword", staffpassword.Text);
            cmd.Parameters.AddWithValue("@StaffId", staffid.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Staff Updated Successfully");
            Con.Close();
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string query = "delete from Staff_tbl where StaffId = " + staffid.Text + "";
            SqlCommand cmd = new SqlCommand(query, Con);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Staff Deleted");
            Con.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string Myquery = "select * from Staff_tbl where StaffName = '" + Staffsearch.Text + "'";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            staffgrid.ItemsSource = dt.AsDataView();
            DataContext = da;
            Con.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Home h = new Home();
            h.Show();
            this.Close();
        }
    }
}
