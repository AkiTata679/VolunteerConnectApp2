using VolunteerConnect2.Views;

namespace VolunteerConnect2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute("OpportunityDetailsPage", typeof(OpportunityDetailsPage));
            Routing.RegisterRoute("RegistrationPage", typeof(RegistrationPage));
            Routing.RegisterRoute("EditRegistrationPage", typeof(EditRegistrationPage));
        }
    }
}
