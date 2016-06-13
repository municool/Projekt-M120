using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaction logic for KalenderWeek.xaml
    /// </summary>
    public partial class KalenderWeek : UserControl
    {
        public KalenderWeek()
        {
            InitializeComponent();

        }

        private void drawGrid()
        {
            double halfhourHeight = mainGrid.ActualHeight / 24;
            double dayWidth = mainGrid.ActualWidth / 7;

            for (int i = 0; i < 25; i++)
            {
                Line l = new Line();
                l.X1 = 0;
                l.X2 = mainGrid.ActualWidth;
                l.Y1 = halfhourHeight * i;
                l.Y2 = halfhourHeight * i;
                if (i % 2 == 1)
                {
                    l.StrokeThickness = 0.5;
                }
                else
                {
                    l.StrokeThickness = 1;
                }
                l.Stroke = Brushes.Black;
                mainGrid.Children.Add(l);
            }

            for (int i = 0; i < 8; i++)
            {
                Line l = new Line();
                l.X1 = dayWidth * i;
                l.X2 = dayWidth * i;
                l.Y1 = 0;
                l.Y2 = mainGrid.ActualHeight;
                l.StrokeThickness = 2;
                l.Stroke = Brushes.Black;
                mainGrid.Children.Add(l);
            }
        }

        private void drawEinsaetze()
        {
            List<Einsatz> einsaetze = Bibliothek.Einsaetz_an_Datum(DateTime.Today);
            double halfhourHeight = mainGrid.ActualHeight / 24;

            foreach (var einsatz in einsaetze)
            {
                int hour = 7;
                TimeSpan ts = einsatz.Start - einsatz.Start.Date;
                double margintop = (ts.TotalHours - 7) * 2 * halfhourHeight;


                Button b = new Button();
                b.Margin = new Thickness(0, margintop, 0, 0);
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mainGrid.Children.Clear();
            drawGrid();
            drawEinsaetze();
        }
    }
}
