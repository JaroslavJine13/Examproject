namespace WebMud.Data
{
    public interface ISettingsService
    {

        Task<bool> SetSettings(
bool _IsEmailVerification,
bool _IsDefaultDark,
string _EmailGeneric,
string _EmailPassword,
string _EmailDisplayed,
string _Smtp,
int _Port,
bool _IsSsl,
bool _IsAdminVerification,
string _GoogleAnalyticsID,
string _MetaDescription,
string _MetaKeywords,
string _Title,
string _Facebook,
string _Instagram,
string _Linkedin,
string _Street,
string _Town,
string _City,
string _ICO,
string _DIC,
bool _IsPublicDefaultDark,
string _GoogleMapsIframe,
string _TristoneWeb,
string _GDPR,
string _Phone,
string _Email,
string _Phone2,
string _Email2,
string _EmailTemplate,
bool _IsContactUsPublished,
bool _IsGalleryPublished,
string _Name

            );

        Task<bool> GetSettings();


    }
}