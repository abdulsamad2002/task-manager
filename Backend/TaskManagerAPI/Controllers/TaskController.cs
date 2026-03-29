using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    /// <summary>
    /// Manages task CRUD operations. All endpoints require a valid JWT token.
    /// Role-based restrictions are applied per endpoint.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all tasks assigned to the currently authenticated user.
        /// Available to all roles.
        /// </summary>
        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var tasks = await _context.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }

        /// <summary>
        /// Returns all tasks across the entire organisation.
        /// Restricted to Admin and Manager roles.
        /// </summary>
        [HttpGet("all-tasks")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.GetAllTasksAdminAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Creates a new task and assigns it to a user.
        /// Restricted to Admin and Manager roles.
        /// </summary>
        [HttpPost("create")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateTask([FromBody] WorkTask task)
        {
            await _context.CreateTaskAsync(task);
            return Ok(new { message = "Task created successfully." });
        }

        /// <summary>
        /// Updates the status of an existing task (e.g., Pending → In Progress → Completed).
        /// Available to all authenticated roles.
        /// </summary>
        [HttpPut("update-status/{taskId:int}")]
        public async Task<IActionResult> UpdateStatus(int taskId, [FromBody] string status)
        {
            await _context.UpdateTaskStatusAsync(taskId, status);
            return Ok(new { message = "Task status updated successfully." });
        }

        /// <summary>
        /// Permanently deletes a task by ID.
        /// Restricted to Admin role only.
        /// </summary>
        [HttpDelete("delete/{taskId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            await _context.DeleteTaskAsync(taskId);
            return Ok(new { message = "Task deleted successfully." });
        }
    }
}