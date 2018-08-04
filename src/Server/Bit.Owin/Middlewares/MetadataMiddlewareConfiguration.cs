﻿using Bit.Core.Models;
using Bit.Owin.Contracts;
using Owin;

namespace Bit.Owin.Middlewares
{
    public class MetadataMiddlewareConfiguration : IOwinMiddlewareConfiguration
    {
        public virtual AppEnvironment AppEnvironment { get; set; }

        public virtual void Configure(IAppBuilder owinApp)
        {
            string path = $"/Metadata/V{AppEnvironment.AppInfo.Version}";

            owinApp.Map(path, innerApp =>
            {
                if (AppEnvironment.DebugMode)
                    innerApp.Use<OwinNoCacheResponseMiddleware>();
                else
                    innerApp.Use<OwinCacheResponseMiddleware>();
                innerApp.UseXContentTypeOptions();
                innerApp.UseXDownloadOptions();
                innerApp.Use<MetadatMiddleware>();
            });
        }
    }
}
