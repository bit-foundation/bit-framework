﻿using System;

namespace Bit.Core.Extensions
{
    public static class PlatformUtilities
    {
        private static readonly Lazy<bool> _isRunningOnMono = new Lazy<bool>(() =>
        {
            try
            {
                return Type.GetType("Mono.Runtime") != null;
            }
            catch
            {
                return false;
            }
        }, isThreadSafe: true);

        private static readonly Lazy<bool> _isRunningOnDotNetCore = new Lazy<bool>(() =>
        {
            try
            {
                return Type.GetType("System.Runtime.Loader.AssemblyLoadContext") != null;
            }
            catch
            {
                return false;
            }
        }, isThreadSafe: true);

        public static bool IsRunningOnMono => _isRunningOnMono.Value;

        public static bool IsRunningOnDotNetCore => _isRunningOnDotNetCore.Value;
    }
}
