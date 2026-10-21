namespace Proyecto.Application.Interfaces;

// Los servicios dependen de esta interfaz y no del disco, para poder cambiar el almacenamiento sin modificarlos
public interface IFileStorageService
{
    Task SaveAsync(Stream content, string fileName);

    void Delete(string fileName);
}
