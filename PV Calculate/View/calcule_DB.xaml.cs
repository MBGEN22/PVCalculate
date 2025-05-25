using GMap.NET.MapProviders;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
    /// Logique d'interaction pour calcule_DB.xaml
    /// </summary>
    public partial class calcule_DB : UserControl
    {
        private List<double> G_array = new List<double>();
        private List<double> Temp_array = new List<double>();
        BL.BL_placement_PV class_placement = new BL.BL_placement_PV();
        public calcule_DB()
        {
            InitializeComponent();
            this.dgProduction24h.ItemsSource = class_placement.get_central_with_parame().DefaultView; 
        }

        private void DrawAllRowsInSingleChart(DataTable dt)
        {
            var hourColumns = dt.Columns.Cast<DataColumn>()
                .Where(col => col.ColumnName.EndsWith("h"))
                .OrderBy(col => col.ColumnName)
                .ToList();

            string[] xLabels = hourColumns.Select(c => c.ColumnName).ToArray();

            Random rand = new Random();
            var seriesList = new SeriesCollection();

            foreach (DataRow row in dt.Rows)
            {
                var values = hourColumns.Select(c => Convert.ToDouble(row[c])).ToArray();

                var line = new LineSeries
                {
                    Title = row["Name"].ToString() + " ID:" + row["ID"].ToString(),
                    Values = new ChartValues<double>(values),
                    Stroke = new SolidColorBrush(Color.FromRgb(
                        (byte)rand.Next(256),
                        (byte)rand.Next(256),
                        (byte)rand.Next(256))),
                    Fill = Brushes.Transparent,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 5
                };

                seriesList.Add(line);
            }

            MainChart.Series = seriesList;

            MainChart.AxisX.Clear();
            MainChart.AxisX.Add(new Axis
            {
                Title = "Heure",
                Labels = xLabels.ToList()
            });

            MainChart.AxisY.Clear();
            MainChart.AxisY.Add(new Axis
            {
                Title = "Valeur"
            });
        }

        public async Task<MeteoData> GetMeteoData(double latitude, double longitude)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m,direct_radiation";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    JsonElement root = doc.RootElement;
                    var hourly = root.GetProperty("hourly");

                    var radiationArray = hourly.GetProperty("direct_radiation").EnumerateArray()
                        .Take(24).Select(x => x.GetDouble()).ToList();

                    var temperatureArray = hourly.GetProperty("temperature_2m").EnumerateArray()
                        .Take(24).Select(x => x.GetDouble()).ToList();

                    return new MeteoData
                    {
                        Radiation = radiationArray,
                        Temperatures = temperatureArray
                    };
                }
            }

        }
        private List<string> CalculerProduction(
                int ID ,string Name ,double q, double k, double T_ref, double G_ref, int Ns_cell,
                double Isc, double Voc, double Impp, double Vmpp, double Pmp,
                double Beta_P, double Rs, double Rsh, double n, int Ns, int Np,
                double NOCT,
                List<double> G_array, List<double> Temp_array)
        {
            List<string> P_max_array = new List<string>();
            P_max_array.Add(ID.ToString());
            P_max_array.Add(Name.ToString());
            for (int t = 0; t < 24; t++)
            {
                double G = G_array[t];
                double T_amb = Temp_array[t];
                double T_cell = T_amb + ((G / G_ref) * (NOCT - 20)) + 273.15;

                double Iph = Isc * (G / G_ref);
                double Vt = (n * k * T_cell) / q;
                double I0 = Isc / (Math.Exp(Voc / (n * Vt)) - 1);

                int N = 100;
                double[] V = new double[N];
                double[] I = new double[N];
                double Vmax = Ns * Voc;

                for (int j = 0; j < N; j++)
                {
                    V[j] = j * Vmax / (N - 1);
                    Func<double, double> fun = Ivar =>
                        Np * (Iph - I0 * (Math.Exp((V[j] / Ns + (Rs * Ivar / Np)) / Vt) - 1)
                        - (V[j] / Ns + (Rs * Ivar / Np)) / Rsh) - Ivar;

                    I[j] = SolveEquation(fun, Isc);
                }

                double maxP = 0;
                for (int j = 0; j < N; j++)
                {
                    double P = V[j] * I[j];
                    if (P > maxP)
                        maxP = P;
                }

                P_max_array.Add(Math.Round(maxP, 4).ToString());
            }

            return P_max_array;
        }
        private double SolveEquation(Func<double, double> f, double x0, double tol = 1e-6, int maxIter = 100)
        {
            double h = 1e-6; // Petits pas pour l'approximation dérivée
            double x = x0;

            for (int i = 0; i < maxIter; i++)
            {
                double fx = f(x);
                double dfx = (f(x + h) - f(x - h)) / (2 * h); // dérivée approx.

                if (Math.Abs(dfx) < 1e-12) break; // éviter division par zéro

                double x_new = x - fx / dfx;

                if (Math.Abs(x_new - x) < tol) return x_new;

                x = x_new;
            }

            return x; // retourne la meilleure estimation même si la tolérance n’est pas atteinte
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtResultats = new DataTable();
            dtResultats.Columns.Add("Système", typeof(int));
            dtResultats.Columns.Add("ID", typeof(int));
            dtResultats.Columns.Add("Name", typeof(string));
            for (int h = 0; h < 24; h++)
            {
                string heure = h.ToString("D2") + "h";  // 00h, 01h, ..., 23h
                dtResultats.Columns.Add(heure, typeof(string));
            }

            List<ResultatRow> resultatsList = new List<ResultatRow>();
            int index = 1;

            int systemIndex = 1;
            foreach (var item in dgProduction24h.Items)
            {
                if (item == CollectionView.NewItemPlaceholder)
                    continue;

                var row = item as DataRowView;
                if (row == null)
                    continue;

                // جلب المعطيات...
                int ID = Convert.ToInt32(row["ID"]);
                string NAME = row["NAME"].ToString();
                double latitude = Convert.ToDouble(row["Latitude"]);
                double longitude = Convert.ToDouble(row["Longitude"]);
                double q = Convert.ToDouble(row["ChargeElementaire"]);
                double k = Convert.ToDouble(row["ConstanteBoltzmann"]);
                double T_ref = Convert.ToDouble(row["TempReference"]);
                double G_ref = Convert.ToDouble(row["IrradianceReference"]);
                int Ns_cell = Convert.ToInt32(row["NbCellulesSerie"]);

                double Isc = Convert.ToDouble(row["Isc"]);
                double Voc = Convert.ToDouble(row["Voc"]);
                double Impp = Convert.ToDouble(row["Impp"]);
                double Vmpp = Convert.ToDouble(row["Vmpp"]);
                double Pmp = Convert.ToDouble(row["Pmp"]);
                double Beta_P = Convert.ToDouble(row["CoeffTemperature"]);
                double Rs = Convert.ToDouble(row["ResistanceSerie"]);
                double Rsh = Convert.ToDouble(row["ResistanceParallele"]);
                double n = Convert.ToDouble(row["FacteurIdealite"]);

                int Ns = Convert.ToInt32(row["NbPanneauxSerie"]);
                int Np = Convert.ToInt32(row["NbChainesParallele"]);

                double NOCT = Convert.ToDouble(row["NOCT"]);

                MeteoData meteo = await GetMeteoData(latitude, longitude);
                List<double> G_array = meteo.Radiation;
                List<double> Temp_array = meteo.Temperatures;

                List<string> P_max_array = CalculerProduction(ID , NAME, q, k, T_ref, G_ref, Ns_cell,
                                                              Isc, Voc, Impp, Vmpp, Pmp,
                                                              Beta_P, Rs, Rsh, n, Ns, Np, NOCT,
                                                              G_array, Temp_array);

                // تحضير السطر الجديد
                DataRow resultRow = dtResultats.NewRow();
                resultRow["Système"] = systemIndex;
                resultRow["ID"] = ID;       // من متغير خارجي أو من السطر الأصلي
                resultRow["Name"] = NAME;   // من متغير خارجي أو من السطر الأصلي

                for (int h = 0; h < 24; h++)
                {
                    string colName = h.ToString("D2") + "h";
                    resultRow[colName] = P_max_array[h+2];
                }

                dtResultats.Rows.Add(resultRow);

                systemIndex++;
            }
            this.dgresult.ItemsSource = dtResultats.DefaultView;

            DrawAllRowsInSingleChart(dtResultats);
            DrawSumOfAllRows(dtResultats);



        }
        private void DrawSumOfAllRows(DataTable dt)
        {
            // تحديد الأعمدة التي تمثل الساعات
            var hourColumns = dt.Columns.Cast<DataColumn>()
                .Where(col => col.ColumnName.EndsWith("h"))
                .OrderBy(col => col.ColumnName)
                .ToList();

            // تحويل أسماء الأعمدة إلى Labels
            string[] xLabels = hourColumns.Select(c => c.ColumnName).ToArray();

            // حساب مجموع كل عمود (أي لكل ساعة)
            List<double> sumValues = new List<double>();

            foreach (var col in hourColumns)
            {
                double sum = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (double.TryParse(row[col].ToString(), out double val))
                        sum += val;
                }
                sumValues.Add(sum);
            }

            // إعداد السلسلة
            var sumSeries = new LineSeries
            {
                Title = "Somme Totale",
                Values = new ChartValues<double>(sumValues),
                Stroke = Brushes.Red,
                Fill = Brushes.Transparent,
                PointGeometry = DefaultGeometries.Square,
                PointGeometrySize = 6
            };

            // تعيين السلسلة للرسم
            MainChartsum.Series = new SeriesCollection { sumSeries };

            // إعداد المحاور
            MainChartsum.AxisX.Clear();
            MainChartsum.AxisX.Add(new Axis
            {
                Title = "Heure",
                Labels = xLabels.ToList()
            });

            MainChartsum.AxisY.Clear();
            MainChartsum.AxisY.Add(new Axis
            {
                Title = "Somme des valeurs"
            });
        }

        public class MeteoData
        {
            public List<double> Radiation { get; set; }
            public List<double> Temperatures { get; set; }
        }
        public class ResultatRow
        {
            public int Index { get; set; }
            public double Ptotal { get; set; }
        }

    }
}
