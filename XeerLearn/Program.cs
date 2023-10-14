using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RazorHtml.RazorClassLib.Services;
using System.Configuration;
using System.Security.Permissions;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Middleware;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;
using XeerLearn.Pages;
using XeerLearn.Pages.Components.Tutor;
using XeerLearn.Seeder;
using XeerLearn.WorkerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("sqlite") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddMemoryCache();
builder.Services.AddIdentity<XeerLearnUser, XeerLearnRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<XeerLearnRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddMvc().AddNToastNotifyToastr();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
//permissions
// configure static file permission
#pragma warning disable SYSLIB0003  
FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "StaticFiles"));
#pragma warning restore SYSLIB0003  
permission.Demand();

/*Register transient here */
builder.Services.AddTransient<UploadControllers>();
builder.Services.AddTransient<StreamController>();
builder.Services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
builder.Services.AddTransient<TutorDashboardModel>();
builder.Services.AddTransient<EmailController>();

/* Register Worker services */
//builder.Services.AddHostedService<MessagingService>();
//builder.Services.AddHostedService<EmailService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/lockout");
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(500);
    options.LoginPath = new PathString("/login");
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});


var app = builder.Build();

//seed models & static folder to database & server
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<XeerLearn.Models.Auth.XeerLearnUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<XeerLearnRole>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    var seed = new UserSeeder(context,roleManager,userManager);
    seed.SeedData();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot","StaticFiles")),
    RequestPath = new PathString("/StaticFiles")
});


app.UseSession();
app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseCookiePolicy();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapRazorPages();
app.MapFallbackToPage("/Index");
//app.UseAccessKeyMiddleware();
app.Run();
