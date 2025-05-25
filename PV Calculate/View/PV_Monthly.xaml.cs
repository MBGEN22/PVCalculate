using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
using System.Text.Json;

namespace PV_Calculate.View
{
    /// <summary>
    /// Logique d'interaction pour PV_Monthly.xaml
    /// </summary>
    public partial class PV_Monthly : UserControl
    {
        public ObservableCollection<string> Labels { get; set; } = new ObservableCollection<string>();
        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public Func<double, string> Formatter { get; set; } = value => value.ToString("N2");
        public PV_Monthly()
        {
            InitializeComponent();
            DataContext = this;
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
        private async void BtnGetData_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtLat.Text, out double lat) ||
        !double.TryParse(txtLon.Text, out double lon) ||
        !double.TryParse(txtPower.Text, out double power))
            {
                MessageBox.Show("يرجى إدخال قيم صحيحة للخط العرض، الطول، والطاقة.");
                return;
            }

            try
            {
                // دالة مساعدة لتحويل رقم الشهر إلى اسم فرنسي مختصر
                string GetFrenchMonthName(int month)
                {
                    string[] frenchMonths = { "Jan", "Fév", "Mar", "Avr", "Mai", "Juin", "Juil", "Aoû", "Sep", "Oct", "Nov", "Déc" };
                    if (month >= 1 && month <= 12)
                        return frenchMonths[month - 1];
                    return month.ToString();
                }

                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://re.jrc.ec.europa.eu/api/PVcalc?lat={lat}&lon={lon}&peakpower={power}&loss=14&outputformat=json";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    JsonDocument doc = JsonDocument.Parse(json);

                    var monthlyData = doc.RootElement
                        .GetProperty("outputs")
                        .GetProperty("monthly")
                        .GetProperty("fixed")
                        .EnumerateArray();

                    List<MonthlyData> dataList = new List<MonthlyData>();

                    Labels.Clear();
                    var energieQuotidienneValues = new ChartValues<double>();

                    foreach (JsonElement monthElement in monthlyData)
                    {
                        int month = monthElement.GetProperty("month").GetInt32();
                        double e_d = monthElement.GetProperty("E_d").GetDouble(); // الطاقة اليومية

                        dataList.Add(new MonthlyData
                        {
                            Mois = GetFrenchMonthName(month),
                            EnergieQuotidienne = e_d,
                            EnergieMensuelle = monthElement.GetProperty("E_m").GetDouble(),
                            IrradiationQuotidienne = monthElement.GetProperty("H(i)_d").GetDouble(),
                            IrradiationMensuelle = monthElement.GetProperty("H(i)_m").GetDouble(),
                            DureeEnsoleillement = monthElement.GetProperty("SD_m").GetDouble()
                        });

                        Labels.Add(GetFrenchMonthName(month)); // أسماء الأشهر في الشارت
                        energieQuotidienneValues.Add(e_d);     // قيم الطاقة اليومية
                    }

                    dataGrid.ItemsSource = dataList;

                    SeriesCollection.Clear();
                    SeriesCollection.Add(new LineSeries
                    {
                        Title = "Énergie quotidienne (E_d)",
                        Values = energieQuotidienneValues,
                        PointGeometry = DefaultGeometries.Circle,
                        PointGeometrySize = 8
                    });

                    DataContext = null;
                    DataContext = this;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب البيانات: " + ex.Message);
            }
        }
        public class MonthlyData
        {
            public string Mois { get; set; } // الشهر
            public double EnergieQuotidienne { get; set; } // E_d
            public double EnergieMensuelle { get; set; } // E_m
            public double IrradiationQuotidienne { get; set; } // H(i)_d
            public double IrradiationMensuelle { get; set; } // H(i)_m
            public double DureeEnsoleillement { get; set; } // SD_m
        }
    }
}
