using VolunteerConnect2.Views;

namespace VolunteerConnect2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("OpportunityDetailsPage", typeof(OpportunityDetailsPage));
            Routing.RegisterRoute("RegistrationPage", typeof(RegistrationPage));
            Routing.RegisterRoute("EditRegistrationPage", typeof(EditRegistrationPage));
        }
    }
}
