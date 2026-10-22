using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Data;
using Proyecto.WebAPI.Controllers;

namespace Proyecto.WebAPI.Tests;

public sealed class ApiFactory : WebApplicationFactory<TechniciansController>
{
    public const int TechnicianId = 100;
    public const int SpecialtyId = 500;

    private readonly string databaseName = $"ApiTests-{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));
        });
    }

    public async Task SeedAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
        context.Technicians.Add(new Technician
        {
            Id = TechnicianId,
            FullName = "Técnico de pruebas",
            IdentificationNumber = "TEST-100",
            Email = "technician@example.test",
            Phone = "2222-0100",
            HourlyRate = 12000m
        });
        context.Specialties.Add(new Specialty
        {
            Id = SpecialtyId,
            Name = "Especialidad de pruebas",
            Description = "Especialidad creada para pruebas automatizadas"
        });
        await context.SaveChangesAsync();
    }
}
