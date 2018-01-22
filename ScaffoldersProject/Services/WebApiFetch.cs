using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScaffoldersProject.Services
{
    public class WebApiFetch : IWebApiFetch
    {
        public async Task<string> WebApiFetchAsync(string url,string path)
        {
            // Create a New HttpClient object.
            using (var client=new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = await client.GetAsync(path);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    var error = $"Message : {e.Message}";
                    return error; 
                }
                finally
                {
                    // Need to call dispose on the HttpClient object
                    // when done using it, so the app doesn't leak resources
                    client.Dispose();
                }
            }
        }
    }
}
