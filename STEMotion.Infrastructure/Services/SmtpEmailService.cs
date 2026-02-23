using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.Interfaces.ConfigurationInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpEmailSettings _settings;
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<SmtpEmailService> _logger;


        public SmtpEmailService(SmtpEmailSettings settings, ILogger<SmtpEmailService> logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger;
            _smtpClient = new SmtpClient();
        }

        public async Task SendEmailAsync(MailRequests request, CancellationToken cancellationToken = default)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };
            if(request.ToAddresses.Any())
            {
                foreach(var toAddress in request.ToAddresses)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(toAddress));
                }
            }
            else
            {
                var toAddress = request.ToAddress;
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
            try
            {
                await _smtpClient.ConnectAsync(_settings.SMTPServer, _settings.Port, _settings.UseSsl, cancellationToken);
                await _smtpClient.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
                await _smtpClient.SendAsync(emailMessage, cancellationToken);
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
                _smtpClient.Dispose();
            }
        }
    }
}
