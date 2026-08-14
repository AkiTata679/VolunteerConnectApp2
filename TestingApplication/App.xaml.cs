using VolunteerConnect2.Services;

namespace VolunteerConnect2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the Shell as the root of the app
            MainPage = new AppShell();

            // Start database initialization in the background
            Task.Run(async () =>
            {
                try
                {
                    await DatabaseService.InitializeAsync();
                }
                catch (Exception ex)
                {
                    // log or handle initialization errors
                    System.Diagnostics.Debug.WriteLine($"DB Init Error: {ex.Message}");
                }
            });
        }
    }
}
