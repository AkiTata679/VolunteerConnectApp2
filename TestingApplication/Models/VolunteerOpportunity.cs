namespace VolunteerConnect2.Models
{
    public class VolunteerOpportunity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public int AvailablePlaces { get; set; }
        public string ImageName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
