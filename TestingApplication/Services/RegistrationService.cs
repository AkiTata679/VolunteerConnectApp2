using VolunteerConnect2.Models;
using SQLite;

namespace VolunteerConnect2.Services
{
    public class RegistrationService
    {
        private SQLiteAsyncConnection _db => DatabaseService.GetConnection();

        public async Task<List<VolunteerRegistration>> GetAllAsync()
        {
            return await _db.Table<VolunteerRegistration>().ToListAsync();
        }

        public async Task<VolunteerRegistration> GetByIdAsync(int id)
        {
            return await _db.Table<VolunteerRegistration>()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(VolunteerRegistration registration)
        {
            await _db.InsertAsync(registration);
        }

        public async Task UpdateAsync(VolunteerRegistration registration)
        {
            await _db.UpdateAsync(registration);
        }

        public async Task DeleteAsync(int id)
        {
            var reg = await GetByIdAsync(id);
            if (reg != null)
                await _db.DeleteAsync(reg);
        }

        // ⭐ ADDED — wrapper for MyRegistrationsPage
        public Task<List<VolunteerRegistration>> GetAll()
        {
            return GetAllAsync();
        }

        // ⭐ ADDED — wrapper for MyRegistrationsPage
        public Task<VolunteerRegistration> GetById(int id)
        {
            return GetByIdAsync(id);
        }

        // ⭐ ADDED — wrapper for MyRegistrationsPage
        public Task Delete(int id)
        {
            return DeleteAsync(id);
        }
    }
}
