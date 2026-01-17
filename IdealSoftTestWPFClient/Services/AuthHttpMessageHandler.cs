using IdealSoftTestWPFClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace IdealSoftTestWPFClient.Services
{
    public class AuthHttpMessageHandler : DelegatingHandler
    {
        private readonly ITokenStore _tokenStore;

        public AuthHttpMessageHandler(ITokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_tokenStore.AccessToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        _tokenStore.AccessToken);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }

}
