using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    [QueryProperty(nameof(OpportunityId), "opportunityId")]
    public partial class RegistrationPage : ContentPage
    {
        private OpportunityService _opportunityService = new OpportunityService();
        private RegistrationService _registrationService = new RegistrationService();

        private VolunteerOpportunity _opportunity;

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
        }

        private async Task LoadOpportunity()
        {
            _opportunity = await _opportunityService.GetByIdAsync(OpportunityId);

            if (_opportunity == null)
            {
                await DisplayAlert("Error", "Opportunity not found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            OpportunityTitleLabel.Text = _opportunity.Title;
        }

        private async void SubmitClicked(object sender, EventArgs e)
        {
            string preferredName = PreferredNameEntry.Text?.Trim();
            string contact = ContactEntry.Text?.Trim();
            string availability = AvailabilityEntry.Text?.Trim();
            string notes = NotesEditor.Text?.Trim();

            // Required fields
            if (string.IsNullOrWhiteSpace(preferredName) ||
                string.IsNullOrWhiteSpace(contact) ||
                string.IsNullOrWhiteSpace(availability))
            {
                await DisplayAlert("Missing Information",
                    "Please fill in all required fields (name, contact, availability).",
                    "OK");
                return;
            }

            // Contact validation
            bool validEmail = contact.Contains("@") && contact.Contains(".");
            bool validPhone = contact.All(char.IsDigit) && contact.Length >= 7;

            if (!validEmail && !validPhone)
            {
                await DisplayAlert("Invalid Contact",
                    "Please enter a valid email address or phone number.",
                    "OK");
                return;
            }

            // Privacy consent
            if (!PrivacyConsentCheckBox.IsChecked)
            {
                await DisplayAlert("Consent Required",
                    "You must provide privacy consent before submitting.",
                    "OK");
                return;
            }

            // Save registration
            var registration = new VolunteerRegistration
            {
                OpportunityId = _opportunity.Id,
                PreferredName = preferredName,
                ContactDetail = contact,
                Availability = availability,
                Notes = notes,
                ConsentGiven = true,
                RegistrationDate = DateTime.Now
            };

            await _registrationService.AddAsync(registration);

            await DisplayAlert("Success", "Registration submitted!", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
