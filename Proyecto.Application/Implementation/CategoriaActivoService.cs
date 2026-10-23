using MapsterMapper;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.DTO;
using Proyecto.Domain.DTO.ResultadoPatron;
using Proyecto.Domain.Entities;
using Proyecto.Infraestructure.Interfaces;

namespace Proyecto.Application.Implementation;

public class CategoriaActivoService : ICategoriaActivoService
{
    // Límite de registros por página
    public const int TamanoMaximoPagina = 50;

    private readonly ICategoriaActivoRepository _repositorio;
    private readonly IMapper _mapeador;

    public CategoriaActivoService(ICategoriaActivoRepository repositorio, IMapper mapeador)
    {
        _repositorio = repositorio;
        _mapeador = mapeador;
    }

    // CONSULTA

    public async Task<Resultado<IEnumerable<CategoriaActivoListaDto>>> ObtenerTodosAsync()
    {
        var categories = await _repositorio.ObtenerTodosAsync();

        var datos = _mapeador.Map<IEnumerable<CategoriaActivoListaDto>>(categories);

        var resultado = Resultado<IEnumerable<CategoriaActivoListaDto>>.Exito(datos);

        return resultado;
    }

    public async Task<Resultado<IEnumerable<CategoriaActivoListaDto>>> ObtenerPaginacionAsync(int paginaNumero, int tamanoPagina)
    {
        if (paginaNumero < 1 || tamanoPagina < 1 || tamanoPagina > TamanoMaximoPagina)
            return Resultado<IEnumerable<CategoriaActivoListaDto>>.Fallo(400, "Solicitud incorrecta",
                $"El número de página debe ser 1 o mayor y el tamaño debe estar entre 1 y {TamanoMaximoPagina}.");

        var categories = await _repositorio.ObtenerPaginacionAsync(paginaNumero, tamanoPagina);
        var registrosTotales = await _repositorio.ContarAsync();

        var datos = _mapeador.Map<IEnumerable<CategoriaActivoListaDto>>(categories);

        var resultado = Resultado<IEnumerable<CategoriaActivoListaDto>>.Paginado(datos, paginaNumero, tamanoPagina, registrosTotales);

        return resultado;
    }

    public async Task<Resultado<CategoriaActivoDetalleDto>> ObtenerPorIdentificadorAsync(int id)
    {
        var categoria = await _repositorio.ObtenerPorIdentificadorAsync(id);

        if (categoria is null)
            return Resultado<CategoriaActivoDetalleDto>.Fallo(404, "No encontrado", $"La categoría {id} no existe.");

        var datos = _mapeador.Map<CategoriaActivoDetalleDto>(categoria);

        var resultado = Resultado<CategoriaActivoDetalleDto>.Exito(datos);

        return resultado;
    }

    // CREACIÓN
    public async Task<Resultado<CategoriaActivoDetalleDto>> AgregarAsync(CategoriaActivoCrearDto categoria)
    {
        // Regla de negocio: el nombre no se puede repetir
        if (await _repositorio.ExistePorNombreAsync(categoria.Nombre!))
            return Resultado<CategoriaActivoDetalleDto>.Fallo(409, "Conflicto",
                $"Ya existe una categoría con el nombre {categoria.Nombre}.");

        // Regla de negocio: el prefijo de código no se puede repetir
        if (await _repositorio.ExistePorPrefijoCodigoAsync(categoria.PrefijoCodigo!))
            return Resultado<CategoriaActivoDetalleDto>.Fallo(409, "Conflicto",
                $"Ya existe una categoría con el prefijo {categoria.PrefijoCodigo}.");

        var entidad = _mapeador.Map<CategoriaActivo>(categoria);
        entidad.EstaActivo = true;                    // Toda categoría nueva se crea activa

        await _repositorio.AgregarAsync(entidad);        // SQL Server asigna el Identificador

        var datos = _mapeador.Map<CategoriaActivoDetalleDto>(entidad);

        var resultado = Resultado<CategoriaActivoDetalleDto>.Exito(datos, 201, "Created", "Registro guardado.");

        return resultado;
    }

    // ACTUALIZACIÓN

    public async Task<Resultado<CategoriaActivoDetalleDto>> ActualizarAsync(int id, CategoriaActivoActualizarDto categoria)
    {
        if (!await _repositorio.ExisteAsync(id))
            return Resultado<CategoriaActivoDetalleDto>.Fallo(404, "No encontrado", $"La categoría {id} no existe.");

        // Misma regla, excluyendo a la categoría que se edita
        if (await _repositorio.ExistePorNombreAsync(categoria.Nombre!, id))
            return Resultado<CategoriaActivoDetalleDto>.Fallo(409, "Conflicto",
                $"Ya existe una categoría con el nombre {categoria.Nombre}.");

        if (await _repositorio.ExistePorPrefijoCodigoAsync(categoria.PrefijoCodigo!, id))
            return Resultado<CategoriaActivoDetalleDto>.Fallo(409, "Conflicto",
                $"Ya existe una categoría con el prefijo {categoria.PrefijoCodigo}.");

        var entidad = _mapeador.Map<CategoriaActivo>(categoria);
        entidad.Identificador = id;                            // El Identificador se toma de la ruta, no del body

        await _repositorio.ActualizarAsync(entidad);

        var datos = _mapeador.Map<CategoriaActivoDetalleDto>(entidad);

        var resultado = Resultado<CategoriaActivoDetalleDto>.Exito(datos, 200, "Correcto", "Registro actualizado.");

        return resultado;
    }

    // ELIMINAR

    public async Task<Resultado<int>> EliminarAsync(int id)
    {
        if (!await _repositorio.ExisteAsync(id))
            return Resultado<int>.Fallo(404, "No encontrado", $"La categoría {id} no existe.");

        // Regla de negocio: no se elimina una categoría con activos asociados
        if (await _repositorio.TieneActivosAsync(id))
            return Resultado<int>.Fallo(409, "Conflicto",
                $"La categoría {id} tiene activos asociados y no se puede eliminar.");

        await _repositorio.EliminarAsync(id);

        var resultado = Resultado<int>.Exito(id, 200, "Correcto", "Registro eliminado.");

        return resultado;
    }
}
