

using Microsoft.AspNetCore.Components;

namespace WebMud.Models
{
    public class AdminSettings : ComponentBase
    {

        public static bool IsEmailVerification { get; set; }

        public static bool IsDefaultDark { get; set; }

        public static string EmailGeneric { get; set; }

        public static string EmailPassword { get; set; }

        public static string EmailDisplayed { get; set; }

        public static string Smtp { get; internal set; }

        public static int Port { get; internal set; }

        public static bool IsSsl { get; internal set; }

        public static bool IsAdminVerification { get; internal set; }

        public static string GoogleAnalyticsID { get; set; } = "";

        public static string MetaDescription { get; set; }

        public static string MetaKeywords { get; set; }

        public static string Title { get; set; }


        public static string Facebook { get; set; }

        public static string Instagram { get; set; }

        public static string Linkedin { get; set; }

        public static string Street { get; set; }

        public static string Town { get; set; }

        public static string City { get; set; }

        public static string ICO { get; set; }

        public static string DIC { get; set; }

        public static bool IsPublicDefaultDark { get; internal set; }

        public static string GoogleMapsIframe { get; set; }

        public static string TristoneWeb { get; set; }

        public static string GDPR { get; set; }

        public static string Phone { get; set; }

        public static string Email { get; set; }

        public static string Phone2 { get; set; }

        public static string Email2 { get; set; }

        public static string EmailTemplate { get; set; }

        public static bool IsContactUsPublished { get; internal set; }

        public static bool IsGalleryPublished { get; internal set; }

        public static string Name { get; set; }
    }
}