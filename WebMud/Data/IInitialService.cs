namespace WebMud.Data
{
    public interface IInitialService
    {

        Task<bool> InitializeAsync();
    }
}