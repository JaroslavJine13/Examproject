using Dapper;
using System.Data;
using WebMud.Dapper;
using WebMud.Models;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;

namespace WebMud.Data
{
    public class SettingsService : ISettingsService
    {
        private readonly SqlConnectionConfiguration _configuration;
        private readonly IConfiguration _Aconfiguration;
        public SettingsService(SqlConnectionConfiguration configuration, IConfiguration aconfiguration)
        {
            _configuration = configuration;
            _Aconfiguration = aconfiguration;
        }

        public async Task<bool> GetSettings()
        {


            using (var conn = new SqlConnection(_configuration.Value))
            {


                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {

                    AdminSettings.IsEmailVerification = await conn.ExecuteScalarAsync<bool>("SELECT TOP 1 IsEmailVerification FROM Settings;");
                    AdminSettings.IsDefaultDark = await conn.ExecuteScalarAsync<bool>("SELECT TOP 1 IsDefaultDark FROM Settings;");
                    AdminSettings.EmailGeneric = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 EmailGeneric FROM Settings;");
                    AdminSettings.EmailPassword = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 EmailPassword FROM Settings;");
                    AdminSettings.EmailDisplayed = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 EmailDisplayed FROM Settings;");
                    AdminSettings.IsAdminVerification = await conn.ExecuteScalarAsync<bool>("SELECT TOP 1 IsAdminVerification FROM Settings;");
                    AdminSettings.Smtp = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 Smtp FROM Settings;");
                    AdminSettings.Port = await conn.ExecuteScalarAsync<int>("SELECT TOP 1 Port FROM Settings;");
                    AdminSettings.IsSsl = await conn.ExecuteScalarAsync<bool>("SELECT TOP 1 IsSsl FROM Settings;");
                    AdminSettings.GoogleAnalyticsID = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 GoogleAnalyticsID FROM Settings;");
                    AdminSettings.MetaDescription = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 MetaDescription FROM Settings;");
                    AdminSettings.MetaKeywords = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 MetaKeywords FROM Settings;");
                    AdminSettings.Title = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 Title FROM Settings;");

                    AdminSettings.Facebook = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [FACEBOOK] FROM Settings;");
                    AdminSettings.Instagram = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [INSTAGRAM] FROM Settings;");
                    AdminSettings.Linkedin = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [LINKEDIN] FROM Settings;");
                    AdminSettings.Street = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [STREET] FROM Settings;");
                    AdminSettings.Town = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [TOWN] FROM Settings;");
                    AdminSettings.City = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [CITY] FROM Settings;");
                    AdminSettings.ICO = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [ICO] FROM Settings;");
                    AdminSettings.DIC = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [DIC] FROM Settings;");
                    AdminSettings.IsPublicDefaultDark = await conn.ExecuteScalarAsync<bool>("SELECT TOP 1 [ISPUBLICDEFAULTDARK] FROM Settings;");
                    AdminSettings.GoogleMapsIframe = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [GOOGLEMAPSIFRAME] FROM Settings;");
                    AdminSettings.TristoneWeb = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [TRISTONEWEB] FROM Settings;");
                    AdminSettings.GDPR = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [GDPR] FROM Settings;");


                    AdminSettings.Phone = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [PHONE] FROM Settings;");
                    AdminSettings.Phone2 = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [PHONE2] FROM Settings;");
                    AdminSettings.Email = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [EMAIL] FROM Settings;");
                    AdminSettings.Email2 = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [EMAIL2] FROM Settings;");
                    AdminSettings.EmailTemplate = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [EMAILTEMPLATE] FROM Settings;");

                    AdminSettings.IsContactUsPublished = await conn.ExecuteScalarAsync<bool>("SELECT TOP 1 IsContactUsPublished FROM Settings;");
                    AdminSettings.IsGalleryPublished = await conn.ExecuteScalarAsync<bool>("SELECT TOP 1 IsGalleryPublished FROM Settings;");
                    AdminSettings.Name = await conn.ExecuteScalarAsync<string>("SELECT TOP 1 [NAME] FROM Settings;");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }


            }
            return true;

        }



