using WebMud.Models;

namespace WebMud.Data
{
    public interface IClientService
    {


        Task<bool> AddClient(Client Client);

        Task<bool> DeleteClient(Guid id);

        Task<bool> ActivateClient(Guid id, bool isactive);

        Task<Client> GetClient(Guid id);

        Task<List<Client>> GetClientOnlyActive();

        Task<List<Client>> GetClientAll();

        Task<int> GetTotalClientCount();

        Task<bool> UpdateClient(Client Client);



    }
}