using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.ResultadoPatron;

namespace Proyecto.Application.Interfaces;

public interface IProveedorService
{
    Task<Resultado<IEnumerable<ProveedorListaDto>>> ObtenerTodosAsync();
    Task<Resultado<IEnumerable<ProveedorListaDto>>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina);
    Task<Resultado<ProveedorDetalleDto>> ObtenerPorIdentificadorAsync(int id);
    Task<Resultado<ProveedorDetalleDto>> AgregarAsync(ProveedorCrearDto proveedor);
    Task<Resultado<ProveedorDetalleDto>> ActualizarAsync(int id, ProveedorActualizarDto proveedor);
    Task<Resultado<int>> EliminarAsync(int id);
}
