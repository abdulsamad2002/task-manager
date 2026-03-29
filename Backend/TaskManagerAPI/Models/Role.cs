
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}