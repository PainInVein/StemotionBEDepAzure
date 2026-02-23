using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEMotion.Application.DTO.RequestDTOs;
namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequests request, CancellationToken cancellationToken = new CancellationToken());
    }
}
