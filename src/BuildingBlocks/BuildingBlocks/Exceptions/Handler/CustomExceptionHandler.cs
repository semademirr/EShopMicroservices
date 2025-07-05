using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler 
    (ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{

    // in this method, our custom handler class will log the exceptions
    // and determine the response based on the exception type and the 
    // format response as a problem details object. 
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // now the first step is we are going to log the incoming 
        // error from the exception object. 
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurrence {time}",
            exception.Message, DateTime.UtcNow);

        // now we will continue to implementation our try handle async
        // method. after logging incoming exception with message, we are 
        // gonna use c# pattern matching to identify exception type.
        // after that we will populate the details of the these exception 
        // values. 


        // we are gonna create an anonymous object that we will 
        // populate these details title and status code values for 
        // each switch statement. 
        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException =>
            (
              exception.Message,
              exception.GetType().Name,
              context.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),

            ValidationException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),

            BadRequestException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),

            NotFoundException =>
            (
                 exception.Message,
                 exception.GetType().Name,
                 context.Response.StatusCode = StatusCodes.Status404NotFound
            ),

            // that mean is if the exception is not including these
            // types, the rest of the exception types will be available
            // with these codes, we will give a detail and suffix could be 
            // 500 internal server error. 
            _ => 
            (
                 exception.Message,
                 exception.GetType().Name,
                 context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )

        };

        // after defining these values as a details, we can specify our
        // problem details to identify the whole exception as a problem detail.
        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        // after identifying and creating problem details, we can add
        // more custom problem details as a extension.
        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if(exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        // after defining all these details, we can use the context response
        // object in order to write these details as a json format. 
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;


    }
}

// so this is the implementation of custom exception handler, and in this 
// code we handle different type of the exceptions such as the validatin
// exceptions, bad request and others, and setting up the appropriate 
// Http status code and response to problem details to the client 
// application as a json format. 