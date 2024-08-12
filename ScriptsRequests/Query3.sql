DECLARE @FechaInicial DATE = '2010-01-01';
DECLARE @FechaFinal DATE = '2010-01-31';

-- Subconsulta para Pérdidas totales por cliente
WITH PerdidasTotal AS (
    SELECT 
        Tramos_Id,
        SUM(Residencial) AS Total_Residencial,
        SUM(Comercial) AS Total_Comercial,
        SUM(Industrial) AS Total_Industrial
    FROM Perdidas
    WHERE Fecha BETWEEN @FechaInicial AND @FechaFinal
    GROUP BY Tramos_Id
)

-- Consulta principal para obtener tramos/clientes con mayores pérdidas
SELECT 
    T.Nombre AS Tramo,
    'Residencial' AS Tipo_Cliente,
    ISNULL(PD.Total_Residencial, 0) AS Perdidas
FROM 
    Tramos T
    LEFT JOIN PerdidasTotal PD ON T.Id = PD.Tramos_Id
UNION ALL
SELECT 
    T.Nombre AS Tramo,
    'Comercial' AS Tipo_Cliente,
    ISNULL(PD.Total_Comercial, 0) AS Perdidas
FROM 
    Tramos T
    LEFT JOIN PerdidasTotal PD ON T.Id = PD.Tramos_Id
UNION ALL
SELECT 
    T.Nombre AS Tramo,
    'Industrial' AS Tipo_Cliente,
    ISNULL(PD.Total_Industrial, 0) AS Perdidas
FROM 
    Tramos T
    LEFT JOIN PerdidasTotal PD ON T.Id = PD.Tramos_Id

-- Ordena por pérdidas en orden descendente
ORDER BY 
    Perdidas DESC;