        public async Task<bool> SetSettings(bool _IsEmailVerification,
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

            )
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"
update dbo.Settings set 


[ISEMAILVERIFICATION] = @_IsEmailVerification,
[ISDEFAULTDARK] = @_IsDefaultDark,
[EMAILGENERIC] = @_EmailGeneric,
[EMAILPASSWORD] = @_EmailPassword,
[EMAILDISPLAYED] = @_EmailDisplayed,
[SMTP] = @_Smtp,
[PORT] = @_Port,
[ISSSL] = @_IsSsl,
[ISADMINVERIFICATION] = @_IsAdminVerification,
[GOOGLEANALYTICSID]= @_GoogleAnalyticsID,
[METADESCRIPTION]= @_MetaDescription,
[METAKEYWORDS]= @_MetaKeywords,
[TITLE]= @_Title,

[FACEBOOK]             =  @_Facebook,
[INSTAGRAM]            =  @_Instagram,
[LINKEDIN]             =  @_Linkedin,
[STREET]               =  @_Street,
[TOWN]                 =  @_Town,
[CITY]                 =  @_City,
[ICO]                  =  @_ICO,
[DIC]                  =  @_DIC,
[ISPUBLICDEFAULTDARK]  =  @_IsPublicDefaultDark,
[GOOGLEMAPSIFRAME]     =  @_GoogleMapsIframe,
[TRISTONEWEB]     =  @_TristoneWeb,
[GDPR]     =  @_GDPR,
[PHONE]                  =  @_Phone,
[EMAIL]                  =  @_Email,
[PHONE2]                  =  @_Phone2,
[EMAIL2]                  =  @_Email2,
[EMAILTEMPLATE]         = @_EmailTemplate,
[IsContactUsPublished]         = @_IsContactUsPublished,
[IsGalleryPublished]         = @_IsGalleryPublished,
[NAME]                  = @_Name

";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new
                    {
                        _IsEmailVerification,
                        _IsDefaultDark,
                        _EmailGeneric,
                        _EmailPassword,
                        _EmailDisplayed,
                        _Smtp,
                        _Port,
                        _IsSsl,
                        _IsAdminVerification,
                        _GoogleAnalyticsID,
                        _MetaDescription,
                        _MetaKeywords,
                        _Title,
                        _Facebook,
                        _Instagram,
                        _Linkedin,
                        _Street,
                        _Town,
                        _City,
                        _ICO,
                        _DIC,
                        _IsPublicDefaultDark,
                        _GoogleMapsIframe,
                        _TristoneWeb,
                        _GDPR,
                        _Phone,
                        _Email,
                        _Phone2,
                        _Email2,
                        _EmailTemplate,
                         _IsContactUsPublished,
                         _IsGalleryPublished,
                         _Name

                    }, commandType: CommandType.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }


            }

            AdminSettings.IsEmailVerification = _IsEmailVerification;
            AdminSettings.IsDefaultDark = _IsDefaultDark;
            AdminSettings.EmailGeneric = _EmailGeneric;
            AdminSettings.EmailPassword = _EmailPassword;
            AdminSettings.EmailDisplayed = _EmailDisplayed;
            AdminSettings.IsAdminVerification = _IsAdminVerification;
            AdminSettings.Smtp = _Smtp;
            AdminSettings.Port = _Port;
            AdminSettings.IsSsl = _IsSsl;
            AdminSettings.GoogleAnalyticsID = _GoogleAnalyticsID;
            AdminSettings.MetaDescription = _MetaDescription;
            AdminSettings.MetaKeywords = _MetaKeywords;
            AdminSettings.Title = _Title;

            AdminSettings.Facebook = _Facebook;
            AdminSettings.Instagram = _Instagram;
            AdminSettings.Linkedin = _Linkedin;
            AdminSettings.Street = _Street;
            AdminSettings.Town = _Town;
            AdminSettings.City = _City;
            AdminSettings.ICO = _ICO;
            AdminSettings.DIC = _DIC;
            AdminSettings.IsPublicDefaultDark = _IsPublicDefaultDark;
            AdminSettings.GoogleMapsIframe = _GoogleMapsIframe;
            AdminSettings.TristoneWeb = _TristoneWeb;
            AdminSettings.GDPR = _GDPR;

            AdminSettings.Phone = _Phone;
            AdminSettings.Phone2 = _Phone2;
            AdminSettings.Email = _Email;
            AdminSettings.Email2 = _Email2;
            AdminSettings.EmailTemplate = _EmailTemplate;
            AdminSettings.IsGalleryPublished= _IsGalleryPublished;
            AdminSettings.IsContactUsPublished= _IsContactUsPublished;
            AdminSettings.Name= _Name;



            UpdateAppsettings(AdminSettings.GoogleAnalyticsID);


            _Aconfiguration["Analytics"] = AdminSettings.GoogleAnalyticsID;



            return true;
        }


        private void UpdateAppsettings(string gtag)
        {



            var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
            var json = File.ReadAllText(appSettingsPath);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new ExpandoObjectConverter());
            jsonSettings.Converters.Add(new StringEnumConverter());

            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

            config.MySettings.Gtag = gtag;


            var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

            File.WriteAllText(appSettingsPath, newJson);



        }
    }
}
