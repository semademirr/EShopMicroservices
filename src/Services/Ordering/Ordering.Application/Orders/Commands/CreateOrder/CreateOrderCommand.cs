
using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;
// this command object basically encapsulates the data needed to 
// create a new order, which is the orderDto. 
// we also need to create order result for the response of this 
// command object. 

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order)
    : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required.");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty.");
    }
}