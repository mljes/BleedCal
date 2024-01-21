namespace BleedCal
{
    public partial class App : Application
    {
        DateManager dateManager;

        public App()
        {
            UserAppTheme = AppTheme.Light;
            
            InitializeComponent();

            dateManager = DateManager.Instance;
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            dateManager.LoadData();
            base.OnStart();
        }

        protected override void OnSleep()
        {
            dateManager.SaveDataToFile();
            base.OnSleep();
        }
    }
}