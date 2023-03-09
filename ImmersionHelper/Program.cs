using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, 
    sqlServerOptions => sqlServerOptions.CommandTimeout(builder.Configuration.GetValue<int>("SqlCommandTimeout"))));

builder.Services.AddScoped<IMyDictionary, JLPTDictionary>();
builder.Services.AddScoped<IMyDictionary, JMDictDictionary>();
builder.Services.AddScoped<DictionaryServices>();
builder.Services.AddScoped<ArticlesServices>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit= true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
});

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("IsAdmin"));
});

builder.Services.AddRazorPages();

var app = builder.Build();

// create default role and default admin user
await CreateRoles();

CreateVocabularyData();

CreateArticleData();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

async Task CreateRoles()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
    if (!adminRoleExists)
    {
        var adminRole = new IdentityRole("Admin");
        var result = await roleManager.CreateAsync(adminRole);
    }

    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        var user = new ApplicationUser()
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(user, "Password123!");
        if (result.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

void CreateVocabularyData()
{
    using (var scope = app.Services.CreateScope())
    {
        var dicServices = scope.ServiceProvider.GetRequiredService<DictionaryServices>();
        dicServices.InitData();
    }
}

void CreateArticleData()
{
    using (var scope = app.Services.CreateScope())
    {
        var articlesServices = scope.ServiceProvider.GetRequiredService<ArticlesServices>();
        articlesServices.InitData();
    }
}