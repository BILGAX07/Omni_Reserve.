using FluentValidation.Results;

namespace OmniReserve.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("Se produjeron uno o más errores de validación.")
    {
        Errors = failures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(failure => failure.ErrorMessage)
                    .ToArray());
    }
}