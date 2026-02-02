using Snowfall.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Ajoute les migrations
builder.Services.AddMigrations(configuration.GetConnectionString("AppDatabaseConnection")!);

var app = builder.Build();

app.Services.MigrateUp();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Evenements}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();