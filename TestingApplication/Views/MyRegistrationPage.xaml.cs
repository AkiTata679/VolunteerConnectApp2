using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    public partial class MyRegistrationPage : ContentPage
    {
        private RegistrationService _registrationService = new RegistrationService();
        private OpportunityService _opportunityService = new OpportunityService();

        private List<RegistrationDisplayModel> _displayList = new();

        public MyRegistrationPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            await LoadRegistrations();
            SetupSelection();
        }

        private async Task LoadRegistrations()
        {
            var registrations = await _registrationService.GetAllAsync();
            var opportunities = await _opportunityService.GetAllAsync();

            _displayList = registrations.Select(reg =>
            {
                var opp = opportunities.FirstOrDefault(o => o.Id == reg.OpportunityId);

                return new RegistrationDisplayModel
                {
                    Id = reg.Id,
                    OpportunityId = reg.OpportunityId,
                    OpportunityTitle = opp?.Title ?? "Unknown Opportunity",
                    ContactDetail = reg.ContactDetail,
                    RegistrationDate = reg.RegistrationDate
                };
            }).ToList();

            RegistrationsList.ItemsSource = _displayList;
        }

        private void SetupSelection()
        {
            RegistrationsList.SelectionChanged += async (s, e) =>
            {
                if (e.CurrentSelection.FirstOrDefault() is RegistrationDisplayModel selected)
                {
                    string action = await DisplayActionSheet(
                        "Registration Options",
                        "Cancel",
                        null,
                        "View / Edit",
                        "Delete");

                    if (action == "View / Edit")
                    {
                        await Shell.Current.GoToAsync(
                            $"//EditRegistrationPage?registrationId={selected.Id}");
                    }
                    else if (action == "Delete")
                    {
                        bool confirm = await DisplayAlert(
                            "Confirm Deletion",
                            "Are you sure you want to delete this registration?",
                            "Yes",
                            "No");

                        if (confirm)
                        {
                            await _registrationService.DeleteAsync(selected.Id);
                            await LoadRegistrations();
                        }
                    }

                    RegistrationsList.SelectedItem = null;
                }
            };
        }
    }

    public class RegistrationDisplayModel
    {
        public int Id { get; set; }
        public int OpportunityId { get; set; }
        public string OpportunityTitle { get; set; }
        public string ContactDetail { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
