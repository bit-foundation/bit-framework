﻿using Bit.Core.Contracts;
using Microsoft.Owin;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Bit.WebApi.ActionFilters
{
    public class LogOperationArgsFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ILogger logger = actionContext.Request.GetOwinContext()
                .GetDependencyResolver()
                .Resolve<ILogger>();

            logger.AddLogData("OperationArgs", actionContext.ActionArguments.Where(arg => LogParameter(arg.Value)).ToArray());

            base.OnActionExecuting(actionContext);
        }

        protected virtual bool LogParameter(object parameter)
        {
            return parameter != null && !(parameter is CancellationToken);
        }
    }
}
