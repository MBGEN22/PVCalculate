using Gmap.net.Enums;
using PV_Calculate.BL;
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

namespace PV_Calculate.View
{
    /// <summary>
    /// Logique d'interaction pour frm_edit_centrale.xaml
    /// </summary>
    public partial class frm_edit_centrale : Window
    {
        BL_placement_PV bL_Placement_PV = new BL_placement_PV();
        BL.BL_Combo BL_Comboox = new BL.BL_Combo();
        public liste_centrale listeCentrale;
        public int ID;
        public frm_edit_centrale()
        {
            InitializeComponent();

            cmb_technique.ItemsSource = BL_Comboox.cmb_technique().DefaultView;
            cmb_technique.DisplayMemberPath = "ParametreId";
            cmb_technique.SelectedValuePath = "ParametreId"; // تأكد من أن العمود ID موجود في DataTable


            cmb_bus.ItemsSource = BL_Comboox.cmb_technique().DefaultView;
            cmb_bus.DisplayMemberPath = "CODE";
            cmb_bus.SelectedValuePath = "CODE"; // تأكد من أن العمود ID موجود في DataTable
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bL_Placement_PV.edit_centrale_info(
                    ID,
                    txt_name.Text, 
                    float.Parse(txt_puiisance.Text),
                    cmb_technique.SelectedValue != null ? (int?)Convert.ToInt32(cmb_technique.SelectedValue) : null,
                    cmb_bus.SelectedValue != null ? cmb_bus.SelectedValue.ToString() : null
                );
            if(listeCentrale != null)
            {
                listeCentrale.datagrid.ItemsSource = bL_Placement_PV.get_liste_centrale().DefaultView; // Rafraîchir la liste
            }
            this.Close();
        }
    }
}
