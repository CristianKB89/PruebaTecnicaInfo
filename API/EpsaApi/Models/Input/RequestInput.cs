using Newtonsoft.Json;

namespace EpsaApi.Models.Input
{
    public class RequestInput
    {
        [JsonProperty("fechaInicial", Required = Required.Always)]
        public string FechaInicial { get; set; }

        [JsonProperty("fechaFinal", Required = Required.Always)]
        public string FechaFinal { get; set; }
    }
}
