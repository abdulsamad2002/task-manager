using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // This locks the whole controller; you MUST have a JWT token
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Get Tasks for the Logged-in User (Calls sp_GetTasksByUserId)
        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var tasks = await _context.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }

        // 2. Admin/Manager: Get ALL Tasks (Calls sp_GetAllTasksAdmin)
        [HttpGet("all-tasks")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.GetAllTasksAdminAsync();
            return Ok(tasks);
        }

        // 3. Admin/Manager: Create Task (Calls sp_CreateTask)
        // 3. Admin/Manager: Create Task (Matches your CreateTaskAsync(WorkTask t))
        [HttpPost("create")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateTask([FromBody] WorkTask task)
        {
            // You are passing the whole 'task' object here as your method requires
            await _context.CreateTaskAsync(task);
            return Ok(new { message = "Task created successfully" });
        }

        // 4. Update Task Status (Matches your UpdateTaskStatusAsync(int taskId, string newStatus))
        [HttpPut("update-status/{taskId}")]
        public async Task<IActionResult> UpdateStatus(int taskId, [FromBody] string status)
        {
            // We pass taskId and the string status to match (int taskId, string newStatus)
            await _context.UpdateTaskStatusAsync(taskId, status);
            return Ok(new { message = "Status updated successfully" });
        }

        // 5. Admin Only: Delete Task (Calls sp_DeleteTask)
        [HttpDelete("delete/{taskId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            await _context.DeleteTaskAsync(taskId);
            return Ok(new { message = "Task deleted" });
        }
    }
}