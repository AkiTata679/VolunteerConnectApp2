using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    [QueryProperty(nameof(RegistrationId), "registrationId")]
    public partial class EditRegistrationPage : ContentPage
    {
        private RegistrationService _registrationService = new RegistrationService();
        private OpportunityService _opportunityService = new OpportunityService();

        public int RegistrationId { get; set; }

        private VolunteerRegistration _registration;
        private VolunteerOpportunity _opportunity;

        public EditRegistrationPage()
        {
            InitializeComponent();
            SetupSaveButton();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            await LoadRegistration();
        }

        private async Task LoadRegistration()
        {
            _registration = await _registrationService.GetByIdAsync(RegistrationId);

            if (_registration == null)
            {
                await DisplayAlert("Error", "Registration not found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            _opportunity = await _opportunityService.GetByIdAsync(_registration.OpportunityId);

            OpportunityTitleLabel.Text = _opportunity?.Title ?? "Unknown Opportunity";

            NameEntry.Text = _registration.PreferredName;
            ContactEntry.Text = _registration.ContactDetail;
            AvailabilityEntry.Text = _registration.Availability;
            NotesEditor.Text = _registration.Notes;
            ConsentCheckBox.IsChecked = _registration.ConsentGiven;
        }

        private void SetupSaveButton()
        {
            SaveButton.Clicked += async (s, e) =>
            {
                if (_registration == null)
                {
                    await DisplayAlert("Error", "Registration not loaded.", "OK");
                    return;
                }

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
                    await DisplayAlert("Consent Required", "You must provide privacy consent.", "OK");
                    return;
                }

                _registration.PreferredName = NameEntry.Text.Trim();
                _registration.ContactDetail = ContactEntry.Text.Trim();
                _registration.Availability = AvailabilityEntry.Text?.Trim();
                _registration.Notes = NotesEditor.Text?.Trim();
                _registration.ConsentGiven = ConsentCheckBox.IsChecked;

                var success = await _registrationService.UpdateAsync(_registration);

                if (!success)
                {
                    await DisplayAlert("Error", "Failed to update registration.", "OK");
                    return;
                }

                await DisplayAlert("Success", "Your registration has been updated.", "OK");
                await Shell.Current.GoToAsync("..");
            };
        }
    }
}
