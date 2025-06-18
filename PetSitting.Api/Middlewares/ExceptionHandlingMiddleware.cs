using System.Net;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Exceptions.Firebase;

namespace PetSitting.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next)//, ILogger logger)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(InternalUserNotFoundException ex)
            {
                //_logger.LogError(ex,"User was not found in the internal database based on the id.");
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(PostNotFoundException ex)
            {
                //_logger.LogError(ex,"Post was not found in the internal database based on the id");
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }            
            catch(JobApplicationAlreadyExistsException ex)
            {
                //_logger.LogError(ex,"The user is trying to submit a job offer for a job post but one already exists. Submitting multiple offers by the same user is not allowed.");
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(GenericValidationException ex)
            {
                //_logger.LogError(ex,"The user is trying to submit a job offer for a job post but one already exists. Submitting multiple offers by the same user is not allowed.");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(FirebaseLoginFailedException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(FirebaseUserCannotBeCreatedException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(FirebaseUserNotFoundException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(FirebaseTokenValidationException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(InternalMessageNotFoundException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(HubException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(CannotReviewException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(ReviewNotFoundException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(StripeWebhookNotHandledException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
            catch(Exception ex)
            {
                //log it.
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new {message = ex.Message});
            }
        }
    }
}