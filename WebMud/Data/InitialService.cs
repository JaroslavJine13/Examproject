

namespace WebMud.Data
{
    public class InitialService: IInitialService
    {

        private readonly ISettingsService _settingsService;

        public InitialService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }



        public async Task<bool> InitializeAsync()
        {

            await _settingsService.GetSettings();
 
            return true;

        }


    }
}
