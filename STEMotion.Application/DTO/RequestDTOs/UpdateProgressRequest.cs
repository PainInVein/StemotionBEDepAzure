using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class UpdateProgressRequest
    {
        public int CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
    }
}
