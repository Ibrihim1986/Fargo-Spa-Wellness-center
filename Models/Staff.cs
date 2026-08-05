using System;
using System.ComponentModel.DataAnnotations;

namespace Family_and_Spa_Wellness.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public Role Role { get; set; } = Role.Viewer;

        // Whether account is active. When false, user should be denied login (integrate with auth flow).
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
    }
}
