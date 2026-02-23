using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class UserResponseDTO
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String RoleName { get; set; }
        public int? GradeLevel { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
