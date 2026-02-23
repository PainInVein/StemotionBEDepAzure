using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IJWTService
    {
        string GenerateToken(Guid userId,string email, string role);
        ClaimsPrincipal ValidateToken(string token);
    }
}
