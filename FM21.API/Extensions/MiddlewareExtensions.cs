using FM21.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;

namespace FM21.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, IExceptionHandler exceptionHandler)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        exceptionHandler.LogError(contextFeature.Error);
                        await context.Response.WriteAsync("An unexpected fault happened.Try again later.");
                    }
                });
            });
        }

        public static void WebWorker(this IApplicationBuilder app, HttpContext httpContext)
        {
            if (httpContext != null)
            {
                ApplicationConstants.RequestID = httpContext.TraceIdentifier;
                if (httpContext.Request.Headers.Any())
                {
                    if (!string.IsNullOrEmpty(httpContext.Request.Headers["Content-Language"]))
                    {
                        string requestLanguage = Convert.ToString(httpContext.Request.Headers["Content-Language"]);
                        if (requestLanguage == null)
                        {
                            requestLanguage = "en-US";
                        }
                        ApplicationConstants.RequestLanguage = requestLanguage;
                    }
                    if (!string.IsNullOrEmpty(httpContext.Request.Headers["UserID"]))
                    {
                        ApplicationConstants.RequestUserID = Convert.ToInt32(httpContext.Request.Headers["UserID"]);
                    }
                }
            }
        }
    }
}