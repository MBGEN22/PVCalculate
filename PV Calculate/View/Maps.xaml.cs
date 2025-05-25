using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms; 
using GMap.NET.WindowsPresentation;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
namespace PV_Calculate.View
{
    /// <summary>
    /// Logique d'interaction pour Maps.xaml
    /// </summary>
    public partial class Maps : UserControl
    {
        DAL.DAL daoo; 
        BL.BL_placement_PV class_placement = new BL.BL_placement_PV();
        private List<GMap.NET.WindowsPresentation.GMapMarker> markersList = new List<GMap.NET.WindowsPresentation.GMapMarker>();
        public Maps()
        {
            InitializeComponent();
            MainMap.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            MainMap.Position = new PointLatLng(36.75, 3.06); // الجزائر العاصمة
            MainMap.MinZoom = 2;
            MainMap.MaxZoom = 18;
            MainMap.Zoom = 5;

            MainMap.MouseLeftButtonDown += MainMap_MouseLeftButtonDown;
        }
        

        private void MainMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(MainMap);
            PointLatLng latlng = MainMap.FromLocalToLatLng((int)point.X, (int)point.Y);
            txtLat.Text = latlng.Lat.ToString("F4");
            txtLon.Text = latlng.Lng.ToString("F4");

             
        }

        private void BtnGetData_Click(object sender, object e)
        {
        }

        private void BtnGetData_Click(object sender, RoutedEventArgs e)
        {
            

        }
        public string Latitude
        {
            get => txtLat.Text;
            set => txtLat.Text = value;
        }

        public string Longitude
        {
            get => txtLon.Text;
            set => txtLon.Text = value;
        }

