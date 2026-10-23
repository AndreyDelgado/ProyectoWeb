# Proyecto Mantenimiento de Activos

API para administrar activos, categorías, ubicaciones, proveedores, técnicos y órdenes de mantenimiento.

## Requisitos

- .NET SDK compatible con los proyectos de la solución.
- SQL Server o SQL Server LocalDB.

## Base de datos

La solución utiliza SQL Server y Entity Framework Core. En desarrollo, la API crea `MantenimientoActivosDbDev` en `(localdb)\MSSQLLocalDB` y carga los datos iniciales al arrancar.

Para inicializarla desde la API, ejecuta desde `Proyecto.WebAPI`:

```powershell
dotnet run
```

También puedes crearla manualmente ejecutando `database/MantenimientoActivos.sql` en SQL Server Management Studio o mediante `sqlcmd -f 65001 -i .\database\MantenimientoActivos.sql`. El script crea las tablas, relaciones, restricciones e inserciones de ejemplo; puede ejecutarse nuevamente sin duplicar esos datos.

El modelo inicial incluye categorías, ubicaciones, proveedores, activos, tipos de mantenimiento y órdenes de mantenimiento. La cadena de conexión de desarrollo está en `Proyecto.WebAPI/appsettings.Development.json` y se puede reemplazar por la instancia de SQL Server del equipo.

## Compilar y probar

Desde la carpeta que contiene `Proyecto.slnx`, ejecuta:

```powershell
dotnet build Proyecto.slnx
dotnet test Proyecto.slnx
```

## Catálogos de la API

Los siguientes endpoints de consulta devuelven respuestas con el formato `Resultado<T>`:

| Endpoint | Contenido |
| --- | --- |
| `GET /api/locations` | Ubicaciones activas, ordenadas por nombre. |
| `GET /api/maintenance-types` | Tipos de mantenimiento y la especialidad requerida, si existe. |
| `GET /api/specialties` | Especialidades registradas. |
| `GET /api/technicians` y `GET /api/technicians/{id}` | Técnicos y sus certificaciones asociadas. |
| `POST /api/technicians/{id}/specialties/{specialtyId}` y `DELETE /api/technicians/{id}/specialties/{specialtyId}` | Asignar una especialidad a un técnico y quitarla. |

Las solicitudes de ejemplo están en `Proyecto.WebAPI/Proyecto.WebAPI.http`.
