using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Security.Permissions;
using Verdon.Controllers;
using Verdon.Data;
using Verdon.Middleware;
using Verdon.Models.Auth;
using Verdon.Models.Utility;
using Verdon.Pages;
using Verdon.Pages.Components.Tutor;
using Verdon.Seeder;
using Verdon.WorkerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("sqlite") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<VerdonUser, VerdonRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddRoles<VerdonRole>()
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
builder.Services.AddTransient<TutorDashboardModel>();

/* Register Worker services */
builder.Services.AddHostedService<MessagingService>();
builder.Services.AddHostedService<EmailService>();



var app = builder.Build();

//seed models & static folder to database & server
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<VerdonUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<VerdonRole>>();
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
app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.MapControllers();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapRazorPages();
app.UseAccessKeyMiddleware();
app.Run();
