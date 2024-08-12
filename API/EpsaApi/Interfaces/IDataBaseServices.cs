using EpsaApi.Models;

namespace EpsaApi.Interfaces
{
    public interface IDataBaseServices
    {
        public Task<IList<HistoricosTramo>> GetHistoricoTramo(string fechaInicial, string fechaFinal);
        public Task<IList<HistoricosConsumoCliente>> GetHistoricoConsumoCliente(string fechaInicial, string fechaFinal);
        public Task<IList<HistoricosPeoresTramosClientes>> GetHistoricoPeoresTramosCliente(string fechaInicial, string fechaFinal);
    }
}
