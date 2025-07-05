
namespace Catalog.API.Products.UpdateProduct;

// this class will be responsible for processing the update command for products
// this time we will develop a command record type. 
public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

// we make it command handler to identify and distinguish query and 
// command handlers. 

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is ruquired.");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters.");
        RuleFor(command => command.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}


internal class UpdateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // in order to implement and interact with the our database 
        // we have to inject a document session.
       
        
        // we will get the products from the database first and we will
        // use session.loadasync method. 
        // this will retrieve the single product. 
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null) {
            throw new ProductNotFoundException(command.Id);
            // if the products doesn't exist, it throws and product not found. 
        }

        // so if the product is not null, we basically update all the 
        // members including name, category, description, image file and 
        // price. that can be updated with incoming command object.

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        // so after updating these variables and members, we can use 
        // sessio.update method and passing the updated product entity
        // which will be document entity for us. 

        session.Update(product);

        // in order to perform update operation we use session.savechangeasync method.
        await session.SaveChangesAsync(cancellationToken);

        // lastly we can return new update product result object with
        // passing the true information.
        // that mean is this is succesfully complete update operation.
        return new UpdateProductResult(true);
    }
}

// in this handle method, we basically use Marten's documentation in 
// order to load the product by id and apply the updates. 