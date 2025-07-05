using MediatR;

namespace BuildingBlocks.CQRS;

// Unit is a representing void type for the mediatR. 
public interface ICommand : ICommand<Unit>
{
}

// this mean is this command will be returning TResponse type.
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
