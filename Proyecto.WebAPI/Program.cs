using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Implementation;
using Proyecto.Application.Interfaces;
using Proyecto.Application.Mapper;
using Proyecto.Domain.DTO.PatternResult;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Implementation;
using Proyecto.Infraestructure.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Sin esto, un DTO inválido responde con el ValidationProblemDetails de ASP.NET y no con Result<T>
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value!.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

            var result = Result<Dictionary<string, string[]>>.Failure(400, "Bad Request",
                "Uno o más datos no son válidos.");
            result.Data = errors;

            return new BadRequestObjectResult(result);
        };
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AssetCategory
builder.Services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();
builder.Services.AddScoped<IAssetCategoryService, AssetCategoryService>();

// Asset
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAssetService, AssetService>();

// Mapster: Pattern Mapper
// 1. Registrar las configuraciones personalizadas
MapsterConfig.RegisterMaps();
// 2. Tomar la configuración global donde quedaron registradas
var config = TypeAdapterConfig.GlobalSettings;
// 3. Registrar Mapster en el contenedor para inyectar IMapper en los servicios
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddMapster();

// Almacenamiento de imágenes de activos: una sola instancia para toda la aplicación
var assetImagesPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "images", "assets");
builder.Services.AddSingleton<IFileStorageService>(new LocalFileStorageService(assetImagesPath));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await database.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseStaticFiles();             // Sirve wwwroot: /images/assets/{nombre}

app.UseAuthorization();

app.MapControllers();

app.Run();
