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
                Projekt p = (Projekt) cbEinsatz.SelectedItem;
                Mitarbeiter m = (Mitarbeiter) cbMitarbeiter.SelectedItem;
                string hour = cbBeginTimeHour.SelectedValue.ToString();
                string min = cbBeginTimeMin.SelectedValue.ToString();
                string aufwand = cbZeitAufwand.SelectedValue.ToString();
                DateTime date = dPdate.SelectedDate.Value;
                long id = long.Parse(lblID.Content.ToString());

                DateTime start = new DateTime(date.Year, date.Month, date.Day, int.Parse(hour), int.Parse(min), 0);
                DateTime ende = new DateTime(date.Year, date.Month, date.Day, int.Parse(hour) + int.Parse(aufwand),
                    int.Parse(min), 0);

                DateTime projektStart = p.StartDatum;
                DateTime projektEnde = p.EndDatum;

                if (projektStart < start && projektEnde > ende)
                {
                    if (lblIsUpdate.Content == "true")
                    {
                        Einsatz e = Bibliothek.Einsatz_nach_ID(int.Parse(lblID.Content.ToString()));
                        e.Projekt = p;
                        e.Mitarbeiter = m;
                        e.Start = start;
                        e.Ende = ende;
                        Bibliothek.EinsatzUpdate(e);
                        MessageBox.Show("Der Einsatz wurde erfolgreich geändert!");
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
                            MessageBox.Show("Der Einsatz wurde erfolgreich erstellt!");
                        }
                        else
                        {
                            MessageBox.Show("Es besteht bereits ein Eintrag mit dieser ID!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Der Einsatz liegt nicht in der Projek!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Es wurden nicht alle Felder ausgefühlt!");
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

