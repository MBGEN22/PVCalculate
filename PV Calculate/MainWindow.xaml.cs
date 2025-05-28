using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PV_Calculate
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Controls.Button _lastSelectedButton = null; 
        View. PV_Monthly pv= new View.PV_Monthly();
        public MainWindow()
        {
            InitializeComponent();
            // تعيين زر "Monthly" مفعّل عند البداية
            btnMonthly.Style = (System.Windows.Style)FindResource("menuButtonSelected"); 
            _lastSelectedButton = btnMonthly;

            MainContent.Content = new View.PV_Monthly();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            { 
                if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
            }
            catch
            {

            }

        }
        bool IsMAximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ClickCount == 2)
            //{
            //    if(IsMAximized == true)
            //    {
            //        this.WindowState = WindowState.Normal;
            //        this.Width = 1280;
            //        this.Height = 780;

            //        IsMAximized=false;
            //    }
            //    else
            //    {
            //        this.WindowState = WindowState.Maximized;  
            //        IsMAximized = true;
            //    }
            //}

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as System.Windows.Controls.Button;
            if (_lastSelectedButton != null)
                _lastSelectedButton.Style = (System.Windows.Style)FindResource("menuButton");
            clickedButton.Style = (System.Windows.Style)FindResource("menuButtonSelected");
            _lastSelectedButton = clickedButton;
            MainContent.Content = pv;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var clickedButton = sender as System.Windows.Controls.Button;
            if (_lastSelectedButton != null)
                _lastSelectedButton.Style = (System.Windows.Style)FindResource("menuButton");
            clickedButton.Style = (System.Windows.Style)FindResource("menuButtonSelected");
            _lastSelectedButton = clickedButton;
            MainContent.Content = new View.liste_téchnique();

        }

        private void maps(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as System.Windows.Controls.Button;
            if (_lastSelectedButton != null)
                _lastSelectedButton.Style = (System.Windows.Style)FindResource("menuButton");
            clickedButton.Style = (System.Windows.Style)FindResource("menuButtonSelected");
            _lastSelectedButton = clickedButton;
            MainContent.Content = new View.Maps();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            var clickedButton = sender as System.Windows.Controls.Button;
            if (_lastSelectedButton != null)
                _lastSelectedButton.Style = (System.Windows.Style)FindResource("menuButton");
            clickedButton.Style = (System.Windows.Style)FindResource("menuButtonSelected");
            _lastSelectedButton = clickedButton;
            MainContent.Content = new View.Caculate_PV();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir fermer le programme ?",
                     "Fermer le programme ?",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as System.Windows.Controls.Button;
            if (_lastSelectedButton != null)
                _lastSelectedButton.Style = (System.Windows.Style)FindResource("menuButton");
            clickedButton.Style = (System.Windows.Style)FindResource("menuButtonSelected");
            _lastSelectedButton = clickedButton;
            MainContent.Content = new View.calcule_DB();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as System.Windows.Controls.Button;
            if (_lastSelectedButton != null)
                _lastSelectedButton.Style = (System.Windows.Style)FindResource("menuButton");
            clickedButton.Style = (System.Windows.Style)FindResource("menuButtonSelected");
            _lastSelectedButton = clickedButton;
            MainContent.Content = new View.liste_centrale();
        }
    }
}
