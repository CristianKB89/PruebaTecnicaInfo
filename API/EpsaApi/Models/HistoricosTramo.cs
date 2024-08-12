namespace EpsaApi.Models
{
    public class HistoricosTramo
    {
        public string Tramo { get; set; }
        public long Consumo_Residencial { get; set; }
        public long Consumo_Comercial { get; set; }
        public long Consumo_Industrial { get; set; }
        public decimal Perdidas_Residencial { get; set; }
        public decimal Perdidas_Comercial { get; set; }
        public decimal Perdidas_Industrial { get; set; }
        public decimal Total_Costo_Residencial { get; set; }
        public decimal Total_Costo_Comercial { get; set; }
        public decimal Total_Costo_Industrial { get; set; }
    }
}
