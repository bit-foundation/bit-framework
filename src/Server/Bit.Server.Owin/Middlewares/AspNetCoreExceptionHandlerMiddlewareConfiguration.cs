﻿using Bit.Core.Contracts;
using Bit.Owin.Contracts;
using Bit.Owin.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Bit.Owin.Middlewares
{
    public class AspNetCoreExceptionHandlerMiddlewareConfiguration : IAspNetCoreMiddlewareConfiguration
    {
        public virtual MiddlewarePosition MiddlewarePosition => MiddlewarePosition.BeforeOwinMiddlewares;

        public virtual void Configure(IApplicationBuilder aspNetCoreApp)
        {
            if (aspNetCoreApp == null)
                throw new ArgumentNullException(nameof(aspNetCoreApp));

            aspNetCoreApp.UseMiddleware<AspNetCoreExceptionHandlerMiddleware>();
        }
    }

    public class AspNetCoreExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public AspNetCoreExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private (bool responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason, bool responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason, bool responseIsOk) GetResponseStatus(HttpContext context)
        {
            string statusCode = context.Response.StatusCode.ToString(CultureInfo.InvariantCulture);
            bool responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason = statusCode.StartsWith("5", StringComparison.InvariantCultureIgnoreCase);
            bool responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason = statusCode.StartsWith("4", StringComparison.InvariantCultureIgnoreCase);
            return (responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason, responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason, responseIsOk: !responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason && !responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason);
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            IScopeStatusManager scopeStatusManager = context.RequestServices.GetRequiredService<IScopeStatusManager>();

            ILogger logger = context.RequestServices.GetRequiredService<ILogger>();

            try
            {
                string? xCorrelationId = context.RequestServices.GetRequiredService<IRequestInformationProvider>().XCorrelationId;

                context.Response.OnStarting(() =>
                {
                    // See OnSendingHeaders of OwinExceptionHandlerMiddleware for more info.
                    string? reasonPhrase = context.Features.Get<IHttpResponseFeature>().ReasonPhrase;

                    bool responseIsOk = GetResponseStatus(context).responseIsOk;

                    if (!responseIsOk)
                    {
                        if (string.IsNullOrEmpty(reasonPhrase))
                            reasonPhrase = BitMetadataBuilder.UnknownError;
                        else if (!string.Equals(reasonPhrase, BitMetadataBuilder.KnownError, StringComparison.InvariantCultureIgnoreCase) && !string.Equals(reasonPhrase, BitMetadataBuilder.UnknownError, StringComparison.InvariantCultureIgnoreCase))
                            reasonPhrase = $"{BitMetadataBuilder.UnknownError}:{reasonPhrase}";
                    }
                    if (!responseIsOk)
                    {
                        context.Features.Get<IHttpResponseFeature>().ReasonPhrase = reasonPhrase;
                        if (!context.Response.Headers.ContainsKey("Reason-Phrase"))
                        {
                            context.Response.Headers.Add("Reason-Phrase", new[] { reasonPhrase });
                        }
                    }

                    if (!context.Response.Headers.ContainsKey("X-Correlation-ID"))
                        context.Response.Headers.Add("X-Correlation-ID", xCorrelationId);

                    return Task.CompletedTask;
                });

                await _next.Invoke(context).ConfigureAwait(false);

                var status = GetResponseStatus(context);
                if (status.responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason ||
                    status.responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason)
                {
                    string? reasonPhrase = context.Features.Get<IHttpResponseFeature>().ReasonPhrase;

                    scopeStatusManager.MarkAsFailed(reasonPhrase);

                    logger.AddLogData("ResponseStatusCode", context.Response.StatusCode);
                    logger.AddLogData("ResponseReasonPhrase", reasonPhrase);

                    if (status.responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason || reasonPhrase == BitMetadataBuilder.KnownError)
                    {
                        await logger.LogWarningAsync("Response has failed status code because of some client side reason").ConfigureAwait(false);
                    }
                    else if (status.responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason)
                    {
                        await logger.LogFatalAsync("Response has failed status code because of some server side reason").ConfigureAwait(false);
                    }
                }
                else if (!scopeStatusManager.WasSucceeded())
                {
                    await logger.LogFatalAsync($"Scope was failed: {scopeStatusManager.FailureReason}").ConfigureAwait(false);
                }
                else
                {
                    scopeStatusManager.MarkAsSucceeded();

                    if (logger.Policy == LogPolicy.Always)
                        await logger.LogInformationAsync("Response succeded.").ConfigureAwait(false);
                }
            }
            catch (Exception exp)
            {
                if (scopeStatusManager.WasSucceeded())
                    scopeStatusManager.MarkAsFailed(exp.Message);
                await logger.LogExceptionAsync(exp, "Request-Execution-Exception").ConfigureAwait(false);
                string statusCode = context.Response.StatusCode.ToString(CultureInfo.InvariantCulture);
                bool responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason = statusCode.StartsWith("5", StringComparison.InvariantCultureIgnoreCase);
                bool responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason = statusCode.StartsWith("4", StringComparison.InvariantCultureIgnoreCase);
                if (responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason == false && responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason == false)
                {
                    IExceptionToHttpErrorMapper exceptionToHttpErrorMapper = context.RequestServices.GetRequiredService<IExceptionToHttpErrorMapper>();
                    context.Response.StatusCode = Convert.ToInt32(exceptionToHttpErrorMapper.GetStatusCode(exp), CultureInfo.InvariantCulture);
                    context.Features.Get<IHttpResponseFeature>().ReasonPhrase = exceptionToHttpErrorMapper.GetReasonPhrase(exp);
                    await context.Response.WriteAsync(exceptionToHttpErrorMapper.GetMessage(exp), context.RequestAborted).ConfigureAwait(false);
                }
                throw;
            }
        }
    }
}
