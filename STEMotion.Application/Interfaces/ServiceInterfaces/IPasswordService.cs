using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IPasswordService
    {
        string HashPasswords(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
