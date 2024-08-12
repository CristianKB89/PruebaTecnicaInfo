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
                    .Matches(@"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4}$")
                    .WithMessage("Fecha inicial debe tener el formato dd-mm-yyyy")
                    .Must(fecha => DateTime.TryParseExact(fecha, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                    .WithMessage("Fecha inicial no es una fecha válida.");


            RuleFor(x => x.FechaFinal)
                    .NotEmpty().WithMessage("Fecha final no puede ser vacía")
                    .Matches(@"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4}$")
                    .WithMessage("Fecha final debe tener el formato dd-mm-yyyy")
                    .Must(fecha => DateTime.TryParseExact(fecha, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                    .WithMessage("Fecha final no es una fecha válida.");
        }
    }
}
