using VolunteerConnect2.Models;
using SQLite;

namespace VolunteerConnect2.Services
{
    public class OpportunityService
    {
        private SQLiteAsyncConnection _db => DatabaseService.GetConnection();

        public async Task<List<VolunteerOpportunity>> GetAllAsync()
        {
            return await _db.Table<VolunteerOpportunity>().ToListAsync();
        }

        public async Task<VolunteerOpportunity> GetByIdAsync(int id)
        {
            return await _db.Table<VolunteerOpportunity>()
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<VolunteerOpportunity>> SearchAsync(string query)
        {
            query = query?.ToLower() ?? "";
            return await _db.Table<VolunteerOpportunity>()
                .Where(o => o.Title.ToLower().Contains(query))
                .ToListAsync();
        }

        public async Task<List<VolunteerOpportunity>> FilterByCategoryAsync(string category)
        {
            return await _db.Table<VolunteerOpportunity>()
                .Where(o => o.Category == category)
                .ToListAsync();
        }
    }
}
