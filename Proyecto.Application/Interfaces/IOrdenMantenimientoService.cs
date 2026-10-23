using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.ResultadoPatron;

namespace Proyecto.Application.Interfaces;

public interface IOrdenMantenimientoService
{
    Task<Resultado<IEnumerable<OrdenMantenimientoListaDto>>> ObtenerTodosAsync();
    Task<Resultado<IEnumerable<OrdenMantenimientoListaDto>>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina);
    Task<Resultado<OrdenMantenimientoDetalleDto>> ObtenerPorIdentificadorAsync(int id);
    Task<Resultado<IEnumerable<OrdenMantenimientoListaDto>>> ObtenerPorActivoAsync(int assetId);
    Task<Resultado<OrdenMantenimientoDetalleDto>> AgregarAsync(OrdenMantenimientoCrearDto orden);
}
