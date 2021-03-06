﻿using Bit.Core.Exceptions.Contracts;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace Bit.Core.Exceptions
{
    [Serializable]
    public class BadRequestException : AppException, IHttpStatusCodeAwareException
    {
        public BadRequestException()
            : this(ExceptionMessageKeys.BadRequestException)
        {

        }

        public BadRequestException(string message)
            : base(message)
        {

        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}
