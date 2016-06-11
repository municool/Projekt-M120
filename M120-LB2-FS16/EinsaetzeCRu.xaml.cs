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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für EinsaetzeCRu.xaml
    /// </summary>
    public partial class EinsaetzeCRu : UserControl
    {
        public EinsaetzeCRu()
        {
            InitializeComponent();
            cbEinsatz.ItemsSource = Bibliothek.Projekt_Alle();
            cbMitarbeiter.ItemsSource = Bibliothek.Mitarbeiter_Alle();
            cbZeitAufwand.ItemsSource = new int[]{1,2,3,4,5,6,7,8};
        }
    }
}
