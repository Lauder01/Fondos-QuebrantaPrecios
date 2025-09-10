# Ejemplos de uso del almacenamiento de imágenes

## Configuración completada

✅ **Base de datos**: Se agregaron las columnas `RowGuid` y `ImageData` a la tabla `BuildingImage`
✅ **Entidad**: Se actualizó `BuildingImage` con las nuevas propiedades
✅ **Entity Framework**: Se configuró para manejar `varbinary(max)`
✅ **Servicio**: Se agregaron métodos para guardar/obtener datos de imagen
✅ **Controlador**: Se creó `ImageStorageController` para las operaciones

## Cómo probar

### 1. Iniciar la aplicación
```bash
cd c:\SC25\Fondos-QuebrantaPrecios\WebAPI
dotnet run
```

### 2. Subir una imagen
```bash
curl -X POST "https://localhost:7124/api/ImageStorage/upload" \
  -F "buildingId=edificio-123" \
  -F "fileName=fachada.jpg" \
  -F "altText=Fachada principal" \
  -F "file=@ruta/a/tu/imagen.jpg" \
  -k
```

### 3. Listar imágenes de un edificio
```bash
curl -X GET "https://localhost:7124/api/ImageStorage/building/edificio-123" -k
```

### 4. Descargar una imagen específica
```bash
curl -X GET "https://localhost:7124/api/ImageStorage/download/{buildingImageId}" -k -o imagen_descargada.jpg
```

## Ventajas de esta implementación

- **Compatible**: Funciona con cualquier versión de SQL Server (incluyendo Azure SQL Database)
- **Simple**: Solo dos columnas adicionales
- **Eficiente**: Los archivos se almacenan directamente en la base de datos
- **Transaccional**: Las imágenes participan en las transacciones
- **Backup automático**: Se incluyen en los backups de la base de datos
- **Sin dependencias**: No requiere configuración de FILESTREAM

## Estructura de respuesta

### Al subir una imagen:
```json
{
  "message": "Imagen subida correctamente",
  "buildingImageId": "uuid-generado",
  "size": 1024000,
  "downloadUrl": "/api/ImageStorage/download/uuid-generado"
}
```

### Al listar imágenes:
```json
[
  {
    "buildingImageId": "uuid-1",
    "fileName": "fachada.jpg",
    "altText": "Fachada principal",
    "isCoverImage": false,
    "hasImageData": true,
    "size": 1024000,
    "downloadUrl": "/api/ImageStorage/download/uuid-1"
  }
]
```

## Migración de datos existentes

Si tienes imágenes almacenadas como URLs, puedes migrarlas ejecutando:

```csharp
// Ejemplo de migración (ejecutar una sola vez)
foreach (var image in existingImages)
{
    if (!string.IsNullOrEmpty(image.Url) && image.ImageData == null)
    {
        using var httpClient = new HttpClient();
        var imageBytes = await httpClient.GetByteArrayAsync(image.Url);
        image.ImageData = imageBytes;
        await _buildingImageService.UpdateAsync(image);
    }
}
```

¡La integración está lista para usar! 🎉
