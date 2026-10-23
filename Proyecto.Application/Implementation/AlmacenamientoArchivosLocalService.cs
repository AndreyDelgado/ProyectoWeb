using Proyecto.Application.Interfaces;

namespace Proyecto.Application.Implementation;

public class AlmacenamientoArchivosLocalService : IAlmacenamientoArchivosService
{
    private readonly string _rutaRaiz;

    public AlmacenamientoArchivosLocalService(string rootPath)
    {
        _rutaRaiz = rootPath;
        Directory.CreateDirectory(_rutaRaiz);
    }

    public async Task GuardarAsync(Stream content, string nombreArchivo)
    {
        var path = BuildPath(nombreArchivo);

        // CreateNew falla si el archivo ya existe: nunca se sobrescribe un archivo
        await using var archivo = new FileStream(path, FileMode.CreateNew);
        await content.CopyToAsync(archivo);
    }

    public void Eliminar(string nombreArchivo)
    {
        var path = BuildPath(nombreArchivo);

        if (File.Exists(path))
            File.Delete(path);
    }

    // Path.GetFileName descarta cualquier carpeta del nombre ("../appsettings.json" → "appsettings.json"),
    // así ninguna operación sale de la carpeta de imágenes
    private string BuildPath(string nombreArchivo)
    {
        var resultado = Path.Combine(_rutaRaiz, Path.GetFileName(nombreArchivo));

        return resultado;
    }
}