        private void BtnGetDa_Click(object sender, RoutedEventArgs e)
        {
            View.PV_Monthly pv = new View.PV_Monthly();
            MainWindow main = new MainWindow();
            // تمرير القيم باستعمال الـ Properties
            pv.Latitude = Latitude;
            pv.Longitude = Longitude;
            ((MainWindow)Application.Current.MainWindow).MainContent.Content = pv; 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainMap.MapProvider = GMapProviders.GoogleMap;
            MainMap.Position = new PointLatLng(36.75, 3.06); // الجزائر العاصمة
            MainMap.MinZoom = 2;
            MainMap.MaxZoom = 20;
            MainMap.Zoom = 8;
            MainMap.ShowCenter = false;
            MainMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            MainMap.CanDragMap = true;
            MainMap.DragButton = MouseButton.Left; 
            ChargerCentralesDepuisDB();

        }
        void ChargerCentralesDepuisDB()
        {
            daoo = new DAL.DAL();

            using (daoo.sqlConnection)
            {
                daoo.sqlConnection.Open();
                string query = "SELECT * FROM TB_PLACEMENT_CENTRALE_PV";
                SqlCommand cmd = new SqlCommand(query, daoo.sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var position = new PointLatLng(
                        Convert.ToDouble(reader["Latitude"]),
                        Convert.ToDouble(reader["Longitude"])
                    );

                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri("C:\\Users\\Informatics\\Desktop\\PV Calculate\\PV Calculate\\View\\cellule-photovoltaique.png")),
                        Width = 32,
                        Height = 32,
                        ToolTip = reader["NAME"].ToString(),
                        Tag = position  // تخزين الإحداثيات في Tag
                    };

                    GMap.NET.WindowsPresentation.GMapMarker marker = null; // لازم يكون خارج الحدث حتى تعرفه فيه

                    image.MouseLeftButtonDown += (s, ev) =>
                    {
                        if (MessageBox.Show("Supprimer cette centrale ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            var pos = (PointLatLng)((Image)s).Tag;
                            bool deleted = class_placement.DELETE_PLACEMENT_BY_COORDS(pos.Lat, pos.Lng);
                            if (deleted)
                            {
                                MainMap.Markers.Remove(marker);
                                markersList.Remove(marker);
                            }
                        }
                    };

                    marker = new GMap.NET.WindowsPresentation.GMapMarker(position)
                    {
                        Shape = image,
                        Offset = new Point(-16, -16)
                    };

                    MainMap.Markers.Add(marker);
                    markersList.Add(marker);
                }
            }
        }

        //void ChargerCentralesDepuisDB()
        //{ 

        //    daoo = new DAL.DAL(); 

        //    using (daoo.sqlConnection)
        //    {
        //        daoo.sqlConnection.Open();
        //        string query = "SELECT * FROM TB_PLACEMENT_CENTRALE_PV";
        //        SqlCommand cmd = new SqlCommand(query, daoo.sqlConnection);
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            var position = new PointLatLng(
        //                Convert.ToDouble(reader["Latitude"]),
        //                Convert.ToDouble(reader["Longitude"])
        //            );

        //            var image = new Image
        //            {
        //                Source = new BitmapImage(new Uri("C:\\Users\\Informatics\\Desktop\\PV Calculate\\PV Calculate\\View\\cellule-photovoltaique.png")),
        //                Width = 32,
        //                Height = 32,
        //                ToolTip = reader["NAME"].ToString()
        //            };

        //            var marker = new GMap.NET.WindowsPresentation.GMapMarker(position)
        //            {
        //                Shape = image,
        //                Offset = new Point(-16, -16)
        //            };

        //            MainMap.Markers.Add(marker);
        //            markersList.Add(marker);
        //        }
        //    }
        //}

        private void gMapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // نتحقق إذا النقرة فوق ماركر أو لا
            var source = e.OriginalSource as DependencyObject;
            while (source != null && !(source is Image))
                source = VisualTreeHelper.GetParent(source);

            // إذا كانت فوق Image (يعني ماركر) => نوقف
            if (source is Image)
                return;

            // هنا فقط إذا النقرة ما كانتش فوق ماركر => نضيف ماركر جديد
            var mousePos = e.GetPosition(MainMap);
            PointLatLng position = MainMap.FromLocalToLatLng((int)mousePos.X, (int)mousePos.Y);

            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:\\Users\\Informatics\\Desktop\\PV Calculate\\PV Calculate\\View\\cellule-photovoltaique.png")),
                Width = 32,
                Height = 32,
                ToolTip = $"PV @ {position.Lat:F4}, {position.Lng:F4}",
                Tag = position
            };

            GMap.NET.WindowsPresentation.GMapMarker marker = null;
            image.MouseLeftButtonDown += (s, ev) =>
            {
                if (MessageBox.Show("Supprimer cette centrale ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var pos = (PointLatLng)((Image)s).Tag;
                    bool deleted = class_placement.DELETE_PLACEMENT_BY_COORDS(pos.Lat, pos.Lng);
                    if (deleted)
                    {
                        MainMap.Markers.Remove(marker);
                        markersList.Remove(marker);
                    }
                }
            };

            marker = new GMap.NET.WindowsPresentation.GMapMarker(position)
            {
                Shape = image,
                Offset = new Point(-16, -16)
            };

            MainMap.Markers.Add(marker);
            markersList.Add(marker);

            class_placement.INSERT_PLACEMENT("Centrale PV ", float.Parse(position.Lat.ToString()), float.Parse(position.Lng.ToString()));

            // INSERT TO DATABASE IF NEEDED

            //var mousePos = e.GetPosition(MainMap);
            //PointLatLng position = MainMap.FromLocalToLatLng((int)mousePos.X, (int)mousePos.Y);

            //// إنشاء صورة الماركر
            //var image = new Image
            //{
            //    Source = new BitmapImage(new Uri("C:\\Users\\Informatics\\Desktop\\PV Calculate\\PV Calculate\\View\\cellule-photovoltaique.png")),
            //    Width = 32,
            //    Height = 32,
            //    ToolTip = $"PV @ {position.Lat:F4}, {position.Lng:F4}"
            //};

            //// حدث حذف عند النقر
            //GMap.NET.WindowsPresentation.GMapMarker marker = null;
            //image.MouseLeftButtonDown += (s, ev) =>
            //{
            //    if (MessageBox.Show("Souhaitez-vous vraiment supprimer cette centrale ?", "Confirmation" +
            //        "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //    {
            //        class_placement.DELETE_PLACEMENT_BY_COORDS(marker.Position.Lat, marker.Position.Lng); 
            //        MainMap.Markers.Remove(marker);
            //        markersList.Remove(marker);
            //    }
            //};

            //// إنشاء الماركر
            //marker = new GMap.NET.WindowsPresentation.GMapMarker(position)
            //{
            //    Shape = image,
            //    Offset = new Point(-16, -16)
            //};

            //// إضافته
            //MainMap.Markers.Add(marker);
            //markersList.Add(marker);
            //class_placement.INSERT_PLACEMENT("Centrale PV ",float.Parse(position.Lat.ToString()), float.Parse(position.Lng.ToString()));
        }
    }
}
