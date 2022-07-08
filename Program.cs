using WebApplication_MyNoteSampleApp;
using WebApplication_MyNoteSampleApp.Models.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => 
{
    options.Cookie.Name = "notes.session";
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

// Add services to the container.
builder.Services.AddControllersWithViews();


//Ayarlarý okur
AppSettings appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

//Baþlangýçta dbde yapýlacak iþlemler için yapý  
DatabaseInitializer databaseInitializer = new DatabaseInitializer(new DatabaseContext(), appSettings);

databaseInitializer.Seed();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
