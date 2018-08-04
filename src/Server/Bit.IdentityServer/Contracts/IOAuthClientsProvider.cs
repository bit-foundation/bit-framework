﻿using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;

namespace Bit.IdentityServer.Contracts
{
    public abstract class BitOAuthClient
    {
        public virtual string ClientName { get; set; }

        public virtual string ClientId { get; set; }

        public virtual string Secret { get; set; }

        public virtual TimeSpan TokensLifetime { get; set; } = TimeSpan.FromDays(7);

        public virtual bool Enabled { get; set; } = true;

        public override string ToString()
        {
            return $"{nameof(ClientId)}: {ClientId}, {nameof(ClientName)}: {ClientName}";
        }
    }

    public class BitImplicitFlowClient : BitOAuthClient
    {
        public virtual IEnumerable<string> RedirectUris { get; set; }

        public virtual IEnumerable<string> PostLogoutRedirectUris { get; set; }
    }

    public class BitResourceOwnerFlowClient : BitOAuthClient
    {

    }

    public interface IOAuthClientsProvider
    {
        IEnumerable<Client> GetClients();
    }
}
