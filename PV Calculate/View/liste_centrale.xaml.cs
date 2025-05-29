using PV_Calculate.BL;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logique d'interaction pour liste_centrale.xaml
    /// </summary>
    public partial class liste_centrale : UserControl
    {
        BL_placement_PV bL_Placement_PV = new BL_placement_PV();

        public liste_centrale()
        {
            InitializeComponent();
            this.datagrid.ItemsSource = bL_Placement_PV.get_liste_centrale().DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Cette option permet de supprimer des données.",
                 "Processus de suppression",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedItem = datagrid.SelectedItem as DataRowView;
                if (selectedItem != null)
                {
                    double longitude = double.Parse(selectedItem.Row["Longitude"].ToString());
                    double latitude = double.Parse(selectedItem.Row["Latitude"].ToString());// أو استخدم اسم العمود بدل [0]
                    bL_Placement_PV.DELETE_PLACEMENT_BY_COORDS(latitude, longitude);
                    datagrid.ItemsSource = bL_Placement_PV.get_liste_centrale().DefaultView; 
                    MessageBox.Show("Le processus de suppression a été effectué avec succès.");
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            frm_edit_centrale editCentraleWindow = new frm_edit_centrale();
            editCentraleWindow.listeCentrale = this; // Passer la référence de liste_centrale à frm_edit_centrale
            var selectedItem = datagrid.SelectedItem as DataRowView;
            int ID = Convert.ToInt32(selectedItem.Row["ID"].ToString());
            string Nom = selectedItem.Row["Nom"].ToString();
            string Puissance = selectedItem.Row["Puissance"].ToString();
            int Code_téchnique = Convert.ToInt32(selectedItem.Row["Code téchnique"].ToString());
            string Code_bus = selectedItem.Row["Code bus"].ToString();
            editCentraleWindow.ID = ID;
            editCentraleWindow.txt_name.Text = Nom;
            editCentraleWindow.txt_puiisance.Text = Puissance;
            editCentraleWindow.cmb_technique.SelectedValue = Code_téchnique; // Assurez-vous que cmb_technique est correctement peuplé
            editCentraleWindow.cmb_bus.SelectedValue = Code_bus; // Assurez-vous que cmb_bus est correctement peuplé
            editCentraleWindow.ShowDialog();
        }
    }
}
