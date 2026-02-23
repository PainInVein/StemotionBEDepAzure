using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs.StudentResponseDTO
{
    public class AddChildrenResponseDTO
    {
        public Guid ParentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? AvatarUrl { get; set; }
        public int? GradeLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User Parent { get; set; }
        public string? Status { get; set; }
    }
}
