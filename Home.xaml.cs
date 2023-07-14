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

namespace Econolodge
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Hide();
        }

        private void clientinfo_Click(object sender, RoutedEventArgs e)
        {
            ClientInfo ci = new ClientInfo();
            ci.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Staffinfo si = new Staffinfo();
            si.Show();
            this.Hide();
        }

        private void Roominfo_Click(object sender, RoutedEventArgs e)
        {
            Roominfo ri = new Roominfo();
            ri.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Reservationinfo resi = new Reservationinfo();
            resi.Show();
            this.Hide();
        }
    }
}
