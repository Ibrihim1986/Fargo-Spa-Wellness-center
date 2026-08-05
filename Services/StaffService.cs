using Family_and_Spa_Wellness.Data;
using Family_and_Spa_Wellness.Models;
using Microsoft.EntityFrameworkCore;

namespace Family_and_Spa_Wellness.Services
{
    public class StaffService
    {
        private readonly AppDbContext _db;

        public StaffService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Staff>> GetAllAsync()
        {
            return await _db.Staff.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToListAsync();
        }

        public async Task<Staff?> GetByIdAsync(int id)
        {
            return await _db.Staff.FindAsync(id);
        }

        public async Task<Staff> CreateAsync(Staff staff)
        {
            _db.Staff.Add(staff);
            await _db.SaveChangesAsync();
            return staff;
        }

        public async Task UpdateRoleAsync(int id, Role newRole)
        {
            var staff = await _db.Staff.FindAsync(id);
            if (staff is null) return;
            staff.Role = newRole;

            // If there is a corresponding application user with same email, update their role string too
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == staff.Email);
            if (user is not null)
            {
                user.Role = newRole switch
                {
                    Role.Administrator => "Admin",
                    Role.Manager => "Manager",
                    Role.Reception => "Reception",
                    Role.Therapist => "Provider",
                    Role.Viewer => "Viewer",
                    _ => user.Role
                };
            }

            await _db.SaveChangesAsync();
        }

        public async Task SetActiveAsync(int id, bool isActive)
        {
            var staff = await _db.Staff.FindAsync(id);
            if (staff is null) return;
            staff.IsActive = isActive;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var staff = await _db.Staff.FindAsync(id);
            if (staff is null) return;
            _db.Staff.Remove(staff);
            await _db.SaveChangesAsync();
        }

        // Simple permission check helper. Customize as needed for resources.
        public bool HasPermission(Role role, Role requiredMinimum)
        {
            return role <= requiredMinimum; // Administrator(0) <= Manager(1) etc.
        }
    }
}
