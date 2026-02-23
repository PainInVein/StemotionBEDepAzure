using Microsoft.Extensions.Logging;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.DBContext
{
    public class StemotionContextSeed
    {
        public static async Task SeedProductAsync(StemotionContext stemotionContext, ILogger logger, IPasswordService passwordService)
        {
            if (!stemotionContext.Roles.Any())
            {
                stemotionContext.Roles.AddRange(getRoles());
                await stemotionContext.SaveChangesAsync();
                logger.LogInformation("Seed Roles thành công.");
            }
            if(!stemotionContext.Users.Any())
            {
                var adminRole = stemotionContext.Roles.FirstOrDefault(r => r.Name == "Admin");
                if (adminRole != null)
                {
                    stemotionContext.Users.Add(new User
                    {
                        Email = "admin@stemotion.com",
                        Password = passwordService.HashPasswords("AdminStemotion123"),
                        FirstName = "System",
                        LastName = "Admin",
                        Phone = "0966340303",
                        RoleId = adminRole.Id,
                        Status = "Active",
                        CreatedAt = DateTime.Now
                    });
                    await stemotionContext.SaveChangesAsync();
                    logger.LogInformation("Seed tài khoản Admin mặc định thành công.");
                }
            }
        }
        private static IEnumerable<Role> getRoles()
        {
            return new List<Role>
            {
                new()
                {
                  Id = new Guid(),
                  Name = "Admin",
                  Description = "Admin",
                  Status = "Active"
                },
                new()
                {
                    Id= new Guid(),
                    Name = "Student",
                    Description = "Học sinh",
                    Status = "Active"
                },
                new()
                {
                    Id= new Guid(),
                    Name = "Parent",
                    Description = "Phụ huynh",
                    Status = "Active"
                }
            };
        }
    }
}
