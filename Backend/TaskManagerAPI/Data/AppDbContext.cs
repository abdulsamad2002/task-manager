using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // These DbSets represent your tables
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        
        public DbSet<WorkTask> Tasks { get; set; }

        // --- MAPPING YOUR 7 STORED PROCEDURES ---

        // 1. Login (Returns a User)
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var result = await this.Users
                .FromSqlRaw("EXEC dbo.sp_ValidateUser @Email={0}, @Password={1}", email, password)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        // 2. Register (Returns the new User)
        public async Task<User?> RegisterUserAsync(string fullName, string email, string password, int roleId)
        {
            var result = await this.Users
                .FromSqlRaw("EXEC dbo.sp_RegisterUser @FullName={0}, @Email={1}, @Password={2}, @RoleId={3}",
                             fullName, email, password, roleId)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        // 3. Get Employee Tasks (Returns a List)
        public async Task<List<WorkTask>> GetTasksByUserIdAsync(int userId)
        {
            return await this.Tasks
                .FromSqlRaw("EXEC dbo.sp_GetTasksByUserId @UserId={0}", userId)
                .ToListAsync();
        }

        // 4. Get All Admin Tasks (Returns a List)
        public async Task<List<WorkTask>> GetAllTasksAdminAsync()
        {
            return await this.Tasks
                .FromSqlRaw("EXEC dbo.sp_GetAllTasksAdmin")
                .ToListAsync();
        }

        // 5. Create Task (Executes Command)
        public async Task CreateTaskAsync(WorkTask t)
        {
            await this.Database.ExecuteSqlRawAsync("EXEC dbo.sp_CreateTask @Title={0}, @Description={1}, @Priority={2}, @DueDate={3}, @AssignedToUserId={4}",
                t.Title, t.Description, t.Priority, t.DueDate, t.AssignedToUserId);
        }

        // 6. Update Task Status (Executes Command)
        public async Task UpdateTaskStatusAsync(int taskId, string newStatus)
        {
            await this.Database.ExecuteSqlRawAsync("EXEC dbo.sp_UpdateTaskStatus @TaskId={0}, @NewStatus={1}", taskId, newStatus);
        }

        // 7. Delete Task (Executes Command)
        public async Task DeleteTaskAsync(int taskId)
        {
            await this.Database.ExecuteSqlRawAsync("EXEC dbo.sp_DeleteTask @TaskId={0}", taskId);
        }
    }
}