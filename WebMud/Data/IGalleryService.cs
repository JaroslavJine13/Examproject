using WebMud.Models;

namespace WebMud.Data
{
    public interface IGalleryService
    {


        Task<bool> AddGallery(Gallery gallery);

        Task<bool> DeleteGallery(Guid id, string file);

        Task<bool> ActivateGallery(Guid id, bool isactive);

        Task<Gallery> GetGallery(Guid id);

        Task<List<Gallery>> GetGalleryOnlyActive();

        Task<List<Gallery>> GetGalleryOnlyActiveAll(Guid id);

        Task<List<Gallery>> GetGalleryAll();

        Task<int> GetTotalGalleryCount();

        Task<bool> UpdateGallery(Gallery gallery);

        Task<List<Gallery>> GetGalleryOnlyActiveAll();

        Task<List<Gallery>> GetGalleryOnlyActiveWelcomeScr();

    }
}