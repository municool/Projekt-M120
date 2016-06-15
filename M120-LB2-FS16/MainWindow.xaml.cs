using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private EinsaetzeCRu _einsaetzeCru;
        private ListView _lvEinsaetze;
        private KalenderWeek _kw;

        public MainWindow()
        {
            DatenBereitstellen();
            InitializeComponent();
        }

        #region Testdaten

        private void demoDatenMitarbeiter()
        {
            Mitarbeiter ma1 = new Mitarbeiter
            {
                ID = 1,
                Name = "Affolter",
                Vorname = "Anton",
                IstAktiv = true,
                Farbe = Colors.Aqua
            };
            Bibliothek.Mitarbeiter_Neu(ma1);

            Mitarbeiter ma2 = new Mitarbeiter
            {
                ID = 2,
                Name = "Bangerter",
                Vorname = "Beat",
                IstAktiv = true,
                Farbe = Colors.BlanchedAlmond
            };
            Bibliothek.Mitarbeiter_Neu(ma2);
        }

        private void demoDatenProjekte()
        {
            Projekt p1 = 
                new Projekt
            {
                ID = 1,
                Name = "Projekt Zeiterfassung",
                IstAktiv = true,
                StartDatum = new DateTime(2016, 3, 1),
                EndDatum = new DateTime(2016, 10, 1),
                GesamtZeitStunden = 120,
                OffeneZeitStunden = 120,
                Farbe = Colors.Violet
            };
            Bibliothek.Projekt_Neu(p1);

            Projekt p2 = new Projekt
            {
                ID = 2,
                Name = "Projekt YellowLabel",
                IstAktiv = true,
                StartDatum = new DateTime(2016, 4, 2),
                EndDatum = new DateTime(2016, 7, 30),
                GesamtZeitStunden = 80,
                OffeneZeitStunden = 80,
                Farbe = Colors.Yellow
            };
            Bibliothek.Projekt_Neu(p2);
        }

        private void demoDatenEinsaetze()
        {
            Einsatz e1 = new Einsatz
            {
                ID = 1,
                Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(1),
                Projekt = Bibliothek.Projekt_nach_ID(1),
                Start = new DateTime(2016, 6, 7, 8, 0, 0),
                Ende = new DateTime(2016, 6, 7, 15, 0, 0)
            };
            Bibliothek.EinsatzNeu(e1);

            Einsatz e2 = new Einsatz
            {
                ID = 2,
                Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(1),
                Projekt = Bibliothek.Projekt_nach_ID(2),
                Start = new DateTime(2016, 6, 10, 11, 0, 0),
                Ende = new DateTime(2016, 6, 10, 18, 0, 0)
            };
            Bibliothek.EinsatzNeu(e2);

            Einsatz e3 = new Einsatz
            {
                ID = 3,
                Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(2),
                Projekt = Bibliothek.Projekt_nach_ID(1),
                Start = new DateTime(2016, 6, 14, 10, 0, 0),
                Ende = new DateTime(2016, 6, 14, 14, 0, 0)
            };
            Bibliothek.EinsatzNeu(e3);

            Einsatz e4 = new Einsatz
            {
                ID = 4,
                Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(2),
                Projekt = Bibliothek.Projekt_nach_ID(1),
                Start = new DateTime(2016, 6, 15, 10, 0, 0),
                Ende = new DateTime(2016, 6, 15, 14, 0, 0)
            };
            Bibliothek.EinsatzNeu(e4);
        }

        private void DatenBereitstellen()
        {
            demoDatenMitarbeiter();
            demoDatenProjekte();
            demoDatenEinsaetze();
        }

        #endregion

        private void btnCreateEinsatz_Click(object sender, RoutedEventArgs e)
        {
            _einsaetzeCru = CreateEinsatzCru();
            Content.Children.Clear();
            Content.Children.Add(_einsaetzeCru);
        }

        private EinsaetzeCRu CreateEinsatzCru()
        {
            _einsaetzeCru = new EinsaetzeCRu
            {
                Name = "EinsaetzeCRU",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(10, 2, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                lblID = {Content = Bibliothek.uIDGenarator().ToString()}
            };
            return _einsaetzeCru;
        }

        private void btnAllEinsaetze_Click(object sender, RoutedEventArgs e)
        {
            _lvEinsaetze = CreateListView();
            Content.Children.Clear();
            Content.Children.Add(_lvEinsaetze);
        }

        private ListView CreateListView()
        {
            _lvEinsaetze = new ListView
            {
                Name = "lvEinsaetze",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Stretch
            };
            _lvEinsaetze.UpdateEinsatz += ShowUpdateEinsatz;
            return _lvEinsaetze;
        }

        public void ShowUpdateEinsatz(object sender, EventArgs e)
        {
            var liste = (ListView)sender;
            ShowEinsatz((Einsatz)liste.dgEinsaetze.SelectedItem);
        }

        public void ShowUpdateEinsatzfromKalender(object sender, EventArgs e)
        {
            var b = (Button) sender;
            var id = long.Parse(b.ToolTip.ToString());
            ShowEinsatz(Bibliothek.Einsatz_nach_ID(id));
        }

        private void ShowEinsatz(Einsatz einsatz)
        {
            var zeitaufwand = einsatz.Ende - einsatz.Start;
            var hour = einsatz.Start.Hour.ToString();

            if (hour == "7" || hour == "8" || hour == "9")
            {
                hour = "0" + hour;
            }

            _einsaetzeCru = CreateEinsatzCru();

            _einsaetzeCru.lblID.Content = einsatz.ID;
            _einsaetzeCru.cbEinsatz.SelectedItem = einsatz.Projekt;
            _einsaetzeCru.cbMitarbeiter.SelectedItem = einsatz.Mitarbeiter;
            _einsaetzeCru.cbBeginTimeHour.SelectedItem = hour;
            _einsaetzeCru.cbBeginTimeMin.SelectedItem = einsatz.Start.Minute.ToString() == "0" ? "00" : "30";
            _einsaetzeCru.cbZeitAufwand.SelectedItem = zeitaufwand.Hours.ToString();
            _einsaetzeCru.dPdate.SelectedDate = einsatz.Start;
            _einsaetzeCru.lblFarbe.Content = einsatz.Farbe.ToString();
            _einsaetzeCru.lblIsUpdate.Content = "true";

            Content.Children.Clear();
            Content.Children.Add(_einsaetzeCru);

        }

        private void btnKalender_Click(object sender, RoutedEventArgs e)
        {
            _kw = CreateKalenderWeek();
            Content.Children.Clear();
            Content.Children.Add(_kw);
        }

        private KalenderWeek CreateKalenderWeek()
        {
            _kw = new KalenderWeek
            {
                Name = "KalenderView",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Stretch
            };
            _kw.UpdateEinsatzFromKalender += ShowUpdateEinsatzfromKalender;
            return _kw;
        }
    }
}
