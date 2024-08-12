using EpsaApi.DataContext;
using EpsaApi.Interfaces;
using EpsaApi.Models.Input;
using EpsaApi.Validator;
using FluentValidation;
using MicroServiciosEpsa.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<DapperContext>();
        services.AddTransient<IDataBaseServices, DataBaseServices>();
        services.AddMvcCore().AddNewtonsoftJson(jsonOptions =>
        {
            jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Include;
        });
        services.AddScoped<IValidator<RequestInput>, RequestInputValidator>();

    })
    .Build();

host.Run();
