using WebApplication_MyNoteSampleApp;
using WebApplication_MyNoteSampleApp.Models.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Ayarlar� okur
AppSettings appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

//Ba�lang��ta dbde yap�lacak i�lemler i�in yap�  
DatabaseInitializer databaseInitializer = new DatabaseInitializer(new DatabaseContext(), appSettings);

databaseInitializer.Seed();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
