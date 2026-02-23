using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class PasswordService : IPasswordService
    {
        #region Hash Password
        public string HashPasswords(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        #endregion Hash Password
        #region VerifyPassword
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        #endregion VerifyPassword
    }
}
