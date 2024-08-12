DECLARE @FechaInicial DATE = '2010-01-01';
DECLARE @FechaFinal DATE = '2010-01-31';

-- Subconsulta para Consumo total
WITH ConsumoTotal AS (
    SELECT 
        Tramos_Id,
        SUM(Residencial) AS Total_Residencial,
        SUM(Comercial) AS Total_Comercial,
        SUM(Industrial) AS Total_Industrial
    FROM Consumo
    WHERE Fecha BETWEEN @FechaInicial AND @FechaFinal
    GROUP BY Tramos_Id
),

-- Subconsulta para Pérdidas totales
PerdidasTotal AS (
    SELECT 
        Tramos_Id,
        SUM(Residencial) AS Total_Residencial,
        SUM(Comercial) AS Total_Comercial,
        SUM(Industrial) AS Total_Industrial
    FROM Perdidas
    WHERE Fecha BETWEEN @FechaInicial AND @FechaFinal
    GROUP BY Tramos_Id
),

-- Subconsulta para Costos totales
CostosTotal AS (
    SELECT 
        Tramos_Id,
        SUM(Residencial) AS Total_Residencial,
        SUM(Comercial) AS Total_Comercial,
        SUM(Industrial) AS Total_Industrial
    FROM Costos
    WHERE Fecha BETWEEN @FechaInicial AND @FechaFinal
    GROUP BY Tramos_Id
)

-- Consulta principal
SELECT 
    T.Nombre AS Tramo,
    -- Consumo total por tipo
    ISNULL(ConsumoTotal.Total_Residencial, 0) AS Consumo_Residencial,
    ISNULL(ConsumoTotal.Total_Comercial, 0) AS Consumo_Comercial,
    ISNULL(ConsumoTotal.Total_Industrial, 0) AS Consumo_Industrial,
    -- Pérdidas totales por tipo
    ISNULL(PerdidasTotal.Total_Residencial, 0) AS Perdidas_Residencial,
    ISNULL(PerdidasTotal.Total_Comercial, 0) AS Perdidas_Comercial,
    ISNULL(PerdidasTotal.Total_Industrial, 0) AS Perdidas_Industrial,
    -- Multiplicación de costo por consumo
    ISNULL(ConsumoTotal.Total_Residencial * CostosTotal.Total_Residencial, 0) AS Total_Costo_Residencial,
    ISNULL(ConsumoTotal.Total_Comercial * CostosTotal.Total_Comercial, 0) AS Total_Costo_Comercial,
    ISNULL(ConsumoTotal.Total_Industrial * CostosTotal.Total_Industrial, 0) AS Total_Costo_Industrial
FROM 
    Tramos T
    LEFT JOIN ConsumoTotal ON T.Id = ConsumoTotal.Tramos_Id
    LEFT JOIN PerdidasTotal ON T.Id = PerdidasTotal.Tramos_Id
    LEFT JOIN CostosTotal ON T.Id = CostosTotal.Tramos_Id
ORDER BY 
    T.Nombre;

SELECT * FROM Consumo

SELECT * FROM Costos

SELECT * FROM Perdidas

--ALTER TABLE CONSUMO
--ALTER COLUMN Industrial BIGINT;