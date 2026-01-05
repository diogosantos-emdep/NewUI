using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.PLM.CommonClasses
{
    public class ApiService : IApiService
    {
        private HttpClient client = new HttpClient();
        private readonly ITokenService tokenService;
        public ApiService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<IList<string>> GetValues()
        {
            List<string> values = new List<string>();
            var token = tokenService.GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
            var res = await client.GetAsync("https://ecos.emdep.com/api/update/products?references=");
            if (res.IsSuccessStatusCode)
            {
                var json = res.Content.ReadAsStringAsync().Result;
                values = JsonConvert.DeserializeObject<List<string>>(json);
            }
            else
            {
                values = new List<string> { res.StatusCode.ToString(), res.ReasonPhrase };
            }
            return values;
        }
    }
}

