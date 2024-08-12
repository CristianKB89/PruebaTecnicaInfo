DECLARE @FechaInicial DATE = '2010-01-01';
DECLARE @FechaFinal DATE = '2010-12-31';

-- Subconsulta para Consumo total por cliente
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

-- Subconsulta para Pérdidas totales por cliente
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

-- Subconsulta para Costos totales por cliente
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

-- Consulta principal con agrupación por tipo de cliente
SELECT 
    T.Nombre AS Tramo,
    'Residencial' AS Tipo_Cliente,
    ISNULL(CS.Total_Residencial, 0) AS Consumo,
    ISNULL(PD.Total_Residencial, 0) AS Perdidas,
    ISNULL(CS.Total_Residencial * CO.Total_Residencial, 0) AS CostoPorConsumo
FROM 
    Tramos T
    LEFT JOIN ConsumoTotal CS ON T.Id = CS.Tramos_Id
    LEFT JOIN PerdidasTotal PD ON T.Id = PD.Tramos_Id
    LEFT JOIN CostosTotal CO ON T.Id = CO.Tramos_Id
UNION ALL
SELECT 
    T.Nombre AS Tramo,
    'Comercial' AS Tipo_Cliente,
    ISNULL(CS.Total_Comercial, 0) AS Consumo,
    ISNULL(PD.Total_Comercial, 0) AS Perdidas,
    ISNULL(CS.Total_Comercial * CO.Total_Comercial, 0) AS CostoPorConsumo
FROM 
    Tramos T
    LEFT JOIN ConsumoTotal CS ON T.Id = CS.Tramos_Id
    LEFT JOIN PerdidasTotal PD ON T.Id = PD.Tramos_Id
    LEFT JOIN CostosTotal CO ON T.Id = CO.Tramos_Id
UNION ALL
SELECT 
    T.Nombre AS Tramo,
    'Industrial' AS Tipo_Cliente,
    ISNULL(CS.Total_Industrial, 0) AS Consumo,
    ISNULL(PD.Total_Industrial, 0) AS Perdidas,
    ISNULL(CS.Total_Industrial * CO.Total_Industrial, 0) AS CostoPorConsumo
FROM 
    Tramos T
    LEFT JOIN ConsumoTotal CS ON T.Id = CS.Tramos_Id
    LEFT JOIN PerdidasTotal PD ON T.Id = PD.Tramos_Id
    LEFT JOIN CostosTotal CO ON T.Id = CO.Tramos_Id
ORDER BY 
    Tramo, Tipo_Cliente;
