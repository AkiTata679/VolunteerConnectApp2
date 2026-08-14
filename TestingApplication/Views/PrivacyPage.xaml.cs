namespace VolunteerConnect2.Views
{
    public partial class PrivacyPage : ContentPage
    {
        public PrivacyPage()
        {
            InitializeComponent();
            SetupActions();
        }

        private void SetupActions()
        {
            ClearMessagesButton.Clicked += async (s, e) =>
            {
                bool confirm = await DisplayAlert(
                    "Clear Messages",
                    "This will clear any temporary messages shown in the app. Your registrations will not be deleted.",
                    "Clear",
                    "Cancel");

                if (confirm)
                {
                    await DisplayAlert("Messages Cleared", "Temporary messages have been cleared.", "OK");
                }
            };
        }
    }
}
