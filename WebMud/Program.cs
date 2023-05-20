
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR;
using MudBlazor;
using MudBlazor.Services;
using System.Data;
using WebMud.Dapper;
using WebMud.Data;
using Dapper;
using System.Data.SqlClient;

using System.Globalization;
using Microsoft.AspNetCore.Localization;
using WebMud.Models;
using Microsoft.Extensions.Localization;
using Blazor.Analytics;

//Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();


builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<IEmailServices, EmailSender>();
builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddSingleton<IInitialService, InitialService>();
builder.Services.AddSingleton<IVisitorsStatisticsService, VisitorsStatisticsService>();
builder.Services.AddSingleton<ITicketsService, TicketsService>();
builder.Services.AddSingleton<IGalleryService, GalleryService>();
builder.Services.AddSingleton<IFolderService, FolderService>();
builder.Services.AddSingleton<IInsuranceService, InsuranceService>();
builder.Services.AddSingleton<IClientService, ClientService>();

builder.Services.AddSingleton<AdminSettings>();

//builder.Services.AddSingleton<IStringLocalizer<WelcomeRes>, StringLocalizer<WelcomeRes>>();

builder.Services.AddControllersWithViews();



builder.Services.AddControllers();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;

});


builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

var sqlConnectionConfiguration = new SqlConnectionConfiguration(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(sqlConnectionConfiguration);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;


});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
  
    options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
    options.LogoutPath = "/pages/authentication/logout";
    options.LoginPath = "/pages/authentication/login";


});

builder.Services.AddGoogleAnalytics(builder.Configuration.GetSection("MySettings")["Gtag"]);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}


app.Services.GetService<IInitialService>().InitializeAsync();



app.UseHttpsRedirection();



app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapHub<BlazorChat>(BlazorChat.HubUrl);



});

//app.MapControllers();
//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");

app.UseMiddleware(typeof(VisitorsCounterService));
app.Run();
