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

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        private List<Einsatz> einsatzListe;

        public ListView()
        {
            InitializeComponent();
            setEinsatzListe();
            refreshListView();
        }

        public void setEinsatzListe()
        {
            einsatzListe = Bibliothek.Einsatz_Alle();
        }

        public void refreshListView()
        {
            dgEinsaetze.ItemsSource = einsatzListe;
        }

        private void dgEinsaetze_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Einsatz einsatz = (Einsatz)dgEinsaetze.SelectedItem;
            
        }
    }
}
