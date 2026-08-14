using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    [QueryProperty(nameof(OpportunityId), "opportunityId")]
    public partial class RegistrationPage : ContentPage
    {
        private OpportunityService _opportunityService = new OpportunityService();
        private RegistrationService _registrationService = new RegistrationService();

        public int OpportunityId { get; set; }

        public RegistrationPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            await LoadOpportunity();
            SetupSubmitButton();
        }

        private async Task LoadOpportunity()
        {
            var opportunity = await _opportunityService.GetByIdAsync(OpportunityId);

            if (opportunity == null)
            {
                await DisplayAlert("Error", "The selected opportunity could not be found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            OpportunityTitleLabel.Text = opportunity.Title;
        }

        private void SetupSubmitButton()
        {
            SubmitButton.Clicked += async (s, e) =>
            {
                // Validation
                if (string.IsNullOrWhiteSpace(NameEntry.Text))
                {
                    await DisplayAlert("Missing Information", "Please enter your preferred name.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ContactEntry.Text))
                {
                    await DisplayAlert("Missing Information", "Please enter your contact details.", "OK");
                    return;
                }

                if (!ConsentCheckBox.IsChecked)
                {
                    await DisplayAlert("Consent Required", "You must provide privacy consent before registering.", "OK");
                    return;
                }

                // Create registration
                var registration = new VolunteerRegistration
                {
                    OpportunityId = OpportunityId,
                    PreferredName = NameEntry.Text.Trim(),
                    ContactDetail = ContactEntry.Text.Trim(),
                    Availability = AvailabilityEntry.Text?.Trim(),
                    Notes = NotesEditor.Text?.Trim(),
                    ConsentGiven = true,
                    RegistrationDate = DateTime.Now
                };

                await _registrationService.AddAsync(registration);

                await DisplayAlert("Success", "Your registration has been saved.", "OK");
                await Shell.Current.GoToAsync("//MyRegistrationPage");
            };
        }
    }
}
