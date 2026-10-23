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

public sealed class FabricaApi : WebApplicationFactory<TecnicosController>
{
    public const int TecnicoId = 100;
    public const int EspecialidadId = 500;

    private readonly string databaseName = $"ApiTests-{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<ContextoAplicacion>>();
            services.RemoveAll<DbContextOptions<ContextoAplicacion>>();
            services.RemoveAll<ContextoAplicacion>();
            services.AddDbContext<ContextoAplicacion>(options =>
                options.UseInMemoryDatabase(databaseName));
        });
    }

    public async Task SembrarAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var contexto = scope.ServiceProvider.GetRequiredService<ContextoAplicacion>();
        await contexto.Database.EnsureCreatedAsync();
        contexto.Tecnicos.Add(new Tecnico
        {
            Identificador = TecnicoId,
            NombreCompleto = "Técnico de pruebas",
            NumeroIdentificacion = "TEST-100",
            Correo = "technician@example.test",
            Telefono = "2222-0100",
            TarifaPorHora = 12000m
        });
        contexto.Especialidades.Add(new Especialidad
        {
            Identificador = EspecialidadId,
            Nombre = "Especialidad de pruebas",
            Descripcion = "Especialidad creada para pruebas automatizadas"
        });
        await contexto.SaveChangesAsync();
    }
}
