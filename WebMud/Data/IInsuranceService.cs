using WebMud.Models;

namespace WebMud.Data
{
    public interface IInsuranceService
    {


        Task<bool> AddInsurance(Insurance Insurance);

        Task<bool> DeleteInsurance(Guid id);

        Task<bool> ActivateInsurance(Guid id, bool isactive);

        Task<Insurance> GetInsurance(Guid id);

 

        Task<List<Insurance>> GetInsuranceOnlyActiveAll(Guid id);

        Task<List<Insurance>> GetInsuranceAll();

        Task<int> GetTotalInsuranceCount();

        Task<bool> UpdateInsurance(Insurance Insurance);

        Task<List<Insurance>> GetInsuranceOnlyActiveAll();

 

    }
}