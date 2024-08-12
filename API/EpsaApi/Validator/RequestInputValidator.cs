using EpsaApi.Models.Input;
using FluentValidation;

namespace EpsaApi.Validator
{
    public class RequestInputValidator : AbstractValidator<RequestInput>
    {
        public RequestInputValidator()
        {
            RuleFor(x => x.FechaInicial)
                .NotEmpty().WithMessage("Fecha inicial no puede ser vacía")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Fecha inicial debe tener el formato yyyy-MM-dd")
                .Must(fecha => DateTime.TryParseExact(fecha, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                .WithMessage("Fecha inicial no es una fecha válida.");



            RuleFor(x => x.FechaFinal)
                .NotEmpty().WithMessage("Fecha final no puede ser vacía")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Fecha final debe tener el formato yyyy-MM-dd")
                .Must(fecha => DateTime.TryParseExact(fecha, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                .WithMessage("Fecha final no es una fecha válida.");

        }
    }
}
