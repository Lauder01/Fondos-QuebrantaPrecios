# Integración de Almacenamiento de Imágenes en BuildingImage

## Resumen
Se ha integrado almacenamiento de imágenes en la tabla `BuildingImage` de manera simple, agregando solo dos columnas necesarias para almacenar imágenes directamente en la base de datos SQL Server usando `varbinary(max)`. Compatible con todas las versiones de SQL Server, incluyendo Azure SQL Database.

## Cambios Realizados

### 1. Entidad BuildingImage
Se agregaron dos propiedades simples:
- `RowGuid`: GUID requerido para FILESTREAM
- `ImageData`: Array de bytes para almacenar la imagen

### 2. Configuración de Base de Datos

#### Script SQL necesario:
- `Scripts/ModifyBuildingImageTable.sql`: Agrega las columnas necesarias

#### Para aplicar los cambios:
1. Ejecutar `ModifyBuildingImageTable.sql` para modificar la tabla (ya ejecutado exitosamente)

### 3. Configuración Entity Framework
La configuración en `AppDbContext` es mínima:
```csharp
entity.Property(e => e.RowGuid)
    .HasDefaultValueSql("NEWSEQUENTIALID()");

entity.Property(e => e.ImageData)
    .HasColumnType("varbinary(max)");
```

### 4. Servicio Actualizado
Se agregaron métodos útiles en `BuildingImageService`:
- `SaveImageData()` / `SaveImageDataAsync()`
- `GetImageData()` / `GetImageDataAsync()`

### 5. Controlador Principal
`ImageStorageController` implementa:
- Subir imágenes: `POST /api/ImageStorage/upload`
- Descargar imágenes: `GET /api/ImageStorage/download/{id}`
- Listar imágenes de un edificio: `GET /api/ImageStorage/building/{buildingId}`

## Uso

### Subir una imagen:
```http
POST /api/ImageStorage/upload
Content-Type: multipart/form-data

buildingId: "edificio-123"
fileName: "fachada.jpg"
altText: "Fachada principal del edificio"
file: [archivo de imagen]
```

### Descargar una imagen:
```http
GET /api/ImageStorage/download/imagen-456
```

## Ventajas de esta implementación

1. **Simplicidad**: Solo dos columnas adicionales
2. **Compatibilidad**: Mantiene la estructura existente y funciona en cualquier SQL Server
3. **Rendimiento**: `varbinary(max)` optimiza el almacenamiento de archivos
4. **Transaccionalidad**: Las imágenes están dentro de las transacciones de la BD
5. **Backup**: Las imágenes se incluyen automáticamente en los backups
6. **Universal**: Funciona en Azure SQL Database, SQL Server Express, etc.

## Consideraciones

- Compatible con todas las versiones de SQL Server
- Recomendado para archivos de cualquier tamaño (SQL Server maneja automáticamente la optimización)
- Si tienes SQL Server on-premises con FILESTREAM habilitado, puedes modificar el script para usarlo
- La columna `Url` se mantiene para compatibilidad con el sistema anterior

## Migración

Para migrar imágenes existentes desde URLs a FILESTREAM:
1. Descargar las imágenes desde las URLs existentes
2. Convertir a array de bytes
3. Guardar en la columna `ImageData`
4. Mantener o actualizar la URL para apuntar al nuevo endpoint
