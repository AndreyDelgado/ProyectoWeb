using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.ResultadoPatron;

namespace Proyecto.Application.Interfaces;

public interface ICategoriaActivoService
{
    Task<Resultado<IEnumerable<CategoriaActivoListaDto>>> ObtenerTodosAsync();
    Task<Resultado<IEnumerable<CategoriaActivoListaDto>>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina);
    Task<Resultado<CategoriaActivoDetalleDto>> ObtenerPorIdentificadorAsync(int id);
    Task<Resultado<CategoriaActivoDetalleDto>> AgregarAsync(CategoriaActivoCrearDto categoria);
    Task<Resultado<CategoriaActivoDetalleDto>> ActualizarAsync(int id, CategoriaActivoActualizarDto categoria);
    Task<Resultado<int>> EliminarAsync(int id);
}
