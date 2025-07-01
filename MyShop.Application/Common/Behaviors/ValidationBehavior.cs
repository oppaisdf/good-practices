using FluentValidation;
using MediatR;

namespace MyShop.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!validators.Any())
            return await next(ct);
        var ctx = new ValidationContext<TRequest>(request);
        var failures = (await Task
             .WhenAll(validators.Select(v => v.ValidateAsync(ctx, ct))))
             .SelectMany(r => r.Errors)
             .Where(f => f is not null)
             .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next(ct);
    }
}
