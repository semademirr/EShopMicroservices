using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators) // that mean is when we are implementing the IValidator request in the command handler method, this is only applying for the particular handle method.  
    // in order to collect all handle methods, we are using the IEnumerable 
    // and validators that we can validate all these validators in one place
    // which is the validator behavior. now we can implement these validation
    // using the validators.
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse> // ICommand only accepts TResponse as a generics.
    // with this where keyword, this validation behavior would not be 
    // work for the IQuery request that means get product and get product
    // by id and so on. 
{
    // first parameter is representing the incoming request from the
    // client, and the second parameter is representing the next handle
    // delegate which means the next pipeline behavior or actual handle method.

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // creating a context object which is the validation context
        // comes from the our fluent validation. 
        // so we are passing the incoming request and reaching the context object.
        var context = new ValidationContext<TRequest>(request);
        // and after that we can run these all the validators using 
        // the validators objects and return back the validation results.

        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        // we used task.whenAll : that mean is complete all validation
        // operations where this comes from validators that we have injected
        // from the primary constructor. and we are gonna select to these 
        // validate asynchronous method and we will call these validate method 
        // for each handle method. and this will be accumulates all 
        // validation results into the validation result object. 

        // and after that we will check that if there is any failures 
        // in any validation result, we can get the failures object from 
        // the validation results and we are filtered by errors and select
        // these errors into the failure object.
        var failures = 
            validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();
        // and lastly, if there is any errors, we can throw an validation
        // exception with a given failure message. 

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        // and at the last time of the handle method we have to return
        // await and next request delegate handler. This will be run the 
        // next pipeline behavior, which will be the our actual command handler method.
        return await next();
        // this is very crucial to calling this next method in order to
        // continue this pipeline request lifecycle. 

    }
}
// in this class we basically implemented IPipeline behavior interface,
// and it checks for the all validation errors in the incoming request in here. 
// and if there is any error which including the validation errors in the 
// request, we will throw a validation exception if any errors are found.
// that mean is the behavior iterates over all provided validators for
// the request type. and it also accumulates any validation failures and 
// throws an exception if there is any exception and ensuring that 
// the only valid request reached our handle methods.
// after developed validation behavior, it is crucial to register these
// behavior into asp.net withing dependency injection for that purpose 
// go to the catalog api and open the program.cs