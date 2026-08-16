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
            if (id <= 0)
                return null;

            return await _db.Table<VolunteerRegistration>()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(VolunteerRegistration registration)
        {
            await _db.InsertAsync(registration);
        }

        public async Task<bool> UpdateAsync(VolunteerRegistration registration)
        {
            if (registration == null || registration.Id <= 0)
                return false;

            int rows = await _db.UpdateAsync(registration);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            var reg = await GetByIdAsync(id);
            if (reg == null)
                return false;

            int rows = await _db.DeleteAsync(reg);
            return rows > 0;
        }

        public Task<List<VolunteerRegistration>> GetAll() => GetAllAsync();
        public Task<VolunteerRegistration> GetById(int id) => GetByIdAsync(id);
        public Task<bool> Delete(int id) => DeleteAsync(id);
    }
}
