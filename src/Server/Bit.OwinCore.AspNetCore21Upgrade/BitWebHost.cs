﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Bit.OwinCore.AspNetCore21Upgrade
{
    public static class BitWebHost
    {
        public static IWebHostBuilder CreateDefaultBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseKestrel(options => options.AddServerHeader = false);
        }
    }
}
