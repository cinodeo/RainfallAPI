using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

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

    // Add comments directly within the responses section
    //c.OperationFilter<AddCommentsToResponses>();
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

app.Run();

public class AddCommentsToResponses : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Clear(); // Clear existing responses

        // Add responses
        operation.Responses.Add("200", new OpenApiResponse
        {
            Description = "A list of rainfall readings successfully retrieved",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema, // Indicate it's a schema reference
                            Id = "#/components/schemas/rainfallReadingResponse" // Path to the schema definition
                        }
                    }
                }
            }
        });

        operation.Responses.Add("400", new OpenApiResponse
        {
            Description = "Invalid request",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema, // Indicate it's a schema reference
                            Id = "#/components/responses/errorResponse" // Path to the schema definition
                        }
                    }
                }
            }
        });

        operation.Responses.Add("404", new OpenApiResponse
        {
            Description = "No readings found for the specified stationId",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema, // Indicate it's a schema reference
                            Id = "#/components/responses/errorResponse" // Path to the schema definition
                        }
                    }
                }
            }
        });

        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal server error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema, // Indicate it's a schema reference
                            Id = "#/components/responses/errorResponse" // Path to the schema definition
                        }
                    }
                }
            }
        });
    }
}

