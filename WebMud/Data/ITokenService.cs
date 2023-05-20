namespace WebMud.Data
{
    public interface ITokenService
    {

     
        Task<bool> VerifyToken(string token );

        Task<bool> VerifyPwdToken(string token);
    }
}