# Endpoint Unificado: Crear Edificio con Imágenes

## Descripción
Endpoint que permite crear un edificio junto con sus imágenes en una sola transacción, simplificando el proceso de registro.

## URL
```
POST /api/building/with-images
```

## Request Body
```json
{
  // Datos del edificio (heredados de BuildingCreatorDto)
  "name": "Edificio Ejemplo",
  "doorway": "123A",
  "floorCount": 5,
  "apartmentsPerFloor": 4,
  "price": 250000,
  "hasGarage": true,
  "districtId": "guid-distrito",
  "streetId": "guid-calle",
  "buildingCompanyId": "guid-empresa",
  "statusId": "guid-estado",
  
  // Datos de la dirección
  "constructedAddress": "Calle Ejemplo 123A",
  "country": "España",
  "city": "Madrid",
  "zipcodeId": "guid-codigo-postal",
  
  // Imágenes en formato base64
  "imageFiles": [
    {
      "fileName": "fachada.jpg",
      "fileContent": "base64-encoded-image-data...",
      "contentType": "image/jpeg",
      "altText": "Fachada del edificio",
      "isCoverImage": true
    },
    {
      "fileName": "interior.png",
      "fileContent": "base64-encoded-image-data...",
      "contentType": "image/png",
      "altText": "Interior del edificio",
      "isCoverImage": false
    }
  ]
}
```

## Response
Devuelve el mismo `BuildingGetterDto` que el endpoint regular de creación de edificios.

## Flujo Interno
1. **Validación** del modelo recibido
2. **Creación del edificio** con datos por defecto si es necesario
3. **Creación de la dirección** asociada al edificio
4. **Procesamiento de imágenes**: 
   - Conversión de base64 a bytes
   - Creación de entidades `BuildingImage` 
   - Almacenamiento en base de datos
5. **Creación de pisos** según `FloorCount`
6. **Creación de apartamentos** según `ApartmentsPerFloor`
7. **Retorno** del edificio completo con relaciones

## Ventajas
- ✅ **Una sola transacción**: Todo se crea de forma atómica
- ✅ **Simplifica frontend**: No necesita múltiples llamadas API
- ✅ **Consistencia**: Si falla algo, se revierte todo
- ✅ **Menos complejidad**: Similar a como se maneja Address

## Diferencias con Endpoint Original
| Aspecto | Endpoint Original | Nuevo Endpoint |
|---------|------------------|----------------|
| Imágenes | Requiere llamadas separadas | Incluidas en la misma request |
| Transacciones | Múltiples | Una sola |
| Frontend | Más complejo | Más simple |
| Datos imágenes | DTOs de imagen | Archivos base64 |

## Uso desde Frontend
```typescript
const buildingData = {
  // datos del edificio...
  imageFiles: this.selectedImages.map(img => ({
    fileName: img.name,
    fileContent: img.base64Data,
    contentType: img.type,
    altText: img.altText || 'Imagen del edificio',
    isCoverImage: img.isCover || false
  }))
};

const response = await this.http.post('/api/building/with-images', buildingData);
```
