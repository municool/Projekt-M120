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
        private DateTime currentWeek;
        public event EventHandler UpdateEinsatzFromKalender;
        public KalenderWeek()
        {
            InitializeComponent();
            currentWeek = DateTime.Today.AddDays(((int)DateTime.Today.DayOfWeek - 1) * -1);
        }

        private void drawGrid()
        {
            double halfhourHeight = mainGrid.ActualHeight / 24;
            double dayWidth = mainGrid.ActualWidth / 7;
            int hour = 7;

            Button btnForward = new Button();
            btnForward.Name = "btnWeekForward";
            btnForward.Content = "Nächste Woche";
            btnForward.Height = 20;
            btnForward.Width = 200;
            btnForward.VerticalAlignment = VerticalAlignment.Top;
            btnForward.HorizontalAlignment = HorizontalAlignment.Right;
            btnForward.Click += new RoutedEventHandler(BtnWeekForward_OnClick);
            HeaderGrid.Children.Add(btnForward);

            Button btnBack = new Button();
            btnForward.Name = "btnWeekBack";
            btnBack.Content = "Woche zuvor";
            btnBack.Height = 20;
            btnBack.Width = 200;
            btnBack.VerticalAlignment = VerticalAlignment.Top;
            btnBack.HorizontalAlignment = HorizontalAlignment.Left;
            btnBack.Click += new RoutedEventHandler(BtnWeekBack_OnClick);
            HeaderGrid.Children.Add(btnBack);

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
            for (int i = 0; i < 8; i++)
            {
                Label lbl = new Label();
                lbl.Content = currentWeek.AddDays(i).ToString("dd.MM.yy");
                lbl.Margin = new Thickness(dayWidth*i,20,0,0);
                lbl.VerticalAlignment = VerticalAlignment.Top;
                lbl.HorizontalAlignment = HorizontalAlignment.Left;
                HeaderGrid.Children.Add(lbl);
            }

            for (int i = 0; i < 12; i++)
            {

                Label l = new Label();
                l.Content = hour == 7 || hour == 8 || hour == 9 ? "0" + hour : hour.ToString();
                l.Margin = new Thickness(0, i * (halfhourHeight * 2), 0, 0);
                hourGrid.Children.Add(l);
                hour++;
            }
        }

        private void drawEinsaetze()
        {

            double halfhourHeight = mainGrid.ActualHeight / 24;
            double dayWidth = mainGrid.ActualWidth / 7;

            for (int i = 0; i < 7; i++)
            {

                List<Einsatz> einsaetze = Bibliothek.Einsaetz_an_Datum(currentWeek.AddDays(i));
                foreach (var einsatz in einsaetze)
                {
                    TimeSpan ts = einsatz.Start - einsatz.Start.Date;
                    double margintop = (ts.TotalHours - 7) * 2 * halfhourHeight;
                    double marginleft = dayWidth * i;
                    TimeSpan dauer = einsatz.Ende - einsatz.Start;
                    double height = (dauer.TotalHours * 2) * halfhourHeight;

                    marginleft = einsatz.Mitarbeiter.ID == 2 ? marginleft + dayWidth / 2 : marginleft;

                    Button b = new Button();
                    b.Margin = new Thickness(marginleft, margintop, 0, 0);
                    b.Height = height;
                    b.Width = dayWidth / 2;
                    b.VerticalAlignment = VerticalAlignment.Top;
                    b.HorizontalAlignment = HorizontalAlignment.Left;
                    b.Content = einsatz.Mitarbeiter.Name + "\n" + einsatz.Projekt.Name;
                    b.ToolTip = einsatz.ID.ToString();
                    b.MouseDoubleClick += new MouseButtonEventHandler(einsatz_MouseDoubleClick);
                    mainGrid.Children.Add(b);

                }
            }
        }

        private void refreshCalender()
        {
            mainGrid.Children.Clear();
            hourGrid.Children.Clear();
            HeaderGrid.Children.Clear();
            drawGrid();
            drawEinsaetze();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            refreshCalender();
        }

        private void BtnWeekForward_OnClick(object sender, RoutedEventArgs e)
        {
            currentWeek = currentWeek.AddDays(7);
            refreshCalender();
        }

        private void BtnWeekBack_OnClick(object sender, RoutedEventArgs e)
        {
            currentWeek = currentWeek.AddDays(-7);
            refreshCalender();
        }

        private void einsatz_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UpdateEinsatzFromKalender != null)
            {
                UpdateEinsatzFromKalender(sender, e);
            }
        }
    }
}
