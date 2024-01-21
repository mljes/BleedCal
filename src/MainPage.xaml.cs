
using System.Globalization;

namespace BleedCal
{

    public partial class MainPage : ContentPage
    {
        int displayedMonth;
        int displayedYear;

        DateManager dateManager;

        public MainPage()
        {
            InitializeComponent();

            displayedMonth = DateTime.Now.Month;
            displayedYear = DateTime.Now.Year;

            dateManager = DateManager.Instance;
            dateManager.LoadData();

            bool isDebug = false;

#if DEBUG
            isDebug = true;
#endif

            this.debugListView.IsVisible = isDebug;

            DrawCalendar();
        }

        private void DrawCalendar()
        {
            this.calGrid.Clear();
            this.monthLabel.Text = $"{DateTimeFormatInfo.CurrentInfo.GetMonthName(displayedMonth)} {displayedYear}";

#if DEBUG
            this.datesListDisplay.Text = $"Current dates:\n{dateManager.ToString()}";
#endif

            bool monthIsPast = (displayedMonth < DateTime.Now.Month);
            bool yearIsPast = (displayedYear < DateTime.Now.Year);
            this.buttonMonthNext.IsEnabled = yearIsPast || monthIsPast;
            
            var firstOfMonth = new DateOnly(displayedYear, displayedMonth, 1);
            var currentDate = firstOfMonth;

            for (int row = 0; row < 6; row++) //rows
            {
                if (currentDate.Month != displayedMonth) break;
                var colStart = ((int)currentDate.DayOfWeek);

                for (int col = colStart; col < 7; col++)
                {
                    if (currentDate.Month != displayedMonth) break;

                    var button = new Button()
                    {
                        BindingContext = currentDate,
                        Text = currentDate.Day.ToString(),
                        BackgroundColor = dateManager.periodDays.Contains(currentDate) ? Colors.DarkRed : Colors.Plum,
                        WidthRequest = 50,
                        HeightRequest = 50,
                        CornerRadius = 25
                    };

                    button.Clicked += (sender, args) => buttonDay_Clicked(sender, args);

                    this.calGrid.Add(button, col, row);

                    currentDate = currentDate.AddDays(1);
                }
            }
        }

        private void buttonMonthPrev_Clicked(object sender, EventArgs e)
        {
            if ((displayedMonth - 1) < 1)
            {
                displayedMonth = 12;
                displayedYear--;
            }
            else
            {
                displayedMonth--;
            }

            DrawCalendar();
        }

        private void buttonMonthNext_Clicked(object sender, EventArgs e)
        {
            if ((displayedMonth + 1) > 12)
            {
                displayedMonth = 1;
                displayedYear++;
            }
            else
            {
                displayedMonth++;
            }

            DrawCalendar();
        }

        private void buttonDay_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var buttonDate = (DateOnly)(button.BindingContext);

            if (dateManager.periodDays.Contains(buttonDate))
            {
                dateManager.periodDays.Remove(buttonDate);
                button.BackgroundColor = Colors.Plum;
            }
            else
            {
                dateManager.periodDays.Add(buttonDate);
                button.BackgroundColor = Colors.DarkRed;
            }

#if DEBUG
            this.datesListDisplay.Text = $"Current dates:\n{dateManager.ToString()}";
#endif

        }
    }
}