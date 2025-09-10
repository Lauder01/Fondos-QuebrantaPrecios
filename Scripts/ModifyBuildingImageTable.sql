-- Script simplificado para almacenar imágenes en la tabla BuildingImage existente
-- Compatible con todas las versiones de SQL Server (incluyendo Azure SQL Database)
USE devdemobbdd;
GO

-- 1. Agregar RowGuid (opcional, útil para identificación única)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BuildingImage') AND name = 'RowGuid')
BEGIN
    ALTER TABLE BuildingImage 
    ADD RowGuid UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID();
END

-- 2. Agregar columna para datos binarios estándar (compatible con todas las versiones)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BuildingImage') AND name = 'ImageData')
BEGIN
    ALTER TABLE BuildingImage 
    ADD ImageData VARBINARY(MAX) NULL;
END

-- 3. Crear índice único en RowGuid
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('BuildingImage') AND name = 'IX_BuildingImage_RowGuid')
BEGIN
    CREATE UNIQUE INDEX IX_BuildingImage_RowGuid 
    ON BuildingImage (RowGuid);
END

-- 4. Verificar la estructura actualizada
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'BuildingImage'
ORDER BY ORDINAL_POSITION;

-- 5. Verificar que las columnas binarias están configuradas correctamente
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length,
    c.is_nullable
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('BuildingImage')
AND c.name IN ('RowGuid', 'ImageData');

PRINT 'Tabla BuildingImage actualizada para almacenamiento de imágenes';
GO
