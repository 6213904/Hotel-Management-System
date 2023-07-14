using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;
using System.Windows.Navigation;


namespace Econolodge
{
    /// <summary>
    /// Interaction logic for ClientInfo.xaml
    /// </summary>
    public partial class ClientInfo : Window
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Urshil\Visual Studio\Econolodge\Hoteldb.mdf"";Integrated Security=True;Connect Timeout=30");
        
        public ClientInfo()
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
            string selectedCountry = (clientcountry.SelectedItem as ComboBoxItem)?.Content.ToString();
            SqlCommand cmd = new SqlCommand("insert into Client_tbl values(@ClientId, @ClientName, @ClientPhone, @ClientCountry)", Con);
            cmd.Parameters.AddWithValue("@ClientId", clientid.Text);
            cmd.Parameters.AddWithValue("@ClientName", clientname.Text);
            cmd.Parameters.AddWithValue("@ClientPhone", clientphone.Text);
            cmd.Parameters.AddWithValue("@ClientCountry", selectedCountry);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Client Successfully Added");
            Con.Close();
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             Con.Open();
            string Myquery = "select * from Client_tbl";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
            
            DataTable dt = new DataTable(); 
            da.Fill(dt);
            clientgrid.ItemsSource= dt.AsDataView();
            DataContext = da;
            Con.Close();
        }

        private void Editbtn_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string selectedCountry = (clientcountry.SelectedItem as ComboBoxItem)?.Content.ToString();
            string myquery = "UPDATE Client_tbl SET ClientName = @ClientName, ClientPhone = @ClientPhone, ClientCountry = @ClientCountry WHERE ClientId = @ClientId";
            SqlCommand cmd = new SqlCommand(myquery, Con);
            cmd.Parameters.AddWithValue("@ClientName", clientname.Text);
            cmd.Parameters.AddWithValue("@ClientPhone", clientphone.Text);
            cmd.Parameters.AddWithValue("@ClientCountry", selectedCountry);
            cmd.Parameters.AddWithValue("@ClientId", clientid.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Client Updated Successfully");
            Con.Close();
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string query = "delete from Client_tbl where ClientId = " + clientid.Text + "";
            SqlCommand cmd = new SqlCommand(query,Con);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Client Deleted");
            Con.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string Myquery = "select * from Client_tbl where ClientName = '"+ClientSearch.Text+"'";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            clientgrid.ItemsSource = dt.AsDataView();
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
