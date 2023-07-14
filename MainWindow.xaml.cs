using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Net.Http;



namespace Econolodge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Urshil\Visual Studio\Econolodge\Hoteldb.mdf"";Integrated Security=True;Connect Timeout=30");
        public MainWindow()
        {
            InitializeComponent();
        }


        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                // Replace "http://localhost:port/api/login" with the actual URL of your Web API
                string apiUrl = "https://localhost:7116/api/login";
                var parameters = new Dictionary<string, string>
        {
            { "username", textUsername.Text },
            { "password", textPassword.Password }
        };

                var response = await client.PostAsync(apiUrl, new FormUrlEncodedContent(parameters));

                if (response.IsSuccessStatusCode)
                {
                    // Authentication successful
                    Home hm = new Home();
                    hm.Show();
                    this.Hide();
                }
                else
                {
                    // Authentication failed
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(errorMessage);
                }
            }
        }



    }
}
