using VolunteerConnect2.Models;
using VolunteerConnect2.Services;

namespace VolunteerConnect2.Views
{
    public partial class OpportunitiesPage : ContentPage
    {
        private OpportunityService _opportunityService = new OpportunityService();
        private List<VolunteerOpportunity> _allOpportunities = new();

        public OpportunitiesPage()
        {
            InitializeComponent();
            LoadData();
            SetupEvents();
        }

        private async void LoadData()
        {
            if (!DatabaseService.IsInitialized)
                await DatabaseService.InitializeAsync();

            _allOpportunities = await _opportunityService.GetAllAsync();
            OpportunitiesList.ItemsSource = _allOpportunities;

            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _allOpportunities
                .Select(o => o.Category)
                .Distinct()
                .ToList();

            categories.Insert(0, "All Categories");
            CategoryPicker.ItemsSource = categories;
            CategoryPicker.SelectedIndex = 0;
        }

        private void SetupEvents()
        {
            SearchBar.TextChanged += (s, e) => ApplyFilters();
            CategoryPicker.SelectedIndexChanged += (s, e) => ApplyFilters();
            AvailabilitySwitch.Toggled += (s, e) => ApplyFilters();

            OpportunitiesList.SelectionChanged += async (s, e) =>
            {
                if (e.CurrentSelection.FirstOrDefault() is VolunteerOpportunity selected)
                {
                    await Shell.Current.GoToAsync($"OpportunityDetailsPage?opportunityId={selected.Id}");
                    OpportunitiesList.SelectedItem = null;
                }
            };
        }

        private void ApplyFilters()
        {
            IEnumerable<VolunteerOpportunity> filtered = _allOpportunities;

            if (!string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                string query = SearchBar.Text.ToLower();
                filtered = filtered.Where(o => o.Title.ToLower().Contains(query));
            }

            if (CategoryPicker.SelectedIndex > 0)
            {
                string category = CategoryPicker.SelectedItem.ToString();
                filtered = filtered.Where(o => o.Category == category);
            }

            if (AvailabilitySwitch.IsToggled)
            {
                filtered = filtered.Where(o => o.IsAvailable);
            }

            OpportunitiesList.ItemsSource = filtered.ToList();
        }

        // View Details button 
        private async void ViewDetailsClicked(object sender, EventArgs e)
        {
            var opportunity = (sender as Button)?.BindingContext as VolunteerOpportunity;
            if (opportunity != null)
            {
                await Shell.Current.GoToAsync($"OpportunityDetailsPage?opportunityId={opportunity.Id}");
            }
        }
    }
}
