﻿#define DEBUG
using Bit.Core.Contracts;
using Bit.Core.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bit.Owin.Implementations
{
    public class DebugLogStore : ILogStore
    {
        public virtual IContentFormatter Formatter { get; set; }

        public virtual void SaveLog(LogEntry logEntry)
        {
            if (Debugger.IsAttached)
                Debug.WriteLine(Formatter.Serialize(logEntry) + Environment.NewLine);
        }

        public virtual Task SaveLogAsync(LogEntry logEntry)
        {
            if (Debugger.IsAttached)
                Debug.WriteLine(Formatter.Serialize(logEntry) + Environment.NewLine);
            return Task.CompletedTask;
        }
    }
}
