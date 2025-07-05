
namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

// first thing we should do that change the class name as a delete 
// delete product command handler to identify command and query handlers.

// we should inject required services into the our class with using the primary constructor.
// after injecting these services with using the primary constructor, 
// we can implement our handle method. 

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}

internal class DeleteProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        // first line of code, we will logging the information about
        // the incoming command object and after that we will perform 
        // our actual logic.
        


        session.Delete<Product>(command.Id);  
        await session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}

// in this handler class we basically use Marten's idocumentsession 
// in order to delete the product by its id. 
// after performing the delete command, we save the changes to the 
// database and ensuring the product is removed. 