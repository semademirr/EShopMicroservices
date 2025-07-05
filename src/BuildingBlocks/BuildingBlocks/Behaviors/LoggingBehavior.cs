
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
    // logging behavior will reflect both http get and other crud
    // operations together. 
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // first line of the code we should always logging the incoming
        // requests for that purpose: start-prefix
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        // after logging the incoming request we can calculate the duration of 
        // the request and process time.
        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;
        if(timeTaken.Seconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} second",
                typeof(TRequest).Name, timeTaken.Seconds);
        }

        // lastly we will logging the end of the handle method.
        logger.LogInformation("[END] Handled {Request} with {Response}",
            typeof(TRequest).Name, typeof(TResponse).Name);

        // and we are returning the respnse object to the next operation.
        return response;
    }
}
