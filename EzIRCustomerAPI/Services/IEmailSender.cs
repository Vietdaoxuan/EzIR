using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Services
{
    /// <summary>
    ///     Service gửi mail.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        ///     Gửi email.
        /// </summary>
        /// <param name="email">địa chỉ gửi tới.</param>
        /// <param name="subject">Tiêu đề.</param>
        /// <param name="message">Nội dung.</param>
        /// <returns></returns>
        Task<CResponseMessage> SendEmailAsync(string email, string subject, string message, string sender, string mailServer, int port, string username, string password);
        Task<CResponseMessage> SendEmailAsync(string email, string cc, string subject, string message, string sender, string mailServer, int port, string username, string password);
        Task<CResponseMessage> SendEmailAttachAsync(string email, string cc, string subject, string message, string sender, string mailServer, int port, string username, string password, byte[] file);
   
    }

    /// <summary>
    ///     Service gửi mail implementation.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IErrorHandler _errorHandler;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        ///     contructor.
        /// </summary>
        /// <param name="emailSettings"></param>
        public EmailSender(IErrorHandler errorHandler, IConfiguration configuration)
        {
            //this._emailSettings = emailSettings;
            this._configuration = configuration;
            this._errorHandler = errorHandler;
        }

        /// <summary>
        ///     Gửi email.
        /// </summary>
        /// <param name="email">địa chỉ gửi tới.</param>
        /// <param name="subject">Tiêu đề.</param>
        /// <param name="message">Nội dung.</param>
        /// <returns></returns>
        public async Task<CResponseMessage> SendEmailAsync(string email, string subject, string message, string sender, string mailServer, int port, string username, string password)
        {
            
            return await Task.Run(() =>
            {
                try
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(sender, sender),
                        Subject = subject,
                        To = { new MailAddress(email) },                        
                        Body = message,
                    };

                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;

                    using (var client = new SmtpClient(mailServer, port))
                    {
                        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                        client.Credentials = new NetworkCredential(username, password);

                        // Note: only needed if the SMTP server requires authentication
                        client.Send(mailMessage);

                        return new CResponseMessage(0, "SEND_EMAIL_SUCCESS");
                    }
                }
                catch (Exception ex)
                {
                    this._errorHandler.WriteToFile(ex);
                    return new CResponseMessage(-1, "SEND_EMAIL_FAIL");
                }
            });
        }

        public async Task<CResponseMessage> SendEmailAsync(string email, string bcc, string subject, string message, string sender, string mailServer, int port, string username, string password)
        {

            return await Task.Run(() =>
            {
                try
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(sender, sender),
                        Subject = subject,
                        To = { new MailAddress(email) },
                        Bcc = { new MailAddress(bcc) },
                        Body = message
                      
                    };

                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;

                    using (var client = new SmtpClient(mailServer, port))
                    {
                        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                        client.Credentials = new NetworkCredential(username, password);

                        // Note: only needed if the SMTP server requires authentication
                        client.Send(mailMessage);

                        return new CResponseMessage(0, "SEND_EMAIL_SUCCESS");
                    }
                }
                catch (Exception ex)
                {
                    this._errorHandler.WriteToFile(ex);
                    return new CResponseMessage(-1, "SEND_EMAIL_FAIL");
                }
            });
        }

        public async Task<CResponseMessage> SendEmailAttachAsync(string email, string bcc, string subject, string message, string sender, string mailServer, int port, string username, string password, byte[] file)
        {

            return await Task.Run(() =>
            {
                try
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(sender, sender),
                        Subject = subject,
                        To = { new MailAddress(email) },
                        Bcc = { new MailAddress(bcc) },
                        Body = message,
                        Attachments = { new Attachment(new MemoryStream(file), "RemindCBTT.pdf") }
                    };

                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;

                    using (var client = new SmtpClient(mailServer, port))
                    {
                        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                        client.Credentials = new NetworkCredential(username, password);

                        // Note: only needed if the SMTP server requires authentication
                        client.Send(mailMessage);

                        return new CResponseMessage(0, "SEND_EMAIL_SUCCESS");
                    }
                }
                catch (Exception ex)
                {
                    this._errorHandler.WriteToFile(ex);
                    return new CResponseMessage(-1, "SEND_EMAIL_FAIL");
                }
            });
        }

    }

}
