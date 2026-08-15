using VolunteerConnect2.Services;

namespace VolunteerConnect2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Task.Run(async () =>
            {
                try
                {
                    await DatabaseService.InitializeAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"DB Init Error: {ex.Message}");
                }
            });
        }
    }
}
