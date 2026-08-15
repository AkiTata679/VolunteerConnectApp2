using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    public partial class MyRegistrationsPage : ContentPage
    {
        private RegistrationService _registrationService = new RegistrationService();
        private OpportunityService _opportunityService = new OpportunityService();

        public MyRegistrationsPage()
        {
            InitializeComponent();
            LoadRegistrations();
        }

        private async void LoadRegistrations()
        {
            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            var items = await _registrationService.GetAll();
            RegistrationsCollection.ItemsSource = items;
        }

        private async void ViewDetailsClicked(object sender, EventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            var registration = await _registrationService.GetById(id);
            if (registration == null)
            {
                await DisplayAlert("Error", "Registration not found.", "OK");
                return;
            }

            await Shell.Current.GoToAsync($"OpportunityDetailsPage?opportunityId={registration.OpportunityId}");
        }

        private async void EditClicked(object sender, EventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            await Shell.Current.GoToAsync($"EditRegistrationPage?registrationId={id}");
        }

        private async void DeleteClicked(object sender, EventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            bool confirm = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this registration?",
                "Delete",
                "Cancel");

            if (!confirm)
                return;

            await _registrationService.Delete(id);

            await DisplayAlert("Deleted", "Registration removed.", "OK");

            LoadRegistrations(); // Refresh list
        }
    }
}
