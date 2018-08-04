﻿using Bit.Core.Models;
using Bit.Owin.Contracts;
using Owin;
using System;

namespace Bit.Owin.Middlewares
{
    public class IndexPageMiddlewareConfiguration : IOwinMiddlewareConfiguration
    {
        public virtual AppEnvironment AppEnvironment { get; set; }

        public virtual void Configure(IAppBuilder owinApp)
        {
            if (owinApp == null)
                throw new ArgumentNullException(nameof(owinApp));

            if (AppEnvironment.GetConfig("RequireSsl", defaultValueOnNotFound: false))
            {
                owinApp.UseHsts(config => config.IncludeSubdomains().MaxAge(days: 30));
            }

            owinApp.UseXContentTypeOptions();

            owinApp.UseXDownloadOptions();

            owinApp.UseXXssProtection(xssProtectionOptions => xssProtectionOptions.EnabledWithBlockMode());

            owinApp.Use<OwinNoCacheResponseMiddleware>();

            owinApp.Use<IndexPageMiddleware>();
        }
    }
}
