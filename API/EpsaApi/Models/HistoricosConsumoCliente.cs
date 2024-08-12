namespace EpsaApi.Models
{
    public class HistoricosConsumoCliente
    {
        public string Tramo { get; set; }
        public string Tipo_Cliente { get; set; }
        public long Consumo { get; set; }
        public decimal Perdidas { get; set; }
        public decimal CostoPorConsumo { get; set; }
    }
}
