using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.ExemploAzureAdClient.WebApi
{
    public class AzureAd
    {
        public string Instance { get; private set; }

        public string Tenant { get; private set; }

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public string Authority { get; private set; }

        public string Scope { get; private set; }

        public AzureAd(IConfiguration configuration)
        {
            Instance = configuration.GetSection("AzureAd").GetValue<string>("Instance");
            Tenant = configuration.GetSection("AzureAd").GetValue<string>("Tenant");
            ClientId = configuration.GetSection("AzureAd").GetValue<string>("ClientId");
            ClientSecret = configuration.GetSection("AzureAd").GetValue<string>("ClientSecret");
            Authority = string.Format(CultureInfo.InvariantCulture, Instance, Tenant);
            Scope = configuration.GetValue<string>("ListScope");
        }
    }
}
