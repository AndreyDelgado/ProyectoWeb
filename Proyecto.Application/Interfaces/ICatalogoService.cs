using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.ResultadoPatron;

namespace Proyecto.Application.Interfaces;

public interface ICatalogoService
{
    Task<Resultado<IEnumerable<UbicacionListaDto>>> ObtenerUbicacionesAsync(CancellationToken tokenCancelacion);
    Task<Resultado<IEnumerable<TipoMantenimientoListaDto>>> ObtenerTiposMantenimientoAsync(CancellationToken tokenCancelacion);
    Task<Resultado<IEnumerable<EspecialidadListaDto>>> ObtenerEspecialidadesAsync(CancellationToken tokenCancelacion);
    Task<Resultado<IEnumerable<TecnicoListaDto>>> ObtenerTecnicosAsync(CancellationToken tokenCancelacion);
    Task<Resultado<TecnicoListaDto>> ObtenerTecnicoAsync(int id, CancellationToken tokenCancelacion);
    Task<Resultado<TecnicoListaDto>> AgregarEspecialidadTecnicoAsync(
        int technicianId,
        int specialtyId,
        SolicitudEspecialidadTecnico solicitud,
        CancellationToken tokenCancelacion);
    Task<Resultado<TecnicoListaDto>> QuitarEspecialidadTecnicoAsync(
        int technicianId,
        int specialtyId,
        CancellationToken tokenCancelacion);
}
