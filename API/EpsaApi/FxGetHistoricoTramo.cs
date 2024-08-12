using EpsaApi.Interfaces;
using EpsaApi.Models;
using EpsaApi.Models.Input;
using EpsaApi.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EpsaApi
{
    public class FxGetHistoricoTramo
    {
        private readonly IDataBaseServices _dataBaseServices;
        private readonly IValidator<RequestInput> _validator;

        public FxGetHistoricoTramo(IDataBaseServices dataBaseServices, IValidator<RequestInput> validator)
        {
            _dataBaseServices = dataBaseServices;
            _validator = validator;
        }

        [Function("FxGetHistoricoTramo")]

        public async Task<IActionResult> GetHistoricoTramo([HttpTrigger(Microsoft.Azure.Functions.Worker.AuthorizationLevel.Function, "get", Route = "Request1/GetHistoricoPorTramo")] HttpRequest req, ILogger _logger)
        {
            var json = await req.ReadAsStringAsync();
            if (json != null)
            {
                RequestInput input = JsonConvert.DeserializeObject<RequestInput>(json);

                var validationResult = _validator.Validate(input);

                if (!validationResult.IsValid)
                    return HttpResponseHelper.BadRequestObjectResult(validationResult.Errors.Select(e => new ResponseResult() { IsError = true, Message = e.ErrorMessage }));

                var result = await _dataBaseServices.GetHistoricoTramo(input.FechaInicial, input.FechaFinal);
                if (result.Count == 0)
                    return HttpResponseHelper.HttpExplicitNoContent();

                return HttpResponseHelper.SuccessfulObjectResult(result);
            }
            return new BadRequestObjectResult("Invalid request");
        }
    }
}
