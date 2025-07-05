

namespace Catalog.API.Products.CreateProduct;

// we are indicating that this product command inherited from the 
// request which is the mediator and when mediator see this request
// comes from the API request this will be trigerred or create product
// command handler. 
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
// now we are ready to using this command object for mediator trigger operations.

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    // now we can specify our actual logic inside of this class.
    // so basically in this class the first thing we should do that 
    // we are gonna create a constructor.

    // inside of the constructor we can define the rules with using the
    // RuleFor() method. 
    public CreateProductCommandValidator()
    {
        // this is the our name validation that we can use before proceeding
        // the handler method. 
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");

    }
}
// how we can implement this validation? we need inject a validator
// in our command handler class in order to validate incoming command.

// we can see that our primary constructor including two objects one for
// the session to perform actual logic, one for the validator.

// we will use validator at the beggining of the handle method that will
// perform the validations with using the validator.validate method. 
internal class CreateProductCommandHandler
    (IDocumentSession session)  // we will use marten's idocumentsession for crud operations
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    // this class will contain the business logic to handle create
    // product command.
    // this is a command that triggers our command handler.
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create Product entity from command object.
        // save to database
        // return CreateProductResult result

        // we will performed by the validation behavior anymore.

        

        // create Product entity from command object.
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        // TODO
        // before implementing the data considerations we will focus 
        // on how to trigger this handle method with using the mediator.
        // that mean is we should expose and HTTP post endpoint API from 
        // our catalog API microservices using the minimal apis.
        // *save to database.
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        // we basically using the session object and session has 
        // store method in order to store our product information as a
        // document database. 


        // return result
        return new CreateProductResult(product.Id);

    }
}
