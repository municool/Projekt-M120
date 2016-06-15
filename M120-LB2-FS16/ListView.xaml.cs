using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für ListView.xaml
    /// </summary>
    public partial class ListView
    {
        private List<Einsatz> _einsatzListe;
        public event EventHandler UpdateEinsatz;

        public ListView()
        {
            InitializeComponent();
            SetEinsatzListe();
            RefreshListView();
        }

        public void SetEinsatzListe()
        {
            _einsatzListe = Bibliothek.Einsatz_Alle();
        }

        public void RefreshListView()
        {
            dgEinsaetze.ItemsSource = _einsatzListe;
        }

        private void dgEinsaetze_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateEinsatz?.Invoke(this, e);
        }
    }
}
