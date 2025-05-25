using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logique d'interaction pour Caculate_PV.xaml
    /// </summary>
    public partial class Caculate_PV : UserControl
    {
        private List<double> G_array = new List<double>();
        private List<double> Temp_array = new List<double>();

        public Caculate_PV()
        {
            InitializeComponent();
        }
        public async Task ChargerEtCalculer()
        {
            double latitude = double.Parse(txt_latude.Text);
            double longitude = double.Parse(txt_longitude.Text);

            MeteoData meteo = await GetMeteoData(latitude, longitude);
            G_array = meteo.Radiation;
            Temp_array = meteo.Temperatures;

            calcule();
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
        private void calcule()
        {
            // استخدام G_array و Temp_array من المتغيرات العضو
            if (G_array.Count == 0 || Temp_array.Count == 0)
            {
                MessageBox.Show("Les données météo ne sont pas encore chargées.");
                return;
            }

            // ==== PARAMÈTRES DU MODULE PV ====
            double q = double.Parse(txtChargeElementaire.Text);       // Charge élémentaire (C)
            double k = double.Parse(txtConstBoltzmann.Text);          // Constante de Boltzmann (J/K)
            double T_ref = double.Parse(txtTempReference.Text);       // Température de référence (K)
            double G_ref = double.Parse(txtIrradianceReference.Text); // Irradiance standard (W/m²)
            int Ns_cell = int.Parse(txtNbCellulesSerie.Text);         // Nombre de cellules en série

            // ==== PARAMÈTRES DU FABRICANT ====
            double Isc = double.Parse(txtCourantCourtCircuit.Text);   // Courant de court-circuit (A)
            double Voc = double.Parse(txtTensionCircuitOuvert.Text);  // Tension circuit ouvert (V)
            double Impp = double.Parse(txtCourantNominalMPP.Text);    // Courant MPP (A)
            double Vmpp = double.Parse(txtTensionNominaleMPP.Text);   // Tension MPP (V)
            double Pmp = double.Parse(txtPuissanceNominale.Text);     // Puissance nominale (W)
            double Beta_P = double.Parse(txtCoeffTemperature.Text);   // Coefficient de température (%/°C)
            double Rs = double.Parse(txtResistanceSerie.Text);        // Résistance série (Ohm)
            double Rsh = double.Parse(txtResistanceParallele.Text);   // Résistance parallèle (Ohm)
            double n = double.Parse(txtFacteurIdealite.Text);         // Facteur d’idéalité

            // ==== CONFIGURATION DU CHAMP PV ====
            int Ns = int.Parse(txtNbPanneauxSerie.Text);              // Nombre de panneaux en série
            int Np = int.Parse(txtNbChainesParallele.Text);           // Nombre de chaînes en parallèle
            double NOCT = 45;
            // Initialisation
            List<double> P_max_array = new List<double>();

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

                    I[j] = SolveEquation(fun, Isc); // equivalent fzero
                }

                double maxP = 0;
                for (int j = 0; j < N; j++)
                {
                    double P = V[j] * I[j];
                    if (P > maxP)
                        maxP = P;
                }

                P_max_array.Add(Math.Round(maxP, 4));
            }


            DataTable table = new DataTable();
            DataRow row = table.NewRow();

            // إضافة أعمدة حسب عدد القيم
            for (int i = 0; i < P_max_array.Count; i++)
            {
                table.Columns.Add(i + ":00", typeof(double));
                row[i] = P_max_array[i];
            }

            // إضافة الصف الوحيد اللي يحتوي على كل القيم
            table.Rows.Add(row);

            // إذا تحب تعرضه في DataGridView
            ResultatsDataGrid.ItemsSource = table.DefaultView; 
            DessinerCourbe(P_max_array);
            DessinerCourbe_Garray(G_array);


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
        public void DessinerCourbe(List<double> P_max_array)
        {
            var values = new ChartValues<double>(P_max_array);

            chartPuissance.Series.Clear();
            chartPuissance.Series.Add(new LineSeries
            {
                Title = "Puissance",
                Values = values,
                Stroke = Brushes.Red,
                Fill = Brushes.Transparent,
                StrokeThickness = 3,
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 5
            });

            // إعداد المحور X (ساعات 0-23)
            chartPuissance.AxisX[0].Labels = GenerateLabels(P_max_array.Count);
        }

        public void DessinerCourbe_Garray(List<double> G_list_array)
        {
            var values = new ChartValues<double>(G_list_array);

            chartIrradiance.Series.Clear();
            chartIrradiance.Series.Add(new LineSeries
            {
                Title = "Irradiance",
                Values = values,
                Stroke = Brushes.Green,
                Fill = Brushes.Transparent,
                StrokeThickness = 3,
                PointGeometry = DefaultGeometries.Square,
                PointGeometrySize = 5,
                LineSmoothness = 0
            });


            chartIrradiance.AxisX[0].Labels = GenerateLabels(G_list_array.Count);
        }

        private List<string> GenerateLabels(int count)
        {
            var labels = new List<string>();
            for (int i = 0; i < count; i++)
            {
                labels.Add(i + ":00");
            }
            return labels;
        }
        private async  void Calculer_Click(object sender, RoutedEventArgs e)
        {
            await ChargerEtCalculer();

        }
    }
    public class MeteoData
    {
        public List<double> Radiation { get; set; }
        public List<double> Temperatures { get; set; }
    }

}
