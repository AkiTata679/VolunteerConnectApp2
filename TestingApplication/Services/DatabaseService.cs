using SQLite;
using VolunteerConnect2.Models;

namespace VolunteerConnect2.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _connection;
        public static bool IsInitialized { get; private set; } = false;

        private static readonly SemaphoreSlim _initLock = new SemaphoreSlim(1, 1);

        public static async Task InitializeAsync()
        {
            if (IsInitialized)
                return;

            await _initLock.WaitAsync();
            try
            {
                if (IsInitialized)
                    return;

                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "volunteerconnect.db");
                _connection = new SQLiteAsyncConnection(dbPath);

                await _connection.CreateTableAsync<VolunteerOpportunity>();
                await _connection.CreateTableAsync<VolunteerRegistration>();

                await SeedOpportunitiesAsync();

                IsInitialized = true;
            }
            finally
            {
                _initLock.Release();
            }
        }

        private static async Task SeedOpportunitiesAsync()
        {
            var count = await _connection.Table<VolunteerOpportunity>().CountAsync();
            if (count > 0)
                return;

            var sampleData = new List<VolunteerOpportunity>
            {
                new VolunteerOpportunity
                {
                    Title = "Community Garden Helper",
                    Category = "Community",
                    Date = DateTime.Today.AddDays(3),
                    Time = "10:00 AM",
                    Location = "Local Community Garden",
                    Description = "Assist with planting, watering, and maintaining the community garden.",
                    Requirements = "Basic gardening skills helpful.",
                    AvailablePlaces = 5,
                    ImageName = "garden.jpg",
                    IsAvailable = true
                },
                new VolunteerOpportunity
                {
                    Title = "Library Support Volunteer",
                    Category = "Education",
                    Date = DateTime.Today.AddDays(5),
                    Time = "1:00 PM",
                    Location = "City Library",
                    Description = "Help organise books, assist visitors, and support library events.",
                    Requirements = "Friendly and organised.",
                    AvailablePlaces = 3,
                    ImageName = "library.jpg",
                    IsAvailable = true
                },
                new VolunteerOpportunity
                {
                    Title = "Food Bank Packing Assistant",
                    Category = "Community",
                    Date = DateTime.Today.AddDays(2),
                    Time = "9:00 AM",
                    Location = "Central Food Bank",
                    Description = "Pack food parcels for families in need.",
                    Requirements = "Able to lift light boxes.",
                    AvailablePlaces = 8,
                    ImageName = "foodbank.jpg",
                    IsAvailable = true
                },
                new VolunteerOpportunity
                {
                    Title = "Beach Clean-up Volunteer",
                    Category = "Environment",
                    Date = DateTime.Today.AddDays(7),
                    Time = "8:00 AM",
                    Location = "Sunny Beach",
                    Description = "Help clean up rubbish and protect marine life.",
                    Requirements = "Comfortable walking on sand.",
                    AvailablePlaces = 20,
                    ImageName = "beach.jpg",
                    IsAvailable = true
                },
                new VolunteerOpportunity
                {
                    Title = "Community Event Assistant",
                    Category = "Events",
                    Date = DateTime.Today.AddDays(10),
                    Time = "4:00 PM",
                    Location = "Community Hall",
                    Description = "Assist with setup, coordination, and pack-down of a community event.",
                    Requirements = "Good communication skills.",
                    AvailablePlaces = 10,
                    ImageName = "event.jpg",
                    IsAvailable = true
                },
                new VolunteerOpportunity
                {
                    Title = "Digital Skills Support Volunteer",
                    Category = "Technology",
                    Date = DateTime.Today.AddDays(6),
                    Time = "11:00 AM",
                    Location = "Tech Learning Centre",
                    Description = "Help community members learn basic digital skills.",
                    Requirements = "Basic computer knowledge.",
                    AvailablePlaces = 4,
                    ImageName = "digitalskills.jpg",
                    IsAvailable = true
                }
            };

            await _connection.InsertAllAsync(sampleData);
        }

        public static SQLiteAsyncConnection GetConnection()
        {
            if (!IsInitialized)
                throw new Exception("Database not initialized yet.");

            return _connection;
        }
    }
}
