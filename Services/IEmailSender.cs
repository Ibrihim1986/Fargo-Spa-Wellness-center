namespace Family_and_Spa_Wellness.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string toAddress, string subject, string body);
}
