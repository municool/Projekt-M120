using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
            cbZeitAufwand.ItemsSource = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            cbBeginTimeHour.ItemsSource = new string[] {"07", "08", "09", "10", "11", "12",
                                                        "13", "14", "15", "16", "17", "18"};
            cbBeginTimeMin.ItemsSource = new string[] { "00", "30" };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveEinsatz();
        }

        private void saveEinsatz()
        {
            try
            {
                bool isAlreadyEinsatz = false;
                Projekt p = (Projekt)cbEinsatz.SelectedItem;
                Mitarbeiter m = (Mitarbeiter)cbMitarbeiter.SelectedItem;
                string hour = cbBeginTimeHour.SelectedValue.ToString();
                string min = cbBeginTimeMin.SelectedValue.ToString();
                string aufwand = cbZeitAufwand.SelectedValue.ToString();
                DateTime date = dPdate.SelectedDate.Value;
                long id = long.Parse(lblID.Content.ToString());
                int aufwandsstunde = int.Parse(hour) + int.Parse(aufwand);
                aufwandsstunde = aufwandsstunde > 19 ? 19 : aufwandsstunde;

                DateTime start = new DateTime(date.Year, date.Month, date.Day, int.Parse(hour), int.Parse(min), 0);
                DateTime ende = new DateTime(date.Year, date.Month, date.Day, aufwandsstunde, int.Parse(min), 0);

                List<Einsatz> einsaetzeAnDatum = Bibliothek.Einsaetz_an_Datum(start);
                if (einsaetzeAnDatum.Count != 0)
                {
                    foreach (var e in einsaetzeAnDatum)
                    {
                        if (e.Mitarbeiter == m)
                        {
                            if ((e.Start >= ende && e.Ende > ende) || (e.Start < start && e.Ende <= start))
                            {
                                isAlreadyEinsatz = true;
                            }
                            else
                            {
                                isAlreadyEinsatz = false;
                            }
                        }
                        else
                        {
                            isAlreadyEinsatz = true;
                        }
                    }
                }
                else
                {
                    isAlreadyEinsatz = true;
                }

                DateTime projektStart = p.StartDatum;
                DateTime projektEnde = p.EndDatum;

                if (projektStart < start && projektEnde > ende)
                {
                    if (isAlreadyEinsatz)
                    {
                        if (lblIsUpdate.Content == "true")
                        {
                            Einsatz e = Bibliothek.Einsatz_nach_ID(long.Parse(lblID.Content.ToString()));
                            e.Projekt = p;
                            e.Mitarbeiter = m;
                            e.Start = start;
                            e.Ende = ende;
                            Bibliothek.EinsatzUpdate(e);
                            lblMeldung.Background = System.Windows.Media.Brushes.Green;
                            lblMeldung.Content = "Der Einsatz wurde erfolgreich geändert!";
                        }
                        else
                        {
                            if (Bibliothek.Einsatz_nach_ID(id) == null)
                            {
                                Einsatz e = new Einsatz();
                                e.ID = id;
                                e.Projekt = p;
                                e.Mitarbeiter = m;
                                e.Start = start;
                                e.Ende = ende;
                                e.Farbe = randomColor();
                                Bibliothek.EinsatzNeu(e);
                                lblMeldung.Background = System.Windows.Media.Brushes.Green;
                                lblMeldung.Content = "Der Einsatz wurde erfolgreich erstellt!";
                            }
                            else
                            {
                                lblMeldung.Background = System.Windows.Media.Brushes.Red;
                                lblMeldung.Content = "Es besteht bereits ein Eintrag mit dieser ID!";
                            }
                        }
                    }
                    else
                    {
                        lblMeldung.Background = System.Windows.Media.Brushes.Red;
                        lblMeldung.Content = "In der angegebenen Zeit existiert bereits ein Einsatz!";
                    }
                }
                else
                {
                    lblMeldung.Background = System.Windows.Media.Brushes.Red;
                    lblMeldung.Content = "Der Einsatz liegt nicht in der Projekzeit!";
                }
            }
            catch (Exception e)
            {
                lblMeldung.Background = System.Windows.Media.Brushes.Red;
                lblMeldung.Content = "Es wurden nicht alle Felder ausgefühlt!";
            }
        }

        private Color randomColor()
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);

            return randomColor;
        }


    }
}

