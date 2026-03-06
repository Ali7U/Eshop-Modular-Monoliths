using FluentValidation;
using MediatR;
using Shared.Contracts.CQRS;
 
namespace Shared.Behaviors;

public class ValidationBehavior <TRequest, TResponse> 
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        
        var ValidationResult = 
            await Task.WhenAll(validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));
        
        var faliures = ValidationResult
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();
        
        if (faliures.Any())
        {
            throw new ValidationException(faliures);
        }
        
        return await next();
    }
}