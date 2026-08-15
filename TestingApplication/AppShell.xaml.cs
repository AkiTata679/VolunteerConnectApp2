using VolunteerConnect2.Views;

namespace VolunteerConnect2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Required route registrations
            Routing.RegisterRoute(nameof(OpportunityDetailsPage), typeof(OpportunityDetailsPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(EditRegistrationPage), typeof(EditRegistrationPage));
        }
    }
}
