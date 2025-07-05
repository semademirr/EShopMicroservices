using MediatR;

namespace BuildingBlocks.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}
// IQuery interface inherit from the IRequest and it is designed to 
// return a result. This is used for the read operations.

// By creating these abstractions, we can separate the responsibilities and manage the cross-cutting concerns.
// like validations more effectively. Commands can have their own
// validations and distinct from the queries. 

// we can see here these CQRS abstraction will be used across each
// microservices by referencing. 