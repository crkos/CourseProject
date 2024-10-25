using CourseProject.Data;
using Microsoft.EntityFrameworkCore;
using CourseProject.Models;
using Microsoft.AspNetCore.Identity;
using CourseProject.Services;
using CourseProject.Interfaces;
using dotenv.net;
using CourseProject.Infraestructure;
using CourseProject.Hubs;

// DOT ENV
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLSTRING"));
});

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDBContext>()
.AddDefaultTokenProviders();

// Services
builder.Services
    .AddScoped<ITemplateService, TemplateService>()
    .AddScoped<IFormService, FormService>()
    .AddTransient<IUploader, CloudinaryUploader>()
    .AddTransient<FileUploadService>()
    .AddTransient<ISearch<Template>, TemplateSearch>()
    .AddTransient<SearchService>();

builder.Services.AddSignalR();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(options =>
{
    options.AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapHub<CommentsHub>("/commentsHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
