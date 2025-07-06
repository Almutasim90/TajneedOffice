using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Configure Entity Framework
builder.Services.AddDbContext<TajneedOfficeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<TajneedOfficeDbContext>()
.AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register services
builder.Services.AddScoped<TajneedOffice.Services.ICandidateService, TajneedOffice.Services.CandidateService>();
builder.Services.AddScoped<TajneedOffice.Services.IReportService, TajneedOffice.Services.ReportService>();
builder.Services.AddScoped<TajneedOffice.Services.ICandidateImportService, TajneedOffice.Services.CandidateImportService>();
builder.Services.AddScoped<TajneedOffice.Services.IFlexibleTestService, TajneedOffice.Services.FlexibleTestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TajneedOfficeDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    context.Database.EnsureCreated();
    
    // Initialize database with seed data
    DbInitializer.Initialize(context);
    
    // Seed roles and admin user
    await SeedRolesAndAdminUser(roleManager, userManager);
}

app.Run();

// Seed roles and admin user
async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
{
    // Create roles
    string[] roles = { "Admin", "CommitteeMember", "Coordinator", "HeadOfCommittee" };
    
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create admin user if it doesn't exist
    var adminUser = await userManager.FindByNameAsync("ADMIN001");
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = "ADMIN001",
            ServiceNumber = "ADMIN001",
            FullName = "مدير النظام",
            RankId = 1, // عقيد ركن
            Position = "مدير النظام",
            IsActive = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
