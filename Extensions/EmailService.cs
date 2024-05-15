using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Cryptography;

namespace Extensions
{
    public static class EmailService
    {
        private const string Email = "Regis.youtube@yandex.ru";
        private const string Password = "hnuhkbyweirzkslz";
        private const string SmtpHost = "smtp.yandex.ru";
        private const string ServiceName = "БИМ";
        private const int Port = 587;

        public static int GenerateNewCode()
        {
            int max = 100000000, min = 10000000;
            long diff = (long)max - min;
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = BitConverter.ToUInt32(RandomNumberGenerator.GetBytes(sizeof(uint)));
            } while (ui >= upperBound);
            return (int)(min + (ui % diff));
        } 
        public static string CreateEmailRegistrationConfirmationMessage(string username, int code) => @"
<h2>Здравствуйте, "+username+@"!</h2>
Вы указали данную почту для регистрации на проекте "+ServiceName+@"
<p>Ваш код регистрации: <b>"+code+@"</b></p><br/>
Если Вы получили это письмо по ошибке, пожалуйста - проигнорируйте его.
";
        public static string CreatePasswordRecoveryEmailMessage(string username, int code) => @"
<h2>Здравствуйте, "+username+@"!</h2>
Вы пытаетесь поменять Ваш текущий пароль. Для этого введите код подтверждения.
<p>Код подтверждения: <b>"+code+@"</b></p><br/>
Если Вы не меняли пароль, пожалуйста - проигнорируйте данное письмо.
";
        public static string CreatePasswordRecoveryNotificationMessage(string username) => @"
<h2>Здравствуйте, "+username+@"!</h2>
Вы успешно поменяли пароль.
Если Вы этого не делали, срочно зайдите в свой аккаунт и смените пароль.
";
        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(ServiceName, Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message,
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(SmtpHost, Port, false);
            await client.AuthenticateAsync(Email, Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
