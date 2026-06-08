using BulkBuddy.Business.Repositories;
using BulkBuddy.Business.Services;
using BulkBuddy.DataAccess.Data;
using BulkBuddy.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Activeer MVC.
builder.Services.AddControllersWithViews();

// Nodig als opslag voor session-data.
builder.Services.AddDistributedMemoryCache();

// Session voor loginstatus.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Infrastructure
builder.Services.AddSingleton<DbConnectionFactory>();

// Data access layer
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMealRepository, MealRepository>();
builder.Services.AddScoped<IMealTemplateRepository, MealTemplateRepository>();

// Application layer
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<CalorieCalculatorService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<MealsPageService>();
builder.Services.AddScoped<MealTemplateService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting(); 

// Session middleware moet vóór MapControllerRoute staan.
app.UseSession();

// Start op login, niet dashboard.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
