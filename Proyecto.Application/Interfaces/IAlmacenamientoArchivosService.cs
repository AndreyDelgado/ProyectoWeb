namespace Proyecto.Application.Interfaces;

// Los servicios dependen de esta interfaz y no del disco, para poder cambiar el almacenamiento sin modificarlos
public interface IAlmacenamientoArchivosService
{
    Task GuardarAsync(Stream content, string nombreArchivo);

    void Eliminar(string nombreArchivo);
}
