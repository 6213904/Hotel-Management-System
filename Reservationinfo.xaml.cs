using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Reservationinfo.xaml
    /// </summary>
    public partial class Reservationinfo : Window
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Urshil\Visual Studio\Econolodge\Hoteldb.mdf"";Integrated Security=True;Connect Timeout=30");

        public Reservationinfo()
        {
            InitializeComponent();
            DisplayCurrentDate();
            FillRoomCombo();
            FillClientCombo();
        }

        private DateTime? datein;
        private DateTime? dateout;

        private void DisplayCurrentDate()
        {
            DateTime currentDate = DateTime.Today;
            string formattedDate = currentDate.ToString("MMMM dd, yyyy");
            dateText.Text = formattedDate;
        }

        public void FillRoomCombo()
        {
            Con.Open();
            string roomstate = "free";
            SqlCommand cmd = new SqlCommand("SELECT RoomId FROM Room_tbl WHERE RoomFree = @state", Con);
            cmd.Parameters.AddWithValue("@state", roomstate);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            roomnum.DisplayMemberPath = "RoomId";
            roomnum.SelectedValuePath = "RoomId";
            roomnum.ItemsSource = dt.DefaultView;
            Con.Close();
        }

        public void FillClientCombo()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT ClientName FROM Client_tbl", Con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("ClientName", typeof(string));
            dt.Load(dr);
            clientid.DisplayMemberPath = "ClientName";
            clientid.SelectedValuePath = "ClientName";
            clientid.ItemsSource = dt.DefaultView;
            Con.Close();
        }

        private void Reservationinfo_Load(object sender, EventArgs e)
        {
            datein = Datein.SelectedDate;
            dateout = Dateout.SelectedDate;
        }

        private void Datein_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Datein.SelectedDate.HasValue)
            {
                datein = Datein.SelectedDate;
                CheckReservationDates();
            }
        }

        private void Dateout_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Dateout.SelectedDate.HasValue)
            {
                dateout = Dateout.SelectedDate;
                CheckReservationDates();
            }
        }

        private void CheckReservationDates()
        {
            if (datein.HasValue && dateout.HasValue)
            {
                int res = DateTime.Compare(datein.Value, dateout.Value);
                if (res >= 0)
                {
                    MessageBox.Show("Invalid date range. Date Out should be after Date In.");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string Myquery = "SELECT * FROM Reservation_tbl WHERE ResId = '" + Reservationsearch.Text + "'";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            reservationgrid.ItemsSource = dt.AsDataView();
            DataContext = da;
            Con.Close();
        }

        public void updatesroomstate()
        {
            Con.Open();
            string newstate = "busy";
            string myquery = "UPDATE Room_tbl SET RoomFree = '" + newstate + "' WHERE RoomId = " + Convert.ToInt32(roomnum.SelectedValue.ToString()) + ";";
            SqlCommand cmd = new SqlCommand(myquery, Con);
            cmd.ExecuteNonQuery();
            Con.Close();
            FillRoomCombo();
        }

        private void updatesRoomStateAfterDeletion()
        {
            if (roomnum.SelectedValue != null)
            {
                Con.Open();
                string newstate = "free";
                string roomId = roomnum.SelectedValue.ToString();
                string myquery = "UPDATE Room_tbl SET RoomFree = @state WHERE RoomId = @roomId";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.Parameters.AddWithValue("@state", newstate);
                cmd.Parameters.AddWithValue("@roomId", roomId);
                cmd.ExecuteNonQuery();
                Con.Close();
                FillRoomCombo();
            }
        }

        private void addroombtn_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Reservation_tbl VALUES ('" + reservationid.Text + "','" + clientid.SelectedValue.ToString() + "','" + roomnum.SelectedValue.ToString() + "','" + Datein.SelectedDate + "','" + Dateout.SelectedDate + "') ", Con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Reservation Successfully Added");
            Con.Close();
            updatesroomstate();
        }

        private void StaffEditbtn_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string myquery = "UPDATE Reservation_tbl SET Client = '" + clientid.SelectedValue.ToString() + "', Room = '" + roomnum.SelectedValue.ToString() + "', DateIn = '" + Datein.SelectedDate + "', DateOut = '" + Dateout.SelectedDate + "' WHERE ResId = " + reservationid.Text + ";";
            SqlCommand cmd = new SqlCommand(myquery, Con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Reservation Updated Successfully");
            Con.Close();
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            if (reservationid.Text != "")
            {
                Con.Open();
                string roomId = string.Empty;
                string query = "SELECT Room FROM Reservation_tbl WHERE ResId = @resId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@resId", reservationid.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    roomId = reader["Room"].ToString();
                }
                reader.Close();

                query = "DELETE FROM Reservation_tbl WHERE ResId = @resId";
                cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@resId", reservationid.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Reservation Deleted");
                Con.Close();

                if (!string.IsNullOrEmpty(roomId))
                {
                    Con.Open();
                    query = "UPDATE Room_tbl SET RoomFree = 'free' WHERE RoomId = @roomId";
                    cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    cmd.ExecuteNonQuery();
                    Con.Close();
                }

                FillRoomCombo();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Con.Open();
            string Myquery = "SELECT * FROM Reservation_tbl";
            SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            reservationgrid.ItemsSource = dt.AsDataView();
            DataContext = da;
            Con.Close();
        }

        private void reservationgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reservationgrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)reservationgrid.SelectedItem;
                reservationid.Text = row["ResId"].ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Home h = new Home();
            h.Show();
            this.Close();
        }

        private void roomnum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
