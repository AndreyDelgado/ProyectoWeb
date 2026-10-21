using Proyecto.Application.Interfaces;

namespace Proyecto.Application.Implementation;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public LocalFileStorageService(string rootPath)
    {
        _rootPath = rootPath;
        Directory.CreateDirectory(_rootPath);
    }

    public async Task SaveAsync(Stream content, string fileName)
    {
        var path = BuildPath(fileName);

        // CreateNew falla si el archivo ya existe: nunca se sobrescribe un archivo
        await using var file = new FileStream(path, FileMode.CreateNew);
        await content.CopyToAsync(file);
    }

    public void Delete(string fileName)
    {
        var path = BuildPath(fileName);

        if (File.Exists(path))
            File.Delete(path);
    }

    // Path.GetFileName descarta cualquier carpeta del nombre ("../appsettings.json" → "appsettings.json"),
    // así ninguna operación sale de la carpeta de imágenes
    private string BuildPath(string fileName)
    {
        var result = Path.Combine(_rootPath, Path.GetFileName(fileName));

        return result;
    }
}
