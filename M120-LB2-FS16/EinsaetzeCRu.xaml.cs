using System;
using System.Drawing;
using System.Windows;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für EinsaetzeCRu.xaml
    /// </summary>
    public partial class EinsaetzeCRu
    {
        public EinsaetzeCRu()
        {
            InitializeComponent();
            cbEinsatz.ItemsSource = Bibliothek.Projekt_Alle();
            cbMitarbeiter.ItemsSource = Bibliothek.Mitarbeiter_Alle();
            cbZeitAufwand.ItemsSource = new[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            cbBeginTimeHour.ItemsSource = new[] {"07", "08", "09", "10", "11", "12",
                                                        "13", "14", "15", "16", "17", "18"};
            cbBeginTimeMin.ItemsSource = new[] { "00", "30" };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveEinsatz();
        }

        private void SaveEinsatz()
        {
            try
            {
                var isAlreadyEinsatz = false;
                var p = (Projekt)cbEinsatz.SelectedItem;
                var m = (Mitarbeiter)cbMitarbeiter.SelectedItem;
                var hour = cbBeginTimeHour.SelectedValue.ToString();
                var min = cbBeginTimeMin.SelectedValue.ToString();
                var aufwand = cbZeitAufwand.SelectedValue.ToString();
                if (dPdate.SelectedDate != null)
                {
                    var date = dPdate.SelectedDate.Value;
                    var id = long.Parse(lblID.Content.ToString());
                    var aufwandsstunde = int.Parse(hour) + int.Parse(aufwand);
                    aufwandsstunde = aufwandsstunde > 19 ? 19 : aufwandsstunde;

                    var start = new DateTime(date.Year, date.Month, date.Day, int.Parse(hour), int.Parse(min), 0);
                    var ende = new DateTime(date.Year, date.Month, date.Day, aufwandsstunde, int.Parse(min), 0);

                    var einsaetzeAnDatum = Bibliothek.Einsaetz_an_Datum(start);
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

                    var projektStart = p.StartDatum;
                    var projektEnde = p.EndDatum;

                    if (projektStart < start && projektEnde > ende)
                    {
                        if (isAlreadyEinsatz)
                        {
                            if ((string) lblIsUpdate.Content == "true")
                            {
                                var e = Bibliothek.Einsatz_nach_ID(long.Parse(lblID.Content.ToString()));
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
                                    var e = new Einsatz
                                    {
                                        ID = id,
                                        Projekt = p,
                                        Mitarbeiter = m,
                                        Start = start,
                                        Ende = ende,
                                        Farbe = randomColor()
                                    };
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
                else
                {
                    lblMeldung.Background = System.Windows.Media.Brushes.Red;
                    lblMeldung.Content = "Es wurden kein Datum angegeben!";
                }
            }
            catch (Exception)
            {
                lblMeldung.Background = System.Windows.Media.Brushes.Red;
                lblMeldung.Content = "Es wurden nicht alle Felder ausgefühlt!";
            }
        }

        private Color randomColor()
        {
            var randomGen = new Random();
            var names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            var randomColorName = names[randomGen.Next(names.Length)];
            var randomColor = Color.FromKnownColor(randomColorName);

            return randomColor;
        }


    }
}

