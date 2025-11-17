using Application.Exceptions;
using Domine.Exceptions;
using System.Net;
using System.Text.Json;

namespace GoodBank.Middleware
{
    public sealed class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteError(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (ValidationException ex)
            {
                await WriteError(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (ConflictException ex)
            {
                await WriteError(context, ex.Message, HttpStatusCode.Conflict);
            }
            catch (DomainException ex)
            {
                await WriteError(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await WriteError(context, "Unexpected server error.", HttpStatusCode.InternalServerError);
            }
        }

        private static Task WriteError(HttpContext ctx, string message, HttpStatusCode code)
        {
            ctx.Response.StatusCode = (int)code;
            ctx.Response.ContentType = "application/json";

            return ctx.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = message,
                status = code.ToString()
            }));
        }
    }
}
