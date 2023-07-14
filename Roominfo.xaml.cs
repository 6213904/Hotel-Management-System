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
    /// Interaction logic for Roominfo.xaml
    /// </summary>
    public partial class Roominfo : Window
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Urshil\Visual Studio\Econolodge\Hoteldb.mdf"";Integrated Security=True;Connect Timeout=30");

        public Roominfo()
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
        private void addroombtn_Click(object sender, RoutedEventArgs e)
        {
            string isfree;
            if (Yesradio != null && Yesradio.IsChecked == true)
                isfree = "free";
            else 
                isfree = "busy";

            Con.Open();
            SqlCommand cmd = new SqlCommand("insert into Room_tbl values('" + roomnum.Text + "','" + roomphone.Text + "','" + isfree + "') ", Con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Room Successfully Added");
            Con.Close();
        }

        private void RoomEditbtn_Click(object sender, RoutedEventArgs e)
        {
            string isfree;
            if (Yesradio != null && Yesradio.IsChecked == true)
                isfree = "free";
            else
                isfree = "busy";
            Con.Open();
            string myquery = "UPDATE Room_tbl set RoomPhone ='" + roomphone.Text + "',RoomFree ='" + isfree + "' where RoomId = " + roomnum.Text + ";";
            SqlCommand cmd = new SqlCommand(myquery, Con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Room Updated Successfully");
            Con.Close();
        }

        private void RoomDeletebtn_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string query = "delete from Room_tbl where RoomId = " + roomnum.Text + "";
            SqlCommand cmd = new SqlCommand(query, Con);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Room Deleted");
            Con.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string Myquery = "select * from Room_tbl where RoomId = '" + Roomsearch.Text + "'";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            roomgrid.ItemsSource = dt.AsDataView();
            DataContext = da;
            Con.Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string Myquery = "select * from Room_tbl";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            roomgrid.ItemsSource = dt.AsDataView();
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
