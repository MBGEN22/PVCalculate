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
    /// Logique d'interaction pour liste_téchnique.xaml
    /// </summary>
    public partial class liste_téchnique : UserControl
    {
        BL.BL_placement_PV bl_placement_PV = new BL.BL_placement_PV();
        public liste_téchnique()
        {
            InitializeComponent();
            this.datagrid.ItemsSource = bl_placement_PV.get_technique_tb().DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frm_add_Technique frm = new frm_add_Technique();
            frm.liste = this;
            frm.ShowDialog();
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
                    int id = Convert.ToInt32(selectedItem.Row["ID"]); // أو استخدم اسم العمود بدل [0]
                    bl_placement_PV.delete_technique(id);
                    datagrid.ItemsSource = bl_placement_PV.get_technique_tb().DefaultView;

                    MessageBox.Show("Le processus de suppression a été effectué avec succès.");
                }
            }

        }
    }
}
