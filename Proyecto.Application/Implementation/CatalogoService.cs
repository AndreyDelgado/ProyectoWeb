using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.ResultadoPatron;
using Proyecto.Infraestructure.Data;

namespace Proyecto.Application.Implementation;

public sealed class CatalogoService(ContextoAplicacion contexto) : ICatalogoService
{
    public async Task<Resultado<IEnumerable<UbicacionListaDto>>> ObtenerUbicacionesAsync(
        CancellationToken tokenCancelacion)
    {
        var locations = await contexto.Ubicaciones
            .AsNoTracking()
            .Where(ubicacion => ubicacion.EstaActivo)
            .OrderBy(ubicacion => ubicacion.Nombre)
            .Select(ubicacion => new UbicacionListaDto
            {
                Identificador = ubicacion.Identificador,
                Nombre = ubicacion.Nombre,
                Descripcion = ubicacion.Descripcion
            })
            .ToListAsync(tokenCancelacion);

        return Resultado<IEnumerable<UbicacionListaDto>>.Exito(locations);
    }

    public async Task<Resultado<IEnumerable<TipoMantenimientoListaDto>>> ObtenerTiposMantenimientoAsync(
        CancellationToken tokenCancelacion)
    {
        var maintenanceTypes = await contexto.TiposMantenimiento
            .AsNoTracking()
            .OrderBy(type => type.Nombre)
            .Select(type => new TipoMantenimientoListaDto
            {
                Identificador = type.Identificador,
                Nombre = type.Nombre,
                Descripcion = type.Descripcion,
                IntervaloPredeterminadoDias = type.IntervaloPredeterminadoDias,
                EsEmergencia = type.EsEmergencia,
                EspecialidadRequeridaId = type.EspecialidadRequeridaId,
                NombreEspecialidadRequerida = type.EspecialidadRequerida == null ? null : type.EspecialidadRequerida.Nombre
            })
            .ToListAsync(tokenCancelacion);

        return Resultado<IEnumerable<TipoMantenimientoListaDto>>.Exito(maintenanceTypes);
    }

    public async Task<Resultado<IEnumerable<EspecialidadListaDto>>> ObtenerEspecialidadesAsync(
        CancellationToken tokenCancelacion)
    {
        var specialties = await contexto.Especialidades
            .AsNoTracking()
            .OrderBy(especialidad => especialidad.Nombre)
            .Select(especialidad => new EspecialidadListaDto
            {
                Identificador = especialidad.Identificador,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            })
            .ToListAsync(tokenCancelacion);

        return Resultado<IEnumerable<EspecialidadListaDto>>.Exito(specialties);
    }

    public async Task<Resultado<IEnumerable<TecnicoListaDto>>> ObtenerTecnicosAsync(
        CancellationToken tokenCancelacion)
    {
        var technicians = await contexto.Tecnicos
            .AsNoTracking()
            .Include(tecnico => tecnico.EspecialidadesTecnico)
                .ThenInclude(link => link.Especialidad)
            .OrderBy(tecnico => tecnico.NombreCompleto)
            .ToListAsync(tokenCancelacion);

        var datos = technicians.Select(MapTechnician).ToArray();

        return Resultado<IEnumerable<TecnicoListaDto>>.Exito(datos);
    }

    public async Task<Resultado<TecnicoListaDto>> ObtenerTecnicoAsync(
        int id,
        CancellationToken tokenCancelacion)
    {
        var tecnico = await contexto.Tecnicos
            .AsNoTracking()
            .Include(item => item.EspecialidadesTecnico)
                .ThenInclude(link => link.Especialidad)
            .FirstOrDefaultAsync(item => item.Identificador == id, tokenCancelacion);

        return tecnico is null
            ? Resultado<TecnicoListaDto>.Fallo(404, "No encontrado", $"No existe el técnico con id {id}.")
            : Resultado<TecnicoListaDto>.Exito(MapTechnician(tecnico));
    }

