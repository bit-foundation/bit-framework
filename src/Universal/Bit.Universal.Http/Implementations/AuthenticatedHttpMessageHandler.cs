﻿using Bit.Core.Models.Events;
using Bit.Http.Contracts;
using Prism.Events;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Bit.Http.Implementations
{
    public class AuthenticatedHttpMessageHandler : DelegatingHandler
    {
        private readonly ISecurityService _securityService;
        private readonly IEventAggregator _eventAggregator;

        public AuthenticatedHttpMessageHandler(IEventAggregator eventAggregator, ISecurityService securityService, HttpMessageHandler defaultHttpMessageHandler)
            : base(defaultHttpMessageHandler)
        {
            _securityService = securityService;
            _eventAggregator = eventAggregator;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers.Authorization == null)
            {
                Token? token = await _securityService.GetCurrentTokenAsync(cancellationToken).ConfigureAwait(false);

                if (token != null)
                    request.Headers.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _eventAggregator.GetEvent<UnauthorizedResponseEvent>().Publish(new UnauthorizedResponseEvent { });

                if ((await _securityService.IsLoggedInAsync(cancellationToken).ConfigureAwait(false)) == false)
                    _eventAggregator.GetEvent<TokenExpiredEvent>().Publish(new TokenExpiredEvent { });
            }

            return response;
        }
    }
}
