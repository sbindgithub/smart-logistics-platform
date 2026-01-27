using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartLogistics.Application.Common.Exceptions;

namespace SmartLogistics.Api.Pipeline;

public sealed class ConcurrencyBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "The resource was modified by another process.");
        }
    }
}
