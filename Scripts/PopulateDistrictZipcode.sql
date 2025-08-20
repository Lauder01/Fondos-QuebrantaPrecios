-- Script para poblar la tabla de relaciones DistrictZipcode
-- Conecta cada distrito con sus códigos postales correspondientes

-- Casco Antiguo (31001) → código postal 31001
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Casco Antiguo' AND z.Code = '31001';

-- Primer Ensanche (31002) → código postal 31002
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Primer Ensanche' AND z.Code = '31002';

-- Segundo Ensanche (31003) → código postal 31003
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Segundo Ensanche' AND z.Code = '31003';

-- Iturrama (31007) → código postal 31007
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Iturrama' AND z.Code = '31007';

-- Iturrama (31007) → código postal 31008 (también pertenece a Iturrama)
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Iturrama' AND z.Code = '31008';

-- Azpilagaña (31005) → código postal 31005
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Azpilagaña' AND z.Code = '31005';

-- Milagrosa (31005) → código postal 31005 (comparte con Azpilagaña)
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Milagrosa' AND z.Code = '31005';

-- Azpilagaña también puede tener 31006
INSERT INTO DistrictZipcode (DistrictId, ZipcodeId)
SELECT 
    d.Id as DistrictId, 
    z.Id as ZipcodeId
FROM District d, Zipcode z
WHERE d.Name = 'Azpilagaña' AND z.Code = '31006';

-- Verificar las relaciones creadas
SELECT 
    d.Name as DistritoNombre,
    d.Code as DistritoCodigo,
    z.Code as CodigoPostal
FROM DistrictZipcode dz
JOIN District d ON dz.DistrictId = d.Id
JOIN Zipcode z ON dz.ZipcodeId = z.Id
ORDER BY d.Name, z.Code;
