﻿using Bit.Core;
using Bit.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IMvcCoreBuilder AddWebApiCore(this IServiceCollection services, IDependencyManager dependencyManager, params Assembly[] controllersAssemblies)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (controllersAssemblies == null)
                throw new ArgumentNullException(nameof(controllersAssemblies));

            controllersAssemblies = AssemblyContainer.Current.AssembliesWithDefaultAssemblies(controllersAssemblies);

            IMvcCoreBuilder builder = services.AddMvcCore()
                .AddJsonFormatters();

            controllersAssemblies.ToList().ForEach(asm => builder.AddApplicationPart(asm));

            dependencyManager.RegisterAssemblyTypes(controllersAssemblies, t => t.GetCustomAttribute<ControllerAttribute>() != null, lifeCycle: DependencyLifeCycle.Transient);

            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            return builder;
        }
    }
}
