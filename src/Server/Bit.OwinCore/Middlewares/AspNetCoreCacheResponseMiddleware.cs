﻿using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bit.OwinCore.Middlewares
{
    public class AspNetCoreCacheResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public AspNetCoreCacheResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Response.Headers.Any(h => string.Equals(h.Key, "Cache-Control", StringComparison.InvariantCultureIgnoreCase)))
                context.Response.Headers.Remove("Cache-Control");
            context.Response.Headers.Add("Cache-Control", new[] { "public", "max-age=31536000" });

            if (context.Response.Headers.Any(h => string.Equals(h.Key, "Pragma", StringComparison.InvariantCultureIgnoreCase)))
                context.Response.Headers.Remove("Pragma");
            context.Response.Headers.Add("Pragma", new[] { "public" });

            if (context.Response.Headers.Any(h => string.Equals(h.Key, "Expires", StringComparison.InvariantCultureIgnoreCase)))
                context.Response.Headers.Remove("Expires");
            context.Response.Headers.Add("Expires", new[] { "max" });

            return _next.Invoke(context);
        }
    }
}
