using dotNet_bakery.Models;
using dotNet_bakery.Repo;
using dotNet_bakery.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<DataRepository>();
builder.Services.AddSingleton<Rabbit>();

// Add services to the container.
builder.Services.AddControllers();
var app = builder.Build();

var rabbit = app.Services.GetRequiredService<Rabbit>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Data}/{action=Index}/{id?}");

app.Run();
