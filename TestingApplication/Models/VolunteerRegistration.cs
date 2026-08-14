namespace VolunteerConnect2.Models
{
    public class VolunteerRegistration
    {
        public int Id { get; set; }
        public int OpportunityId { get; set; }

        public string PreferredName { get; set; }
        public string ContactDetail { get; set; }
        public string Availability { get; set; }
        public string Notes { get; set; }

        public bool ConsentGiven { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
