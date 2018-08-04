﻿using Bit.Core.Contracts;
using Bit.Owin.Contracts;
using Microsoft.Owin;
using Owin;
using System;

namespace Bit.Owin.Middlewares
{
    public class RedirectToSsoIfNotLoggedInMiddlewareConfiguration : IOwinMiddlewareConfiguration
    {
        public virtual void Configure(IAppBuilder owinApp)
        {
            if (owinApp == null)
                throw new ArgumentNullException(nameof(owinApp));

            owinApp.MapWhen(IfIsNotLoggedIn,
                innerApp =>
                {
                    innerApp.Use<OwinNoCacheResponseMiddleware>();
                    innerApp.Use<RedirectToSsoIfNotLoggedInMiddleware>();
                });
        }

        public virtual bool IfIsNotLoggedIn(IOwinContext cntx)
        {
            return !cntx.GetDependencyResolver().Resolve<IUserInformationProvider>().IsAuthenticated();
        }
    }
}
