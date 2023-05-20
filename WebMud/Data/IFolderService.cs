using WebMud.Models;

namespace WebMud.Data
{
    public interface IFolderService
    {


        Task<bool> AddFolder(Folder folder);

        Task<bool> DeleteFolder(Guid id);

        Task<bool> ActivateFolder(Guid id, bool isactive);

        Task<Folder> GetFolder(Guid id);

        Task<List<Folder>> GetFolderOnlyActive();

        Task<List<Folder>> GetFolderAll();

        Task<int> GetTotalFolderCount();

        Task<bool> UpdateFolder(Folder folder);



    }
}