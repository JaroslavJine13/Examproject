namespace WebMud.Data
{
    public interface IEmailServices
    {
        Task <bool> SendEmailAsync(string toAddress, string subject, string body);




    }
}