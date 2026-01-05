using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Emdep.Geos.Data.Common.SynchronizationClass
{

    public class AuthTokenService : ITokenService
        {
            private AuthToken token = new AuthToken();
        

          

            public async Task<string> GetToken()
            {
                if (!this.token.IsValidAndNotExpiring)
                {
                    this.token = await this.GetNewAccessToken();
                }
                return token.AccessToken;
            }

            private async Task<AuthToken> GetNewAccessToken()
            {
                var token = new AuthToken();
                var client = new HttpClient();
                var client_id = 3;
                var client_secret = "5H6Bc55lnUSB5i0uYVNdOikMSYe2bnTkG6ddD7WB";
                var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

                var postMessage = new Dictionary<string, string>();
                postMessage.Add("grant_type", "client_credentials");
                //postMessage.Add("scope", "access_token");
                var request = new HttpRequestMessage(HttpMethod.Post, "http://ecos.emdep.com/oauth/token")
                {
                    Content = new FormUrlEncodedContent(postMessage)
                };

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<AuthToken>(json);
                    token.ExpiresAt = DateTime.UtcNow.AddSeconds(this.token.ExpiresIn);
                }
                else
                {
                    throw new ApplicationException("Unable to retrieve access token from Okta");
                }

                return token;
            }

            public class AuthToken
        {
                [JsonProperty(PropertyName = "access_token")]
                public string AccessToken { get; set; }

                [JsonProperty(PropertyName = "expires_in")]
                public int ExpiresIn { get; set; }

                public DateTime ExpiresAt { get; set; }

                public string Scope { get; set; }

                [JsonProperty(PropertyName = "token_type")]
                public string TokenType { get; set; }

                public bool IsValidAndNotExpiring
                {
                    get
                    {
                        return !String.IsNullOrEmpty(this.AccessToken) && this.ExpiresAt > DateTime.UtcNow.AddSeconds(30);
                    }
                }
            }
        }
   
}
