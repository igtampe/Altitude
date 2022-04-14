using Igtampe.Altitude.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var CORS = "CORS";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("V1", new OpenApiInfo {
        Version = "V1", Title = "Altitude API",
        Description = "Altitude is a trip schedulign and basic planning app. This is its API.",
        //TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact {
            Name = "Chopo",
            Url = new Uri("https://twitter.com/igtampe"),
            Email = "igtampe@gmail.com",
        },
        License = new OpenApiLicense {
            Name = "CC0",
            //Url = new Uri("https://example.com/license") //TODO: Actually specify the license once this is done
        }
    });
    options.IncludeXmlComments("./Igtampe.Altitude.API.xml");
});

builder.Services.AddDbContext<AltitudeContext>();

builder.Services.AddCors(options => {
    options.AddPolicy(name: CORS,
    builder => {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/V1/swagger.json", "Altitude API"));
    app.UseDeveloperExceptionPage();
}

app.UseCors(CORS);

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();