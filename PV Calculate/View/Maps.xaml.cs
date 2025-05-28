using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms; 
using GMap.NET.WindowsPresentation;
using MaterialDesignThemes.Wpf;
using PV_Calculate.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
        private System.Timers.Timer searchTimer;
        DAL.DAL daoo; 
        BL.BL_placement_PV class_placement = new BL.BL_placement_PV();
        BL.BL_Combo BL_Comboox = new BL.BL_Combo();
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

            cmb_technique.ItemsSource =    BL_Comboox.cmb_technique().DefaultView; 
            cmb_technique.DisplayMemberPath = "ParametreId";
            cmb_technique.SelectedValuePath = "ParametreId"; // تأكد من أن العمود ID موجود في DataTable


            cmb_bus.ItemsSource = BL_Comboox.cmb_technique().DefaultView;
            cmb_bus.DisplayMemberPath = "CODE";
            cmb_bus.SelectedValuePath = "CODE"; // تأكد من أن العمود ID موجود في DataTable
        }
        

        private void MainMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(MainMap);
            PointLatLng latlng = MainMap.FromLocalToLatLng((int)point.X, (int)point.Y);
            //txtLat.Text = latlng.Lat.ToString("F4");
            //txtLon.Text = latlng.Lng.ToString("F4");

             
        }

        private void BtnGetData_Click(object sender, object e)
        {
        }

        private void BtnGetData_Click(object sender, RoutedEventArgs e)
        {
            

        }
        //public string Latitude
        //{
        //    get => txtLat.Text;
        //    set => txtLat.Text = value;
        //}

        //public string Longitude
        //{
        //    get => txtLon.Text;
        //    set => txtLon.Text = value;
        //}

        private void BtnGetDa_Click(object sender, RoutedEventArgs e)
        {
            //View.PV_Monthly pv = new View.PV_Monthly();
            //MainWindow main = new MainWindow();
            //// تمرير القيم باستعمال الـ Properties
            //pv.Latitude = Latitude;
            //pv.Longitude = Longitude;
            //((MainWindow)Application.Current.MainWindow).MainContent.Content = pv; 
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
            cmb_technique.SelectedIndex = 0; // تعيين أول عنصر كمحدد افتراضي

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
                        Source = new BitmapImage(new Uri("C:\\Users\\ARC COMPUTER\\Source\\Repos\\PVCalculate\\PV Calculate\\View\\cellule-photovoltaique.png")),
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

        private async void gMapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txt_puiisance.Text == "00" || txt_name.Text== "")
            {
                MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter une centrale.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                // نتحقق إذا النقرة فوق ماركر أو لا
                var source = e.OriginalSource as DependencyObject;
                while (source != null && !(source is Image))
                    source = VisualTreeHelper.GetParent(source);

                if (source is Image)
                    return;

                // إذا النقرة ما كانتش على ماركر
                var mousePos = e.GetPosition(MainMap);
                PointLatLng position = MainMap.FromLocalToLatLng((int)mousePos.X, (int)mousePos.Y);

                // 🟡 نجيب اسم المكان من الإحداثيات
                string placeName = await GetPlaceNameFromCoordinates(position.Lat, position.Lng);
                if(txt_name.Text == "Centrale PV :")
                { 
                    txt_name.Text = "Centrale PV :" + placeName ?? "Inconnu";
                } 

                // 🔵 نرسم الماركر
                var image = new Image
                {
                    Source = new BitmapImage(new Uri("C:\\Users\\ARC COMPUTER\\Source\\Repos\\PVCalculate\\PV Calculate\\View\\cellule-photovoltaique.png")),
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

                // 🔴 نضيف للقاعدة
                class_placement.INSERT_PLACEMENT(
                    txt_name.Text,
                    float.Parse(position.Lat.ToString()),
                    float.Parse(position.Lng.ToString()),
                    float.Parse(txt_puiisance.Text),
                    cmb_technique.SelectedValue != null ? (int?)Convert.ToInt32(cmb_technique.SelectedValue) : null,
                    cmb_bus.SelectedValue != null ? cmb_bus.SelectedValue.ToString() : null
                );
            }


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
        private async Task<string> GetPlaceNameFromCoordinates(double lat, double lng)
        {
            string url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat.ToString(CultureInfo.InvariantCulture)}&lon={lng.ToString(CultureInfo.InvariantCulture)}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("WPFApp");
                try
                {
                    string response = await client.GetStringAsync(url);
                    var result = JsonSerializer.Deserialize<ReverseResult>(response);
                    return result?.display_name;
                }
                catch
                {
                    return null;
                }
            }
        }

        public class ReverseResult
        {
            public string display_name { get; set; }
        }

        private void txtCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTimer != null)
            {
                searchTimer.Stop();
                searchTimer.Dispose();
            }

            searchTimer = new System.Timers.Timer(800); // تأخير 800ms
            searchTimer.Elapsed += async (s, args) =>
            {
                searchTimer.Stop();
                searchTimer.Dispose();

                string city = "";
                Dispatcher.Invoke(() =>
                {
                    city = txtCity.Text.Trim();
                });

                if (string.IsNullOrWhiteSpace(city))
                    return;

                await Dispatcher.InvokeAsync(() => SearchAndMove(city));
            };
            searchTimer.Start();
        }
        private async Task SearchAndMove(string city)
        {
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json&limit=1";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("WPFApp");

                try
                {
                    string response = await client.GetStringAsync(url);
                    var results = JsonSerializer.Deserialize<List<LocationResult>>(response);

                    if (results != null && results.Count > 0)
                    {
                        double lat = double.Parse(results[0].lat, CultureInfo.InvariantCulture);
                        double lon = double.Parse(results[0].lon, CultureInfo.InvariantCulture);

                        MainMap.Position = new PointLatLng(lat, lon);
                        MainMap.Zoom = 15;
                    }
                }
                catch
                {
                    // تجاهل الأخطاء الصامتة أثناء الكتابة
                }
            }
        }
        public class LocationResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }
    

    }
}
