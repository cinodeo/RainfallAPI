using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var apiUrl = builder.Configuration.GetValue<string>("ApiSettings:ApiUrl");

builder.Services.AddSingleton(apiUrl);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Rainfall API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com")
        },
        Description = "An API which provides rainfall reading data"
    });
    c.MapType<int>(() => new OpenApiSchema { Type = "number", Minimum = 1, Maximum = 100 }); //count min and max

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //get xml file path
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); //get error responses from xml file from path
    c.IncludeXmlComments(xmlPath); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rainfall API v1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
  name: "rainfall",
  pattern: "rainfall/id/{stationId}/readings",
  defaults: new { controller = "Rainfall", action = "GetRainfallReadings" });
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger/index.html");
        return Task.CompletedTask;
    });
});
app.Run();