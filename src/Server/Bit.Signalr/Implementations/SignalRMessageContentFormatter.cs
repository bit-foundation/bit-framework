﻿using Bit.Core.Contracts;
using Bit.Core.Implementations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bit.Signalr.Implementations
{
    public class SignalRMessageContentFormatter : IMessageContentFormatter
    {
        private JsonSerializerSettings _settingsCache;

        public virtual T DeSerialize<T>(string objAsStr)
        {
            throw new InvalidOperationException("This content formatter must be used only for serialization purposes");
        }

        public virtual string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, GetSettings());
        }

        protected virtual JsonSerializerSettings GetSettings()
        {
            if (_settingsCache == null)
            {
                _settingsCache = DefaultJsonContentFormatter.SerializeSettings();
                _settingsCache.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                _settingsCache.TypeNameHandling = TypeNameHandling.All;
                _settingsCache.Converters = new List<JsonConverter>
                {
                    new ThrowExceptionForDateTimeOffsetValues()
                };
            }
            return _settingsCache;
        }

        private class ThrowExceptionForDateTimeOffsetValues : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                if (objectType == null)
                    throw new ArgumentNullException(nameof(objectType));

                bool isDateTimeOffset = objectType.GetTypeInfo() == typeof(DateTimeOffset).GetTypeInfo() || objectType == typeof(DateTimeOffset?).GetTypeInfo();

                if (isDateTimeOffset)
                    throw new InvalidOperationException("You may not use date time values in signalr content formatter");

                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