    public async Task<Resultado<TecnicoListaDto>> AgregarEspecialidadTecnicoAsync(
        int technicianId,
        int specialtyId,
        SolicitudEspecialidadTecnico solicitud,
        CancellationToken tokenCancelacion)
    {
        if (solicitud.CertificadoDesde == default || solicitud.CertificadoDesde > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return Resultado<TecnicoListaDto>.Fallo(
                400, "Solicitud incorrecta", "La fecha de certificación debe ser válida y no puede estar en el futuro.");
        }

        var tecnico = await contexto.Tecnicos
            .FirstOrDefaultAsync(item => item.Identificador == technicianId, tokenCancelacion);
        if (tecnico is null)
        {
            return Resultado<TecnicoListaDto>.Fallo(404, "No encontrado", $"No existe el técnico con id {technicianId}.");
        }

        var specialtyExists = await contexto.Especialidades
            .AnyAsync(item => item.Identificador == specialtyId, tokenCancelacion);
        if (!specialtyExists)
        {
            return Resultado<TecnicoListaDto>.Fallo(404, "No encontrado", $"No existe la especialidad con id {specialtyId}.");
        }

        var alreadyAssigned = await contexto.EspecialidadesTecnico
            .AnyAsync(link => link.TecnicoId == technicianId && link.EspecialidadId == specialtyId, tokenCancelacion);
        if (alreadyAssigned)
        {
            return Resultado<TecnicoListaDto>.Fallo(
                409, "Conflicto", "El técnico ya tiene asignada esta especialidad.");
        }

        contexto.EspecialidadesTecnico.Add(new Domain.Entities.TecnicoEspecialidad
        {
            TecnicoId = technicianId,
            EspecialidadId = specialtyId,
            NivelCertificacion = solicitud.NivelCertificacion,
            CertificadoDesde = solicitud.CertificadoDesde.ToDateTime(TimeOnly.MinValue)
        });
        await contexto.SaveChangesAsync(tokenCancelacion);

        var resultado = await ObtenerTecnicoAsync(technicianId, tokenCancelacion);
        if (resultado.EsExitoso)
        {
            resultado.Estado = 201;
            resultado.Titulo = "Created";
        }

        return resultado;
    }

    public async Task<Resultado<TecnicoListaDto>> QuitarEspecialidadTecnicoAsync(
        int technicianId,
        int specialtyId,
        CancellationToken tokenCancelacion)
    {
        var technicianExists = await contexto.Tecnicos
            .AnyAsync(item => item.Identificador == technicianId, tokenCancelacion);
        if (!technicianExists)
        {
            return Resultado<TecnicoListaDto>.Fallo(404, "No encontrado", $"No existe el técnico con id {technicianId}.");
        }

        var link = await contexto.EspecialidadesTecnico
            .FirstOrDefaultAsync(
                item => item.TecnicoId == technicianId && item.EspecialidadId == specialtyId,
                tokenCancelacion);
        if (link is null)
        {
            return Resultado<TecnicoListaDto>.Fallo(
                404, "No encontrado", "El técnico no tiene asignada esta especialidad.");
        }

        contexto.EspecialidadesTecnico.Remove(link);
        await contexto.SaveChangesAsync(tokenCancelacion);

        return await ObtenerTecnicoAsync(technicianId, tokenCancelacion);
    }

    private static TecnicoListaDto MapTechnician(Domain.Entities.Tecnico tecnico) =>
        new()
        {
            Identificador = tecnico.Identificador,
            NombreCompleto = tecnico.NombreCompleto,
            NumeroIdentificacion = tecnico.NumeroIdentificacion,
            Correo = tecnico.Correo,
            Telefono = tecnico.Telefono,
            EstaDisponible = tecnico.EstaDisponible,
            TarifaPorHora = tecnico.TarifaPorHora,
            Especialidades = tecnico.EspecialidadesTecnico
                .OrderBy(link => link.Especialidad.Nombre)
                .Select(link => new TecnicoEspecialidadListaDto
                {
                    Identificador = link.EspecialidadId,
                    Nombre = link.Especialidad.Nombre,
                    NivelCertificacion = link.NivelCertificacion,
                    CertificadoDesde = DateOnly.FromDateTime(link.CertificadoDesde)
                })
                .ToArray()
        };
}
