using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ECommerceCore.Infrastructure.Services
{
    public class EmailService(IOptions<EmailSettings> options) : IEmailService
    {
        private readonly EmailSettings emailSettings = options.Value;

        /// <summary>
        /// Sends an email asynchronously with the specified recipient email address, subject, and HTML message content.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlMessage">The HTML content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //logic to send email

            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse(emailSettings.Email);
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = CreateEmailTemplate(htmlMessage)
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }

        /// <summary>
        /// Sends a two-factor authentication (2FA) code to the specified email address asynchronously.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="token">The 2FA token to be sent.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Send2FACodeToEmailAsync(string email, string token)
        {
            //logic to send email

            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse(emailSettings.Email);
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Your Two-Factor Authentication Code";

            var builder = new BodyBuilder
            {
                HtmlBody = Create2FAEmailTemplate(token)
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }

        /// <summary>
        /// Generates an HTML email template with the specified content.
        /// </summary>
        /// <param name="content">The content to be included in the email body.</param>
        /// <returns>A string containing the complete HTML email template.</returns>
        private string CreateEmailTemplate(string content)
        {
            return $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Email</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f4f4f9;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            padding: 20px;
                            border: 1px solid #e1e1e1;
                            border-radius: 8px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        }}
                        .email-header {{
                            text-align: center;
                            background: #4caf50;
                            color: #ffffff;
                            padding: 10px 0;
                            border-top-left-radius: 8px;
                            border-top-right-radius: 8px;
                        }}
                        .email-body {{
                            padding: 20px;
                            color: #333333;
                            line-height: 1.6;
                        }}
                        .email-footer {{
                            text-align: center;
                            font-size: 12px;
                            color: #888888;
                            margin-top: 20px;
                            border-top: 1px solid #dddddd;
                            padding-top: 10px;
                        }}
                        a {{
                            color: #4caf50;
                            text-decoration: none;
                        }}
                        a:hover {{
                            text-decoration: underline;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='email-header'>
                            <h1>Ecommerce Core</h1>
                        </div>
                        <div class='email-body'>
                            {content}
                        </div>
                        <div class='email-footer'>
                            <p>Thank you for using our services!</p>
                            <p><a href='https://yourwebsite.com'>Visit our website</a></p>
                        </div>
                    </div>
                </body>
                </html>
                ";
        }

        /// <summary>
        /// Generates an HTML template for a two-factor authentication (2FA) email with the specified code.
        /// </summary>
        /// <param name="code">The 2FA code to be included in the email.</param>
        /// <returns>A string containing the complete HTML 2FA email template.</returns>
        private string Create2FAEmailTemplate(string code)
        {
            return $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Two-Factor Authentication</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f8f9fa;
                            color: #333333;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            padding: 20px;
                            border: 1px solid #e1e1e1;
                            border-radius: 8px;
                            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            text-align: center;
                            background: #007bff;
                            color: #ffffff;
                            padding: 20px;
                            border-top-left-radius: 8px;
                            border-top-right-radius: 8px;
                        }}
                        .header h1 {{
                            margin: 0;
                            font-size: 24px;
                        }}
                        .body {{
                            padding: 20px;
                            text-align: center;
                        }}
                        .body p {{
                            font-size: 16px;
                            line-height: 1.6;
                            margin: 10px 0;
                        }}
                        .code {{
                            font-size: 24px;
                            font-weight: bold;
                            background: #f8f9fa;
                            padding: 10px 20px;
                            border: 1px dashed #007bff;
                            display: inline-block;
                            margin: 20px 0;
                        }}
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            font-size: 14px;
                            color: #888888;
                            border-top: 1px solid #dddddd;
                            padding-top: 10px;
                        }}
                        a {{
                            color: #007bff;
                            text-decoration: none;
                            font-weight: bold;
                        }}
                        a:hover {{
                            text-decoration: underline;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Secure Your Account</h1>
                        </div>
                        <div class='body'>
                            <p>Hello,</p>
                            <p>We have received a request to access your account using two-factor authentication (2FA). Your 2FA code is:</p>
                            <div class='code'>{code}</div>
                            <p>This code will expire in 10 minutes. If you did not request this, please ignore this email or contact support immediately.</p>
                            <p><a href='https://yourwebsite.com/support'>Contact Support</a></p>
                        </div>
                        <div class='footer'>
                            <p>Thank you for keeping your account secure.</p>
                            <p><a href='https://yourwebsite.com'>Visit Our Website</a></p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        /// <summary>
        /// Sends an order confirmation email to the specified recipient with order details.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="orderHeader">The order header containing order details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendOrderConfirmEmailAsync(string email, string subject, OrderHeader orderHeader)
        {
            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse(emailSettings.Email);
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = OrderConfirmEmailTemplate(orderHeader)
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }

        /// <summary>
        /// Generates the HTML email template for order confirmation with dynamic order details.
        /// </summary>
        /// <param name="orderHeader">The order header containing order and product details.</param>
        /// <returns>A string representing the HTML content of the email.</returns>
        private string OrderConfirmEmailTemplate(OrderHeader orderHeader)
        {
            var orderItemsHtml = string.Join( "", orderHeader.OrderDetails.Select(
                    item => $@"
                        <tr>
                            <td>{item.Product.Title}</td>
                            <td>{item.Count}</td>
                            <td>{item.Price:C}</td>
                            <td>{(item.Count * item.Price):C}</td>
                        </tr>" )
            );

            return $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f7f7f7;
                            margin: 0;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: auto;
                            background-color: #fff;
                            border-radius: 8px;
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                            padding: 20px;
                        }}
                        .header {{
                            text-align: center;
                            padding: 20px;
                            border-bottom: 2px solid #007bff;
                        }}
                        .header h1 {{
                            color: #007bff;
                        }}
                        .order-details {{
                            margin-top: 20px;
                        }}
                        .order-details p {{
                            font-size: 16px;
                            line-height: 1.6;
                        }}
                        .order-summary {{
                            width: 100%;
                            border-collapse: collapse;
                            margin-top: 20px;
                        }}
                        .order-summary th, .order-summary td {{
                            border: 1px solid #ddd;
                            padding: 10px;
                            text-align: left;
                        }}
                        .order-summary th {{
                            background-color: #007bff;
                            color: #fff;
                        }}
                        .footer {{
                            margin-top: 30px;
                            text-align: center;
                            font-size: 14px;
                            color: #777;
                        }}
                        @media (max-width: 600px) {{
                            .container {{
                                width: 100%;
                                padding: 10px;
                            }}
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Order Confirmation</h1>
                            <p>Thank you for your purchase!</p>
                        </div>
                        <div class='order-details'>
                            <p>Your order has been successfully placed.</p>
                            <p><strong>Order Number:</strong> #{orderHeader.Id}</p>
                            <p><strong>Order Date:</strong> {orderHeader.OrderDate:MMMM dd, yyyy}</p>
                            <p><strong>Total Amount:</strong> {orderHeader.OrderTotal:C}</p>
                            <p><strong>Shipping Address:</strong> {orderHeader.ApplicationUser.StreetAddress}</p>
                        </div>
                        <table class='order-summary'>
                            <thead>
                                <tr>
                                    <th>Product</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody>
                                {orderItemsHtml}
                            </tbody>
                        </table>
                        <div class='footer'>
                            <p>Thank you for shopping with us!</p>
                            <p>&copy; {DateTime.Now.Year} Your Company Name. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}