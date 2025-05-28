using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PV_Calculate.View
{
    /// <summary>
    /// Logique d'interaction pour frm_add_Technique.xaml
    /// </summary>
    public partial class frm_add_Technique : Window
    {
        BL.BL_placement_PV bl_placement_PV = new BL.BL_placement_PV();
        public liste_téchnique liste;
        public frm_add_Technique()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                float chargeElementaire = float.Parse(txtChargeElementaire.Text, CultureInfo.InvariantCulture);
                float constanteBoltzmann = float.Parse(txtConstBoltzmann.Text, CultureInfo.InvariantCulture);
                float tempReference = float.Parse(txtTempReference.Text, CultureInfo.InvariantCulture);
                float irradianceReference = float.Parse(txtIrradianceReference.Text, CultureInfo.InvariantCulture);
                int nbCellulesSerie = int.Parse(txtNbCellulesSerie.Text);
                int nbPanneauxSerie = int.Parse(txtNbPanneauxSerie.Text);
                int nbChainesParallele = int.Parse(txtNbChainesParallele.Text);
                float isc = float.Parse(txtCourantCourtCircuit.Text, CultureInfo.InvariantCulture);
                float voc = float.Parse(txtTensionCircuitOuvert.Text, CultureInfo.InvariantCulture);
                float impp = float.Parse(txtCourantNominalMPP.Text, CultureInfo.InvariantCulture);
                float vmpp = float.Parse(txtTensionNominaleMPP.Text, CultureInfo.InvariantCulture);
                float pmp = float.Parse(txtPuissanceNominale.Text, CultureInfo.InvariantCulture);
                float coeffTemp = float.Parse(txtCoeffTemperature.Text, CultureInfo.InvariantCulture);
                float rSerie = float.Parse(txtResistanceSerie.Text, CultureInfo.InvariantCulture);
                float rParallele = float.Parse(txtResistanceParallele.Text, CultureInfo.InvariantCulture);
                float facteurIdealite = float.Parse(txtFacteurIdealite.Text, CultureInfo.InvariantCulture); 
                float noct = float.Parse(txt_noct.Text, CultureInfo.InvariantCulture);

                // استدعاء الدالة التي تنفذ الإجراء المخزن
                bl_placement_PV.InsertParametresTechniques(
                    chargeElementaire,
                    constanteBoltzmann,
                    tempReference,
                    irradianceReference,
                    nbCellulesSerie,
                    isc,
                    voc,
                    impp,
                    vmpp,
                    pmp,
                    coeffTemp,
                    rSerie,
                    rParallele,
                    facteurIdealite,
                    nbPanneauxSerie,
                    nbChainesParallele,
                    noct
                );

                MessageBox.Show("Les paramètres ont été enregistrés avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                if (liste != null)
                {
                    liste.datagrid.ItemsSource = bl_placement_PV.get_technique_tb().DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la sauvegarde : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }
    }
}
