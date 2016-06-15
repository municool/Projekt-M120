using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaction logic for KalenderWeek.xaml
    /// </summary>
    public partial class KalenderWeek
    {
        private DateTime _currentWeek;
        public event EventHandler UpdateEinsatzFromKalender;
        public KalenderWeek()
        {
            InitializeComponent();
            _currentWeek = DateTime.Today.AddDays(((int)DateTime.Today.DayOfWeek - 1) * -1);
        }

        private void DrawGrid()
        {
            var halfhourHeight = mainGrid.ActualHeight / 24;
            var dayWidth = mainGrid.ActualWidth / 7;
            var hour = 7;

            var btnForward = new Button
            {
                Name = "btnWeekForward",
                Content = "Nächste Woche",
                Height = 20,
                Width = 200,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnForward.Click += BtnWeekForward_OnClick;
            HeaderGrid.Children.Add(btnForward);

            var btnBack = new Button
            {
                Name = "btnWeekBack",
                Content = "Woche zuvor",
                Height = 20,
                Width = 200,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            btnBack.Click += BtnWeekBack_OnClick;
            HeaderGrid.Children.Add(btnBack);

            for (var i = 0; i < 25; i++)
            {
                var l = new Line
                {
                    X1 = 0,
                    X2 = mainGrid.ActualWidth,
                    Y1 = halfhourHeight * i,
                    Y2 = halfhourHeight * i,
                    StrokeThickness = 1,
                    Stroke = i % 2 == 1 ? Brushes.Gray : Brushes.Black
                };
                mainGrid.Children.Add(l);
            }

            for (var i = 0; i < 8; i++)
            {
                var l = new Line
                {
                    X1 = dayWidth * i,
                    X2 = dayWidth * i,
                    Y1 = 0,
                    Y2 = mainGrid.ActualHeight,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black
                };
                mainGrid.Children.Add(l);
            }
            for (int i = 0; i < 8; i++)
            {
                var lbl = new Label
                {
                    Content = _currentWeek.AddDays(i).ToString("dd.MM.yy"),
                    Margin = new Thickness(dayWidth * i, 20, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                HeaderGrid.Children.Add(lbl);
            }

            for (var i = 0; i < 12; i++)
            {

                var l = new Label
                {
                    Content = hour == 7 || hour == 8 || hour == 9 ? "0" + hour : hour.ToString(),
                    Margin = new Thickness(0, i * (halfhourHeight * 2), 0, 0)
                };
                hourGrid.Children.Add(l);
                hour++;
            }
        }

        private void DrawEinsaetze()
        {

            var halfhourHeight = mainGrid.ActualHeight / 24;
            var dayWidth = mainGrid.ActualWidth / 7;

            for (int i = 0; i < 7; i++)
            {

                var einsaetze = Bibliothek.Einsaetz_an_Datum(_currentWeek.AddDays(i));
                var twoEinsaetze = einsaetze.Count > 1;

                foreach (var einsatz in einsaetze)
                {
                    var ts = einsatz.Start - einsatz.Start.Date;
                    var margintop = (ts.TotalHours - 7) * 2 * halfhourHeight;
                    var marginleft = dayWidth * i;
                    var dauer = einsatz.Ende - einsatz.Start;
                    var height = (dauer.TotalHours * 2) * halfhourHeight;
                    var width = dayWidth;
                    var color = new SolidColorBrush(einsatz.Mitarbeiter.Farbe);

                    if (twoEinsaetze)
                    {
                        width = dayWidth / 2;
                        marginleft = einsatz.Mitarbeiter.ID == 2 ? marginleft + dayWidth / 2 : marginleft;
                    }
                    var b = new Button
                    {
                        Margin = new Thickness(marginleft, margintop, 0, 0),
                        Height = height,
                        Width = width,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Content = einsatz.Mitarbeiter.Name + "\n" + einsatz.Projekt.Name,
                        ToolTip = einsatz.ID.ToString(),
                        Background = color

                    };
                    b.MouseDoubleClick += einsatz_MouseDoubleClick;
                    mainGrid.Children.Add(b);

                }
            }
        }

        private void RefreshCalender()
        {
            mainGrid.Children.Clear();
            hourGrid.Children.Clear();
            HeaderGrid.Children.Clear();
            DrawGrid();
            DrawEinsaetze();
        }

        #region Handlers
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshCalender();
        }

        private void BtnWeekForward_OnClick(object sender, RoutedEventArgs e)
        {
            _currentWeek = _currentWeek.AddDays(7);
            RefreshCalender();
        }

        private void BtnWeekBack_OnClick(object sender, RoutedEventArgs e)
        {
            _currentWeek = _currentWeek.AddDays(-7);
            RefreshCalender();
        }

        private void einsatz_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateEinsatzFromKalender?.Invoke(sender, e);
        }
        #endregion
    }
}
