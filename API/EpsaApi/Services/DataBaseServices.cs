using Dapper;
using EpsaApi.DataContext;
using EpsaApi.Interfaces;
using EpsaApi.Models;
using System.Data;

namespace MicroServiciosEpsa.Services
{
    public class DataBaseServices : IDataBaseServices
    {
        private readonly DapperContext _context;

        public DataBaseServices(DapperContext context)
        {
            _context = context;
        }

        public async Task<IList<HistoricosConsumoCliente>> GetHistoricoConsumoCliente(string fechaInicial, string fechaFinal)
        {
            using IDbConnection db = _context.CreateConnection();

            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string query = @"WITH ConsumoTotal AS (
                                        SELECT 
                                            Tramos_Id,
                                            SUM(Residencial) AS Total_Residencial,
                                            SUM(Comercial) AS Total_Comercial,
                                            SUM(Industrial) AS Total_Industrial
                                        FROM Consumo
                                        WHERE Fecha BETWEEN @FechaInicial AND @FechaFinal
                                        GROUP BY Tramos_Id
                                    ),
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
                                        Tramo, Tipo_Cliente;";

                var parameters = new DynamicParameters();

                parameters.Add("@FechaInicial", fechaInicial, DbType.Date);
                parameters.Add("@FechaFinal", fechaFinal, DbType.Date);

                var result = (IList<HistoricosConsumoCliente>)await db.QueryAsync<HistoricosConsumoCliente>(query, parameters);
                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();
            }
        }

        public async Task<IList<HistoricosPeoresTramosClientes>> GetHistoricoPeoresTramosCliente(string fechaInicial, string fechaFinal)
        {
            using IDbConnection db = _context.CreateConnection();

            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string query = @"WITH PerdidasTotal AS (
                                        SELECT 
                                            Tramos_Id,
                                            SUM(Residencial) AS Total_Residencial,
                                            SUM(Comercial) AS Total_Comercial,
                                            SUM(Industrial) AS Total_Industrial
                                        FROM Perdidas
                                        WHERE Fecha BETWEEN @FechaInicial AND @FechaFinal
                                        GROUP BY Tramos_Id
                                    )
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

                                    ORDER BY 
                                        Perdidas DESC;";

                var parameters = new DynamicParameters();

                parameters.Add("@FechaInicial", fechaInicial, DbType.Date);
                parameters.Add("@FechaFinal", fechaFinal, DbType.Date);

                var result = (IList<HistoricosPeoresTramosClientes>)await db.QueryAsync<HistoricosPeoresTramosClientes>(query, parameters);
                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();
            }
        }

        public async Task<IList<HistoricosTramo>> GetHistoricoTramo(string fechaInicial, string fechaFinal)
        {
            using IDbConnection db = _context.CreateConnection();

            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string query = @"WITH ConsumoTotal AS (
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
                                        T.Nombre;";

                var parameters = new DynamicParameters();

                parameters.Add("@FechaInicial", fechaInicial, DbType.Date);
                parameters.Add("@FechaFinal", fechaFinal, DbType.Date);

                var result = (IList<HistoricosTramo>)await db.QueryAsync<HistoricosTramo>(query, parameters);
                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();
            }
        }
    }
}
